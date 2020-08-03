using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Color = Chroma.Graphics.Color;

namespace Projection.UI
{
    public class TextBlock : UiElement
    {
        public string Text { get; set; } = string.Empty;
        
        public Color Foreground { get; set; } = Color.White;
        public Color Background { get; set; } = Color.Transparent;
        public TrueTypeFont Font { get; set; } = GUI.DefaultFont;

        public TextBlock(Vector2 position, Size size) : base(position, size)
        {
            SizeToContent = AutoSizeMode.Both;
            ClipContent = true;
        }

        protected override int MeasureWidth()
            => Font.Measure(Text).Width;

        protected override int MeasureHeight()
            => Font.Measure(Text).Height;
        
        protected override Size MeasureSize()
            => Font.Measure(Text);

        protected override void DrawContent(RenderContext context)
        {
            context.Rectangle(
                ShapeMode.Fill,
                AbsolutePosition,
                Size,
                Background
            );
            
            context.DrawString(
                Font,
                Text,
                AbsolutePosition,
                Foreground
            );
        }
    }
}