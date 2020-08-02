using System;
using System.Collections.Generic;
using System.Runtime;
using Chroma.Diagnostics.Logging;
using Yarn;
using Yarn.Compiler;

namespace Projection.DialogSystem
{
    public class Dialog
    {
        private Log _log = LogManager.GetForCurrentAssembly();
        
        private Yarn.Program _prog;
        private Dialogue _dial;
        private MemoryVariableStore _varStore;
        
        private IDictionary<string, StringInfo> _stringTable;

        public bool IsRunning = false;
        
        public event EventHandler<string> NodeStarted;
        public event EventHandler<string> NodeEnded;
        public event EventHandler DialogComplete;
        
        public Dialog(string fileName)
        {
            Compiler.CompileFile(fileName, out _prog, out _stringTable);
            
            _varStore = new MemoryVariableStore();
            _dial = new Dialogue(_varStore)
            {
                LogDebugMessage = (s) => _log.Debug(s),
                LogErrorMessage = (s) => _log.Error(s),
                lineHandler = HandleLine,
                commandHandler = HandleCommand,
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
        }

        private Dialogue.HandlerExecutionType HandleCommand(Command command)
        {
            throw new System.NotImplementedException();
        }

        private Dialogue.HandlerExecutionType HandleLine(Line line)
        {
            return Dialogue.HandlerExecutionType.ContinueExecution;
        }
        
        void HandleDialogueComplete()
        {
            IsRunning = false;
            DialogComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}