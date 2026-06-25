# Examples/Web — raylib-cs in the browser (WebAssembly)

A small browser app that runs raylib examples in the browser via WebAssembly, with a dropdown to
switch between them. It exists to **prove the `Raylib-cs` NuGet package's `browser-wasm` support
works end-to-end**: the package's `buildTransitive/Raylib-cs.targets` automatically links the
shipped `runtimes/browser-wasm/native/raylib.a` (and adds `-sUSE_GLFW=3`) into the .NET wasm
runtime. The browser host lives in `Examples/Web`, while `Examples.csproj` remains the single
examples project.

The browser-wasm configuration is enabled only when publishing `Examples` with
`RuntimeIdentifier=browser-wasm`, so normal solution builds do not require the `wasm-tools`
workload.

## Prerequisites

- .NET 10 SDK
- `dotnet workload install wasm-tools`
- The `Raylib-cs` package matching `$(RaylibCsVersion)` (see `Directory.Build.props`) — from
  nuget.org, or built locally into the repo's `./nuget` feed (`dotnet pack Raylib-cs -c Release --output nuget`).

## Build

```bash
dotnet publish Examples -f net10.0 -r browser-wasm -c Release
# -> Examples/bin/Release/net10.0/browser-wasm/AppBundle/
```

### Toolchain caveat

If the link step fails with:

```
wasm-opt: Unknown option '--enable-bulk-memory-opt'
```

your installed `wasm-tools` workload (Binaryen) is out of sync with the SDK — emscripten passes a
feature flag the bundled `wasm-opt` doesn't understand (commonly caused by a stale workload band
or a conflicting system EMSDK on PATH/`$EMSDK`). Proper fix:

```bash
dotnet workload update
```

`Examples.csproj` defaults browser-wasm publishes to unoptimized native linking so the standard
publish command works on affected local toolchains. A fully optimized publish can override those
MSBuild properties after updating the workload/toolchain.

## Run

WebAssembly must be served over HTTP (not `file://`):

```bash
dotnet serve -d Examples/bin/Release/net10.0/browser-wasm/AppBundle    # dotnet tool install -g dotnet-serve
# or:  npx http-server Examples/bin/Release/net10.0/browser-wasm/AppBundle
```

Open the printed URL and use the **Example** dropdown to switch examples.

## How it works

A browser can't run raylib's blocking `while (!WindowShouldClose())` loop (it would freeze the
page), so frames are driven from JavaScript:

- `Host.Main()` calls `InitWindow` once and selects the default example (`main.js` runs it via
  `runMain()`).
- `main.js` binds the page `<canvas>` to the runtime, then calls `Host.UpdateFrame()` every
  `requestAnimationFrame` tick. `UpdateFrame`, `SetExample`, and `GetExampleNames` are `[JSExport]`.
- Each base example implements `IExample` (`Init` / `Update` / `Unload`) in `#if BROWSER` partials.
  The host owns the window;
  examples never call `InitWindow`/`CloseWindow`.

## Adding more examples

For a new desktop example (`../Core/...`, `../Shaders/...`, etc.), add a browser partial
(`*.Browser.cs`) that implements `IExample` and mirrors the split of its monolithic `Main`:

```
Main() { <setup>; while(!WindowShouldClose()){ <body> } <cleanup> }
  ->  Init()  { <setup, minus InitWindow/SetTargetFPS> }   // loop-spanning locals become fields
      Update(){ <body> }                                   // keep BeginDrawing..EndDrawing
      Unload(){ <cleanup, minus CloseWindow> }
```

Then add `new YourExample()` to the `Examples` list in `Host.cs`.

Examples that load files (`resources/...`) also need those assets in the wasm virtual filesystem.
`Examples.csproj` bundles `../resources/` into the browser app when publishing with
`RuntimeIdentifier=browser-wasm`.

## Coverage

All 115 desktop examples are adapted and registered: **Core, Shapes, Models, Textures, Text,
Audio, Shaders**. Asset examples load from the bundled `resources/`; audio plays after a user
gesture (browser autoplay policy).

Skipped (no browser equivalent): `Core/DropFiles`, `Core/LoadingThread`, `Text/Unicode`
(also excluded from the desktop build). Drag-and-drop blocks inside otherwise-supported examples
(e.g. `ModelLoading`, `FontFilters`) are no-ops.

Shader examples are retargeted to `glsl100` (raylib's WebGL build uses GLSL ES 1.00). Simple
2D/screen-space shaders render fine; a few advanced ones need WebGL2-era features and likely
won't render on WebGL1 — **SimpleMask** (missing `mask.vs`), **BasicPbr**, **MeshInstancing**,
**HybridRender**, **WriteDepth**. They're still listed; the host catches a failing example so it
can't break the page.
