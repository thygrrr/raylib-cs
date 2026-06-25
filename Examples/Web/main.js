import { dotnet } from './_framework/dotnet.js'

const status = document.getElementById('status');
const select = document.getElementById('examples');
const canvas = document.getElementById('canvas');
const CANVAS_WIDTH = 800;
const CANVAS_HEIGHT = 450;

function enforceCanvasSize() {
    
    if (canvas.width !== CANVAS_WIDTH) {
        canvas.width = CANVAS_WIDTH;
    }
    if (canvas.height !== CANVAS_HEIGHT) {
        canvas.height = CANVAS_HEIGHT;
    }
    
    const widthPx = `${CANVAS_WIDTH}px`;
    const heightPx = `${CANVAS_HEIGHT}px`;
    if (canvas.style.width !== widthPx) {
        canvas.style.width = widthPx;
    }
    if (canvas.style.height !== heightPx) {
        canvas.style.height = heightPx;
    }

}

enforceCanvasSize();

const { getAssemblyExports, getConfig, runMain } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const Host = exports.Examples.Web.Host;

// raylib's GLFW/emscripten backend renders into this canvas.
dotnet.instance.Module['canvas'] = canvas;

// Runs Host.Main() -> InitWindow + default example Init(). Must come after the canvas is bound.
await runMain();
enforceCanvasSize();

// Populate the navigation dropdown from the registered examples.
select.innerHTML = '';
for (const name of Host.GetExampleNames().split('\n').filter(n => n.length > 0)) {
    const opt = document.createElement('option');
    opt.value = name;
    opt.textContent = name;
    select.appendChild(opt);
}
select.addEventListener('change', () => Host.SetExample(select.value));

status.textContent = 'Running. Use the dropdown to switch examples.';

// Drive raylib one frame per animation tick (never block the browser).
function mainLoop() {
    enforceCanvasSize();
    Host.UpdateFrame();
    requestAnimationFrame(mainLoop);
}
requestAnimationFrame(mainLoop);
