# Examples.Web — raylib-cs in the browser (WebAssembly)

A small browser app that runs raylib examples in the browser via WebAssembly, with a dropdown to
switch between them. It exists to **prove the `Raylib-cs` NuGet package's `browser-wasm` support
works end-to-end**: the package's `buildTransitive/Raylib-cs.targets` automatically links the
shipped `runtimes/browser-wasm/native/raylib.a` (and adds `-sUSE_GLFW=3`) into the .NET wasm
runtime — this project just consumes the package and renders.

This project is intentionally **not** part of `Raylib-cs.sln` (it hardcodes
`RuntimeIdentifier=browser-wasm` and requires the `wasm-tools` workload, which would break the
normal solution build). Build it on its own with `dotnet publish`.

## Prerequisites

- .NET 10 SDK
- `dotnet workload install wasm-tools`
- The `Raylib-cs` package matching `$(RaylibCsVersion)` (see `Directory.Build.props`) — from
  nuget.org, or built locally into the repo's `./nuget` feed (`dotnet pack Raylib-cs -c Release --output nuget`).

## Build

```bash
dotnet publish Examples.Web -c Release
# -> Examples.Web/bin/Release/net10.0/browser-wasm/AppBundle/
```

### Toolchain caveat (local Windows dev)

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

Quick local workaround (skips the `wasm-opt` passes; produces an unoptimized but working bundle):

```bash
dotnet publish Examples.Web -c Release \
  -p:EmccLinkOptimizationFlag=-O0 -p:EmccCompileOptimizationFlag=-O0 \
  -p:WasmNativeStrip=false -p:WasmEmitSymbolMap=false
```

These flags are **not** committed to the csproj on purpose — a correct toolchain (e.g. CI) builds
the optimized bundle with no extra flags.

## Run

WebAssembly must be served over HTTP (not `file://`):

```bash
dotnet serve -d Examples.Web/bin/Release/net10.0/browser-wasm/AppBundle    # dotnet tool install -g dotnet-serve
# or:  npx http-server Examples.Web/bin/Release/net10.0/browser-wasm/AppBundle
```

Open the printed URL and use the **Example** dropdown to switch examples.

## How it works

A browser can't run raylib's blocking `while (!WindowShouldClose())` loop (it would freeze the
page), so frames are driven from JavaScript:

- `Host.Main()` calls `InitWindow` once and selects the default example (`main.js` runs it via
  `runMain()`).
- `main.js` binds the page `<canvas>` to the runtime, then calls `Host.UpdateFrame()` every
  `requestAnimationFrame` tick. `UpdateFrame`, `SetExample`, and `GetExampleNames` are `[JSExport]`.
- Each example implements `IWebExample` (`Init` / `Update` / `Unload`). The host owns the window;
  examples never call `InitWindow`/`CloseWindow`.

## Adding more examples

Convert a desktop example (`../Examples/...`) by splitting its monolithic `Main`:

```
Main() { <setup>; while(!WindowShouldClose()){ <body> } <cleanup> }
  ->  Init()  { <setup, minus InitWindow/SetTargetFPS> }   // loop-spanning locals become fields
      Update(){ <body> }                                   // keep BeginDrawing..EndDrawing
      Unload(){ <cleanup, minus CloseWindow> }
```

Then add `new YourExample()` to the `Examples` list in `Host.cs`.

Examples that load files (`resources/...`) also need those assets in the wasm virtual filesystem —
uncomment the `WasmFilesToIncludeInFileSystem` item in `Examples.Web.csproj` to bundle
`../Examples/resources/`.

## Adapted so far (starter batch, asset-free)

Core: Basic Window, Input Keys, Input Mouse, Mouse Wheel · Shapes: Basic Shapes, Bouncing Ball,
Following Eyes · Text: Writing Animation, Format Text.

The remaining desktop examples can be converted incrementally with the pattern above.
