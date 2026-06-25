#if BROWSER
using Examples;
namespace Examples.Core;

public partial class BasicScreenManager : IExample
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
        public string Name => "Core / Basic Screen Manager";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private enum GameScreen
        {
            Logo = 0,
            Title,
            Gameplay,
            Ending
        }

        private GameScreen _currentScreen;
        private int _framesCounter;

        public void Init()
        {
            _currentScreen = GameScreen.Logo;

            // TODO: Initialize all required variables and load all required data here!

            // Useful to count frames
            _framesCounter = 0;
        }

        public void Update()
        {
            switch (_currentScreen)
            {
                case GameScreen.Logo:
                    {
                        // TODO: Update LOGO screen variables here!

                        // Count frames
                        _framesCounter++;

                        // Wait for 2 seconds (120 frames) before jumping to TITLE screen
                        if (_framesCounter > 120)
                        {
                            _currentScreen = GameScreen.Title;
                        }
                    }
                    break;
                case GameScreen.Title:
                    {
                        // TODO: Update TITLE screen variables here!

                        // Press enter to change to GAMEPLAY screen
                        if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                        {
                            _currentScreen = GameScreen.Gameplay;
                        }
                    }
                    break;
                case GameScreen.Gameplay:
                    {
                        // TODO: Update GAMEPLAY screen variables here!

                        // Press enter to change to ENDING screen
                        if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                        {
                            _currentScreen = GameScreen.Ending;
                        }
                    }
                    break;
                case GameScreen.Ending:
                    {
                        // TODO: Update ENDING screen variables here!

                        // Press enter to return to TITLE screen
                        if (IsKeyPressed(KeyboardKey.Enter) || IsGestureDetected(Gesture.Tap))
                        {
                            _currentScreen = GameScreen.Title;
                        }
                    }
                    break;
                default:
                    break;
            }

            BeginDrawing();

            ClearBackground(Color.RayWhite);

            switch (_currentScreen)
            {
                case GameScreen.Logo:
                    {
                        // TODO: Draw LOGO screen here!
                        DrawText("LOGO SCREEN", 20, 20, 40, Color.LightGray);
                        DrawText("WAIT for 2 SECONDS...", 290, 220, 20, Color.Gray);

                    }
                    break;
                case GameScreen.Title:
                    {
                        // TODO: Draw TITLE screen here!
                        DrawRectangle(0, 0, screenWidth, screenHeight, Color.Green);
                        DrawText("TITLE SCREEN", 20, 20, 40, Color.DarkGreen);
                        DrawText("PRESS ENTER or TAP to JUMP to GAMEPLAY SCREEN", 120, 220, 20, Color.DarkGreen);

                    }
                    break;
                case GameScreen.Gameplay:
                    {
                        // TODO: Draw GAMEPLAY screen here!
                        DrawRectangle(0, 0, screenWidth, screenHeight, Color.Purple);
                        DrawText("GAMEPLAY SCREEN", 20, 20, 40, Color.Maroon);
                        DrawText("PRESS ENTER or TAP to JUMP to ENDING SCREEN", 130, 220, 20, Color.Maroon);

                    }
                    break;
                case GameScreen.Ending:
                    {
                        // TODO: Draw ENDING screen here!
                        DrawRectangle(0, 0, screenWidth, screenHeight, Color.Blue);
                        DrawText("ENDING SCREEN", 20, 20, 40, Color.DarkBlue);
                        DrawText("PRESS ENTER or TAP to RETURN to TITLE SCREEN", 120, 220, 20, Color.DarkBlue);

                    }
                    break;
                default:
                    break;
            }

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
