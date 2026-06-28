#if BROWSER
using Examples;
using System;

namespace Examples.Textures;

public partial class Bunnymark : IExample
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
        public string Name => "Textures / Bunnymark";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        // limits
        private const int MaxBunnies = 500_000;
        private const int BunnyIncrement = 500;
        private const int BunnyDecrement = 2_500;

        private const int TARGET_FPS = 60;

        // This is the maximum amount of elements (quads) per batch
        private const int MAX_BATCH_ELEMENTS = Rlgl.DEFAULT_BATCH_BUFFER_ELEMENTS;

        private record struct Bunny()
        {
            public Vector2 Position { get; set; } = GetMousePosition();
            public Vector2 Speed
            {
                get; set;
            } = new(
                GetRandomValue(-250, 250) / (float)TARGET_FPS,
                GetRandomValue(-250, 250) / (float)TARGET_FPS);
            public Color Color
            {
                get;
            } = new(
                GetRandomValue(50, 240),
                GetRandomValue(80, 240),
                GetRandomValue(100, 240), 255);
        }

        private Texture2D _texBunny;
        private Vector2 _halfSize;
        private Bunny[] _bunnies;
        private int _bunniesCount;

        public void Init()
        {
            // Load bunny texture
            _texBunny = LoadTexture("resources/wabbit_alpha.png");
            _halfSize = new Vector2(_texBunny.Width, _texBunny.Height) / 2;

            // Initialize bunnies storage
            _bunnies = new Bunny[MaxBunnies];
            _bunniesCount = 0;
        }

        public void Update()
        {
            Span<Bunny> bunnies = _bunnies;

            if (IsMouseButtonDown(MouseButton.Left) && _bunniesCount < MaxBunnies)
            {
                // Add a range of new bunnies
                foreach (ref var bunny in bunnies[_bunniesCount..(_bunniesCount + BunnyIncrement)])
                {
                    bunny = new();
                }
                _bunniesCount += BunnyIncrement;
            }
            else if (IsMouseButtonDown(MouseButton.Right))
            {
                // Remove the oldest bunnies, shifting them back in the span
                if (_bunniesCount > BunnyDecrement)
                {
                    bunnies[BunnyDecrement.._bunniesCount].CopyTo(bunnies);
                }
                _bunniesCount = Math.Max(0, _bunniesCount - BunnyDecrement);
            }

            // Update bunnies
            foreach (ref var bunny in bunnies[.._bunniesCount])
            {
                // Integrate position
                bunny.Position += bunny.Speed;

                // Bounce bunnies off the screen borders
                bunny.Speed *= (bunny.Position + _halfSize) switch
                {
                    { X: < 0 or > screenWidth, Y: < 40 or > screenHeight } => new(-1, -1),
                    { X: < 0 or > screenWidth } => new(-1, 1),
                    { Y: < 40 or > screenHeight } => new(1, -1),
                    _ => Vector2.One,
                };
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            foreach (var bunny in bunnies[.._bunniesCount])
            {
                // NOTE: When internal batch buffer limit is reached (MAX_BATCH_ELEMENTS),
                // a draw call is launched and buffer starts being filled again;
                // before issuing a draw call, updated vertex data from internal CPU buffer is send to GPU...
                // Process of sending data is costly, and it could happen that GPU data has not been completely
                // processed for drawing while new data is tried to be sent (updating current in-use buffers)
                // it could generate a stall and consequently a frame drop, limiting the number of drawn bunnies
                DrawTexture(_texBunny, (int)bunny.Position.X, (int)bunny.Position.Y, bunny.Color);
            }

            DrawRectangle(0, 0, screenWidth, 40, Color.Black);
            DrawText($"bunnies: {_bunniesCount}", 120, 10, 20, Color.Green);
            DrawText($"batched draw calls: {1 + _bunniesCount / MAX_BATCH_ELEMENTS}", 320, 10, 20, Color.Maroon);
            DrawText("Left Mouse: Add Bunnies!!! :D", 10, 400, 20, Color.LightGray);
            DrawText("Right Mouse: Remove Bunnies", 10, 420, 20, Color.LightGray);

            DrawFPS(10, 10);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texBunny);
            _bunnies = null;
            _bunniesCount = 0;
        }
    }
}
#endif
