using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Yarn;
using Yarn.Compiler;

namespace Projection.DialogSystem
{
    public class Dialog : DisposableResource
    {
        private static Log _log = LogManager.GetForCurrentAssembly();

        private static readonly Dictionary<string, MethodInfo> _commands;

        private Yarn.Program _prog;
        private Dialogue _dial;

        public MemoryVariableStore VariableStore { get; private set; }
        public bool IsRunning => _dial.IsActive;

        public event EventHandler<string> NodeStarted;
        public event EventHandler<string> NodeEnded;
        public event EventHandler DialogComplete;
        public event EventHandler<OptionSet> Choice;
        public event EventHandler<LineEventArgs> NewLine;

        public IDictionary<string, Yarn.Compiler.StringInfo> StringTable { get; private set; }

        static Dialog()
        {
            _commands = new Dictionary<string, MethodInfo>();

            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes();

            for (var i = 0; i < types.Length; i++)
            {
                var methods = types[i].GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToList();

                for (var j = 0; j < methods.Count; j++)
                {
                    var m = methods[j];

                    if (m.ReturnType != typeof(bool))
                        continue;

                    var p = m.GetParameters();
                    if (p.Length != 1)
                    {
                        continue;
                    }
                    
                    if (p[0].ParameterType != typeof(string[]))
                    {
                        continue;
                    }
                    
                    var attrib = m.GetCustomAttributes().FirstOrDefault(x => x is DialogCommandAttribute) as
                        DialogCommandAttribute;

                    if (attrib != null)
                    {
                        if (_commands.TryAdd(attrib.CommandName, methods[j]))
                        {
                            _log.Info($"Added command handler '{attrib.CommandName}'!");
                        }
                    }
                }
            }
        }

        public Dialog(string fileName)
        {
            Compiler.CompileFile(fileName, out _prog, out var stringTable);
            StringTable = stringTable;

            VariableStore = new MemoryVariableStore();
            _dial = new Dialogue(VariableStore)
            {
                LogDebugMessage = (s) => _log.Debug(s),
                LogErrorMessage = (s) => _log.Error(s),
                lineHandler = HandleLine,
                commandHandler = HandleCommand,
                optionsHandler = HandleChoice,
                nodeStartHandler = (node) =>
                {
                    NodeStarted?.Invoke(this, node);
                    return Dialogue.HandlerExecutionType.ContinueExecution;
                },
                nodeCompleteHandler = (node) =>
                {
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
            if (!IsRunning)
            {
                _log.Warning("Refusing to stop a non-running dialog.");
                return;
            }
            
            _dial.Stop();
        }

        private Dialogue.HandlerExecutionType HandleCommand(Command command)
        {
            var tokens = command.Text.Split(' ').ToList();
            var cmd = tokens[0];
            
            if (_commands.ContainsKey(cmd))
            {
                tokens.RemoveAt(0);
                var result = (bool?)_commands[cmd]?.Invoke(null, new object[] {tokens.ToArray()});

                if (result.HasValue)
                {
                    return result.Value ? Dialogue.HandlerExecutionType.ContinueExecution
                                        : Dialogue.HandlerExecutionType.PauseExecution;
                }
                else
                {
                    _log.Warning($"Command handler for '{command.Text}' has failed for whatever reason.");
                }
            }
            else
            {
                _log.Warning($"No command defined for trigger '{command.Text}'.");
            }

            return Dialogue.HandlerExecutionType.ContinueExecution;
        }

        private Dialogue.HandlerExecutionType HandleLine(Line line)
        {
            var localeCode = CultureInfo.CurrentCulture.Name;
            var lineInfo = StringTable[line.ID];
            _log.Info(lineInfo.text);

            if (line.Substitutions.Length != 0)
            {
                for (var i = 0; i < line.Substitutions.Length; i++)
                {
                    lineInfo.text = lineInfo.text.Replace($"{{{i}}}", line.Substitutions[i]);
                }
            }

            lineInfo.text = Dialogue.ExpandFormatFunctions(lineInfo.text, localeCode);
            var args = new LineEventArgs(lineInfo.text);
            NewLine?.Invoke(this, args);
            
            if (args.AutoContinue)
            {
                return Dialogue.HandlerExecutionType.ContinueExecution;
            }

            return Dialogue.HandlerExecutionType.PauseExecution;
        }

        private void HandleChoice(OptionSet optSet)
        {
            Choice?.Invoke(this, optSet);
        }

        private void HandleDialogueComplete()
        {
            DialogComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}