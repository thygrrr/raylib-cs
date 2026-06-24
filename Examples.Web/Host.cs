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

    // Adapted examples (asset-free). Add more here as they are converted to IWebExample.
    private static readonly List<IWebExample> Examples = new()
    {
        // Core
        new CoreBasicWindow(),
        new CoreBasicScreenManager(),
        new CoreCamera2dDemo(),
        new CoreCamera2dPlatformer(),
        new CoreCamera3dMode(),
        new CoreCamera3dFree(),
        new CoreCamera3dFirstPerson(),
        new CorePicking3d(),
        new CoreWorldScreen(),
        new CoreDeltaTime(),
        new CoreRandomValues(),
        new CoreScissorTest(),
        new CoreStorageValues(),
        new CoreWindowFlags(),
        new CoreCustomLogging(),
        new CoreInputKeys(),
        new CoreInputMouse(),
        new CoreInputMouseWheel(),
        new CoreInputMultitouch(),
        new CoreInputGestures(),
        new CoreInputGesturesTestBed(),
        new CoreInputVirtualControls(),

        // Shapes
        new ShapesBasicShapes(),
        new ShapesBouncingBall(),
        new ShapesFollowingEyes(),
        new ShapesCollisionArea(),
        new ShapesColorsPalette(),
        new ShapesDrawCircleSector(),
        new ShapesDrawRectangleRounded(),
        new ShapesDrawRing(),
        new ShapesEasingsBallAnim(),
        new ShapesEasingsBoxAnim(),
        new ShapesEasingsRectangleArray(),
        new ShapesLinesBezier(),
        new ShapesLogoRaylibAnim(),
        new ShapesLogoRaylibShape(),
        new ShapesRectangleScaling(),

        // Models (procedural 3D, no assets)
        new ModelsBoxCollisions(),
        new ModelsGeometricShapes(),
        new ModelsOrthographicProjection(),
        new ModelsSolarSystem(),
        new ModelsWavingCubes(),

        // Text
        new TextFormatText(),
        new TextWritingAnim(),
        new TextInputBox(),
        new TextRectangleBounds(),
    };

    private static IWebExample _current;

    public static void Main()
    {
        InitWindow(ScreenWidth, ScreenHeight, "raylib-cs web examples");
        SetTargetFPS(60);

        SetExample(Examples[0].Name);
    }

    /// <summary>Render one frame of the current example (called every requestAnimationFrame tick).</summary>
    [JSExport]
    public static void UpdateFrame()
    {
        try
        {
            _current?.Update();
        }
        catch (Exception ex)
        {
            // Don't let one misbehaving example kill the whole page; log and stop driving it.
            Console.WriteLine($"[Examples.Web] '{_current?.Name}' threw during Update: {ex}");
            _current = null;
        }
    }

    /// <summary>Switch the active example by name (called from the nav dropdown).</summary>
    [JSExport]
    public static void SetExample(string name)
    {
        var next = Examples.Find(e => e.Name == name);
        if (next == null)
        {
            return;
        }

        try
        {
            _current?.Unload();
            _current = next;
            _current.Init();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Examples.Web] '{next.Name}' failed to initialize: {ex}");
            _current = null;
        }
    }

    /// <summary>Newline-separated example names, used to populate the nav dropdown.</summary>
    [JSExport]
    public static string GetExampleNames()
    {
        return string.Join("\n", Examples.ConvertAll(e => e.Name));
    }
}
