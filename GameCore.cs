using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Projection.UI;

namespace Projection
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        internal GameCore()
        {
            Log.Info("Hello, world!");

            var b = new Button(new Vector2(100), new Size(120, 28));
            b.Text = "sproket is drunk";
            b.Pressed += (sender, args) => b.Text = "yes he is UwU";

            GUI.AddChild(b);
        }

        protected override void LoadContent()
        {
            GUI.LoadContent(Content);
        }

        protected override void Draw(RenderContext context)
        {
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
    }
}