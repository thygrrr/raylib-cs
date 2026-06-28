#if BROWSER
using Examples;
namespace Examples.Textures;

public partial class ImageDrawing : IExample
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
        public string Name => "Textures / Image Drawing";

        private const int screenWidth = 800;
        private const int screenHeight = 450;

        private Texture2D _texture;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)

            Image cat = LoadImage("resources/cat.png");
            ImageCrop(ref cat, new Rectangle(100, 10, 280, 380));
            ImageFlipHorizontal(ref cat);
            ImageResize(ref cat, 150, 200);

            Image parrots = LoadImage("resources/parrots.png");

            // Draw one image over the other with a scaling of 1.5f
            Rectangle src = new(0, 0, cat.Width, cat.Height);
            ImageDraw(ref parrots, cat, src, new Rectangle(30, 40, cat.Width * 1.5f, cat.Height * 1.5f), Color.White);
            ImageCrop(ref parrots, new Rectangle(0, 50, parrots.Width, parrots.Height - 100));

            // Draw on the image with a few image draw methods
            ImageDrawPixel(ref parrots, 10, 10, Color.RayWhite);
            ImageDrawCircle(ref parrots, 10, 10, 5, Color.RayWhite);
            ImageDrawRectangle(ref parrots, 5, 20, 10, 10, Color.RayWhite);

            UnloadImage(cat);

            // Load custom font for frawing on image
            Font font = LoadFont("resources/fonts/custom_jupiter_crash.png");

            // Draw over image using custom font
            ImageDrawTextEx(ref parrots, font, "PARROTS & CAT", new Vector2(300, 230), font.BaseSize, -2, Color.White);

            // Unload custom spritefont (already drawn used on image)
            UnloadFont(font);

            _texture = LoadTextureFromImage(parrots);
            UnloadImage(parrots);
        }

        public void Update()
        {
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            int x = screenWidth / 2 - _texture.Width / 2;
            int y = screenHeight / 2 - _texture.Height / 2;
            DrawTexture(_texture, x, y - 40, Color.White);
            DrawRectangleLines(x, y - 40, _texture.Width, _texture.Height, Color.DarkGray);

            DrawText("We are drawing only one texture from various images composed!", 240, 350, 10, Color.DarkGray);

            string text = "Source images have been cropped, scaled, flipped and copied one over the other.";
            DrawText(text, 90, 370, 10, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_texture);
        }
    }
}
#endif
