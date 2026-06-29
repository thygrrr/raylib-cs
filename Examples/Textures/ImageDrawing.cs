/*******************************************************************************************
*
*   raylib [textures] example - image drawing
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   NOTE: Images are loaded in CPU memory (RAM); textures are loaded in GPU memory (VRAM)
*
*   Example originally created with raylib 1.4, last time updated with raylib 1.4
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2016-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using static Raylib_cs.Raylib;

namespace Examples.Textures;

public partial class ImageDrawing
{
    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - image drawing");

        // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)

        Image cat = LoadImage("resources/cat.png");            // Load image in CPU memory (RAM)
        ImageCrop(ref cat, new Rectangle(100, 10, 280, 380));  // Crop an image piece
        ImageFlipHorizontal(ref cat);                          // Flip cropped image horizontally
        ImageResize(ref cat, 150, 200);                        // Resize flipped-cropped image

        Image parrots = LoadImage("resources/parrots.png");    // Load image in CPU memory (RAM)

        // Draw one image over the other with a scaling of 1.5f
        Rectangle src = new(0, 0, cat.Width, cat.Height);
        ImageDraw(ref parrots, cat, src, new Rectangle(30, 40, cat.Width * 1.5f, cat.Height * 1.5f), Color.White);
        ImageCrop(ref parrots, new Rectangle(0, 50, parrots.Width, parrots.Height - 100)); // Crop resulting image

        // Draw on the image with a few image draw methods
        ImageDrawPixel(ref parrots, 10, 10, Color.RayWhite);
        ImageDrawCircleLines(ref parrots, 10, 10, 5, Color.RayWhite);
        ImageDrawRectangle(ref parrots, 5, 20, 10, 10, Color.RayWhite);

        UnloadImage(cat);       // Unload image from RAM

        // Load custom font for drawing on image
        Font font = LoadFont("resources/custom_jupiter_crash.png");

        // Draw over image using custom font
        ImageDrawTextEx(ref parrots, font, "PARROTS & CAT", new Vector2(300, 230), font.BaseSize, -2, Color.White);

        UnloadFont(font);       // Unload custom font (already drawn used on image)

        Texture2D texture = LoadTextureFromImage(parrots);      // Image converted to texture, uploaded to GPU memory (VRAM)
        UnloadImage(parrots);   // Once image has been converted to texture and uploaded to VRAM, it can be unloaded from RAM

        SetTargetFPS(60);
        //---------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            int x = screenWidth / 2 - texture.Width / 2;
            int y = screenHeight / 2 - texture.Height / 2;
            DrawTexture(texture, x, y - 40, Color.White);
            DrawRectangleLines(x, y - 40, texture.Width, texture.Height, Color.DarkGray);

            DrawText("We are drawing only one texture from various images composed!", 240, 350, 10, Color.DarkGray);
            DrawText("Source images have been cropped, scaled, flipped and copied one over the other.", 190, 370, 10, Color.DarkGray);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(texture);       // Texture unloading

        CloseWindow();                // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
