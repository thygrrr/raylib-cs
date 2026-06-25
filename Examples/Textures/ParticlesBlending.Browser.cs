#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ParticlesBlending : IExample
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
        public string Name => "Textures / Particles Blending";

        private const int MaxParticles = 200;
        private const int screenHeight = 450;

        // Particle structure with basic data
        private struct Particle
        {
            public Vector2 Position;
            public Color Color;
            public float Alpha;
            public float Size;
            public float Rotation;
            // NOTE: Use it to activate/deactive particle
            public bool Active;
        }

        private Texture2D _smoke;
        private Particle[] _mouseTail;
        private float _gravity;
        private BlendMode _blending;

        public void Init()
        {
            // Particles pool, reuse them!
            _mouseTail = new Particle[MaxParticles];

            // Initialize particles
            for (int i = 0; i < _mouseTail.Length; i++)
            {
                _mouseTail[i].Position = new Vector2(0, 0);
                _mouseTail[i].Color = new Color(
                    GetRandomValue(0, 255),
                    GetRandomValue(0, 255),
                    GetRandomValue(0, 255),
                    255
                );
                _mouseTail[i].Alpha = 1.0f;
                _mouseTail[i].Size = (float)GetRandomValue(1, 30) / 20.0f;
                _mouseTail[i].Rotation = GetRandomValue(0, 360);
                _mouseTail[i].Active = false;
            }

            _gravity = 3.0f;
            _smoke = LoadTexture("resources/spark_flame.png");
            _blending = BlendMode.Alpha;
        }

        public void Update()
        {
            // Activate one particle every frame and Update active particles
            // NOTE: Particles initial position should be mouse position when activated
            // NOTE: Particles fall down with gravity and rotation... and disappear after 2 seconds (alpha = 0)
            // NOTE: When a particle disappears, active = false and it can be reused.
            for (int i = 0; i < _mouseTail.Length; i++)
            {
                if (!_mouseTail[i].Active)
                {
                    _mouseTail[i].Active = true;
                    _mouseTail[i].Alpha = 1.0f;
                    _mouseTail[i].Position = GetMousePosition();
                    i = _mouseTail.Length;
                }
            }

            for (int i = 0; i < _mouseTail.Length; i++)
            {
                if (_mouseTail[i].Active)
                {
                    _mouseTail[i].Position.Y += _gravity / 2;
                    _mouseTail[i].Alpha -= 0.005f;

                    if (_mouseTail[i].Alpha <= 0.0f)
                    {
                        _mouseTail[i].Active = false;
                    }

                    _mouseTail[i].Rotation += 2.0f;
                }
            }

            if (IsKeyPressed(KeyboardKey.Space))
            {
                if (_blending == BlendMode.Alpha)
                {
                    _blending = BlendMode.Additive;
                }
                else
                {
                    _blending = BlendMode.Alpha;
                }
            }

            BeginDrawing();
            ClearBackground(Color.DarkGray);

            BeginBlendMode(_blending);

            // Draw active particles
            for (int i = 0; i < _mouseTail.Length; i++)
            {
                if (_mouseTail[i].Active)
                {
                    Rectangle source = new(0, 0, _smoke.Width, _smoke.Height);
                    Rectangle dest = new(
                        _mouseTail[i].Position.X,
                        _mouseTail[i].Position.Y,
                        _smoke.Width * _mouseTail[i].Size,
                        _smoke.Height * _mouseTail[i].Size
                    );
                    Vector2 position = new(
                        _smoke.Width * _mouseTail[i].Size / 2,
                        _smoke.Height * _mouseTail[i].Size / 2
                    );
                    Color color = ColorAlpha(_mouseTail[i].Color, _mouseTail[i].Alpha);
                    DrawTexturePro(_smoke, source, dest, position, _mouseTail[i].Rotation, color);
                }
            }

            EndBlendMode();

            DrawText("PRESS SPACE to CHANGE BLENDING MODE", 180, 20, 20, Color.Black);

            if (_blending == BlendMode.Alpha)
            {
                DrawText("ALPHA BLENDING", 290, screenHeight - 40, 20, Color.Black);
            }
            else
            {
                DrawText("ADDITIVE BLENDING", 280, screenHeight - 40, 20, Color.RayWhite);
            }

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_smoke);
        }
    }
}
#endif
