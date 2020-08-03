using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Projection.UI
{
    public class Panel : UiElement
    {
        public HashSet<UiElement> Children { get; }
        public Color Background { get; } = Color.DarkGreen;

        public Panel(Vector2 position, Size size) : base(position, size)
        {
            Children = new HashSet<UiElement>();

            ClipContent = true;
            SizeToContent = AutoSizeMode.Height;
        }

        public void AddChild(UiElement child)
        {
            if (!Children.Contains(child))
            {
                child.Parent = this;
                Children.Add(child);
            }
        }

        public void RemoveAll<T>() where T : UiElement
            => Children.RemoveWhere((u) => u is T);

        protected override void DrawContent(RenderContext context)
        {
            context.Rectangle(
                ShapeMode.Fill,
                AbsolutePosition,
                Size,
                Background
            );

            DoForAll((u) => u.Draw(context));
        }

        protected override int MeasureWidth()
        {
            var w = 0;

            DoForAll((u) =>
            {
                if (u.Position.X + u.Size.Width > w)
                    w += (int)(u.Position.X + u.Size.Width) - w;
            });

            return w;
        }

        protected override int MeasureHeight()
        {
            var h = 0;
            
            DoForAll((u) =>
            {
                if (u.Position.Y + u.Size.Height > h)
                    h += (int)(u.Position.Y + u.Size.Height) - h;
            });

            return h;
        }

        protected override Size MeasureSize()
        {
            var s = new Size(0, 0);

            DoForAll((u) =>
            {
                if (u.Position.X + u.Size.Width > s.Width)
                {
                    var toExpandBy = (int)(u.Position.X + u.Size.Width) - s.Width;
                    s += new Size(toExpandBy, 0);
                }

                if (u.Position.Y + u.Size.Height > s.Height)
                {
                    var toExpandBy = (int)(u.Position.Y + u.Size.Height) - s.Height;
                    s += new Size(0, toExpandBy);
                }
            });

            return s;
        }

        protected override void UpdateState(float delta)
            => DoForAll((u) => u.Update(delta));

        protected override void OnMouseButtonDown(MouseButtonEventArgs e)
            => DoForAll((u) => u.MousePressed(e));

        protected override void OnMouseButtonUp(MouseButtonEventArgs e)
            => DoForAll((u) => u.MouseReleased(e));

        protected override void OnMouseMoved(MouseMoveEventArgs e)
            => DoForAll((u) => u.MouseMoved(e));

        protected override void OnKeyPressed(KeyEventArgs e)
            => DoForAll((u) => u.KeyPressed(e));

        protected override void OnKeyReleased(KeyEventArgs e)
            => DoForAll((u) => u.KeyReleased(e));

        protected override void OnTextInput(TextInputEventArgs e)
            => DoForAll((u) => u.TextInput(e));

        private void DoForAll(Action<UiElement> action)
        {
            for (var i = 0; i < Children.Count; i++)
                action(Children.ElementAt(i));
        }
    }
}