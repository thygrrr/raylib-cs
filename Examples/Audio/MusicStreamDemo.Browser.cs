#if BROWSER
using Examples;
namespace Examples.Audio;

public partial class MusicStreamDemo : IExample
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
        public string Name => "Audio / Music Stream Demo";

        private Music _music;
        private float _timePlayed;
        private bool _pause;
        private float _pan;
        private float _volume;

        public void Init()
        {
            InitAudioDevice();

            _music = LoadMusicStream("resources/audio/country.mp3");

            PlayMusicStream(_music);

            _timePlayed = 0.0f;
            _pause = false;

            _pan = 0.0f;
            SetMusicPan(_music, _pan);

            _volume = 0.8f;
            SetMusicVolume(_music, _volume);
        }

        public void Update()
        {
            UpdateMusicStream(_music);   // Update music buffer with new stream data

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

            // Set audio pan
            if (IsKeyDown(KeyboardKey.Left))
            {
                _pan -= 0.05f;
                if (_pan < -1.0f)
                {
                    _pan = -1.0f;
                }
                SetMusicPan(_music, _pan);
            }
            else if (IsKeyDown(KeyboardKey.Right))
            {
                _pan += 0.05f;
                if (_pan > 1.0f)
                {
                    _pan = 1.0f;
                }
                SetMusicPan(_music, _pan);
            }

            // Set audio volume
            if (IsKeyDown(KeyboardKey.Down))
            {
                _volume -= 0.05f;
                if (_volume < 0.0f)
                {
                    _volume = 0.0f;
                }
                SetMusicVolume(_music, _volume);
            }
            else if (IsKeyDown(KeyboardKey.Up))
            {
                _volume += 0.05f;
                if (_volume > 1.0f)
                {
                    _volume = 1.0f;
                }
                SetMusicVolume(_music, _volume);
            }

            // Get normalized time played for current music stream
            _timePlayed = GetMusicTimePlayed(_music) / GetMusicTimeLength(_music);

            if (_timePlayed > 1.0f)
            {
                _timePlayed = 1.0f;   // Make sure time played is no longer than music
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("MUSIC SHOULD BE PLAYING!", 255, 150, 20, Color.LightGray);

            DrawText("LEFT-RIGHT for PAN CONTROL", 320, 74, 10, Color.DarkBlue);
            DrawRectangle(300, 100, 200, 12, Color.LightGray);
            DrawRectangleLines(300, 100, 200, 12, Color.Gray);
            DrawRectangle((int)(300 + (_pan + 1.0f) / 2.0f * 200 - 5), 92, 10, 28, Color.DarkGray);

            DrawRectangle(200, 200, 400, 12, Color.LightGray);
            DrawRectangle(200, 200, (int)(_timePlayed * 400.0f), 12, Color.Maroon);
            DrawRectangleLines(200, 200, 400, 12, Color.Gray);

            DrawText("PRESS SPACE TO RESTART MUSIC", 215, 250, 20, Color.LightGray);
            DrawText("PRESS P TO PAUSE/RESUME MUSIC", 208, 280, 20, Color.LightGray);

            DrawText("UP-DOWN for VOLUME CONTROL", 320, 334, 10, Color.DarkGreen);
            DrawRectangle(300, 360, 200, 12, Color.LightGray);
            DrawRectangleLines(300, 360, 200, 12, Color.Gray);
            DrawRectangle((int)(300 + _volume * 200 - 5), 352, 10, 28, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadMusicStream(_music);

            CloseAudioDevice();
        }
    }
}
#endif
