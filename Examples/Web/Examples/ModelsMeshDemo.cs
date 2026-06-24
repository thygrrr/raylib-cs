// Adapted for the browser from Examples/Models/MeshDemo.cs
using System;

namespace Examples.Web;

public unsafe class ModelsMeshDemo : IWebExample
{
    public string Name => "Models / Mesh Demo";

    private Camera3D _camera;

    private Model _model;
    private Texture2D _texture;
    private float _rotationAngle;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = Vector3.One * 1.5f;
        _camera.Target = Vector3.Zero;
        _camera.Up = Vector3.UnitY;
        _camera.FovY = 60.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Generate a mesh using utils to allocate/access mesh attribute data
        Mesh tetrahedron = new(4, 4);
        tetrahedron.AllocVertices();
        tetrahedron.AllocTexCoords();
        tetrahedron.AllocColors();
        tetrahedron.AllocIndices();
        Span<Vector3> vertices = tetrahedron.VerticesAs<Vector3>();
        Span<Vector2> texcoords = tetrahedron.TexCoordsAs<Vector2>();
        Span<Color> colors = tetrahedron.ColorsAs<Color>();
        Span<ushort> indices = tetrahedron.IndicesAs<ushort>();

        // Coordinates for a regular tetrahedron
        vertices[0] = new(MathF.Sqrt(8f / 9f), 0f, -1f / 3f);
        vertices[1] = new(-MathF.Sqrt(2f / 9f), MathF.Sqrt(2f / 3f), -1f / 3f);
        vertices[2] = new(-MathF.Sqrt(2f / 9f), -MathF.Sqrt(2f / 3f), -1f / 3f);
        vertices[3] = Vector3.UnitZ;

        texcoords[0] = Vector2.Zero;
        texcoords[1] = Vector2.UnitX;
        texcoords[2] = Vector2.UnitY;
        texcoords[3] = Vector2.One;

        colors[0] = Color.Pink;
        colors[1] = Color.Lime;
        colors[2] = Color.SkyBlue;
        colors[3] = Color.Violet;

        indices[0] = 2;
        indices[1] = 1;
        indices[2] = 0;

        indices[3] = 1;
        indices[4] = 3;
        indices[5] = 0;

        indices[6] = 2;
        indices[7] = 3;
        indices[8] = 1;

        indices[9] = 0;
        indices[10] = 3;
        indices[11] = 2;

        _rotationAngle = 0f;
        Raylib.UploadMesh(ref tetrahedron, false);
        _model = Raylib.LoadModelFromMesh(tetrahedron);

        Image image = Raylib.GenImagePerlinNoise(16, 16, 0, 0, 1000f);
        Raylib.ImageBlurGaussian(ref image, 2);
        Raylib.ImageColorBrightness(ref image, 100);
        Raylib.ImageDither(ref image, 4, 4, 4, 4);
        _texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);

        Raylib.SetMaterialTexture(ref _model, 0, MaterialMapIndex.Diffuse, ref _texture);
    }

    public void Update()
    {
        // Update
        UpdateCamera(ref _camera, CameraMode.Free);
        _rotationAngle = Raymath.Wrap(_rotationAngle += 1f, 0f, 360f);

        // Draw
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);
        Raylib.DrawModelEx(_model, Vector3.Zero, Vector3.UnitX, _rotationAngle, Vector3.One, Color.White);
        EndMode3D();

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
        UnloadModel(_model);
    }
}
