#if BROWSER
using Examples;
using static Raylib_cs.Raymath;

namespace Examples.Models;

public partial class YawPitchRoll : IExample
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

    private unsafe sealed class BrowserAdapter : IExample
    {
        public string Name => "Models / Yaw Pitch Roll";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Camera3D _camera;

        private Model _model;
        private Texture2D _texture;

        private float _pitch;
        private float _roll;
        private float _yaw;

        public void Init()
        {
            _camera = new();
            _camera.Position = new Vector3(0.0f, 50.0f, -120.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 30.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Model loading
            _model = LoadModel("resources/models/obj/plane.obj");
            _texture = LoadTexture("resources/models/obj/plane_diffuse.png");

            // Force Repeat to avoid issue on Web version
            SetTextureWrap(_texture, TextureWrap.Repeat);

            _model.Materials[0].Maps[(int)MaterialMapIndex.Diffuse].Texture = _texture;

            _pitch = 0.0f;
            _roll = 0.0f;
            _yaw = 0.0f;
        }

        public void Update()
        {
            // Update

            // Plane pitch (x-axis) controls
            if (IsKeyDown(KeyboardKey.Down))
            {
                _pitch += 0.6f;
            }
            else if (IsKeyDown(KeyboardKey.Up))
            {
                _pitch -= 0.6f;
            }
            else
            {
                if (_pitch > 0.3f)
                {
                    _pitch -= 0.3f;
                }
                else if (_pitch < -0.3f)
                {
                    _pitch += 0.3f;
                }
            }

            // Plane yaw (y-axis) controls
            if (IsKeyDown(KeyboardKey.S))
            {
                _yaw += 1.0f;
            }
            else if (IsKeyDown(KeyboardKey.A))
            {
                _yaw -= 1.0f;
            }
            else
            {
                if (_yaw > 0.0f)
                {
                    _yaw -= 0.5f;
                }
                else if (_yaw < 0.0f)
                {
                    _yaw += 0.5f;
                }
            }

            // Plane roll (z-axis) controls
            if (IsKeyDown(KeyboardKey.Left))
            {
                _roll += 1.0f;
            }
            else if (IsKeyDown(KeyboardKey.Right))
            {
                _roll -= 1.0f;
            }
            else
            {
                if (_roll > 0.0f)
                {
                    _roll -= 0.5f;
                }
                else if (_roll < 0.0f)
                {
                    _roll += 0.5f;
                }
            }

            // Tranformation matrix for rotations
            _model.Transform = MatrixRotateXYZ(new Vector3(DEG2RAD * _pitch, DEG2RAD * _yaw, DEG2RAD * _roll));

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw 3D model (recomended to draw 3D always before 2D)
            BeginMode3D(_camera);

            // Draw 3d model with texture
            DrawModel(_model, new Vector3(0.0f, -8.0f, 0.0f), 1.0f, Color.White);
            DrawGrid(10, 10.0f);

            EndMode3D();

            // Draw controls info
            DrawRectangle(30, 370, 260, 70, Fade(Color.Green, 0.5f));
            DrawRectangleLines(30, 370, 260, 70, Fade(Color.DarkGreen, 0.5f));
            DrawText("Pitch controlled with: KEY_UP / KEY_DOWN", 40, 380, 10, Color.DarkGray);
            DrawText("Roll controlled with: KEY_LEFT / KEY_RIGHT", 40, 400, 10, Color.DarkGray);
            DrawText("Yaw controlled with: KEY_A / KEY_S", 40, 420, 10, Color.DarkGray);

            DrawText(
                "(c) WWI Plane Model created by GiaHanLam",
                screenWidth - 240,
                screenHeight - 20,
                10,
                Color.DarkGray
            );

            EndDrawing();
        }

        public void Unload()
        {
            UnloadModel(_model);
        }
    }
}
#endif
