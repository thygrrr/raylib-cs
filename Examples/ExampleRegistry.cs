using Examples.Audio;
using Examples.Core;
using Examples.Models;
using Examples.Shapes;
using Examples.Shaders;
using Examples.Text;
using Examples.Textures;

namespace Examples;

public static class ExampleRegistry
{
    public static readonly Type[] Desktop =
    [
        // Core
        typeof(DeltaTime),
        typeof(InputGesturesTestBed),
        typeof(InputVirtualControls),
        typeof(Camera2dPlatformer),
        typeof(Camera2dDemo),
        typeof(Camera3dFirstPerson),
        typeof(Camera3dFree),
        typeof(Camera3dMode),
        typeof(Picking3d),
        typeof(BasicScreenManager),
        typeof(BasicWindow),
        typeof(CustomLogging),
        typeof(DropFiles),
        typeof(InputGamepad),
        typeof(InputGestures),
        typeof(InputKeys),
        typeof(InputMouseWheel),
        typeof(InputMouse),
        typeof(InputMultitouch),
        typeof(RandomValues),
        typeof(ScissorTest),
        typeof(SmoothPixelPerfect),
        typeof(SplitScreen),
        typeof(StorageValues),
        typeof(VrSimulator),
        typeof(WindowFlags),
        typeof(WindowLetterbox),
        typeof(WorldScreen),

        // Shapes
        typeof(BasicShapes),
        typeof(BouncingBall),
        typeof(CollisionArea),
        typeof(ColorsPalette),
        typeof(EasingsBallAnim),
        typeof(EasingsBoxAnim),
        typeof(EasingsRectangleArray),
        typeof(FollowingEyes),
        typeof(LinesBezier),
        typeof(LogoRaylibAnim),
        typeof(LogoRaylibShape),
        typeof(RectangleScaling),

        // Textures
        typeof(BackgroundScrolling),
        typeof(BlendModes),
        typeof(Bunnymark),
        typeof(DrawTiled),
        typeof(ImageDrawing),
        typeof(ImageGeneration),
        typeof(ImageLoading),
        typeof(ImageProcessing),
        typeof(Textures.ImageText),
        typeof(LogoRaylibTexture),
        typeof(MousePainting),
        typeof(NpatchDrawing),
        typeof(ParticlesBlending),
        typeof(TexturedCurve),
        typeof(Polygon),
        typeof(RawData),
        typeof(SpriteAnim),
        typeof(SpriteButton),
        typeof(SpriteExplosion),
        typeof(SrcRecDstRec),
        typeof(ToImage),

        // Text
        typeof(CodepointsLoading),
        typeof(FontFilters),
        typeof(FontLoading),
        typeof(FontSdf),
        typeof(FontSpritefont),
        typeof(FormatText),
        typeof(InputBox),
        typeof(RaylibFonts),
        typeof(RectangleBounds),
        typeof(WritingAnim),

        // Models
        typeof(LoadingIqm),
        typeof(LoadingGltf),
        typeof(BillboardDemo),
        typeof(BoxCollisions),
        typeof(CubicmapDemo),
        typeof(ModelCubeTexture),
        typeof(FirstPersonMaze),
        typeof(GeometricShapes),
        typeof(HeightmapDemo),
        typeof(MeshDemo),
        typeof(ModelLoading),
        typeof(MeshGeneration),
        typeof(MeshPicking),
        typeof(OrthographicProjection),
        typeof(SolarSystem),
        typeof(SkyboxDemo),
        typeof(WavingCubes),
        typeof(YawPitchRoll),
        typeof(DynamicMesh),

        // Shaders
        typeof(BasicLighting),
        typeof(BasicPbr),
        typeof(CustomUniform),
        typeof(Eratosthenes),
        typeof(Fog),
        typeof(HotReloading),
        typeof(HybridRender),
        typeof(JuliaSet),
        typeof(ModelShader),
        typeof(MultiSample2d),
        typeof(PaletteSwitch),
        typeof(PostProcessing),
        typeof(Raymarching),
        typeof(MeshInstancing),
        typeof(ShapesTextures),
        typeof(SimpleMask),
        typeof(Spotlight),
        typeof(TextureDrawing),
        typeof(TextureOutline),
        typeof(TextureWaves),
        typeof(WriteDepth),

        // Audio
        typeof(ModulePlaying),
        typeof(MusicStreamDemo),
        typeof(SoundLoading),
    ];

    public static readonly Type[] DesktopExcludedFromBrowser =
    [
        typeof(DropFiles),
        typeof(SkyboxDemo),
    ];

    public static readonly Type[] BrowserOnly =
    [
        typeof(DrawCircleSector),
        typeof(DrawRectangleRounded),
        typeof(DrawRing),
    ];

    public static int ExpectedBrowserCount =>
        Desktop.Length - DesktopExcludedFromBrowser.Length + BrowserOnly.Length;

    public static void ValidateDesktop(string[] registeredNames)
    {
        if (registeredNames.Length != Desktop.Length)
        {
            throw new InvalidOperationException(
                $"ExampleList has {registeredNames.Length} entries but ExampleRegistry.Desktop defines {Desktop.Length}."
            );
        }

        for (var i = 0; i < registeredNames.Length; i++)
        {
            var expected = Desktop[i].Name;
            if (!registeredNames[i].Equals(expected, StringComparison.Ordinal))
            {
                throw new InvalidOperationException(
                    $"ExampleList entry at index {i} is '{registeredNames[i]}' but expected '{expected}' ({Desktop[i].FullName})."
                );
            }
        }
    }

    public static void ValidateBrowserCount(int registeredCount)
    {
        var expected = ExpectedBrowserCount;
        if (registeredCount != expected)
        {
            throw new InvalidOperationException(
                $"Browser host registered {registeredCount} examples but expected {expected} " +
                $"({Desktop.Length} desktop - {DesktopExcludedFromBrowser.Length} excluded " +
                $"+ {BrowserOnly.Length} browser-only)."
            );
        }
    }
}
