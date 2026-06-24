// Adapted for the browser from Examples/Core/BasicWindow.cs
// raylib [core] example - basic window  (c) Ramon Santamaria (@raysan5), zlib/libpng license

namespace Examples.Web;

public class CoreBasicWindow : IWebExample
{
    public string Name => "Core / Basic Window";

    public void Init()
    {
    }

    public void Update()
    {
        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText("Congrats! You created your first window!", 190, 200, 20, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
    }
}
