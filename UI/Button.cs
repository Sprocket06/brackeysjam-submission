using System;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

namespace Projection.UI
{
    public class Button : UiElement
    {
        public Color Foreground { get; set; } = Color.Black;
        public Color Background { get; private set; }

        public Color RegularBackground { get; set; } = Color.Gray;
        public Color PressedBackground { get; set; } = Color.Yellow;
        public Color HoverBackground { get; set; } = Color.LightGray;

        public string Text { get; set; }
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

        public Button(Vector2 position, Size size) : base(position, size)
        {
        }

        public override void Update(float delta)
        {
            if (IsPressed)
            {
                Background = PressedBackground;
            }
            else if (IsMouseOver)
            {
                Background = HoverBackground;
            }
            else
            {
                Background = RegularBackground;
            }
        }

        protected override void DrawContent(RenderContext context)
        {
            context.Rectangle(
                ShapeMode.Fill,
                Position,
                Size,
                Background
            );

            context.DrawString(
                GUI.DefaultFont,
                Text,
                GetTextPositionForAlignment(TextAlignment),
                Foreground
            );
        }

        private Vector2 GetTextPositionForAlignment(TextAlignment alignment)
        {
            var measure = GUI.DefaultFont.Measure(Text);

            var verticalCenter = (Position.Y + Size.Height / 2) - measure.Height / 2;
            
            switch (alignment)
            {
                case TextAlignment.Center:
                    return new Vector2(
                        (Position.X + Size.Width / 2) - measure.Width / 2,
                        verticalCenter
                    );
                
                case TextAlignment.Right:
                    return new Vector2(
                        Position.X + Size.Width - measure.Width,
                        verticalCenter
                    );
                
                case TextAlignment.Left:
                    return new Vector2(
                        Position.X,
                        verticalCenter
                    );
                
                default: return Position;
            }
        }
    }
}