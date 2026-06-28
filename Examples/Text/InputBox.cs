/*******************************************************************************************
*
*   raylib [text] example - input box
*
*   Example complexity rating: [★★☆☆] 2/4
*
*   Example originally created with raylib 1.7, last time updated with raylib 3.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2017-2025 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using static Raylib_cs.Raylib;

namespace Examples.Text;

public partial class InputBox
{
    public const int MaxInputChars = 9;

    public static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [text] example - input box");

        // NOTE: One extra space required for null terminator char '\0'
        char[] name = new char[MaxInputChars + 1];
        int letterCount = 0;

        Rectangle textBox = new(screenWidth / 2 - 100, 180, 225, 50);
        bool mouseOnText = false;

        int framesCounter = 0;

        SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())    // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            if (CheckCollisionPointRec(GetMousePosition(), textBox))
            {
                mouseOnText = true;
            }
            else
            {
                mouseOnText = false;
            }

            if (mouseOnText)
            {
                // Set the window's cursor to the I-Beam
                SetMouseCursor(MouseCursor.IBeam);

                // Get char pressed (unicode character) on the queue
                int key = GetCharPressed();

                // Check if more characters have been pressed on the same frame
                while (key > 0)
                {
                    // NOTE: Only allow keys in range [32..125]
                    if ((key >= 32) && (key <= 125) && (letterCount < MaxInputChars))
                    {
                        name[letterCount] = (char)key;
                        name[letterCount + 1] = '\0'; // Add null terminator at the end of the string
                        letterCount++;
                    }

                    key = GetCharPressed();  // Check next character in the queue
                }

                if (IsKeyPressed(KeyboardKey.Backspace))
                {
                    letterCount -= 1;
                    if (letterCount < 0)
                    {
                        letterCount = 0;
                    }
                    name[letterCount] = '\0';
                }
            }
            else
            {
                SetMouseCursor(MouseCursor.Default);
            }

            if (mouseOnText)
            {
                framesCounter += 1;
            }
            else
            {
                framesCounter = 0;
            }
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RayWhite);

            DrawText("PLACE MOUSE OVER INPUT BOX!", 240, 140, 20, Color.Gray);
            DrawRectangleRec(textBox, Color.LightGray);

            if (mouseOnText)
            {
                DrawRectangleLines(
                    (int)textBox.X,
                    (int)textBox.Y,
                    (int)textBox.Width,
                    (int)textBox.Height,
                    Color.Red
                );
            }
            else
            {
                DrawRectangleLines(
                    (int)textBox.X,
                    (int)textBox.Y,
                    (int)textBox.Width,
                    (int)textBox.Height,
                    Color.DarkGray
                );
            }

            DrawText(new string(name), (int)textBox.X + 5, (int)textBox.Y + 8, 40, Color.Maroon);
            DrawText($"INPUT CHARS: {letterCount}/{MaxInputChars}", 315, 250, 20, Color.DarkGray);

            if (mouseOnText)
            {
                if (letterCount < MaxInputChars)
                {
                    // Draw blinking underscore char
                    if ((framesCounter / 20 % 2) == 0)
                    {
                        DrawText(
                            "_",
                            (int)textBox.X + 8 + MeasureText(new string(name), 40),
                            (int)textBox.Y + 12,
                            40,
                            Color.Maroon
                        );
                    }
                }
                else
                {
                    DrawText("Press BACKSPACE to delete chars...", 230, 300, 20, Color.Gray);
                }
            }

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
