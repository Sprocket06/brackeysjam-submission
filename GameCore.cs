using System;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Projection.UI;
using Projection.DialogSystem;
using Color = Chroma.Graphics.Color;

namespace Projection
{
    internal class GameCore : Game
    {
        private static Log Log { get; } = LogManager.GetForCurrentAssembly();

        private string _dialogText;

        internal GameCore()
        {
            Log.Info("Hello, world!");
            GraphicsManager.AutoClearColor = Color.Black;
        }

        protected override void LoadContent()
        {
            RegisterCustomImporters();

            GUI.LoadContent(Content);

            var tex = Content.Load<Texture>("Sprites/UI/button_hover.png");
            var tex2 = Content.Load<Texture>("Sprites/UI/button_regular.png");
            var tex3 = Content.Load<Texture>("Sprites/UI/button_pressed.png");
            var test = Content.Load<Dialog>("Dialog/TestFile.yarn");

            test.Choice += (sender, s) =>
            {
                GUI.ClearVisualTree();

                var baseVector = new Vector2(100, 100);
                for (var i = 0; i < s.Options.Length; i++)
                {
                    var button = new Button(baseVector + new Vector2(0, 25 * i), Size.Empty)
                        {SizeToContent = true};

                    var id = i;
                    button.Text = test.StringTable[s.Options[i].Line.ID].text;
                    button.Clicked += (o, args) =>
                    {
                        var sid = s.Options[id].Line.ID;
                        Log.Debug($"{sid} was clicked UwU: {test.StringTable[sid].text}");

                        test.Choose(s.Options[id].ID);
                        test.Continue();
                    };

                    GUI.AddChild(button);
                }
            };

            test.NewLine += async (sender, args) =>
            {
                GUI.ClearVisualTree();
                args.AutoContinue = false;
                _dialogText = string.Empty;
                
                await Task.Run(async () =>
                {
                    foreach (var c in args.Line)
                    {
                        _dialogText += c;

                        if (c == '.')
                            await Task.Delay(500);
                        else
                            await Task.Delay(30);
                    }
                });
                
                test.Continue();
            };

            test.DialogComplete += (sender, args) => GUI.ClearVisualTree();

            test.SetNode("start");
            test.Continue();
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(_dialogText, new Vector2(100, 75));
            GUI.Draw(context);
        }

        protected override void Update(float delta)
        {
            GUI.Update(delta);
        }

        protected override void MouseMoved(MouseMoveEventArgs e)
        {
            GUI.MouseMoved(e);
        }

        protected override void MouseReleased(MouseButtonEventArgs e)
        {
            GUI.MouseReleased(e);
        }

        protected override void MousePressed(MouseButtonEventArgs e)
        {
            GUI.MousePressed(e);
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            GUI.KeyPressed(e);
        }

        protected override void KeyReleased(KeyEventArgs e)
        {
            GUI.KeyReleased(e);
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            GUI.TextInput(e);
        }

        private void RegisterCustomImporters()
        {
            Content.RegisterImporter<Dialog>((path, args) => { return new Dialog(path); });
        }

        [DialogCommand("debugcommand")]
        public static bool HandleDebugCommand(string[] parameters)
        {
            Log.Info("UwU I am a debug command!");
            return true;
        }
    }
}