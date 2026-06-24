// Adapted for the browser from Examples/Core/StorageValues.cs
// NOTE: Reads/writes "storage.data" in the wasm in-memory virtual filesystem (not persisted
// across page reloads), which is enough to exercise the save/load round-trip in the browser.
namespace Examples.Web;

public unsafe class CoreStorageValues : IWebExample
{
    public string Name => "Core / Storage Values";

    // NOTE: Storage positions must start with 0, directly related to file memory layout
    private enum StorageData
    {
        Score,
        HiScore
    }

    private const string StorageDataFile = "storage.data";

    private int _score;
    private int _hiscore;
    private int _framesCounter;

    public void Init()
    {
        _score = 0;
        _hiscore = 0;
        _framesCounter = 0;
    }

    public void Update()
    {
        if (IsKeyPressed(KeyboardKey.R))
        {
            _score = GetRandomValue(1000, 2000);
            _hiscore = GetRandomValue(2000, 4000);
        }

        if (IsKeyPressed(KeyboardKey.Enter))
        {
            SaveStorageValue(StorageDataFile, (int)StorageData.Score, _score);
            SaveStorageValue(StorageDataFile, (int)StorageData.HiScore, _hiscore);
        }
        else if (IsKeyPressed(KeyboardKey.Space))
        {
            // NOTE: If requested position could not be found, value 0 is returned
            _score = LoadStorageValue(StorageDataFile, (int)StorageData.Score);
            _hiscore = LoadStorageValue(StorageDataFile, (int)StorageData.HiScore);
        }

        _framesCounter++;

        BeginDrawing();
        ClearBackground(Color.RayWhite);

        DrawText($"SCORE: {_score}", 280, 130, 40, Color.Maroon);
        DrawText($"HI-SCORE: {_hiscore}", 210, 200, 50, Color.Black);

        DrawText($"frames: {_framesCounter}", 10, 10, 20, Color.Lime);

        DrawText("Press R to generate random numbers", 220, 40, 20, Color.LightGray);
        DrawText("Press ENTER to SAVE values", 250, 310, 20, Color.LightGray);
        DrawText("Press SPACE to LOAD values", 252, 350, 20, Color.LightGray);

        EndDrawing();
    }

    public void Unload()
    {
    }

    // Save integer value to storage file (to defined position)
    // NOTE: Storage positions is directly related to file memory layout (4 bytes each integer)
    private static bool SaveStorageValue(string fileName, int position, int value)
    {
        using var fileNameBuffer = fileName.ToUtf8Buffer();

        bool success = false;
        int dataSize = 0;
        int newDataSize = 0;

        byte* fileData = LoadFileData(fileNameBuffer.AsPointer(), &dataSize);
        byte* newFileData = null;

        if (fileData != null)
        {
            if (dataSize <= (position * sizeof(int)))
            {
                // Increase data size up to position and store value
                newDataSize = (position + 1) * sizeof(int);
                newFileData = (byte*)MemRealloc(fileData, (uint)newDataSize);

                if (newFileData != null)
                {
                    // RL_REALLOC succeded
                    int* dataPtr = (int*)newFileData;
                    dataPtr[position] = value;
                }
                else
                {
                    // RL_REALLOC failed
                    int positionInBytes = position * sizeof(int);
                    TraceLog(
                        TraceLogLevel.Warning,
                        @$"FILEIO: [{fileName}] Failed to realloc data ({dataSize}),
                            position in bytes({positionInBytes}) bigger than actual file size"
                    );

                    // We store the old size of the file
                    newFileData = fileData;
                    newDataSize = dataSize;
                }
            }
            else
            {
                // Store the old size of the file
                newFileData = fileData;
                newDataSize = dataSize;

                // Replace value on selected position
                int* dataPtr = (int*)newFileData;
                dataPtr[position] = value;
            }

            success = SaveFileData(fileNameBuffer.AsPointer(), newFileData, newDataSize);
            MemFree(newFileData);

            TraceLog(TraceLogLevel.Info, $"FILEIO: [{fileName}] Saved storage value: {value}");
        }
        else
        {
            TraceLog(TraceLogLevel.Info, $"FILEIO: [{fileName}] File created successfully");

            dataSize = (position + 1) * sizeof(int);
            fileData = (byte*)MemAlloc((uint)dataSize);
            int* dataPtr = (int*)fileData;
            dataPtr[position] = value;

            success = SaveFileData(fileNameBuffer.AsPointer(), fileData, dataSize);
            UnloadFileData(fileData);

            TraceLog(TraceLogLevel.Info, $"FILEIO: [{fileName}] Saved storage value: {value}");
        }

        return success;
    }

    // Load integer value from storage file (from defined position)
    // NOTE: If requested position could not be found, value 0 is returned
    private static int LoadStorageValue(string fileName, uint position)
    {
        using var fileNameBuffer = fileName.ToUtf8Buffer();

        int value = 0;
        int dataSize = 0;
        byte* fileData = LoadFileData(fileNameBuffer.AsPointer(), &dataSize);

        if (fileData != null)
        {
            if (dataSize < (position * 4))
            {
                TraceLog(
                    TraceLogLevel.Warning,
                    $"FILEIO: [{fileName}] Failed to find storage position: {value}"
                );
            }
            else
            {
                int* dataPtr = (int*)fileData;
                value = dataPtr[position];
            }

            UnloadFileData(fileData);

            TraceLog(TraceLogLevel.Info, $"FILEIO: [{fileName}] Loaded storage value: {value}");
        }

        return value;
    }
}
