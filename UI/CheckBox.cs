using System;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;

namespace Projection.UI
{
    public class CheckBox : UiElement
    {
        public bool IsChecked { get; private set; } = false;

        public Brush Foreground { get; set; }
        public Brush Background { get; set; }

        public CheckBox(Vector2 position, Size size) : base(position, size)
        {

        }

        protected override void OnClicked()
        {
            IsChecked = !IsChecked;
        }

        protected override void DrawContent(RenderContext context)
        {
            if(Background is SolidColorBrush scb)
            {
                context.Rectangle(
                    ShapeMode.Fill,
                    AbsolutePosition,
                    Size,
                    scb.Color
                );
            }else if(Background is TextureBrush tb)
            {
                var prevFilter = tb.Texture.FilteringMode;
                tb.Texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
                tb.Texture.VirtualResolution = new Size(Size.Width, Size.Height);
                context.DrawTexture(
                    tb.Texture,
                    AbsolutePosition,
                    Vector2.One,
                    Vector2.Zero,
                    0f
                );
                tb.Texture.VirtualResolution = null;
                tb.Texture.FilteringMode = prevFilter;
            }

            if (IsChecked) {

                if(Foreground is SolidColorBrush fb)
                {
                    context.Line(
                        new Vector2(AbsolutePosition.X, AbsolutePosition.Y + (Size.Height / 2)),
                        new Vector2(AbsolutePosition.X + (Size.Width / 3), AbsolutePosition.Y),
                        fb.Color
                    );
                    context.Line(
                        new Vector2(AbsolutePosition.X + (Size.Width / 3), AbsolutePosition.Y),
                        new Vector2(AbsolutePosition.X + Size.Width, AbsolutePosition.Y + Size.Height),
                        fb.Color
                    );
                }else if(Foreground is TextureBrush tb)
                {
                    var prevFilter = tb.Texture.FilteringMode;
                    tb.Texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
                    tb.Texture.VirtualResolution = new Size(Size.Width, Size.Height);
                    context.DrawTexture(
                        tb.Texture,
                        AbsolutePosition,
                        Vector2.One,
                        Vector2.Zero,
                        0f
                    );
                    tb.Texture.VirtualResolution = null;
                    tb.Texture.FilteringMode = prevFilter;
                }
            }
        }

    }
}
