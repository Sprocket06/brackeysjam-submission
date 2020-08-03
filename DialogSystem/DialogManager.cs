using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Chroma.Diagnostics.Logging;
using Projection.UI;
using Yarn;

namespace Projection.DialogSystem
{
    public static class DialogManager
    {
        private static Log _log = LogManager.GetForCurrentAssembly();

        private static TextBlock _textBlock;
        private static Panel _panel;
        private static Dialog _currentDialog;

        static DialogManager()
        {
            _panel = new Panel(Vector2.Zero, new Size(GameCore.Window.Size.Width, 0));
            
            _textBlock = new TextBlock(Vector2.Zero, Size.Empty);
            _panel.AddChild(_textBlock);
            
            GUI.AddChild(_panel);
        }

        public static void StartDialog(Dialog dialog, string startNode = "start")
        {
            if (_currentDialog != null)
            {
                _currentDialog.Choice -= OnChoice;
                _currentDialog.NewLine -= OnNewLine;
                _currentDialog.DialogComplete -= OnDialogFinished;
            }

            _textBlock.Text = string.Empty;
            _currentDialog = dialog;
            _currentDialog.Choice += OnChoice;
            _currentDialog.NewLine += OnNewLine;
            _currentDialog.DialogComplete += OnDialogFinished;

            _currentDialog.SetNode(startNode);
            _currentDialog.Continue();
        }

        public static void Update(float delta)
        {
            _panel.Position = new Vector2(0, GameCore.Window.Size.Height - _panel.Size.Height);
        }

        private static void OnChoice(object sender, OptionSet optionSet)
        {
            _panel.RemoveAll<Button>();

            // todo: baseVector needs some tweaking and stuff...
            for (var i = 0; i < optionSet.Options.Length; i++)
            {
                var button = new Button(new Vector2(0, (_textBlock.Size.Height + 8) + 25 * i), Size.Empty)
                {
                    SizeToContent = AutoSizeMode.Both,
                    Text = _currentDialog.StringTable[optionSet.Options[i].Line.ID].text
                };

                var id = i;
                button.Clicked += (o, args) =>
                {
                    var sid = optionSet.Options[id].Line.ID;
                    _log.Debug($"{sid} was clicked UwU: {_currentDialog.StringTable[sid].text}");

                    _currentDialog.Choose(optionSet.Options[id].ID);
                    _currentDialog.Continue();
                };

                _panel.AddChild(button);
            }
        }

        private static async void OnNewLine(object sender, LineEventArgs e)
        {
            _panel.RemoveAll<Button>();


            e.AutoContinue = false;
            _textBlock.Text = string.Empty;

            await Task.Run(async () =>
            {
                foreach (var c in e.Line)
                {
                    _textBlock.Text += c;

                    if (c == '.')
                        await Task.Delay(500);
                    else
                        await Task.Delay(30);
                }
            });

            _currentDialog.Continue();
        }

        private static void OnDialogFinished(object sender, EventArgs e)
        {
            _panel.RemoveAll<Button>();
        }
    }
}