using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace Projection.UI
{
    public class Button : UiElement
    {
        public Color Foreground { get; set; } = Color.Black;
        public Brush Background { get; private set; }

        public Brush RegularBrush { get; set; } = new SolidColorBrush(Color.Gray);
        public Brush PressedBrush { get; set; } = new SolidColorBrush(Color.Yellow);
        public Brush HoverBrush { get; set; } = new SolidColorBrush(Color.LightGray);

        public string Text { get; set; }
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;

        public Button(Vector2 position, Size size) : base(position, size)
        {
            RefreshBrushes();
        }

        public void RefreshBrushes()
        {
            if (IsPressed)
            {
                Background = PressedBrush;
            }
            else if (IsMouseOver)
            {
                Background = HoverBrush;
            }
            else
            {
                Background = RegularBrush;
            }
        }

        protected override void OnMouseButtonUp()
        {
            if (IsMouseOver)
            {
                Background = HoverBrush;
            }
            else
            {
                Background = RegularBrush;
            }
        }

        protected override void OnMouseButtonDown()
        {
            if (IsPressed)
            {
                Background = PressedBrush;
            }
        }

        protected override void OnMouseEnter()
        {
            if (IsPressed)
            {
                Background = PressedBrush;
            }
            else
            {
                Background = HoverBrush;
            }
        }

        protected override void OnMouseLeave()
        {
            Background = RegularBrush;
        }

        protected override Size MeasureSize()
        {
            var size = GUI.DefaultFont.Measure(Text);
            return new Size(size.Width + 8, size.Height + 4);
        }

        protected override void DrawContent(RenderContext context)
        {
            if (Background is SolidColorBrush scb)
            {
                context.Rectangle(
                    ShapeMode.Fill,
                    Position,
                    Size,
                    scb.Color
                );
            }
            else if (Background is TextureBrush tb)
            {
                var prevFilteringMode = tb.Texture.FilteringMode;
                tb.Texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
                tb.Texture.VirtualResolution = new Size(Size.Height, Size.Width);
                context.DrawTexture(
                    tb.Texture,
                    Position,
                    Vector2.One, 
                    Vector2.Zero,
                    0f
                );
                tb.Texture.VirtualResolution = null;
                tb.Texture.FilteringMode = prevFilteringMode;
            }

            context.DrawString(
                GUI.DefaultFont,
                Text,
                GetTextPositionForAlignment(Text, TextAlignment),
                Foreground
            );
        }
    }
}