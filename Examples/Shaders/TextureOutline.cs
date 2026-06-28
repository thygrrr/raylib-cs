/*******************************************************************************************
*
*   raylib [shaders] example - texture outline
*
*   Example complexity rating: [★★★☆] 3/4
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version
*
*   Example originally created with raylib 4.0, last time updated with raylib 4.0
*
*   Example contributed by Serenity Skiff (@GoldenThumbs) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2021-2025 Serenity Skiff (@GoldenThumbs) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Shaders;

public partial class TextureOutline
{
    const int GLSL_VERSION = 330;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shaders] example - texture outline");

        Texture2D texture = LoadTexture("resources/fudesumi.png");
        Shader shdrOutline = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/outline.fs");

        float outlineSize = 2.0f;

        // Normalized RED color
        float[] outlineColor = new[] { 1.0f, 0.0f, 0.0f, 1.0f };
        float[] textureSize = { (float)texture.Width, (float)texture.Height };

        // Get shader locations
        int outlineSizeLoc = GetShaderLocation(shdrOutline, "outlineSize");
        int outlineColorLoc = GetShaderLocation(shdrOutline, "outlineColor");
        int textureSizeLoc = GetShaderLocation(shdrOutline, "textureSize");

        // Set shader values (they can be changed later)
        Raylib.SetShaderValue(
            shdrOutline,
            outlineSizeLoc,
            outlineSize,
            ShaderUniformDataType.Float
        );
        Raylib.SetShaderValue(
            shdrOutline,
            outlineColorLoc,
            outlineColor,
            ShaderUniformDataType.Vec4
        );
        Raylib.SetShaderValue(
            shdrOutline,
            textureSizeLoc,
            textureSize,
            ShaderUniformDataType.Vec2
        );

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            outlineSize += GetMouseWheelMove();
            if (outlineSize < 1.0f)
            {
                outlineSize = 1.0f;
            }

            Raylib.SetShaderValue(
                shdrOutline,
                outlineSizeLoc,
                outlineSize,
                ShaderUniformDataType.Float
            );
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();

            ClearBackground(Color.RayWhite);

            BeginShaderMode(shdrOutline);
            DrawTexture(texture, GetScreenWidth() / 2 - texture.Width / 2, -30, Color.White);
            EndShaderMode();

            DrawText("Shader-based\ntexture\noutline", 10, 10, 20, Color.Gray);
            DrawText("Scroll mouse wheel to\nchange outline size", 10, 72, 20, Color.Gray);
            DrawText($"Outline size: {(int)outlineSize} px", 10, 120, 20, Color.Maroon);

            DrawFPS(710, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(texture);
        UnloadShader(shdrOutline);

        CloseWindow();        // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
