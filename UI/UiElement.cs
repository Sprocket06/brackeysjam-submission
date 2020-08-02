using System;
using System.Drawing;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Projection.UI
{
    public abstract class UiElement
    {
        public Vector2 Position { get; set; }
        public Size Size { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsMouseOver { get; private set; }
        public bool IsPressed { get; private set; }

        public bool SizeToContent { get; set; }
        public bool ClipContent { get; set; }

        public virtual bool AcceptsFocus { get; set; }
        public bool HasKeyboardFocus => GUI.FocusedElement == this;

        public event EventHandler MouseLeave;
        public event EventHandler MouseEnter;
        public event EventHandler Clicked;
        public event EventHandler<MouseButtonEventArgs> MouseButtonUp;
        public event EventHandler<MouseButtonEventArgs> MouseButtonDown;

        public UiElement(Vector2 position, Size size)
        {
            Position = position;
            Size = size;

            IsEnabled = true;
            ClipContent = true;
        }

        public void Draw(RenderContext context)
        {
            if (ClipContent)
            {
                context.Scissor = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Size.Width,
                    Size.Height
                );
            }

            DrawContent(context);
            
            context.Scissor = Rectangle.Empty;
        }

        public virtual void Update(float delta)
        {
            if (SizeToContent)
            {
                Size = MeasureSize();
            }
        }

        protected Size MeasureSize()
            => Size;

        protected virtual void DrawContent(RenderContext context)
        {
        }

        protected virtual void OnMouseMoved(MouseMoveEventArgs e)
        {
        }

        protected virtual void OnTextInput(TextInputEventArgs e)
        {
        }

        protected virtual void OnKeyPressed(KeyEventArgs e)
        {
        }

        protected virtual void OnKeyReleased(KeyEventArgs e)
        {
        }

        protected virtual void OnClicked()
        {
        }

        protected virtual void OnMouseButtonUp()
        {
        }

        protected virtual void OnMouseEnter()
        {
        }

        protected virtual void OnMouseLeave()
        {
        }
        
        public void MouseMoved(MouseMoveEventArgs e)
        {
            if (!IsEnabled)
            {
                IsMouseOver = false;
                return;
            }

            OnMouseMoved(e);
            
            if (e.Position.X >= Position.X && e.Position.X < Position.X + Size.Width &&
                e.Position.Y >= Position.Y && e.Position.Y < Position.Y + Size.Height)
            {
                if (!IsMouseOver)
                {
                    IsMouseOver = true;
                    OnMouseEnter();
                    MouseEnter?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                if (IsMouseOver)
                {
                    IsMouseOver = false;
                    OnMouseLeave();
                    MouseLeave?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void MousePressed(MouseButtonEventArgs e)
        {
            if (!IsEnabled)
                return;
            
            if (IsMouseOver)
            {
                if (e.Button == MouseButton.Left)
                {
                    IsPressed = true;
                    
                    if (AcceptsFocus)
                        GUI.SetFocus(this);
                }
                
                MouseButtonDown?.Invoke(this, e);
            }
        }

        public void MouseReleased(MouseButtonEventArgs e)
        {
            if (!IsEnabled)
                return;
            
            if (IsPressed)
            {
                IsPressed = false;

                if (IsMouseOver)
                {
                    OnClicked();
                    Clicked?.Invoke(this, EventArgs.Empty);
                }

                OnMouseButtonUp();
                MouseButtonUp?.Invoke(this, e);
            }
        }

        public void TextInput(TextInputEventArgs e)
        {
            if (!IsEnabled)
                return;
            
            if (HasKeyboardFocus)
            {
                OnTextInput(e);
            }
        }

        public void KeyReleased(KeyEventArgs e)
        {
            if (!IsEnabled)
                return;
            
            if (HasKeyboardFocus)
            {
                OnKeyReleased(e);
            }
        }

        public void KeyPressed(KeyEventArgs e)
        {
            if (!IsEnabled)
                return;
            
            if (HasKeyboardFocus)
            {
                OnKeyPressed(e);
            }
        }
    }
}