/*******************************************************************************************
*
*   raylib [text] example - font sdf
*
*   Example complexity rating: [★★★☆] 3/4
*
*   Example originally created with raylib 1.3, last time updated with raylib 4.0
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2015-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_cs.Raylib;

namespace Examples.Text;

public partial class FontSdf
{
    const int GlslVersion = 330;

    public unsafe static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [text] example - font sdf");

        // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)

        string msg = "Signed Distance Fields";

        // Loading file to memory
        int fileSize = 0;
        byte* fileData = LoadFileData("resources/fonts/anonymous_pro_bold.ttf", ref fileSize);

        // Default font generation from TTF font
        Font fontDefault = new();
        fontDefault.BaseSize = 16;
        fontDefault.GlyphCount = 95;

        // Loading font data from memory data
        // Parameters > font size: 16, no glyphs array provided (0), glyphs count: 95 (autogenerate chars array)
        fontDefault.Glyphs = LoadFontData(fileData, (int)fileSize, 16, null, 95, FontType.Default, &fontDefault.GlyphCount);
        // Parameters > glyphs count: 95, font size: 16, glyphs padding in image: 4 px, pack method: 0 (default)
        Image atlas = GenImageFontAtlas(fontDefault.Glyphs, &fontDefault.Recs, 95, 16, 4, 0);
        fontDefault.Texture = LoadTextureFromImage(atlas);
        UnloadImage(atlas);

        // SDF font generation from TTF font
        Font fontSDF = new();
        fontSDF.BaseSize = 16;
        fontSDF.GlyphCount = 95;
        // Parameters > font size: 16, no glyphs array provided (0), glyphs count: 0 (defaults to 95)
        fontSDF.Glyphs = LoadFontData(fileData, (int)fileSize, 16, null, 0, FontType.Sdf, &fontDefault.GlyphCount);
        // Parameters > glyphs count: 95, font size: 16, glyphs padding in image: 0 px, pack method: 1 (Skyline algorythm)
        atlas = GenImageFontAtlas(fontSDF.Glyphs, &fontSDF.Recs, 95, 16, 0, 1);
        fontSDF.Texture = LoadTextureFromImage(atlas);
        UnloadImage(atlas);

        UnloadFileData(fileData);      // Free memory from loaded file

        // Load SDF required shader (we use default vertex shader)
        Shader shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/sdf.fs");
        SetTextureFilter(fontSDF.Texture, TextureFilter.Bilinear);    // Required for SDF font

        Vector2 fontPosition = new(40, screenHeight / 2.0f - 50);
        Vector2 textSize = new(0.0f);
        float fontSize = 16.0f;
        int currentFont = 0;            // 0 - fontDefault, 1 - fontSDF

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            fontSize += GetMouseWheelMove() * 8.0f;

            if (fontSize < 6)
            {
                fontSize = 6;
            }

            if (IsKeyDown(KeyboardKey.Space))
            {
                currentFont = 1;
            }
            else
            {
                currentFont = 0;
            }

            if (currentFont == 0)
            {
                textSize = MeasureTextEx(fontDefault, msg, fontSize, 0);
            }
            else
            {
                textSize = MeasureTextEx(fontSDF, msg, fontSize, 0);
            }

            fontPosition.X = GetScreenWidth() / 2 - textSize.X / 2;
            fontPosition.Y = GetScreenHeight() / 2 - textSize.Y / 2 + 80;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            if (currentFont == 1)
            {
                // NOTE: SDF fonts require a custom SDf shader to compute fragment color
                BeginShaderMode(shader);    // Activate SDF font shader
                DrawTextEx(fontSDF, msg, fontPosition, fontSize, 0, Color.Black);
                EndShaderMode();            // Activate our default shader for next drawings

                DrawTexture(fontSDF.Texture, 10, 10, Color.Black);
            }
            else
            {
                DrawTextEx(fontDefault, msg, fontPosition, fontSize, 0, Color.Black);
                DrawTexture(fontDefault.Texture, 10, 10, Color.Black);
            }

            if (currentFont == 1)
            {
                DrawText("SDF!", 320, 20, 80, Color.Red);
            }
            else
            {
                DrawText("default font", 315, 40, 30, Color.Gray);
            }

            DrawText("FONT SIZE: 16.0", GetScreenWidth() - 240, 20, 20, Color.DarkGray);
            DrawText($"RENDER SIZE: {fontSize:00.00}", GetScreenWidth() - 240, 50, 20, Color.DarkGray);
            DrawText("Use MOUSE WHEEL to SCALE TEXT!", GetScreenWidth() - 240, 90, 10, Color.DarkGray);

            DrawText("HOLD SPACE to USE SDF FONT VERSION!", 340, GetScreenHeight() - 30, 20, Color.Maroon);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadFont(fontDefault);    // Default font unloading
        UnloadFont(fontSDF);        // SDF font unloading

        UnloadShader(shader);       // Unload SDF shader

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
