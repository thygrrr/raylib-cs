#if BROWSER
using Examples;
using static Raylib_cs.Raymath;

namespace Examples.Models;

public partial class MeshPicking : IExample
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
        public string Name => "Models / Mesh Picking";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Camera3D _camera;

        private Model _tower;
        private Texture2D _texture;
        private Vector3 _towerPos;
        private BoundingBox _towerBBox;

        // Ground quad
        private Vector3 _g0;
        private Vector3 _g1;
        private Vector3 _g2;
        private Vector3 _g3;

        // Test triangle
        private Vector3 _ta;
        private Vector3 _tb;
        private Vector3 _tc;

        private Vector3 _bary;

        // Test sphere
        private Vector3 _sp;
        private float _sr;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(20.0f, 20.0f, 20.0f);
            _camera.Target = new Vector3(0.0f, 8.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.6f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            _tower = LoadModel("resources/models/obj/turret.obj");
            _texture = LoadTexture("resources/models/obj/turret_diffuse.png");
            Raylib.SetMaterialTexture(ref _tower, 0, MaterialMapIndex.Albedo, ref _texture);

            _towerPos = new(0.0f, 0.0f, 0.0f);
            _towerBBox = GetMeshBoundingBox(_tower.Meshes[0]);

            // Ground quad
            _g0 = new(-50.0f, 0.0f, -50.0f);
            _g1 = new(-50.0f, 0.0f, 50.0f);
            _g2 = new(50.0f, 0.0f, 50.0f);
            _g3 = new(50.0f, 0.0f, -50.0f);

            // Test triangle
            _ta = new(-25.0f, 0.5f, 0.0f);
            _tb = new(-4.0f, 2.5f, 1.0f);
            _tc = new(-8.0f, 6.5f, 0.0f);

            _bary = new(0.0f, 0.0f, 0.0f);

            // Test sphere
            _sp = new(-30.0f, 5.0f, 5.0f);
            _sr = 4.0f;
        }

        public void Update()
        {
            // Update
            if (IsCursorHidden())
            {
                UpdateCamera(ref _camera, CameraMode.FirstPerson);
            }

            // Toggle camera controls
            if (IsMouseButtonPressed(MouseButton.Right))
            {
                if (IsCursorHidden())
                {
                    EnableCursor();
                }
                else
                {
                    DisableCursor();
                }
            }

            // Display information about closest hit
            RayCollision collision = new();
            string hitObjectName = "None";
            collision.Distance = float.MaxValue;
            collision.Hit = false;
            Color cursorColor = Color.White;

            // Get ray and test against objects
            Ray ray = GetScreenToWorldRay(GetMousePosition(), _camera);

            // Check ray collision against ground quad
            RayCollision groundHitInfo = GetRayCollisionQuad(ray, _g0, _g1, _g2, _g3);
            if (groundHitInfo.Hit && (groundHitInfo.Distance < collision.Distance))
            {
                collision = groundHitInfo;
                cursorColor = Color.Green;
                hitObjectName = "Ground";
            }

            // Check ray collision against test triangle
            RayCollision triHitInfo = GetRayCollisionTriangle(ray, _ta, _tb, _tc);
            if (triHitInfo.Hit && (triHitInfo.Distance < collision.Distance))
            {
                collision = triHitInfo;
                cursorColor = Color.Purple;
                hitObjectName = "Triangle";

                _bary = Vector3Barycenter(collision.Point, _ta, _tb, _tc);
            }

            // Check ray collision against test sphere
            RayCollision sphereHitInfo = GetRayCollisionSphere(ray, _sp, _sr);
            if ((sphereHitInfo.Hit) && (sphereHitInfo.Distance < collision.Distance))
            {
                collision = sphereHitInfo;
                cursorColor = Color.Orange;
                hitObjectName = "Sphere";
            }

            // Check ray collision against bounding box first, before trying the full ray-mesh test
            RayCollision boxHitInfo = GetRayCollisionBox(ray, _towerBBox);
            if (boxHitInfo.Hit && boxHitInfo.Distance < collision.Distance)
            {
                collision = boxHitInfo;
                cursorColor = Color.Orange;
                hitObjectName = "Box";

                // Check ray collision against model meshes
                RayCollision meshHitInfo = new();
                for (int m = 0; m < _tower.MeshCount; m++)
                {
                    // NOTE: We consider the model.Transform for the collision check but
                    // it can be checked against any transform matrix, used when checking against same
                    // model drawn multiple times with multiple transforms
                    meshHitInfo = GetRayCollisionMesh(ray, _tower.Meshes[m], _tower.Transform);
                    if (meshHitInfo.Hit)
                    {
                        // Save the closest hit mesh
                        if ((!collision.Hit) || (collision.Distance > meshHitInfo.Distance))
                        {
                            collision = meshHitInfo;
                        }
                        break;
                    }
                }

                if (meshHitInfo.Hit)
                {
                    collision = meshHitInfo;
                    cursorColor = Color.Orange;
                    hitObjectName = "Mesh";
                }
            }

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            // Draw the tower
            // WARNING: If scale is different than 1.0f,
            // not considered by GetRayCollisionModel()
            DrawModel(_tower, _towerPos, 1.0f, Color.White);

            // Draw the test triangle
            DrawLine3D(_ta, _tb, Color.Purple);
            DrawLine3D(_tb, _tc, Color.Purple);
            DrawLine3D(_tc, _ta, Color.Purple);

            // Draw the test sphere
            DrawSphereWires(_sp, _sr, 8, 8, Color.Purple);

            // Draw the mesh bbox if we hit it
            if (boxHitInfo.Hit)
            {
                DrawBoundingBox(_towerBBox, Color.Lime);
            }

            // If we hit something, draw the cursor at the hit point
            if (collision.Hit)
            {
                DrawCube(collision.Point, 0.3f, 0.3f, 0.3f, cursorColor);
                DrawCubeWires(collision.Point, 0.3f, 0.3f, 0.3f, Color.Red);

                Vector3 normalEnd = collision.Point + collision.Normal;
                DrawLine3D(collision.Point, normalEnd, Color.Red);
            }

            DrawRay(ray, Color.Maroon);

            DrawGrid(10, 10.0f);

            EndMode3D();

            // Draw some debug GUI text
            DrawText($"Hit Object: {hitObjectName}", 10, 50, 10, Color.Black);

            if (collision.Hit)
            {
                int ypos = 70;

                DrawText($"Distance: {collision.Distance}", 10, ypos, 10, Color.Black);

                DrawText($"Hit Pos: {collision.Point}", 10, ypos + 15, 10, Color.Black);

                DrawText($"Hit Norm: {collision.Normal}", 10, ypos + 30, 10, Color.Black);

                if (triHitInfo.Hit && hitObjectName == "Triangle")
                {
                    DrawText($"Barycenter: {_bary}", 10, ypos + 45, 10, Color.Black);
                }
            }

            DrawText("Right click mouse to toggle camera controls", 10, 430, 10, Color.Gray);

            DrawText("(c) Turret 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadModel(_tower);
            UnloadTexture(_texture);
        }
    }
}
#endif
