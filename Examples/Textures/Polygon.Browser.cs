#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class Polygon : IExample
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
        public string Name => "Textures / Textured Polygon";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _texture;
        private Vector2[] _texcoords;
        private Vector2[] _points;
        private Vector2[] _positions;
        private float _angle;

        public void Init()
        {
            _texcoords = new[]
            {
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
                new Vector2(0, 0.75f),
                new Vector2(0.25f, 1),
                new Vector2(0.375f, 0.875f),
                new Vector2(0.625f, 0.875f),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.75f),
                new Vector2(1, 0.5f),
                // Close the poly
                new Vector2(0.75f, 0)
            };

            _points = new Vector2[11];

            // Define the base poly vertices from the UV's
            // NOTE: They can be specified in any other way
            for (int i = 0; i < _points.Length; i++)
            {
                _points[i].X = (_texcoords[i].X - 0.5f) * 256.0f;
                _points[i].Y = (_texcoords[i].Y - 0.5f) * 256.0f;
            }

            // Define the vertices drawing position
            // NOTE: Initially same as points but updated every frame
            _positions = new Vector2[_points.Length];
            for (int i = 0; i < _positions.Length; i++)
            {
                _positions[i] = _points[i];
            }

            _texture = LoadTexture("resources/cat.png");
            _angle = 0;
        }

        public void Update()
        {
            _angle += 1;
            for (int i = 0; i < _positions.Length; i++)
            {
                _positions[i] = Raymath.Vector2Rotate(_points[i], _angle * DEG2RAD);
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("Textured Polygon", 20, 20, 20, Color.DarkGray);
            Vector2 center = new(screenWidth / 2, screenHeight / 2);
            DrawTexturePoly(_texture, center, _positions, _texcoords, _positions.Length, Color.White);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
        }

        // Draw textured polygon, defined by vertex and texture coordinates
        // NOTE: Polygon center must have straight line path to all points
        // without crossing perimeter, points must be in anticlockwise order
        private static void DrawTexturePoly(
            Texture2D texture,
            Vector2 center,
            Vector2[] points,
            Vector2[] texcoords,
            int pointCount,
            Color tint
        )
        {
            Rlgl.SetTexture(texture.Id);

            // Texturing is only supported on RL_QUADS
            Rlgl.Begin(DrawMode.Quads);

            Rlgl.Color4ub(tint.R, tint.G, tint.B, tint.A);

            for (int i = 0; i < pointCount - 1; i++)
            {
                Rlgl.TexCoord2f(0.5f, 0.5f);
                Rlgl.Vertex2f(center.X, center.Y);

                Rlgl.TexCoord2f(texcoords[i].X, texcoords[i].Y);
                Rlgl.Vertex2f(points[i].X + center.X, points[i].Y + center.Y);

                Rlgl.TexCoord2f(texcoords[i + 1].X, texcoords[i + 1].Y);
                Rlgl.Vertex2f(points[i + 1].X + center.X, points[i + 1].Y + center.Y);

                Rlgl.TexCoord2f(texcoords[i + 1].X, texcoords[i + 1].Y);
                Rlgl.Vertex2f(points[i + 1].X + center.X, points[i + 1].Y + center.Y);
            }
            Rlgl.End();

            Rlgl.SetTexture(0);
        }
    }
}
#endif
