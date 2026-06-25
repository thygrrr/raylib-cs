#if BROWSER
using Examples;
namespace Examples.Core;

public partial class ScissorTest : IExample
{
    private readonly BrowserAdapter _browserAdapter = new();

    public string Name => _browserAdapter.Name;

    public void Init()
    {
        _browserAdapter.Init();
    }

    public void Update()
    {
        _browserAdapter.Update();
    }

    public void Unload()
    {
        _browserAdapter.Unload();
    }

    private sealed class BrowserAdapter : IExample
    {
        public string Name => "Core / Scissor Test";

        private Rectangle _scissorArea;
        private bool _scissorMode;

        public void Init()
        {
            _scissorArea = new Rectangle(0, 0, 300, 300);
            _scissorMode = true;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.S))
            {
                _scissorMode = !_scissorMode;
            }

            // Centre the scissor area around the mouse position
            _scissorArea.X = GetMouseX() - _scissorArea.Width / 2;
            _scissorArea.Y = GetMouseY() - _scissorArea.Height / 2;

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (_scissorMode)
            {
                BeginScissorMode((int)_scissorArea.X, (int)_scissorArea.Y, (int)_scissorArea.Width, (int)_scissorArea.Height);
            }

            // Draw full screen rectangle and some text
            // NOTE: Only part defined by scissor area will be rendered
            DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), Color.Red);
            DrawText("Move the mouse around to reveal this text!", 190, 200, 20, Color.LightGray);

            if (_scissorMode)
            {
                EndScissorMode();
            }

            DrawRectangleLinesEx(_scissorArea, 1, Color.Black);
            DrawText("Press S to toggle scissor test", 10, 10, 20, Color.Black);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
