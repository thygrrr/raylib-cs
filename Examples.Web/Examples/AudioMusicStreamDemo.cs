// Adapted for the browser from Examples/Audio/MusicStreamDemo.cs
namespace Examples.Web;

public class AudioMusicStreamDemo : IWebExample
{
    public string Name => "Audio / Music Stream Demo";

    private Music _music;
    private float _timePlayed;
    private bool _pause;

    public void Init()
    {
        InitAudioDevice();

        _music = LoadMusicStream("resources/audio/country.mp3");
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

        // Get timePlayed scaled to bar dimensions (400 pixels)
        _timePlayed = GetMusicTimePlayed(_music) / GetMusicTimeLength(_music) * 400;

        if (_timePlayed > 400)
        {
            StopMusicStream(_music);
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("MUSIC SHOULD BE PLAYING!", 255, 150, 20, Color.LightGray);

        DrawRectangle(200, 200, 400, 12, Color.LightGray);
        DrawRectangle(200, 200, (int)_timePlayed, 12, Color.Maroon);
        DrawRectangleLines(200, 200, 400, 12, Color.Gray);

        DrawText("PRESS SPACE TO RESTART MUSIC", 215, 250, 20, Color.LightGray);
        DrawText("PRESS P TO PAUSE/RESUME MUSIC", 208, 280, 20, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadMusicStream(_music);

        CloseAudioDevice();
    }
}
