using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Projection.UI;
using Color = Chroma.Graphics.Color;

namespace Projection
{
    internal class GameCore : Game
    {
        private Log Log { get; } = LogManager.GetForCurrentAssembly();
    
        internal GameCore()
        {
            Log.Info("Hello, world!");
            GraphicsManager.AutoClearColor = Color.Gray;
        }

        protected override void LoadContent()
        {
            GUI.LoadContent(Content);

            var tex = Content.Load<Texture>("Sprites/UI/button_hover.png");
            var tex2 = Content.Load<Texture>("Sprites/UI/button_regular.png");
            var tex3 = Content.Load<Texture>("Sprites/UI/button_pressed.png");
            
            var b = new Button(new Vector2(100), new Size(120, 28));
            b.Text = "sproket is drunk";
            b.Clicked += (sender, args) => b.Text = "yes he is UwU";
            b.RegularBrush = new TextureBrush(tex2);
            b.HoverBrush = new TextureBrush(tex);
            b.PressedBrush = new TextureBrush(tex3);
            b.Foreground = Color.Cyan;
            
            b.RefreshBrushes();
            GUI.AddChild(b);
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