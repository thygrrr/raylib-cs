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
    public ExampleInfo(string name, Func<object> create, Func<int> run)
    {
        this.Name = name;
        this.Create = create;
        this.Run = run;
    }

    public string Name
    {
        get; set;
    }

    public Func<object> Create
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
        new ExampleInfo("DeltaTime", () => new DeltaTime(), DeltaTime.Main),
        new ExampleInfo("InputGesturesTestBed", () => new InputGesturesTestBed(), InputGesturesTestBed.Main),
        new ExampleInfo("InputVirtualControls", () => new InputVirtualControls(), InputVirtualControls.Main),
        new ExampleInfo("Camera2dPlatformer", () => new Camera2dPlatformer(), Camera2dPlatformer.Main),
        new ExampleInfo("Camera2dDemo", () => new Camera2dDemo(), Camera2dDemo.Main),
        new ExampleInfo("Camera3dFirstPerson", () => new Camera3dFirstPerson(), Camera3dFirstPerson.Main),
        new ExampleInfo("Camera3dFree", () => new Camera3dFree(), Camera3dFree.Main),
        new ExampleInfo("Camera3dMode", () => new Camera3dMode(), Camera3dMode.Main),
        new ExampleInfo("Picking3d", () => new Picking3d(), Picking3d.Main),
        new ExampleInfo("BasicScreenManager", () => new BasicScreenManager(), BasicScreenManager.Main),
        new ExampleInfo("BasicWindow", () => new BasicWindow(), BasicWindow.Main),
        new ExampleInfo("CustomLogging", () => new CustomLogging(), CustomLogging.Main),
        new ExampleInfo("DropFiles", () => new DropFiles(), DropFiles.Main),
        new ExampleInfo("InputGamepad", () => new InputGamepad(), InputGamepad.Main),
        new ExampleInfo("InputGestures", () => new InputGestures(), InputGestures.Main),
        new ExampleInfo("InputKeys", () => new InputKeys(), InputKeys.Main),
        new ExampleInfo("InputMouseWheel", () => new InputMouseWheel(), InputMouseWheel.Main),
        new ExampleInfo("InputMouse", () => new InputMouse(), InputMouse.Main),
        new ExampleInfo("InputMultitouch", () => new InputMultitouch(), InputMultitouch.Main),
        new ExampleInfo("RandomValues", () => new RandomValues(), RandomValues.Main),
        new ExampleInfo("ScissorTest", () => new ScissorTest(), ScissorTest.Main),
        new ExampleInfo("SmoothPixelPerfect", () => new SmoothPixelPerfect(), SmoothPixelPerfect.Main),
        new ExampleInfo("SplitScreen", () => new SplitScreen(), SplitScreen.Main),
        new ExampleInfo("StorageValues", () => new StorageValues(), StorageValues.Main),
        new ExampleInfo("VrSimulator", () => new VrSimulator(), VrSimulator.Main),
        new ExampleInfo("WindowFlags", () => new WindowFlags(), WindowFlags.Main),
        new ExampleInfo("WindowLetterbox", () => new WindowLetterbox(), WindowLetterbox.Main),
        new ExampleInfo("WorldScreen", () => new WorldScreen(), WorldScreen.Main),

        // Shapes
        new ExampleInfo("BasicShapes", () => new BasicShapes(), BasicShapes.Main),
        new ExampleInfo("BouncingBall", () => new BouncingBall(), BouncingBall.Main),
        new ExampleInfo("CollisionArea", () => new CollisionArea(), CollisionArea.Main),
        new ExampleInfo("ColorsPalette", () => new ColorsPalette(), ColorsPalette.Main),
        new ExampleInfo("EasingsBallAnim", () => new EasingsBallAnim(), EasingsBallAnim.Main),
        new ExampleInfo("EasingsBoxAnim", () => new EasingsBoxAnim(), EasingsBoxAnim.Main),
        new ExampleInfo("EasingsRectangleArray", () => new EasingsRectangleArray(), EasingsRectangleArray.Main),
        new ExampleInfo("FollowingEyes", () => new FollowingEyes(), FollowingEyes.Main),
        new ExampleInfo("LinesBezier", () => new LinesBezier(), LinesBezier.Main),
        new ExampleInfo("LogoRaylibAnim", () => new LogoRaylibAnim(), LogoRaylibAnim.Main),
        new ExampleInfo("LogoRaylibShape", () => new LogoRaylibShape(), LogoRaylibShape.Main),
        new ExampleInfo("RectangleScaling", () => new RectangleScaling(), RectangleScaling.Main),

        // Textures
        new ExampleInfo("BackgroundScrolling", () => new BackgroundScrolling(), BackgroundScrolling.Main),
        new ExampleInfo("BlendModes", () => new BlendModes(), BlendModes.Main),
        new ExampleInfo("Bunnymark", () => new Bunnymark(), Bunnymark.Main),
        new ExampleInfo("DrawTiled", () => new DrawTiled(), DrawTiled.Main),
        new ExampleInfo("ImageDrawing", () => new ImageDrawing(), ImageDrawing.Main),
        new ExampleInfo("ImageGeneration", () => new ImageGeneration(), ImageGeneration.Main),
        new ExampleInfo("ImageLoading", () => new ImageLoading(), ImageLoading.Main),
        new ExampleInfo("ImageProcessing", () => new ImageProcessing(), ImageProcessing.Main),
        new ExampleInfo("ImageText", () => new ImageText(), ImageText.Main),
        new ExampleInfo("LogoRaylibTexture", () => new LogoRaylibTexture(), LogoRaylibTexture.Main),
        new ExampleInfo("MousePainting", () => new MousePainting(), MousePainting.Main),
        new ExampleInfo("NpatchDrawing", () => new NpatchDrawing(), NpatchDrawing.Main),
        new ExampleInfo("ParticlesBlending", () => new ParticlesBlending(), ParticlesBlending.Main),
        new ExampleInfo("TexturedCurve", () => new TexturedCurve(), TexturedCurve.Main),
        new ExampleInfo("Polygon", () => new Polygon(), Polygon.Main),
        new ExampleInfo("RawData", () => new RawData(), RawData.Main),
        new ExampleInfo("SpriteAnim", () => new SpriteAnim(), SpriteAnim.Main),
        new ExampleInfo("SpriteButton", () => new SpriteButton(), SpriteButton.Main),
        new ExampleInfo("SpriteExplosion", () => new SpriteExplosion(), SpriteExplosion.Main),
        new ExampleInfo("SrcRecDstRec", () => new SrcRecDstRec(), SrcRecDstRec.Main),
        new ExampleInfo("ToImage", () => new ToImage(), ToImage.Main),

        // Text
        new ExampleInfo("CodepointsLoading", () => new CodepointsLoading(), CodepointsLoading.Main),
        new ExampleInfo("FontFilters", () => new FontFilters(), FontFilters.Main),
        new ExampleInfo("FontLoading", () => new FontLoading(), FontLoading.Main),
        new ExampleInfo("FontSdf", () => new FontSdf(), FontSdf.Main),
        new ExampleInfo("FontSpritefont", () => new FontSpritefont(), FontSpritefont.Main),
        new ExampleInfo("FormatText", () => new FormatText(), FormatText.Main),
        new ExampleInfo("InputBox", () => new InputBox(), InputBox.Main),
        new ExampleInfo("RaylibFonts", () => new RaylibFonts(), RaylibFonts.Main),
        new ExampleInfo("RectangleBounds", () => new RectangleBounds(), RectangleBounds.Main),
        new ExampleInfo("WritingAnim", () => new WritingAnim(), WritingAnim.Main),

        // Models
        new ExampleInfo("LoadingIqm", () => new LoadingIqm(), LoadingIqm.Main),
        new ExampleInfo("LoadingGltf", () => new LoadingGltf(), LoadingGltf.Main),
        new ExampleInfo("BillboardDemo", () => new BillboardDemo(), BillboardDemo.Main),
        new ExampleInfo("BoxCollisions", () => new BoxCollisions(), BoxCollisions.Main),
        new ExampleInfo("CubicmapDemo", () => new CubicmapDemo(), CubicmapDemo.Main),
        new ExampleInfo("ModelCubeTexture", () => new ModelCubeTexture(), ModelCubeTexture.Main),
        new ExampleInfo("FirstPersonMaze", () => new FirstPersonMaze(), FirstPersonMaze.Main),
        new ExampleInfo("GeometricShapes", () => new GeometricShapes(), GeometricShapes.Main),
        new ExampleInfo("HeightmapDemo", () => new HeightmapDemo(), HeightmapDemo.Main),
        new ExampleInfo("MeshDemo", () => new MeshDemo(), MeshDemo.Main),
        new ExampleInfo("ModelLoading", () => new ModelLoading(), ModelLoading.Main),
        new ExampleInfo("MeshGeneration", () => new MeshGeneration(), MeshGeneration.Main),
        new ExampleInfo("MeshPicking", () => new MeshPicking(), MeshPicking.Main),
        new ExampleInfo("OrthographicProjection", () => new OrthographicProjection(), OrthographicProjection.Main),
        new ExampleInfo("SolarSystem", () => new SolarSystem(), SolarSystem.Main),
        new ExampleInfo("SkyboxDemo", () => new SkyboxDemo(), SkyboxDemo.Main),
        new ExampleInfo("WavingCubes", () => new WavingCubes(), WavingCubes.Main),
        new ExampleInfo("YawPitchRoll", () => new YawPitchRoll(), YawPitchRoll.Main),
        new ExampleInfo("DynamicMesh", () => new DynamicMesh(), DynamicMesh.Main),

        // Shaders
        new ExampleInfo("BasicLighting", () => new BasicLighting(), BasicLighting.Main),
        new ExampleInfo("BasicPbr", () => new BasicPbr(), BasicPbr.Main),
        new ExampleInfo("CustomUniform", () => new CustomUniform(), CustomUniform.Main),
        new ExampleInfo("Eratosthenes", () => new Eratosthenes(), Eratosthenes.Main),
        new ExampleInfo("Fog", () => new Fog(), Fog.Main),
        new ExampleInfo("HotReloading", () => new HotReloading(), HotReloading.Main),
        new ExampleInfo("HybridRender", () => new HybridRender(), HybridRender.Main),
        new ExampleInfo("JuliaSet", () => new JuliaSet(), JuliaSet.Main),
        new ExampleInfo("ModelShader", () => new ModelShader(), ModelShader.Main),
        new ExampleInfo("MultiSample2d", () => new MultiSample2d(), MultiSample2d.Main),
        new ExampleInfo("PaletteSwitch", () => new PaletteSwitch(), PaletteSwitch.Main),
        new ExampleInfo("PostProcessing", () => new PostProcessing(), PostProcessing.Main),
        new ExampleInfo("Raymarching", () => new Raymarching(), Raymarching.Main),
        new ExampleInfo("MeshInstancing", () => new MeshInstancing(), MeshInstancing.Main),
        new ExampleInfo("ShapesTextures", () => new ShapesTextures(), ShapesTextures.Main),
        new ExampleInfo("SimpleMask", () => new SimpleMask(), SimpleMask.Main),
        new ExampleInfo("Spotlight", () => new Spotlight(), Spotlight.Main),
        new ExampleInfo("TextureDrawing", () => new TextureDrawing(), TextureDrawing.Main),
        new ExampleInfo("TextureOutline", () => new TextureOutline(), TextureOutline.Main),
        new ExampleInfo("TextureWaves", () => new TextureWaves(), TextureWaves.Main),
        new ExampleInfo("WriteDepth", () => new WriteDepth(), WriteDepth.Main),

        // Audio
        new ExampleInfo("ModulePlaying", () => new ModulePlaying(), ModulePlaying.Main),
        new ExampleInfo("MusicStreamDemo", () => new MusicStreamDemo(), MusicStreamDemo.Main),
        new ExampleInfo("SoundLoading", () => new SoundLoading(), SoundLoading.Main),
    };

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
