// Adapted for the browser from Examples/Text/FormatText.cs
namespace Examples.Web;

public class TextFormatText : IWebExample
{
    public string Name => "Text / Format Text";

    private const int Score = 100020;
    private const int HiScore = 200450;
    private const int Lives = 5;

    public void Init()
    {
    }

    public void Update()
    {
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText($"Score: {Score}", 200, 80, 20, Color.Red);
        DrawText($"HiScore: {HiScore}", 200, 120, 20, Color.Green);
        DrawText($"Lives: {Lives}", 200, 160, 40, Color.Blue);
        DrawText($"Elapsed Time: {GetFrameTime() * 1000} ms", 200, 220, 20, Color.Black);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
