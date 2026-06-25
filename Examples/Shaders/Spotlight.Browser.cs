#if BROWSER
using Examples;
using System;

namespace Examples.Shaders;

public partial class Spotlight : IExample
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
        public string Name => "Shaders / Spotlight";

        // NOTE: It must be the same as define in shader
        private const int MaxSpots = 3;
        private const int MaxStars = 400;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        // Spot data
        private struct Spot
        {
            public Vector2 pos;
            public Vector2 vel;
            public float inner;
            public float radius;

            // Shader locations
            public int posLoc;
            public int innerLoc;
            public int radiusLoc;
        }

        // Stars in the star field have a position and velocity
        private struct Star
        {
            public Vector2 pos;
            public Vector2 vel;
        }

        private Texture2D _texRay;
        private Star[] _stars;
        private Spot[] _spots;
        private Shader _shdrSpot;
        private int _frameCounter;

        public void Init()
        {
            HideCursor();

            _texRay = LoadTexture("resources/raysan.png");

            _stars = new Star[MaxStars];

            for (int n = 0; n < MaxStars; n++)
            {
                ResetStar(ref _stars[n]);
            }

            // Progress all the stars on, so they don't all start in the centre
            for (int m = 0; m < screenWidth / 2.0; m++)
            {
                for (int n = 0; n < MaxStars; n++)
                {
                    UpdateStar(ref _stars[n]);
                }
            }

            _frameCounter = 0;

            // Use default vert shader
            _shdrSpot = LoadShader(null, "resources/shaders/glsl100/spotlight.fs");

            // Get the locations of spots in the shader
            _spots = new Spot[MaxSpots];

            for (int i = 0; i < MaxSpots; i++)
            {
                string posName = $"spots[{i}].pos";
                string innerName = $"spots[{i}].inner";
                string radiusName = $"spots[{i}].radius";

                _spots[i].posLoc = GetShaderLocation(_shdrSpot, posName);
                _spots[i].innerLoc = GetShaderLocation(_shdrSpot, innerName);
                _spots[i].radiusLoc = GetShaderLocation(_shdrSpot, radiusName);
            }

            // Tell the shader how wide the screen is so we can have
            // a pitch Color.black half and a dimly lit half.
            int wLoc = GetShaderLocation(_shdrSpot, "screenWidth");
            float sw = (float)GetScreenWidth();
            SetShaderValue(_shdrSpot, wLoc, sw, ShaderUniformDataType.Float);

            // Randomise the locations and velocities of the spotlights
            // and initialise the shader locations
            for (int i = 0; i < MaxSpots; i++)
            {
                _spots[i].pos.X = GetRandomValue(64, screenWidth - 64);
                _spots[i].pos.Y = GetRandomValue(64, screenHeight - 64);
                _spots[i].vel = new Vector2(0, 0);

                while ((MathF.Abs(_spots[i].vel.X) + MathF.Abs(_spots[i].vel.Y)) < 2)
                {
                    _spots[i].vel.X = GetRandomValue(-40, 40) / 10.0f;
                    _spots[i].vel.Y = GetRandomValue(-40, 40) / 10.0f;
                }

                _spots[i].inner = 28.0f * (i + 1);
                _spots[i].radius = 48.0f * (i + 1);

                SetShaderValue(
                    _shdrSpot,
                    _spots[i].posLoc,
                    _spots[i].pos,
                    ShaderUniformDataType.Vec2
                );
                SetShaderValue(
                    _shdrSpot,
                    _spots[i].innerLoc,
                    _spots[i].inner,
                    ShaderUniformDataType.Float
                );
                SetShaderValue(
                    _shdrSpot,
                    _spots[i].radiusLoc,
                    _spots[i].radius,
                    ShaderUniformDataType.Float
                );
            }
        }

        public void Update()
        {
            _frameCounter++;

            // Move the stars, resetting them if the go offscreen
            for (int n = 0; n < MaxStars; n++)
            {
                UpdateStar(ref _stars[n]);
            }

            // Update the spots, send them to the shader
            for (int i = 0; i < MaxSpots; i++)
            {
                if (i == 0)
                {
                    Vector2 mp = GetMousePosition();
                    _spots[i].pos.X = mp.X;
                    _spots[i].pos.Y = screenHeight - mp.Y;
                }
                else
                {
                    _spots[i].pos.X += _spots[i].vel.X;
                    _spots[i].pos.Y += _spots[i].vel.Y;

                    if (_spots[i].pos.X < 64)
                    {
                        _spots[i].vel.X = -_spots[i].vel.X;
                    }

                    if (_spots[i].pos.X > (screenWidth - 64))
                    {
                        _spots[i].vel.X = -_spots[i].vel.X;
                    }

                    if (_spots[i].pos.Y < 64)
                    {
                        _spots[i].vel.Y = -_spots[i].vel.Y;
                    }

                    if (_spots[i].pos.Y > (screenHeight - 64))
                    {
                        _spots[i].vel.Y = -_spots[i].vel.Y;
                    }
                }

                SetShaderValue(
                    _shdrSpot,
                    _spots[i].posLoc,
                    _spots[i].pos,
                    ShaderUniformDataType.Vec2
                );
            }

            BeginDrawing();
            ClearBackground(Color.DarkBlue);

            // Draw stars and bobs
            for (int n = 0; n < MaxStars; n++)
            {
                // MathF.Single pixel is just too small these days!
                DrawRectangle((int)_stars[n].pos.X, (int)_stars[n].pos.Y, 2, 2, Color.White);
            }

            for (int i = 0; i < 16; i++)
            {
                DrawTexture(
                    _texRay,
                    (int)((screenWidth / 2.0) + MathF.Cos((_frameCounter + i * 8) / 51.45f) * (screenWidth / 2.2) - 32),
                    (int)((screenHeight / 2.0) + MathF.Sin((_frameCounter + i * 8) / 17.87f) * (screenHeight / 4.2)),
                    Color.White
                );
            }

            // Draw spot lights
            BeginShaderMode(_shdrSpot);

            // Instead of a blank rectangle you could render a render texture of the full screen used to do screen
            // scaling (slight adjustment to shader would be required to actually pay attention to the colour!)
            DrawRectangle(0, 0, screenWidth, screenHeight, Color.White);
            EndShaderMode();

            DrawFPS(10, 10);

            DrawText("Move the mouse!", 10, 30, 20, Color.Green);
            DrawText("Pitch Color.Black", (int)(screenWidth * 0.2f), screenHeight / 2, 20, Color.Green);
            DrawText("Dark", (int)(screenWidth * 0.66f), screenHeight / 2, 20, Color.Green);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texRay);
            UnloadShader(_shdrSpot);
        }

        private static void ResetStar(ref Star s)
        {
            s.pos = new Vector2(GetScreenWidth() / 2.0f, GetScreenHeight() / 2.0f);

            do
            {
                s.vel.X = (float)GetRandomValue(-1000, 1000) / 100.0f;
                s.vel.Y = (float)GetRandomValue(-1000, 1000) / 100.0f;
            } while (!((MathF.Abs(s.vel.X) + (MathF.Abs(s.vel.Y)) > 1)));

            s.pos += s.pos + (s.vel * new Vector2(8.0f, 8.0f));
        }

        private static void UpdateStar(ref Star s)
        {
            s.pos += s.vel;

            if ((s.pos.X < 0) || (s.pos.X > GetScreenWidth()) ||
                (s.pos.Y < 0) || (s.pos.Y > GetScreenHeight()))
            {
                ResetStar(ref s);
            }
        }
    }
}
#endif
