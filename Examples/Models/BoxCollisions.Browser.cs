#if BROWSER
using Examples;
namespace Examples.Models;

public partial class BoxCollisions : IExample
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
        public string Name => "Models / Box Collisions";

        private Camera3D _camera;

        private Vector3 _playerPosition;
        private Vector3 _playerSize;
        private Color _playerColor;

        private Vector3 _enemyBoxPos;
        private Vector3 _enemyBoxSize;

        private Vector3 _enemySpherePos;
        private float _enemySphereSize;

        private bool _collision;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(0.0f, 10.0f, 10.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            _playerPosition = new(0.0f, 1.0f, 2.0f);
            _playerSize = new(1.0f, 2.0f, 1.0f);
            _playerColor = Color.Green;

            _enemyBoxPos = new(-4.0f, 1.0f, 0.0f);
            _enemyBoxSize = new(2.0f, 2.0f, 2.0f);

            _enemySpherePos = new(4.0f, 0.0f, 0.0f);
            _enemySphereSize = 1.5f;

            _collision = false;
        }

        public void Update()
        {
            // Move player
            if (IsKeyDown(KeyboardKey.Right))
            {
                _playerPosition.X += 0.2f;
            }
            else if (IsKeyDown(KeyboardKey.Left))
            {
                _playerPosition.X -= 0.2f;
            }
            else if (IsKeyDown(KeyboardKey.Down))
            {
                _playerPosition.Z += 0.2f;
            }
            else if (IsKeyDown(KeyboardKey.Up))
            {
                _playerPosition.Z -= 0.2f;
            }

            _collision = false;

            // Check collisions player vs enemy-box
            BoundingBox box1 = new(
                _playerPosition - (_playerSize / 2),
                _playerPosition + (_playerSize / 2)
            );
            BoundingBox box2 = new(
                _enemyBoxPos - (_enemyBoxSize / 2),
                _enemyBoxPos + (_enemyBoxSize / 2)
            );

            if (CheckCollisionBoxes(box1, box2))
            {
                _collision = true;
            }

            // Check collisions player vs enemy-sphere
            if (CheckCollisionBoxSphere(box1, _enemySpherePos, _enemySphereSize))
            {
                _collision = true;
            }

            if (_collision)
            {
                _playerColor = Color.Red;
            }
            else
            {
                _playerColor = Color.Green;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            // Draw enemy-box
            DrawCube(_enemyBoxPos, _enemyBoxSize.X, _enemyBoxSize.Y, _enemyBoxSize.Z, Color.Gray);
            DrawCubeWires(_enemyBoxPos, _enemyBoxSize.X, _enemyBoxSize.Y, _enemyBoxSize.Z, Color.DarkGray);

            // Draw enemy-sphere
            DrawSphere(_enemySpherePos, _enemySphereSize, Color.Gray);
            DrawSphereWires(_enemySpherePos, _enemySphereSize, 16, 16, Color.DarkGray);

            // Draw player
            DrawCubeV(_playerPosition, _playerSize, _playerColor);

            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawText("Move player with cursors to collide", 220, 40, 20, Color.Gray);
            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
        }
    }
}
#endif
