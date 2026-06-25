using System;
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
    public const int screenWidth = 800;
    public const int screenHeight = 450;

    // Base examples implementing IExample.
    private static readonly List<IExample> Examples = new()
    {
        // Core
        new global::Examples.Core.BasicScreenManager(),
        new global::Examples.Core.BasicWindow(),
        new global::Examples.Core.Camera2dDemo(),
        new global::Examples.Core.Camera2dPlatformer(),
        new global::Examples.Core.Camera3dFirstPerson(),
        new global::Examples.Core.Camera3dFree(),
        new global::Examples.Core.Camera3dMode(),
        new global::Examples.Core.CustomLogging(),
        new global::Examples.Core.DeltaTime(),
        new global::Examples.Core.InputGamepad(),
        new global::Examples.Core.InputGestures(),
        new global::Examples.Core.InputGesturesTestBed(),
        new global::Examples.Core.InputKeys(),
        new global::Examples.Core.InputMouse(),
        new global::Examples.Core.InputMouseWheel(),
        new global::Examples.Core.InputMultitouch(),
        new global::Examples.Core.InputVirtualControls(),
        new global::Examples.Core.Picking3d(),
        new global::Examples.Core.RandomValues(),
        new global::Examples.Core.ScissorTest(),
        new global::Examples.Core.SmoothPixelPerfect(),
        new global::Examples.Core.SplitScreen(),
        new global::Examples.Core.StorageValues(),
        new global::Examples.Core.VrSimulator(),
        new global::Examples.Core.WindowFlags(),
        new global::Examples.Core.WindowLetterbox(),
        new global::Examples.Core.WorldScreen(),

        // Shapes
        new global::Examples.Shapes.BasicShapes(),
        new global::Examples.Shapes.BouncingBall(),
        new global::Examples.Shapes.CollisionArea(),
        new global::Examples.Shapes.ColorsPalette(),
        new global::Examples.Shapes.DrawCircleSector(),
        new global::Examples.Shapes.DrawRectangleRounded(),
        new global::Examples.Shapes.DrawRing(),
        new global::Examples.Shapes.EasingsBallAnim(),
        new global::Examples.Shapes.EasingsBoxAnim(),
        new global::Examples.Shapes.EasingsRectangleArray(),
        new global::Examples.Shapes.FollowingEyes(),
        new global::Examples.Shapes.LinesBezier(),
        new global::Examples.Shapes.LogoRaylibAnim(),
        new global::Examples.Shapes.LogoRaylibShape(),
        new global::Examples.Shapes.RectangleScaling(),

        // Models
        new global::Examples.Models.BillboardDemo(),
        new global::Examples.Models.BoxCollisions(),
        new global::Examples.Models.CubicmapDemo(),
        new global::Examples.Models.DynamicMesh(),
        new global::Examples.Models.FirstPersonMaze(),
        new global::Examples.Models.GeometricShapes(),
        new global::Examples.Models.HeightmapDemo(),
        new global::Examples.Models.LoadingGltf(),
        new global::Examples.Models.LoadingIqm(),
        new global::Examples.Models.MeshDemo(),
        new global::Examples.Models.MeshGeneration(),
        new global::Examples.Models.MeshPicking(),
        new global::Examples.Models.ModelCubeTexture(),
        new global::Examples.Models.ModelLoading(),
        new global::Examples.Models.OrthographicProjection(),
        new global::Examples.Models.SolarSystem(),
        new global::Examples.Models.WavingCubes(),
        new global::Examples.Models.YawPitchRoll(),

        // Textures
        new global::Examples.Textures.BackgroundScrolling(),
        new global::Examples.Textures.BlendModes(),
        new global::Examples.Textures.Bunnymark(),
        new global::Examples.Textures.TexturedCurve(),
        new global::Examples.Textures.DrawTiled(),
        new global::Examples.Textures.ImageDrawing(),
        new global::Examples.Textures.ImageGeneration(),
        new global::Examples.Textures.ImageLoading(),
        new global::Examples.Textures.ImageProcessing(),
        new global::Examples.Textures.ImageText(),
        new global::Examples.Textures.LogoRaylibTexture(),
        new global::Examples.Textures.MousePainting(),
        new global::Examples.Textures.NpatchDrawing(),
        new global::Examples.Textures.ParticlesBlending(),
        new global::Examples.Textures.Polygon(),
        new global::Examples.Textures.RawData(),
        new global::Examples.Textures.SpriteAnim(),
        new global::Examples.Textures.SpriteButton(),
        new global::Examples.Textures.SpriteExplosion(),
        new global::Examples.Textures.SrcRecDstRec(),
        new global::Examples.Textures.ToImage(),

        // Text
        new global::Examples.Text.CodepointsLoading(),
        new global::Examples.Text.FontFilters(),
        new global::Examples.Text.FontLoading(),
        new global::Examples.Text.FontSdf(),
        new global::Examples.Text.FontSpritefont(),
        new global::Examples.Text.FormatText(),
        new global::Examples.Text.InputBox(),
        new global::Examples.Text.RaylibFonts(),
        new global::Examples.Text.RectangleBounds(),
        new global::Examples.Text.WritingAnim(),

        // Audio
        new global::Examples.Audio.ModulePlaying(),
        new global::Examples.Audio.MusicStreamDemo(),
        new global::Examples.Audio.SoundLoading(),

        // Shaders (retargeted to glsl100 for WebGL; some advanced ones may not render on WebGL1)
        new global::Examples.Shaders.BasicLighting(),
        new global::Examples.Shaders.BasicPbr(),
        new global::Examples.Shaders.CustomUniform(),
        new global::Examples.Shaders.Eratosthenes(),
        new global::Examples.Shaders.Fog(),
        new global::Examples.Shaders.HotReloading(),
        new global::Examples.Shaders.HybridRender(),
        new global::Examples.Shaders.JuliaSet(),
        new global::Examples.Shaders.MeshInstancing(),
        new global::Examples.Shaders.ModelShader(),
        new global::Examples.Shaders.MultiSample2d(),
        new global::Examples.Shaders.PaletteSwitch(),
        new global::Examples.Shaders.PostProcessing(),
        new global::Examples.Shaders.Raymarching(),
        new global::Examples.Shaders.ShapesTextures(),
        new global::Examples.Shaders.SimpleMask(),
        new global::Examples.Shaders.Spotlight(),
        new global::Examples.Shaders.TextureDrawing(),
        new global::Examples.Shaders.TextureOutline(),
        new global::Examples.Shaders.TextureWaves(),
        new global::Examples.Shaders.WriteDepth(),
    };

    private static IExample _current;

    public static void Main()
    {
        InitWindow(screenWidth, screenHeight, "raylib-cs web examples");
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
