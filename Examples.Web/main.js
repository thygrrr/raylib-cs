import { dotnet } from './_framework/dotnet.js'

const status = document.getElementById('status');
const select = document.getElementById('examples');

const { getAssemblyExports, getConfig, runMain } = await dotnet
    .withDiagnosticTracing(false)
    .create();

const config = getConfig();
const exports = await getAssemblyExports(config.mainAssemblyName);
const Host = exports.Examples.Web.Host;

// raylib's GLFW/emscripten backend renders into this canvas.
dotnet.instance.Module['canvas'] = document.getElementById('canvas');

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

// Drive raylib one frame per animation tick (never block the browser).
function mainLoop() {
    Host.UpdateFrame();
    requestAnimationFrame(mainLoop);
}
requestAnimationFrame(mainLoop);
