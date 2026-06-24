using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;

namespace Examples.Web;

/// <summary>
/// Browser host: owns the single raylib window and the "current" example. The frame loop is
/// driven from JavaScript (main.js, via requestAnimationFrame) since a blocking C# loop would
/// freeze the page. Methods marked [JSExport] are called from main.js.
/// </summary>
public partial class Host
{
    public const int ScreenWidth = 960;
    public const int ScreenHeight = 540;

    // Adapted examples. Add more here as they are converted to IWebExample.
    private static readonly List<IWebExample> Examples = new()
    {
        new CoreBasicWindow(),
        new CoreInputKeys(),
        new CoreInputMouse(),
        new CoreInputMouseWheel(),
        new ShapesBasicShapes(),
        new ShapesBouncingBall(),
        new ShapesFollowingEyes(),
        new TextWritingAnim(),
        new TextFormatText(),
    };

    private static IWebExample _current;

    public static void Main()
    {
        InitWindow(ScreenWidth, ScreenHeight, "raylib-cs web examples");
        SetTargetFPS(60);

        _current = Examples[0];
        _current.Init();
    }

    /// <summary>Render one frame of the current example (called every requestAnimationFrame tick).</summary>
    [JSExport]
    public static void UpdateFrame()
    {
        _current?.Update();
    }

    /// <summary>Switch the active example by name (called from the nav dropdown).</summary>
    [JSExport]
    public static void SetExample(string name)
    {
        var next = Examples.Find(e => e.Name == name);
        if (next == null || next == _current)
        {
            return;
        }

        _current?.Unload();
        _current = next;
        _current.Init();
    }

    /// <summary>Newline-separated example names, used to populate the nav dropdown.</summary>
    [JSExport]
    public static string GetExampleNames()
    {
        return string.Join("\n", Examples.ConvertAll(e => e.Name));
    }
}
