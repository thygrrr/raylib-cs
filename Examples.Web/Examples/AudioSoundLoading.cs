// Adapted for the browser from Examples/Audio/SoundLoading.cs
namespace Examples.Web;

public class AudioSoundLoading : IWebExample
{
    public string Name => "Audio / Sound Loading";

    private Sound _fxWav;
    private Sound _fxOgg;

    public void Init()
    {
        InitAudioDevice();

        _fxWav = LoadSound("resources/audio/sound.wav");
        _fxOgg = LoadSound("resources/audio/target.ogg");
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.Space))
        {
            PlaySound(_fxWav);
        }

        if (IsKeyPressed(KeyboardKey.Enter))
        {
            PlaySound(_fxOgg);
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("Press SPACE to PLAY the WAV sound!", 200, 180, 20, Color.LightGray);
        DrawText("Press ENTER to PLAY the OGG sound!", 200, 220, 20, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
        UnloadSound(_fxWav);
        UnloadSound(_fxOgg);
    }
}
