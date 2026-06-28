#if BROWSER
using Examples;
using System;

namespace Examples.Text;

public partial class WritingAnim : IExample
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
        public string Name => "Text / Writing Animation";

        private const string Message = "This sample illustrates a text writing\nanimation effect! Check it out! ;)";
        private int _framesCounter;

        public void Init()
        {
            _framesCounter = 0;
        }

        public void Update()
        {
            _framesCounter += IsKeyDown(KeyboardKey.Space) ? 8 : 1;

            if (IsKeyPressed(KeyboardKey.Enter))
            {
                _framesCounter = 0;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            int count = Math.Min(Message.Length, _framesCounter / 10);
            DrawText(Message.Substring(0, count), 210, 160, 20, Color.Maroon);

            DrawText("PRESS [ENTER] to RESTART!", 240, 260, 20, Color.LightGray);
            DrawText("PRESS [SPACE] to SPEED UP!", 239, 300, 20, Color.LightGray);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
