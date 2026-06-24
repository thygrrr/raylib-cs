// Adapted for the browser from Examples/Core/RandomValues.cs
namespace Examples.Web;

public class CoreRandomValues : IWebExample
{
    public string Name => "Core / Random Values";

    private int _framesCounter;
    private int _randValue;

    public void Init()
    {
        // Variable used to count frames
        _framesCounter = 0;

        // Get a random integer number between -8 and 5 (both included)
        _randValue = GetRandomValue(-8, 5);
    }

    public void Update()
    {
        _framesCounter++;

        // Every two seconds (120 frames) a new random value is generated
        if (((_framesCounter / 120) % 2) == 1)
        {
            _randValue = GetRandomValue(-8, 5);
            _framesCounter = 0;
        }

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("Every 2 seconds a new random value is generated:", 130, 100, 20, Color.Maroon);

        DrawText($"{_randValue}", 360, 180, 80, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
