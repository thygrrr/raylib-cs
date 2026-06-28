/*******************************************************************************************
*
*   raylib [shaders] example - mesh instancing
*
*   Example complexity rating: [★★★★] 4/4
*
*   Example originally created with raylib 3.7, last time updated with raylib 4.2
*
*   Example contributed by seanpringle (@seanpringle) and reviewed by Max (@moliad) and Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2020-2025 seanpringle (@seanpringle), Max (@moliad) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using static Raylib_cs.Raylib;
using Examples.Shared;

namespace Examples.Shaders;

public partial class MeshInstancing
{
    const int GLSL_VERSION = 330;

    const int MaxInstances = 10000;

    public unsafe static int Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [shaders] example - mesh instancing");

        // Define the camera to look into our 3d world
        Camera3D camera = new();
        camera.Position = new Vector3(-125.0f, 125.0f, -125.0f);    // Camera position
        camera.Target = new Vector3(0.0f, 0.0f, 0.0f);              // Camera looking at point
        camera.Up = new Vector3(0.0f, 1.0f, 0.0f);                  // Camera up vector (rotation towards target)
        camera.FovY = 45.0f;                                        // Camera field-of-view Y
        camera.Projection = CameraProjection.Perspective;           // Camera projection type

        // Define mesh to be instanced
        Mesh cube = GenMeshCube(1.0f, 1.0f, 1.0f);

        // Define transforms to be uploaded to GPU for instances
        Matrix4x4[] transforms = new Matrix4x4[MaxInstances];   // Pre-multiplied transformations passed to rlgl

        // Translate and rotate cubes randomly
        for (int i = 0; i < MaxInstances; i++)
        {
            Matrix4x4 translation = Matrix4x4.CreateTranslation(
                GetRandomValue(-50, 50),
                GetRandomValue(-50, 50),
                GetRandomValue(-50, 50)
            );
            Vector3 axis = Vector3.Normalize(new Vector3(
                GetRandomValue(0, 360),
                GetRandomValue(0, 360),
                GetRandomValue(0, 360)
            ));
            float angle = GetRandomValue(0, 180) * DEG2RAD;
            Matrix4x4 rotation = Matrix4x4.CreateFromAxisAngle(axis, angle);

            transforms[i] = Matrix4x4.Transpose(Matrix4x4.Multiply(rotation, translation));
        }

        // Load lighting shader
        Shader shader = LoadShader(
            $"resources/shaders/glsl{GLSL_VERSION}/lighting_instancing.vs",
            $"resources/shaders/glsl{GLSL_VERSION}/lighting.fs"
        );
        // Get shader locations
        shader.Locs[(int)ShaderLocationIndex.MatrixMvp] = GetShaderLocation(shader, "mvp");
        shader.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(shader, "viewPos");

        // Set shader value: ambient light level
        int ambientLoc = GetShaderLocation(shader, "ambient");
        Raylib.SetShaderValue(
            shader,
            ambientLoc,
            new float[] { 0.2f, 0.2f, 0.2f, 1.0f },
            ShaderUniformDataType.Vec4
        );

        // Create one light
        Rlights.CreateLight(
            0,
            LightType.Directorional,
            new Vector3(50.0f, 50.0f, 0.0f),
            Vector3.Zero,
            Color.White,
            shader
        );

        // NOTE: We are assigning the intancing shader to material.shader
        // to be used on mesh drawing with DrawMeshInstanced()
        Material matInstances = LoadMaterialDefault();
        matInstances.Shader = shader;
        matInstances.Maps[(int)MaterialMapIndex.Diffuse].Color = Color.Red;

        // Load default material (using raylib intenral default shader) for non-instanced mesh drawing
        // WARNING: Default shader enables vertex color attribute BUT GenMeshCube() does not generate vertex colors, so,
        // when drawing the color attribute is disabled and a default color value is provided as input for thevertex attribute
        Material matDefault = LoadMaterialDefault();
        matDefault.Maps[(int)MaterialMapIndex.Diffuse].Color = Color.Blue;

        SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())        // Detect window close button or ESC key
        {
            // Update
            //----------------------------------------------------------------------------------
            UpdateCamera(ref camera, CameraMode.Orbital);

            // Update the light shader with the camera view position
            float[] cameraPos = { camera.Position.X, camera.Position.Y, camera.Position.Z };
            Raylib.SetShaderValue(
                shader,
                shader.Locs[(int)ShaderLocationIndex.VectorView],
                cameraPos,
                ShaderUniformDataType.Vec3
            );
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();

            ClearBackground(Color.RayWhite);

            BeginMode3D(camera);

            // Draw cube mesh with default material (BLUE)
            DrawMesh(cube, matDefault, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(-10.0f, 0.0f, 0.0f)));

            // Draw meshes instanced using material containing instancing shader (RED + lighting),
            // transforms[] for the instances should be provided, they are dynamically
            // updated in GPU every frame, so we can animate the different mesh instances
            DrawMeshInstanced(cube, matInstances, transforms, MaxInstances);

            // Draw cube mesh with default material (BLUE)
            DrawMesh(cube, matDefault, Matrix4x4.Transpose(Matrix4x4.CreateTranslation(10.0f, 0.0f, 0.0f)));

            EndMode3D();

            DrawFPS(10, 10);

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        CloseWindow();          // Close window and OpenGL context
        //--------------------------------------------------------------------------------------

        return 0;
    }
}
