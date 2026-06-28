#if BROWSER
using Examples;
namespace Examples.Models;

public partial class MeshGeneration : IExample
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
        public string Name => "Models / Mesh Generation";

        private Camera3D _camera;

        private Texture2D _texture;
        private Model[] _models;
        private Vector3 _position;
        private int _currentModel;

        public void Init()
        {
            // We generate a checked image for texturing
            Image isChecked = GenImageChecked(2, 2, 1, 1, Color.Red, Color.Green);
            _texture = LoadTextureFromImage(isChecked);
            UnloadImage(isChecked);

            _models = new Model[9];

            _models[0] = LoadModelFromMesh(GenMeshPlane(2, 2, 4, 3));
            _models[1] = LoadModelFromMesh(GenMeshCube(2.0f, 1.0f, 2.0f));
            _models[2] = LoadModelFromMesh(GenMeshSphere(2, 32, 32));
            _models[3] = LoadModelFromMesh(GenMeshHemiSphere(2, 16, 16));
            _models[4] = LoadModelFromMesh(GenMeshCylinder(1, 2, 16));
            _models[5] = LoadModelFromMesh(GenMeshTorus(0.25f, 4.0f, 16, 32));
            _models[6] = LoadModelFromMesh(GenMeshKnot(1.0f, 2.0f, 16, 128));
            _models[7] = LoadModelFromMesh(GenMeshPoly(5, 2.0f));
            _models[8] = LoadModelFromMesh(GenMeshCustom());

            // NOTE: Generated meshes could be exported using ExportMesh()

            // Set checked texture as default diffuse component for all models material
            for (int i = 0; i < _models.Length; i++)
            {
                // Set map diffuse texture
                Raylib.SetMaterialTexture(ref _models[i], 0, MaterialMapIndex.Albedo, ref _texture);
            }

            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(5.0f, 5.0f, 5.0f);
            _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Model drawing position
            _position = new(0.0f, 0.0f, 0.0f);

            _currentModel = 0;
        }

        public void Update()
        {
            // Update
            UpdateCamera(ref _camera, CameraMode.Orbital);

            if (IsMouseButtonPressed(MouseButton.Left))
            {
                _currentModel = (_currentModel + 1) % _models.Length; // Cycle between the textures
            }

            if (IsKeyPressed(KeyboardKey.Right))
            {
                _currentModel++;
                if (_currentModel >= _models.Length)
                {
                    _currentModel = 0;
                }
            }
            else if (IsKeyPressed(KeyboardKey.Left))
            {
                _currentModel--;
                if (_currentModel < 0)
                {
                    _currentModel = _models.Length - 1;
                }
            }

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawModel(_models[_currentModel], _position, 1.0f, Color.White);
            DrawGrid(10, 1.0f);

            EndMode3D();

            DrawRectangle(30, 400, 310, 30, Fade(Color.SkyBlue, 0.5f));
            DrawRectangleLines(30, 400, 310, 30, Fade(Color.DarkBlue, 0.5f));
            DrawText("MOUSE LEFT BUTTON to CYCLE PROCEDURAL MODELS", 40, 410, 10, Color.Blue);

            switch (_currentModel)
            {
                case 0:
                    DrawText("PLANE", 680, 10, 20, Color.DarkBlue);
                    break;
                case 1:
                    DrawText("CUBE", 680, 10, 20, Color.DarkBlue);
                    break;
                case 2:
                    DrawText("SPHERE", 680, 10, 20, Color.DarkBlue);
                    break;
                case 3:
                    DrawText("HEMISPHERE", 640, 10, 20, Color.DarkBlue);
                    break;
                case 4:
                    DrawText("CYLINDER", 680, 10, 20, Color.DarkBlue);
                    break;
                case 5:
                    DrawText("TORUS", 680, 10, 20, Color.DarkBlue);
                    break;
                case 6:
                    DrawText("KNOT", 680, 10, 20, Color.DarkBlue);
                    break;
                case 7:
                    DrawText("POLY", 680, 10, 20, Color.DarkBlue);
                    break;
                case 8:
                    DrawText("Custom (triangle)", 580, 10, 20, Color.DarkBlue);
                    break;
                default:
                    break;
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);

            for (int i = 0; i < _models.Length; i++)
            {
                UnloadModel(_models[i]);
            }
        }

        // Generate a simple triangle mesh from code
        private static Mesh GenMeshCustom()
        {
            Mesh mesh = new(3, 1);
            mesh.AllocVertices();
            mesh.AllocTexCoords();
            mesh.AllocNormals();
            Span<Vector3> vertices = mesh.VerticesAs<Vector3>();
            Span<Vector2> texcoords = mesh.TexCoordsAs<Vector2>();
            Span<Vector3> normals = mesh.NormalsAs<Vector3>();

            // Vertex at (0, 0, 0)
            vertices[0] = new(0, 0, 0);
            normals[0] = new(0, 1, 0);
            texcoords[0] = new(0, 0);

            // Vertex at (1, 0, 2)
            vertices[1] = new(1, 0, 2);
            normals[1] = new(0, 1, 0);
            texcoords[1] = new(0.5f, 1.0f);

            // Vertex at (2, 0, 0)
            vertices[2] = new(2, 0, 0);
            normals[2] = new(0, 1, 0);
            texcoords[2] = new(1, 0);

            // Upload mesh data from CPU (RAM) to GPU (VRAM) memory
            UploadMesh(ref mesh, false);

            return mesh;
        }
    }
}
#endif
