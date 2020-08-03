using System;
using System.Collections.Generic;
using System.Runtime;
using System.Globalization;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Yarn;
using Yarn.Compiler;

namespace Projection.DialogSystem
{
    public class Dialog : DisposableResource
    {
        private Log _log = LogManager.GetForCurrentAssembly();
        
        private Yarn.Program _prog;
        private Dialogue _dial;
        private MemoryVariableStore _varStore;

        public bool IsRunning = false;
        
        public event EventHandler<string> NodeStarted;
        public event EventHandler<string> NodeEnded;
        public event EventHandler DialogComplete;
        public event EventHandler<OptionSet> Choice;
        public event EventHandler<LineEventArgs> NewLine;

        public IDictionary<string, Yarn.Compiler.StringInfo> StringTable { get; private set; }

        public Dialog(string fileName)
        {
            Compiler.CompileFile(fileName, out _prog, out var stringTable);
            StringTable = stringTable;

            _varStore = new MemoryVariableStore();
            _dial = new Dialogue(_varStore)
            {
                LogDebugMessage = (s) => _log.Debug(s),
                LogErrorMessage = (s) => _log.Error(s),
                lineHandler = HandleLine,
                commandHandler = HandleCommand,
                optionsHandler = HandleChoice,
                nodeStartHandler = (node) => {
                    NodeStarted?.Invoke(this, node);
                    return Dialogue.HandlerExecutionType.ContinueExecution;
                },
                nodeCompleteHandler = (node) => {
                    NodeEnded?.Invoke(this, node);
                    return Dialogue.HandlerExecutionType.ContinueExecution;
                },
                dialogueCompleteHandler = HandleDialogueComplete
            };
            _dial.AddProgram(_prog);
        }

        public void SetNode(string nodeName)
        {
            _dial.SetNode(nodeName);
        }

        public void Continue()
        {
            _dial.Continue();
        }

        public void Choose(int optID)
        {
            _dial.SetSelectedOption(optID);
        }

        public void Stop()
        {
            _dial.Stop();
        }

        private Dialogue.HandlerExecutionType HandleCommand(Command command)
        {
            //pretty sure this is where Yarn.Library comes in,
            //but Command only has the one text property
            //and I'm too lazy to debug exactly what to do with it.
            throw new System.NotImplementedException();
        }

        private Dialogue.HandlerExecutionType HandleLine(Line line)
        {
            String localeCode = CultureInfo.CurrentCulture.Name;
            Yarn.Compiler.StringInfo lineInfo = StringTable[line.ID];
            _log.Info(lineInfo.text);

            if(line.Substitutions.Length != 0)
            {
                for(var i = 0; i < line.Substitutions.Length; i++)
                {
                    lineInfo.text.Replace($"{{{i}}}", line.Substitutions[i]);
                }
            }
            
            lineInfo.text = Dialogue.ExpandFormatFunctions(lineInfo.text, localeCode);
            var args = new LineEventArgs(lineInfo.text);
            NewLine?.Invoke(this, args);
            if (args.autoContinue)
            {
                return Dialogue.HandlerExecutionType.ContinueExecution;
            }
            return Dialogue.HandlerExecutionType.PauseExecution;
            
        }
        
        private void HandleChoice(OptionSet optSet)
        {
            _log.Info("Choice:");
            foreach(var opt in optSet.Options)
            {
                _log.Info($"{opt.Line}");
            }
            
            Choice?.Invoke(this, optSet);
        }
        
        void HandleDialogueComplete()
        {
            IsRunning = false;
            DialogComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}