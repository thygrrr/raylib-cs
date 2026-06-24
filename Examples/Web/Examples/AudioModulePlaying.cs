// Adapted for the browser from Examples/Audio/ModulePlaying.cs
namespace Examples.Web;

public class AudioModulePlaying : IWebExample
{
    public string Name => "Audio / Module Playing";

    private const int screenWidth = 960;
    private const int screenHeight = 540;
    private const int MaxCircles = 64;

    private struct CircleWave
    {
        public Vector2 Position;
        public float Radius;
        public float Alpha;
        public float Speed;
        public Color Color;
    }

    private CircleWave[] _circles;
    private Music _music;
    private float _pitch;
    private float _timePlayed;
    private bool _pause;

    public void Init()
    {
        InitAudioDevice();

        // Creates some circles for visual effect
        _circles = new CircleWave[MaxCircles];

        for (int i = MaxCircles - 1; i >= 0; i--)
        {
            _circles[i].Alpha = 0.0f;
            _circles[i].Radius = GetRandomValue(10, 40);
            _circles[i].Position.X = GetRandomValue((int)_circles[i].Radius, screenWidth - (int)_circles[i].Radius);
            _circles[i].Position.Y = GetRandomValue((int)_circles[i].Radius, screenHeight - (int)_circles[i].Radius);
            _circles[i].Speed = (float)GetRandomValue(1, 100) / 20000.0f;
            _circles[i].Color = colors[GetRandomValue(0, 13)];
        }

        _music = LoadMusicStream("resources/audio/mini1111.xm");
        _music.Looping = false;
        _pitch = 1.0f;

        PlayMusicStream(_music);

        _timePlayed = 0.0f;
        _pause = false;
    }

    public void Update()
    {
        UpdateMusicStream(_music);        // Update music buffer with new stream data

        // Restart music playing (stop and play)
        if (IsKeyPressed(KeyboardKey.Space))
        {
            StopMusicStream(_music);
            PlayMusicStream(_music);
        }

        // Pause/Resume music playing
        if (IsKeyPressed(KeyboardKey.P))
        {
            _pause = !_pause;

            if (_pause)
            {
                PauseMusicStream(_music);
            }
            else
            {
                ResumeMusicStream(_music);
            }
        }

        if (IsKeyDown(KeyboardKey.Down))
        {
            _pitch -= 0.01f;
        }
        else if (IsKeyDown(KeyboardKey.Up))
        {
            _pitch += 0.01f;
        }

        SetMusicPitch(_music, _pitch);

        // Get timePlayed scaled to bar dimensions
        _timePlayed = GetMusicTimePlayed(_music) / GetMusicTimeLength(_music) * (screenWidth - 40);

        // Color circles animation
        for (int i = MaxCircles - 1; (i >= 0) && !_pause; i--)
        {
            _circles[i].Alpha += _circles[i].Speed;
            _circles[i].Radius += _circles[i].Speed * 10.0f;

            if (_circles[i].Alpha > 1.0f)
            {
                _circles[i].Speed *= -1;
            }

            if (_circles[i].Alpha <= 0.0f)
            {
                _circles[i].Alpha = 0.0f;
                _circles[i].Radius = GetRandomValue(10, 40);
                _circles[i].Position.X = GetRandomValue(
                    (int)_circles[i].Radius,
                    screenWidth - (int)_circles[i].Radius
                );
                _circles[i].Position.Y = GetRandomValue(
                    (int)_circles[i].Radius,
                    screenHeight - (int)_circles[i].Radius
                );
                _circles[i].Color = colors[GetRandomValue(0, 13)];
                _circles[i].Speed = (float)GetRandomValue(1, 100) / 2000.0f;
            }
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        for (int i = MaxCircles - 1; i >= 0; i--)
        {
            DrawCircleV(
                _circles[i].Position,
                _circles[i].Radius,
                ColorAlpha(_circles[i].Color, _circles[i].Alpha)
            );
        }

        // Draw time bar
        DrawRectangle(20, screenHeight - 20 - 12, screenWidth - 40, 12, Color.LightGray);
        DrawRectangle(20, screenHeight - 20 - 12, (int)_timePlayed, 12, Color.Maroon);
        DrawRectangleLines(20, screenHeight - 20 - 12, screenWidth - 40, 12, Color.Gray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadMusicStream(_music);

        CloseAudioDevice();
    }

    private static readonly Color[] colors = new Color[14] {
        Color.Orange,
        Color.Red,
        Color.Gold,
        Color.Lime,
        Color.Blue,
        Color.Violet,
        Color.Brown,
        Color.LightGray,
        Color.Pink,
        Color.Yellow,
        Color.Green,
        Color.SkyBlue,
        Color.Purple,
        Color.Beige
    };
}
