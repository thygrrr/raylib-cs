using System.Diagnostics;
using Examples.Core;
using Examples.Shapes;
using Examples.Textures;
using Examples.Text;
using Examples.Models;
using Examples.Shaders;
using Examples.Audio;

namespace Examples;

public class ExampleInfo
{
    public ExampleInfo(string name, Func<int> run)
    {
        this.Name = name;
        this.Run = run;
    }

    public string Name
    {
        get; set;
    }

    public Func<int> Run
    {
        get; set;
    }
}

public class ExampleList
{
    public static ExampleInfo[] AllExamples = new[]
    {
        // Core
        new ExampleInfo(nameof(DeltaTime), DeltaTime.Main),
        new ExampleInfo(nameof(InputGesturesTestBed), InputGesturesTestBed.Main),
        new ExampleInfo(nameof(InputVirtualControls), InputVirtualControls.Main),
        new ExampleInfo(nameof(Camera2dPlatformer), Camera2dPlatformer.Main),
        new ExampleInfo(nameof(Camera2dDemo), Camera2dDemo.Main),
        new ExampleInfo(nameof(Camera3dFirstPerson), Camera3dFirstPerson.Main),
        new ExampleInfo(nameof(Camera3dFree), Camera3dFree.Main),
        new ExampleInfo(nameof(Camera3dMode), Camera3dMode.Main),
        new ExampleInfo(nameof(Picking3d), Picking3d.Main),
        new ExampleInfo(nameof(BasicScreenManager), BasicScreenManager.Main),
        new ExampleInfo(nameof(BasicWindow), BasicWindow.Main),
        new ExampleInfo(nameof(CustomLogging), CustomLogging.Main),
        new ExampleInfo(nameof(DropFiles), DropFiles.Main),
        new ExampleInfo(nameof(InputGamepad), InputGamepad.Main),
        new ExampleInfo(nameof(InputGestures), InputGestures.Main),
        new ExampleInfo(nameof(InputKeys), InputKeys.Main),
        new ExampleInfo(nameof(InputMouseWheel), InputMouseWheel.Main),
        new ExampleInfo(nameof(InputMouse), InputMouse.Main),
        new ExampleInfo(nameof(InputMultitouch), InputMultitouch.Main),
        new ExampleInfo(nameof(RandomValues), RandomValues.Main),
        new ExampleInfo(nameof(ScissorTest), ScissorTest.Main),
        new ExampleInfo(nameof(SmoothPixelPerfect), SmoothPixelPerfect.Main),
        new ExampleInfo(nameof(SplitScreen), SplitScreen.Main),
        new ExampleInfo(nameof(StorageValues), StorageValues.Main),
        new ExampleInfo(nameof(VrSimulator), VrSimulator.Main),
        new ExampleInfo(nameof(WindowFlags), WindowFlags.Main),
        new ExampleInfo(nameof(WindowLetterbox), WindowLetterbox.Main),
        new ExampleInfo(nameof(WorldScreen), WorldScreen.Main),

        // Shapes
        new ExampleInfo(nameof(BasicShapes), BasicShapes.Main),
        new ExampleInfo(nameof(BouncingBall), BouncingBall.Main),
        new ExampleInfo(nameof(CollisionArea), CollisionArea.Main),
        new ExampleInfo(nameof(ColorsPalette), ColorsPalette.Main),
        new ExampleInfo(nameof(EasingsBallAnim), EasingsBallAnim.Main),
        new ExampleInfo(nameof(EasingsBoxAnim), EasingsBoxAnim.Main),
        new ExampleInfo(nameof(EasingsRectangleArray), EasingsRectangleArray.Main),
        new ExampleInfo(nameof(FollowingEyes), FollowingEyes.Main),
        new ExampleInfo(nameof(LinesBezier), LinesBezier.Main),
        new ExampleInfo(nameof(LogoRaylibAnim), LogoRaylibAnim.Main),
        new ExampleInfo(nameof(LogoRaylibShape), LogoRaylibShape.Main),
        new ExampleInfo(nameof(RectangleScaling), RectangleScaling.Main),

        // Textures
        new ExampleInfo(nameof(BackgroundScrolling), BackgroundScrolling.Main),
        new ExampleInfo(nameof(BlendModes), BlendModes.Main),
        new ExampleInfo(nameof(Bunnymark), Bunnymark.Main),
        new ExampleInfo(nameof(DrawTiled), DrawTiled.Main),
        new ExampleInfo(nameof(ImageDrawing), ImageDrawing.Main),
        new ExampleInfo(nameof(ImageGeneration), ImageGeneration.Main),
        new ExampleInfo(nameof(ImageLoading), ImageLoading.Main),
        new ExampleInfo(nameof(ImageProcessing), ImageProcessing.Main),
        new ExampleInfo(nameof(Textures.ImageText), Textures.ImageText.Main),
        new ExampleInfo(nameof(LogoRaylibTexture), LogoRaylibTexture.Main),
        new ExampleInfo(nameof(MousePainting), MousePainting.Main),
        new ExampleInfo(nameof(NpatchDrawing), NpatchDrawing.Main),
        new ExampleInfo(nameof(ParticlesBlending), ParticlesBlending.Main),
        new ExampleInfo(nameof(TexturedCurve), TexturedCurve.Main),
        new ExampleInfo(nameof(Polygon), Polygon.Main),
        new ExampleInfo(nameof(RawData), RawData.Main),
        new ExampleInfo(nameof(SpriteAnim), SpriteAnim.Main),
        new ExampleInfo(nameof(SpriteButton), SpriteButton.Main),
        new ExampleInfo(nameof(SpriteExplosion), SpriteExplosion.Main),
        new ExampleInfo(nameof(SrcRecDstRec), SrcRecDstRec.Main),
        new ExampleInfo(nameof(ToImage), ToImage.Main),

        // Text
        new ExampleInfo(nameof(CodepointsLoading), CodepointsLoading.Main),
        new ExampleInfo(nameof(FontFilters), FontFilters.Main),
        new ExampleInfo(nameof(FontLoading), FontLoading.Main),
        new ExampleInfo(nameof(FontSdf), FontSdf.Main),
        new ExampleInfo(nameof(FontSpritefont), FontSpritefont.Main),
        new ExampleInfo(nameof(FormatText), FormatText.Main),
        new ExampleInfo(nameof(InputBox), InputBox.Main),
        new ExampleInfo(nameof(RaylibFonts), RaylibFonts.Main),
        new ExampleInfo(nameof(RectangleBounds), RectangleBounds.Main),
        new ExampleInfo(nameof(WritingAnim), WritingAnim.Main),

        // Models
        new ExampleInfo(nameof(LoadingIqm), LoadingIqm.Main),
        new ExampleInfo(nameof(LoadingGltf), LoadingGltf.Main),
        new ExampleInfo(nameof(BillboardDemo), BillboardDemo.Main),
        new ExampleInfo(nameof(BoxCollisions), BoxCollisions.Main),
        new ExampleInfo(nameof(CubicmapDemo), CubicmapDemo.Main),
        new ExampleInfo(nameof(ModelCubeTexture), ModelCubeTexture.Main),
        new ExampleInfo(nameof(FirstPersonMaze), FirstPersonMaze.Main),
        new ExampleInfo(nameof(GeometricShapes), GeometricShapes.Main),
        new ExampleInfo(nameof(HeightmapDemo), HeightmapDemo.Main),
        new ExampleInfo(nameof(MeshDemo), MeshDemo.Main),
        new ExampleInfo(nameof(ModelLoading), ModelLoading.Main),
        new ExampleInfo(nameof(MeshGeneration), MeshGeneration.Main),
        new ExampleInfo(nameof(MeshPicking), MeshPicking.Main),
        new ExampleInfo(nameof(OrthographicProjection), OrthographicProjection.Main),
        new ExampleInfo(nameof(SolarSystem), SolarSystem.Main),
        new ExampleInfo(nameof(SkyboxDemo), SkyboxDemo.Main),
        new ExampleInfo(nameof(WavingCubes), WavingCubes.Main),
        new ExampleInfo(nameof(YawPitchRoll), YawPitchRoll.Main),
        new ExampleInfo(nameof(DynamicMesh), DynamicMesh.Main),

        // Shaders
        new ExampleInfo(nameof(BasicLighting), BasicLighting.Main),
        new ExampleInfo(nameof(BasicPbr), BasicPbr.Main),
        new ExampleInfo(nameof(CustomUniform), CustomUniform.Main),
        new ExampleInfo(nameof(Eratosthenes), Eratosthenes.Main),
        new ExampleInfo(nameof(Fog), Fog.Main),
        new ExampleInfo(nameof(HotReloading), HotReloading.Main),
        new ExampleInfo(nameof(HybridRender), HybridRender.Main),
        new ExampleInfo(nameof(JuliaSet), JuliaSet.Main),
        new ExampleInfo(nameof(ModelShader), ModelShader.Main),
        new ExampleInfo(nameof(MultiSample2d), MultiSample2d.Main),
        new ExampleInfo(nameof(PaletteSwitch), PaletteSwitch.Main),
        new ExampleInfo(nameof(PostProcessing), PostProcessing.Main),
        new ExampleInfo(nameof(Raymarching), Raymarching.Main),
        new ExampleInfo(nameof(MeshInstancing), MeshInstancing.Main),
        new ExampleInfo(nameof(ShapesTextures), ShapesTextures.Main),
        new ExampleInfo(nameof(SimpleMask), SimpleMask.Main),
        new ExampleInfo(nameof(Spotlight), Spotlight.Main),
        new ExampleInfo(nameof(TextureDrawing), TextureDrawing.Main),
        new ExampleInfo(nameof(TextureOutline), TextureOutline.Main),
        new ExampleInfo(nameof(TextureWaves), TextureWaves.Main),
        new ExampleInfo(nameof(WriteDepth), WriteDepth.Main),

        // Audio
        new ExampleInfo(nameof(ModulePlaying), ModulePlaying.Main),
        new ExampleInfo(nameof(MusicStreamDemo), MusicStreamDemo.Main),
        new ExampleInfo(nameof(SoundLoading), SoundLoading.Main),
    };

    static ExampleList()
    {
        ExampleRegistry.ValidateDesktop(Array.ConvertAll(AllExamples, e => e.Name));
    }

    public static ExampleInfo GetExample(string name)
    {
        var example = Array.Find(ExampleList.AllExamples, x =>
            x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        );
        return example;
    }
}

class Program
{
    static unsafe void Main(string[] args)
    {
        Raylib.SetTraceLogCallback(&Logging.LogConsole);

        if (args.Length > 0)
        {
            var example = ExampleList.GetExample(args[0]);
            example?.Run?.Invoke();
            return;
        }

        foreach (var example in ExampleList.AllExamples)
        {
            RunExampleProcess(Environment.ProcessPath, example.Name);
        }
    }

    static void RunExampleProcess(
        string fileName,
        string exampleName
    )
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = exampleName
        };
        using var process = Process.Start(processStartInfo);
        process?.WaitForExit();
    }
}
