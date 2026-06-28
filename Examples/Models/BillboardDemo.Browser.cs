#if BROWSER
using Examples;
namespace Examples.Models;

public partial class BillboardDemo : IExample
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
        public string Name => "Models / Billboard Demo";

        private Camera3D _camera;

        // Our billboard texture
        private Texture2D _bill;

        // Position of static / rotating billboard
        private Vector3 _billPositionStatic;
        private Vector3 _billPositionRotating;

        // Entire billboard texture, source is used to take a segment from a larger texture
        private Rectangle _source;

        // NOTE: Billboard locked on axis-Y
        private Vector3 _billUp;

        // Set the height of the rotating billboard to 1.0 with the aspect ratio fixed
        private Vector2 _size;

        // Rotate around origin
        // Here we choose to rotate around the image center
        private Vector2 _origin;

        // Distance is needed for the correct billboard draw order
        // Larger distance (further away from the camera) should be drawn prior to smaller distance
        private float _distanceStatic;
        private float _distanceRotating;

        private float _rotation;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(5.0f, 4.0f, 5.0f);
            _camera.Target = new Vector3(0.0f, 2.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Our billboard texture
            _bill = LoadTexture("resources/billboard.png");

            // Position of static / rotating billboard
            _billPositionStatic = new(0.0f, 2.0f, 0.0f);
            _billPositionRotating = new(1.0f, 2.0f, 1.0f);

            // Entire billboard texture, source is used to take a segment from a larger texture
            _source = new(0.0f, 0.0f, (float)_bill.Width, (float)_bill.Height);

            // NOTE: Billboard locked on axis-Y
            _billUp = new(0.0f, 1.0f, 0.0f);

            // Set the height of the rotating billboard to 1.0 with the aspect ratio fixed
            _size = new(_source.Width / _source.Height, 1.0f);

            // Rotate around origin
            _origin = _size * 0.5f;

            _distanceStatic = 0.0f;
            _distanceRotating = 0.0f;

            _rotation = 0.0f;
        }

        public void Update()
        {
            // Update
            UpdateCamera(ref _camera, CameraMode.Orbital);
            _rotation += 0.4f;
            _distanceStatic = Vector3.Distance(_camera.Position, _billPositionStatic);
            _distanceRotating = Vector3.Distance(_camera.Position, _billPositionRotating);

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawGrid(10, 1.0f);

            // Draw order matters!
            if (_distanceStatic > _distanceRotating)
            {
                DrawBillboard(_camera, _bill, _billPositionStatic, 2.0f, Color.White);
                DrawBillboardPro(_camera, _bill, _source, _billPositionRotating, _billUp, _size, _origin, _rotation, Color.White);
            }
            else
            {
                DrawBillboardPro(_camera, _bill, _source, _billPositionRotating, _billUp, _size, _origin, _rotation, Color.White);
                DrawBillboard(_camera, _bill, _billPositionStatic, 2.0f, Color.White);
            }

            EndMode3D();

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_bill);
        }
    }
}
#endif
