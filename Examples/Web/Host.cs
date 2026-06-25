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

    // Adapted examples. Add more here as they are converted to IWebExample.
    private static readonly List<IWebExample> Examples = new()
    {
        // Core
        new CoreBasicScreenManager(),
        new CoreBasicWindow(),
        new CoreCamera2dDemo(),
        new CoreCamera2dPlatformer(),
        new CoreCamera3dFirstPerson(),
        new CoreCamera3dFree(),
        new CoreCamera3dMode(),
        new CoreCustomLogging(),
        new CoreDeltaTime(),
        new CoreInputGamepad(),
        new CoreInputGestures(),
        new CoreInputGesturesTestBed(),
        new CoreInputKeys(),
        new CoreInputMouse(),
        new CoreInputMouseWheel(),
        new CoreInputMultitouch(),
        new CoreInputVirtualControls(),
        new CorePicking3d(),
        new CoreRandomValues(),
        new CoreScissorTest(),
        new CoreSmoothPixelperfect(),
        new CoreSplitScreen(),
        new CoreStorageValues(),
        new CoreVrSimulator(),
        new CoreWindowFlags(),
        new CoreWindowLetterbox(),
        new CoreWorldScreen(),

        // Shapes
        new ShapesBasicShapes(),
        new ShapesBouncingBall(),
        new ShapesCollisionArea(),
        new ShapesColorsPalette(),
        new ShapesDrawCircleSector(),
        new ShapesDrawRectangleRounded(),
        new ShapesDrawRing(),
        new ShapesEasingsBallAnim(),
        new ShapesEasingsBoxAnim(),
        new ShapesEasingsRectangleArray(),
        new ShapesFollowingEyes(),
        new ShapesLinesBezier(),
        new ShapesLogoRaylibAnim(),
        new ShapesLogoRaylibShape(),
        new ShapesRectangleScaling(),

        // Models
        new ModelsBillboardDemo(),
        new ModelsBoxCollisions(),
        new ModelsCubicmapDemo(),
        new ModelsDynamicMesh(),
        new ModelsFirstPersonMaze(),
        new ModelsGeometricShapes(),
        new ModelsHeightmapDemo(),
        new ModelsLoadingGltf(),
        new ModelsLoadingIqm(),
        new ModelsMeshDemo(),
        new ModelsMeshGeneration(),
        new ModelsMeshPicking(),
        new ModelsModelCubeTexture(),
        new ModelsModelLoading(),
        new ModelsOrthographicProjection(),
        new ModelsSolarSystem(),
        new ModelsWavingCubes(),
        new ModelsYawPitchRoll(),

        // Textures
        new TexturesBackgroundScrolling(),
        new TexturesBlendModes(),
        new TexturesBunnymark(),
        new TexturesCurvePoint(),
        new TexturesDrawTiled(),
        new TexturesImageDrawing(),
        new TexturesImageGeneration(),
        new TexturesImageLoading(),
        new TexturesImageProcessing(),
        new TexturesImageText(),
        new TexturesLogoRaylibTexture(),
        new TexturesMousePainting(),
        new TexturesNpatchDrawing(),
        new TexturesParticlesBlending(),
        new TexturesPolygon(),
        new TexturesRawData(),
        new TexturesSpriteAnim(),
        new TexturesSpriteButton(),
        new TexturesSpriteExplosion(),
        new TexturesSrcRecDstRec(),
        new TexturesToImage(),

        // Text
        new TextCodepointsLoading(),
        new TextFontFilters(),
        new TextFontLoading(),
        new TextFontSdf(),
        new TextFontSpritefont(),
        new TextFormatText(),
        new TextInputBox(),
        new TextRaylibFonts(),
        new TextRectangleBounds(),
        new TextWritingAnim(),

        // Audio
        new AudioModulePlaying(),
        new AudioMusicStreamDemo(),
        new AudioSoundLoading(),

        // Shaders (retargeted to glsl100 for WebGL; some advanced ones may not render on WebGL1)
        new ShadersBasicLighting(),
        new ShadersBasicPbr(),
        new ShadersCustomUniform(),
        new ShadersEratosthenes(),
        new ShadersFog(),
        new ShadersHotReloading(),
        new ShadersHybridRender(),
        new ShadersJuliaSet(),
        new ShadersMeshInstancing(),
        new ShadersModelShader(),
        new ShadersMultiSample2d(),
        new ShadersPaletteSwitch(),
        new ShadersPostProcessing(),
        new ShadersRaymarching(),
        new ShadersShapesTextures(),
        new ShadersSimpleMask(),
        new ShadersSpotlight(),
        new ShadersTextureDrawing(),
        new ShadersTextureOutline(),
        new ShadersTextureWaves(),
        new ShadersWriteDepth(),
    };

    private static IWebExample _current;

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
