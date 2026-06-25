#if BROWSER
using Examples;
namespace Examples.Shaders;

public partial class PostProcessing : IExample
{
    private readonly BrowserAdapter _browserAdapter = new();

    public string Name => _browserAdapter.Name;

    public void Init()
    {
        _browserAdapter.Init();
    }

    public void Update()
    {
        _browserAdapter.Update();
    }

    public void Unload()
    {
        _browserAdapter.Unload();
    }

    private sealed class BrowserAdapter : IExample
    {
        public string Name => "Shaders / Post Processing";

        private const int GLSL_VERSION = 100;

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private enum PostproShader
        {
            FxGrayScale = 0,
            FxPosterization,
            FxDreamVision,
            FxPixelizer,
            FxCrossHatching,
            FxCrossStiching,
            FxPredatorView,
            FxScanLines,
            FxFishEye,
            FxSobel,
            FxBloom,
            FxBlur,
            //FX_FXAA
            Max
        }

        private static readonly string[] postproShaderText = new string[] {
            "GRAYSCALE",
            "POSTERIZATION",
            "DREAM_VISION",
            "PIXELIZER",
            "CROSS_HATCHING",
            "CROSS_STITCHING",
            "PREDATOR_VIEW",
            "SCANLINES",
            "FISHEYE",
            "SOBEL",
            "BLOOM",
            "BLUR",
            //"FXAA"
        };

        private Camera3D _camera;
        private Model _model;
        private Texture2D _texture;
        private Vector3 _position;
        private Shader[] _shaders;
        private int _currentShader;
        private RenderTexture2D _target;

        public void Init()
        {
            // Define the camera to look into our 3d world
            _camera = new();
            _camera.Position = new Vector3(2.0f, 3.0f, 2.0f);
            _camera.Target = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera.FovY = 45.0f;
            _camera.Projection = CameraProjection.Perspective;

            _model = LoadModel("resources/models/obj/church.obj");
            _texture = LoadTexture("resources/models/obj/church_diffuse.png");

            // Set model diffuse texture
            SetMaterialTexture(ref _model, 0, MaterialMapIndex.Albedo, ref _texture);

            _position = new(0.0f, 0.0f, 0.0f);

            // Load all postpro shaders
            // NOTE 1: All postpro shader use the base vertex shader (DEFAULT_VERTEX_SHADER)
            // NOTE 2: We load the correct shader depending on GLSL version
            _shaders = new Shader[(int)PostproShader.Max];

            // NOTE: Defining null (NULL) for vertex shader forces usage of internal default vertex shader
            string shaderPath = "resources/shaders/glsl100";
            _shaders[(int)PostproShader.FxGrayScale] = LoadShader(null, $"{shaderPath}/grayscale.fs");
            _shaders[(int)PostproShader.FxPosterization] = LoadShader(null, $"{shaderPath}/posterization.fs");
            _shaders[(int)PostproShader.FxDreamVision] = LoadShader(null, $"{shaderPath}/dream_vision.fs");
            _shaders[(int)PostproShader.FxPixelizer] = LoadShader(null, $"{shaderPath}/pixelizer.fs");
            _shaders[(int)PostproShader.FxCrossHatching] = LoadShader(null, $"{shaderPath}/cross_hatching.fs");
            _shaders[(int)PostproShader.FxCrossStiching] = LoadShader(null, $"{shaderPath}/cross_stitching.fs");
            _shaders[(int)PostproShader.FxPredatorView] = LoadShader(null, $"{shaderPath}/predator.fs");
            _shaders[(int)PostproShader.FxScanLines] = LoadShader(null, $"{shaderPath}/scanlines.fs");
            _shaders[(int)PostproShader.FxFishEye] = LoadShader(null, $"{shaderPath}/fisheye.fs");
            _shaders[(int)PostproShader.FxSobel] = LoadShader(null, $"{shaderPath}/sobel.fs");
            _shaders[(int)PostproShader.FxBloom] = LoadShader(null, $"{shaderPath}/bloom.fs");
            _shaders[(int)PostproShader.FxBlur] = LoadShader(null, $"{shaderPath}/blur.fs");

            _currentShader = (int)PostproShader.FxGrayScale;

            // Create a RenderTexture2D to be used for render to texture
            _target = LoadRenderTexture(screenWidth, screenHeight);
        }

        public void Update()
        {
            UpdateCamera(ref _camera, CameraMode.Orbital);

            if (IsKeyPressed(KeyboardKey.Right))
            {
                _currentShader++;
            }
            else if (IsKeyPressed(KeyboardKey.Left))
            {
                _currentShader--;
            }

            if (_currentShader >= (int)PostproShader.Max)
            {
                _currentShader = 0;
            }
            else if (_currentShader < 0)
            {
                _currentShader = (int)PostproShader.Max - 1;
            }

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Enable drawing to texture
            BeginTextureMode(_target);
            ClearBackground(Color.RayWhite);

            BeginMode3D(_camera);

            DrawModel(_model, _position, 0.1f, Color.White);

            DrawGrid(10, 1.0f);

            EndMode3D();

            // End drawing to texture (now we have a texture available for next passes)
            EndTextureMode();

            // Render previously generated texture using selected postpro shader
            BeginShaderMode(_shaders[_currentShader]);

            // NOTE: Render texture must be y-flipped due to default OpenGL coordinates (left-bottom)
            DrawTextureRec(
                _target.Texture,
                new Rectangle(0, 0, _target.Texture.Width, -_target.Texture.Height),
                new Vector2(0, 0),
                Color.White
            );

            EndShaderMode();

            DrawRectangle(0, 9, 580, 30, ColorAlpha(Color.LightGray, 0.7f));

            DrawText("(c) Church 3D model by Alberto Cano", screenWidth - 200, screenHeight - 20, 10, Color.Gray);

            DrawText("CURRENT POSTPRO SHADER:", 10, 15, 20, Color.Black);
            DrawText(postproShaderText[_currentShader], 330, 15, 20, Color.Red);
            DrawText("< >", 540, 10, 30, Color.DarkBlue);

            DrawFPS(700, 15);

            EndDrawing();
        }

        public void Unload()
        {
            for (int i = 0; i < (int)PostproShader.Max; i++)
            {
                UnloadShader(_shaders[i]);
            }

            UnloadTexture(_texture);
            UnloadModel(_model);
            UnloadRenderTexture(_target);
        }
    }
}
#endif
