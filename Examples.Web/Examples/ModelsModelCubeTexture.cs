// Adapted for the browser from Examples/Models/ModelCubeTexture.cs
namespace Examples.Web;

public class ModelsModelCubeTexture : IWebExample
{
    public string Name => "Models / Model Cube Texture";

    private Camera3D _camera;

    private Texture2D _texture;

    public void Init()
    {
        // Define the camera to look into our 3d world
        _camera = new();
        _camera.Position = new Vector3(0.0f, 10.0f, 10.0f);
        _camera.Target = new Vector3(0.0f, 0.0f, 0.0f);
        _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
        _camera.FovY = 45.0f;
        _camera.Projection = CameraProjection.Perspective;

        // Load texture to be applied to the cubes sides
        _texture = LoadTexture("resources/cubicmap_atlas.png");
    }

    public void Update()
    {
        // Draw
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        BeginMode3D(_camera);

        // Draw cube with an applied texture
        DrawCubeTexture(_texture, new Vector3(-2.0f, 2.0f, 0.0f), 2.0f, 4.0f, 2.0f, Color.White);

        // Draw cube with an applied texture, but only a defined rectangle piece of the texture
        DrawCubeTextureRec(
            _texture,
            new Rectangle(0, _texture.Height / 2, _texture.Width / 2, _texture.Height / 2),
            new Vector3(2.0f, 1.0f, 0.0f),
            2.0f,
            2.0f,
            2.0f,
            Color.White
        );

        DrawGrid(10, 1.0f);

        EndMode3D();

        EndDrawing();
    }

    public void Unload()
    {
        UnloadTexture(_texture);
    }

    // Draw cube textured
    // NOTE: Cube position is the center position
    private static void DrawCubeTexture(
        Texture2D texture,
        Vector3 position,
        float width,
        float height,
        float length,
        Color color
    )
    {
        float x = position.X;
        float y = position.Y;
        float z = position.Z;

        // Set desired texture to be enabled while drawing following vertex data
        Rlgl.SetTexture(texture.Id);

        Rlgl.Begin(DrawMode.Quads);
        Rlgl.Color4ub(color.R, color.G, color.B, color.A);

        // Front Face
        // Normal Pointing Towards Viewer
        Rlgl.Normal3f(0.0f, 0.0f, 1.0f);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);

        // Back Face
        // Normal Pointing Away From Viewer
        Rlgl.Normal3f(0.0f, 0.0f, -1.0f);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);

        // Top Face
        // Normal Pointing Up
        Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);

        // Bottom Face
        // Normal Pointing Down
        Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);

        // Right face
        // Normal Pointing Right
        Rlgl.Normal3f(1.0f, 0.0f, 0.0f);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);

        // Left Face
        // Normal Pointing Left
        Rlgl.Normal3f(-1.0f, 0.0f, 0.0f);
        Rlgl.TexCoord2f(0.0f, 0.0f);
        // Bottom Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(1.0f, 0.0f);
        // Bottom Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f(1.0f, 1.0f);
        // Top Right Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(0.0f, 1.0f);
        // Top Left Of The Texture and Quad
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        Rlgl.End();

        Rlgl.SetTexture(0);
    }

    // Draw cube with texture piece applied to all faces
    private static void DrawCubeTextureRec(
        Texture2D texture,
        Rectangle source,
        Vector3 position,
        float width,
        float height,
        float length,
        Color color
    )
    {
        float x = position.X;
        float y = position.Y;
        float z = position.Z;
        float texWidth = (float)texture.Width;
        float texHeight = (float)texture.Height;

        // Set desired texture to be enabled while drawing following vertex data
        Rlgl.SetTexture(texture.Id);

        // We calculate the normalized texture coordinates for the desired texture-source-rectangle
        // It means converting from (tex.Width, tex.Height) coordinates to [0.0f, 1.0f] equivalent
        Rlgl.Begin(DrawMode.Quads);
        Rlgl.Color4ub(color.R, color.G, color.B, color.A);

        // Front face
        Rlgl.Normal3f(0.0f, 0.0f, 1.0f);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);

        // Back face
        Rlgl.Normal3f(0.0f, 0.0f, -1.0f);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);

        // Top face
        Rlgl.Normal3f(0.0f, 1.0f, 0.0f);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);

        // Bottom face
        Rlgl.Normal3f(0.0f, -1.0f, 0.0f);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);

        // Right face
        Rlgl.Normal3f(1.0f, 0.0f, 0.0f);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z - length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x + width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x + width / 2, y - height / 2, z + length / 2);

        // Left face
        Rlgl.Normal3f(-1.0f, 0.0f, 0.0f);
        Rlgl.TexCoord2f(source.X / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z - length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, (source.Y + source.Height) / texHeight);
        Rlgl.Vertex3f(x - width / 2, y - height / 2, z + length / 2);
        Rlgl.TexCoord2f((source.X + source.Width) / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z + length / 2);
        Rlgl.TexCoord2f(source.X / texWidth, source.Y / texHeight);
        Rlgl.Vertex3f(x - width / 2, y + height / 2, z - length / 2);

        Rlgl.End();

        Rlgl.SetTexture(0);
    }
}
