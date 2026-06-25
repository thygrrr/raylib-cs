#if BROWSER
using Examples;
using System;

namespace Examples.Models;

public partial class DynamicMesh : IExample
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
        public string Name => "Models / Dynamic Mesh";

        private const int triangleRows = 48;
        private const int vertexRows = triangleRows + 1;

        private Camera3D _camera;
        private Mesh _dynamicMesh;
        private Texture2D _texture;
        private Color[] _pixels;
        private Material _material;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = Vector3.One * 1.5f;
            _camera.Target = _camera.Position + new Vector3(1f, -0.25f, 1f);
            _camera.Up = Vector3.UnitY;
            _camera.FovY = 60.0f;
            _camera.Projection = CameraProjection.Perspective;

            // Generate a dynamic mesh using utils to allocate/access mesh attribute data
            _dynamicMesh = new(vertexRows * vertexRows, triangleRows * triangleRows * 2);
            _dynamicMesh.AllocVertices();
            _dynamicMesh.AllocTexCoords();
            _dynamicMesh.AllocIndices();
            Span<ushort> indices = _dynamicMesh.IndicesAs<ushort>();
            for (int z = 0, i = 0; z < triangleRows; z++)
            {
                for (int x = 0; x < triangleRows; x++, i += 6)
                {
                    indices[i + 0] = (ushort)(x + (z * vertexRows));
                    indices[i + 1] = (ushort)(indices[i] + vertexRows);
                    indices[i + 2] = (ushort)(indices[i] + 1);
                    indices[i + 3] = (ushort)(indices[i] + 1);
                    indices[i + 4] = (ushort)(indices[i] + vertexRows);
                    indices[i + 5] = (ushort)(indices[i] + vertexRows + 1);
                }
            }
            UploadMesh(ref _dynamicMesh, true);

            // Allocate the texture
            Image image = GenImageColor(triangleRows, triangleRows, Color.Blank);
            _texture = LoadTextureFromImage(image);
            _pixels = new Color[_texture.Width * _texture.Height];
            UnloadImage(image);

            // Load the material
            _material = LoadMaterialDefault();
            SetMaterialTexture(ref _material, MaterialMapIndex.Diffuse, _texture);
        }

        public void Update()
        {
            // Update
            float time = (float)GetTime();
            Random random = new(42);

            Span<Vector3> vertices = _dynamicMesh.VerticesAs<Vector3>();
            Span<Vector2> texcoords = _dynamicMesh.TexCoordsAs<Vector2>();

            for (int z = 0, i = 0; z < vertexRows; z++)
            {
                for (int x = 0; x < vertexRows; x++, i++)
                {
                    float noiseX = SmoothNoise(time + random.Next(10000));
                    float noiseZ = SmoothNoise(time + random.Next(10000));
                    vertices[i].X = x + noiseX - .5f;
                    vertices[i].Y = (noiseX + noiseZ) / 2;
                    vertices[i].Z = z + noiseZ - .5f;
                    texcoords[i].X = (x - noiseZ) / triangleRows;
                    texcoords[i].Y = (z - noiseX) / triangleRows;
                }
            }
            UpdateMeshBuffer<Vector3>(_dynamicMesh, Mesh.VboIdIndexVertices, vertices, 0);
            UpdateMeshBuffer<Vector2>(_dynamicMesh, Mesh.VboIdIndexTexCoords, texcoords, 0);

            for (int y = 0, i = 0; y < _texture.Height; y++)
            {
                for (int x = 0; x < _texture.Width; x++, i++)
                {
                    _pixels[i] = new(32, 178, 170, 255);
                    _pixels[i] = ColorBrightness(_pixels[i], (SmoothNoise(time + random.Next(10000)) / 8) - (1 / 16f));
                    _pixels[i] = ColorAlpha(_pixels[i], (triangleRows - new Vector2(x, y).Length()) / triangleRows);
                }
            }
            UpdateTexture(_texture, _pixels);

            // Draw
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);
            DrawMesh(_dynamicMesh, _material, Matrix4x4.Identity);
            EndMode3D();

            EndDrawing();
        }

        public void Unload()
        {
            UnloadMaterial(_material);
            // No need to unload the texture. UnloadMaterial(Material) already unloaded it for us
            UnloadMesh(_dynamicMesh);
        }

        private static float SmoothNoise(float value)
        {
            return ((MathF.Sin(value) + MathF.Cos(value * MathF.E)) / 4) + .5f;
        }
    }
}
#endif
