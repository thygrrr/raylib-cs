/*******************************************************************************************
*
*   raylib [shaders] example - texture waves
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   Example originally created with raylib 2.5, last time updated with raylib 3.7
*
*   Example contributed by Anata (@anatagawa) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2019-2025 Anata (@anatagawa) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Shaders;

public partial class TextureWaves
{
    const int GlslVersion = 330;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shaders] example - texture waves");

        // Load texture texture to apply shaders
        Texture2D texture = LoadTexture("resources/space.png");

        // Load shader and setup location points and values
        Shader shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/wave.fs");

        int secondsLoc = GetShaderLocation(shader, "seconds");
        int freqXLoc = GetShaderLocation(shader, "freqX");
        int freqYLoc = GetShaderLocation(shader, "freqY");
        int ampXLoc = GetShaderLocation(shader, "ampX");
        int ampYLoc = GetShaderLocation(shader, "ampY");
        int speedXLoc = GetShaderLocation(shader, "speedX");
        int speedYLoc = GetShaderLocation(shader, "speedY");

        // Shader uniform values that can be updated at any time
        float freqX = 25.0f;
        float freqY = 25.0f;
        float ampX = 5.0f;
        float ampY = 5.0f;
        float speedX = 8.0f;
        float speedY = 8.0f;

        float[] screenSize = { (float)GetScreenWidth(), (float)GetScreenHeight() };
        Raylib.SetShaderValue(
            shader,
            GetShaderLocation(shader, "size"),
            screenSize,
            ShaderUniformDataType.Vec2
        );
        Raylib.SetShaderValue(shader, freqXLoc, freqX, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, freqYLoc, freqY, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, ampXLoc, ampX, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, ampYLoc, ampY, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, speedXLoc, speedX, ShaderUniformDataType.Float);
        Raylib.SetShaderValue(shader, speedYLoc, speedY, ShaderUniformDataType.Float);

        float seconds = 0.0f;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            seconds += GetFrameTime();

            Raylib.SetShaderValue(shader, secondsLoc, seconds, ShaderUniformDataType.Float);
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            BeginShaderMode(shader);

            DrawTexture(texture, 0, 0, Color.White);
            DrawTexture(texture, texture.Width, 0, Color.White);

            EndShaderMode();

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadShader(shader);         // Unload shader
        UnloadTexture(texture);       // Unload texture

        CloseWindow();              // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
