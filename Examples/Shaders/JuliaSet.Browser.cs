#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class JuliaSet : IExample
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
        public string Name => "Shaders / Julia Set";

        // NOTE: raylib's web build uses GLSL ES 1.00 (WebGL1), so we target glsl100
        private const int GlslVersion = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;
        private const float zoomSpeed = 1.01f;
        private const float offsetSpeedMul = 2.0f;
        private const float startingZoom = 0.75f;

        // A few good julia sets
        private static readonly float[][] PointsOfInterest = new float[][] {
                new float[] { -0.348827f, 0.607167f },
                new float[] { -0.786268f, 0.169728f },
                new float[] { -0.8f, 0.156f },
                new float[] { 0.285f, 0.0f },
                new float[] { -0.835f, -0.2321f },
                new float[] { -0.70176f, -0.3842f },
            };

        private Shader _shader;
        private RenderTexture2D _target;
        private float[] _c;
        private float[] _offset;
        private float _zoom;
        private int _cLoc;
        private int _zoomLoc;
        private int _offsetLoc;
        private int _incrementSpeed;
        private bool _showControls;

        public void Init()
        {
            // Load julia set shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            _shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/julia_set.fs");

            // Create a RenderTexture2D to be used for render to texture
            _target = LoadRenderTexture(screenWidth, screenHeight);

            // c constant to use in z^2 + c
            _c = new float[] { PointsOfInterest[0][0], PointsOfInterest[0][1] };

            // Offset and zoom to draw the julia set at. (centered on screen and default size)
            _offset = new float[] { 0, 0 };
            _zoom = startingZoom;

            // Get variable (uniform) locations on the shader to connect with the program
            // NOTE: If uniform variable could not be found in the shader, function returns -1
            _cLoc = GetShaderLocation(_shader, "c");
            _zoomLoc = GetShaderLocation(_shader, "zoom");
            _offsetLoc = GetShaderLocation(_shader, "offset");

            // Upload the shader uniform values!
            Raylib.SetShaderValue(_shader, _cLoc, _c, ShaderUniformDataType.Vec2);
            Raylib.SetShaderValue(_shader, _zoomLoc, _zoom, ShaderUniformDataType.Float);
            Raylib.SetShaderValue(_shader, _offsetLoc, _offset, ShaderUniformDataType.Vec2);

            // Multiplier of speed to change c value
            _incrementSpeed = 0;
            // Show controls
            _showControls = true;
        }

        public void Update()
        {
            // Press [1 - 6] to reset c to a point of interest
            if (IsKeyPressed(KeyboardKey.One) ||
                IsKeyPressed(KeyboardKey.Two) ||
                IsKeyPressed(KeyboardKey.Three) ||
                IsKeyPressed(KeyboardKey.Four) ||
                IsKeyPressed(KeyboardKey.Five) ||
                IsKeyPressed(KeyboardKey.Six))
            {

                if (IsKeyPressed(KeyboardKey.One))
                {
                    _c[0] = PointsOfInterest[0][0];
                    _c[1] = PointsOfInterest[0][1];
                }
                else if (IsKeyPressed(KeyboardKey.Two))
                {
                    _c[0] = PointsOfInterest[1][0];
                    _c[1] = PointsOfInterest[1][1];
                }
                else if (IsKeyPressed(KeyboardKey.Three))
                {
                    _c[0] = PointsOfInterest[2][0];
                    _c[1] = PointsOfInterest[2][1];
                }
                else if (IsKeyPressed(KeyboardKey.Four))
                {
                    _c[0] = PointsOfInterest[3][0];
                    _c[1] = PointsOfInterest[3][1];
                }
                else if (IsKeyPressed(KeyboardKey.Five))
                {
                    _c[0] = PointsOfInterest[4][0];
                    _c[1] = PointsOfInterest[4][1];
                }
                else if (IsKeyPressed(KeyboardKey.Six))
                {
                    _c[0] = PointsOfInterest[5][0];
                    _c[1] = PointsOfInterest[5][1];
                }
                Raylib.SetShaderValue(_shader, _cLoc, _c, ShaderUniformDataType.Vec2);
            }

            if (IsKeyPressed(KeyboardKey.R))
            {
                _zoom = startingZoom;
                _offset[0] = 1f;
                _offset[1] = 1f;
                Raylib.SetShaderValue(_shader, _zoomLoc, _zoom, ShaderUniformDataType.Float);
                Raylib.SetShaderValue(_shader, _offsetLoc, _offset, ShaderUniformDataType.Vec2);
            }

            // Pause animation (c change)
            if (IsKeyPressed(KeyboardKey.Space))
            {
                _incrementSpeed = 0;
            }

            // Toggle whether or not to show controls
            if (IsKeyPressed(KeyboardKey.F1))
            {
                _showControls = !_showControls;
            }

            if (IsKeyPressed(KeyboardKey.Right))
            {
                _incrementSpeed++;
            }
            else if (IsKeyPressed(KeyboardKey.Left))
            {
                _incrementSpeed--;
            }

            // If either left or right button is pressed, zoom in/out
            if (IsMouseButtonDown(MouseButton.Left) || IsMouseButtonDown(MouseButton.Right))
            {
                if (IsMouseButtonDown(MouseButton.Left))
                {
                    _zoom *= zoomSpeed;
                }

                if (IsMouseButtonDown(MouseButton.Right))
                {
                    _zoom *= 1.0f / zoomSpeed;
                }

                Vector2 mousePos = GetMousePosition();
                Vector2 offsetVelocity = Vector2.Zero;

                offsetVelocity.X = (mousePos.X / screenWidth - 0.5f) * offsetSpeedMul / _zoom;
                offsetVelocity.Y = (mousePos.Y / screenHeight - 0.5f) * offsetSpeedMul / _zoom;

                // Apply move velocity to camera
                _offset[0] += GetFrameTime() * offsetVelocity.X;
                _offset[1] += GetFrameTime() * offsetVelocity.Y;

                Raylib.SetShaderValue(_shader, _zoomLoc, _zoom, ShaderUniformDataType.Float);
                Raylib.SetShaderValue(_shader, _offsetLoc, _offset, ShaderUniformDataType.Vec2);
            }

            // Increment c value with time
            float amount = GetFrameTime() * _incrementSpeed * 0.0005f;
            _c[0] += amount;
            _c[1] += amount;

            Raylib.SetShaderValue(_shader, _cLoc, _c, ShaderUniformDataType.Vec2);

            // Using a render texture to draw Julia set
            // Enable drawing to texture
            BeginTextureMode(_target);
            ClearBackground(Color.Black);

            // Draw a rectangle in shader mode to be used as shader canvas
            // NOTE: Rectangle uses font Color.white character texture coordinates,
            // so shader can not be applied here directly because input vertexTexCoord
            // do not represent full screen coordinates (space where want to apply shader)
            DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), Color.Black);
            EndTextureMode();

            BeginDrawing();
            ClearBackground(Color.Black);

            // Draw the saved texture and rendered julia set with shader
            // NOTE: We do not invert texture on Y, already considered inside shader
            BeginShaderMode(_shader);
            DrawTexture(_target.Texture, 0, 0, Color.White);
            EndShaderMode();

            if (_showControls)
            {
                DrawText("Press Mouse buttons right/left to zoom in/out and move", 10, 15, 10, Color.RayWhite);
                DrawText("Press KEY_F1 to toggle these controls", 10, 30, 10, Color.RayWhite);
                DrawText("Press KEYS [1 - 6] to change point of interest", 10, 45, 10, Color.RayWhite);
                DrawText("Press KEY_LEFT | KEY_RIGHT to change speed", 10, 60, 10, Color.RayWhite);
                DrawText("Press KEY_SPACE to pause movement animation", 10, 75, 10, Color.RayWhite);
                DrawText("Press KEY_R to recenter the camera", 10, 90, 10, Color.RayWhite);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadShader(_shader);
            UnloadRenderTexture(_target);
        }
    }
}
#endif
