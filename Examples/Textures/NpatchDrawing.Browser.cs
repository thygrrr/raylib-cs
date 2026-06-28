#if BROWSER
using Examples;
using System;

namespace Examples.Textures;

public partial class NpatchDrawing : IExample
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
        public string Name => "Textures / N-patch Drawing";

        private Texture2D _nPatchTexture;

        private Vector2 _origin;
        private Rectangle _dstRec1;
        private Rectangle _dstRec2;
        private Rectangle _dstRecH;
        private Rectangle _dstRecV;
        private NPatchInfo _ninePatchInfo1;
        private NPatchInfo _ninePatchInfo2;
        private NPatchInfo _h3PatchInfo;
        private NPatchInfo _v3PatchInfo;

        public void Init()
        {
            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            _nPatchTexture = LoadTexture("resources/ninepatch_button.png");

            _origin = new Vector2(0.0f, 0.0f);

            // Position and size of the n-patches
            _dstRec1 = new Rectangle(480.0f, 160.0f, 32.0f, 32.0f);
            _dstRec2 = new Rectangle(160.0f, 160.0f, 32.0f, 32.0f);
            _dstRecH = new Rectangle(160.0f, 93.0f, 32.0f, 32.0f);
            _dstRecV = new Rectangle(92.0f, 160.0f, 32.0f, 32.0f);

            // A 9-patch (NPATCH_NINE_PATCH) changes its sizes in both axis
            _ninePatchInfo1 = new NPatchInfo
            {
                Source = new Rectangle(0.0f, 0.0f, 64.0f, 64.0f),
                Left = 12,
                Top = 40,
                Right = 12,
                Bottom = 12,
                Layout = NPatchLayout.NinePatch
            };
            _ninePatchInfo2 = new NPatchInfo
            {
                Source = new Rectangle(0.0f, 128.0f, 64.0f, 64.0f),
                Left = 16,
                Top = 16,
                Right = 16,
                Bottom = 16,
                Layout = NPatchLayout.NinePatch
            };

            // A horizontal 3-patch (NPATCH_THREE_PATCH_HORIZONTAL) changes its sizes along the x axis only
            _h3PatchInfo = new NPatchInfo
            {
                Source = new Rectangle(0.0f, 64.0f, 64.0f, 64.0f),
                Left = 8,
                Top = 8,
                Right = 8,
                Bottom = 8,
                Layout = NPatchLayout.ThreePatchHorizontal
            };

            // A vertical 3-patch (NPATCH_THREE_PATCH_VERTICAL) changes its sizes along the y axis only
            _v3PatchInfo = new NPatchInfo
            {
                Source = new Rectangle(0.0f, 192.0f, 64.0f, 64.0f),
                Left = 6,
                Top = 6,
                Right = 6,
                Bottom = 6,
                Layout = NPatchLayout.ThreePatchVertical
            };
        }

        public void Update()
        {
            Vector2 mousePosition = GetMousePosition();

            // Resize the n-patches based on mouse position
            _dstRec1.Width = mousePosition.X - _dstRec1.X;
            _dstRec1.Height = mousePosition.Y - _dstRec1.Y;
            _dstRec2.Width = mousePosition.X - _dstRec2.X;
            _dstRec2.Height = mousePosition.Y - _dstRec2.Y;
            _dstRecH.Width = mousePosition.X - _dstRecH.X;
            _dstRecV.Height = mousePosition.Y - _dstRecV.Y;

            // Set a minimum width and/or height
            _dstRec1.Width = Math.Clamp(_dstRec1.Width, 1.0f, 300.0f);
            _dstRec1.Height = MathF.Max(_dstRec1.Height, 1.0f);
            _dstRec2.Width = Math.Clamp(_dstRec2.Width, 1.0f, 300.0f);
            _dstRec2.Height = MathF.Max(_dstRec2.Height, 1.0f);
            _dstRecH.Width = MathF.Max(_dstRecH.Width, 1.0f);
            _dstRecV.Height = MathF.Max(_dstRecV.Height, 1.0f);

            BeginDrawing();
            ClearBackground(Color.RayWhite);

            // Draw the n-patches
            DrawTextureNPatch(_nPatchTexture, _ninePatchInfo2, _dstRec2, _origin, 0.0f, Color.White);
            DrawTextureNPatch(_nPatchTexture, _ninePatchInfo1, _dstRec1, _origin, 0.0f, Color.White);
            DrawTextureNPatch(_nPatchTexture, _h3PatchInfo, _dstRecH, _origin, 0.0f, Color.White);
            DrawTextureNPatch(_nPatchTexture, _v3PatchInfo, _dstRecV, _origin, 0.0f, Color.White);

            // Draw the source texture
            DrawRectangleLines(5, 88, 74, 266, Color.Blue);
            DrawTexture(_nPatchTexture, 10, 93, Color.White);
            DrawText("TEXTURE", 15, 360, 10, Color.DarkGray);

            DrawText("Move the mouse to stretch or shrink the n-patches", 10, 20, 20, Color.DarkGray);

            EndDrawing();
        }

        public void Unload()
        {
            UnloadTexture(_nPatchTexture);
        }
    }
}
#endif
