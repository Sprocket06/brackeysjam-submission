using System;
using System.Collections.Generic;
using Chroma.ContentManagement;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input.EventArgs;

namespace Projection.UI
{
    public static class GUI
    {
        private static List<UiElement> _children;

        public static UiElement FocusedElement { get; private set; }
        public static TrueTypeFont DefaultFont { get; private set; }

        static GUI()
        {
            _children = new List<UiElement>();
        }

        public static void LoadContent(IContentProvider content)
        {
            DefaultFont = content.Load<TrueTypeFont>("Fonts/cmd.ttf", 16);
            DefaultFont.ForceAutoHinting = true;
        }

        public static void SetFocus(UiElement element)
            => FocusedElement = element;

        public static void AddChild(UiElement element)
            => _children.Add(element);

        public static void ClearVisualTree()
            => _children.Clear();

        public static void Draw(RenderContext context)
            => DoForAll((u) => u.Draw(context));

        public static void Update(float delta)
            => DoForAll((u) => u.Update(delta));

        public static void MousePressed(MouseButtonEventArgs e)
            => DoForAll((u) => u.MousePressed(e));

        public static void MouseReleased(MouseButtonEventArgs e)
            => DoForAll((u) => u.MouseReleased(e));

        public static void MouseMoved(MouseMoveEventArgs e)
            => DoForAll((u) => u.MouseMoved(e));

        public static void KeyPressed(KeyEventArgs e)
            => DoForAll((u) => u.KeyPressed(e));

        public static void KeyReleased(KeyEventArgs e)
            => DoForAll((u) => u.KeyReleased(e));

        public static void TextInput(TextInputEventArgs e)
            => DoForAll((u) => u.TextInput(e));

        private static void DoForAll(Action<UiElement> action)
        {
            for (var i = 0; i < _children.Count; i++)
                action(_children[i]);
        }
    }
}