import { dotnet } from './_framework/dotnet.js'

const status = document.getElementById('status');
const select = document.getElementById('examples');
const viewport = document.getElementById('viewport');
const canvas = document.getElementById('canvas');

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

function fitCanvasToViewport() {
    const width = viewport.clientWidth;
    const height = viewport.clientHeight;
    if (width <= 0 || height <= 0) {
        return;
    }

    const aspect = 960 / 540;
    let targetWidth = width;
    let targetHeight = Math.floor(targetWidth / aspect);

    if (targetHeight > height) {
        targetHeight = height;
        targetWidth = Math.floor(targetHeight * aspect);
    }

    canvas.style.width = `${targetWidth}px`;
    canvas.style.height = `${targetHeight}px`;
}

fitCanvasToViewport();
window.addEventListener('resize', fitCanvasToViewport);
if (typeof ResizeObserver !== 'undefined') {
    const resizeObserver = new ResizeObserver(fitCanvasToViewport);
    resizeObserver.observe(viewport);
}

// Drive raylib one frame per animation tick (never block the browser).
function mainLoop() {
    Host.UpdateFrame();
    requestAnimationFrame(mainLoop);
}
requestAnimationFrame(mainLoop);
