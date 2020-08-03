using Chroma;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input.EventArgs;
using Chroma.Windowing;
using Projection.DialogSystem;
using Projection.UI;

namespace Projection
{
    internal class GameCore : Game
    {
        private static Log Log { get; } = LogManager.GetForCurrentAssembly();

        public new static Window Window { get; private set; }

        internal GameCore()
        {
            Window = base.Window;
            
            Log.Info("Hello, world!");
            GraphicsManager.AutoClearColor = Color.Black;
        }

        protected override void LoadContent()
        {
            RegisterCustomImporters();

            GUI.LoadContent(Content);
            var test = Content.Load<Dialog>("Dialog/TestFile.yarn");

            DialogManager.StartDialog(test);
        }

        protected override void Draw(RenderContext context)
        {
            GUI.Draw(context);
        }

        protected override void Update(float delta)
        {
            DialogManager.Update(delta);
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