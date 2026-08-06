/**
 * SufiTheme - Viewport breakpoint (classic script for Blazor global invocation).
 * Loaded via script bundle; attaches SufiThemeViewportInit and SufiThemeViewportDispose to window.
 */
(function () {
    'use strict';
    var listeners = new Map();

    function getIsMobile(bp) {
        return typeof window !== 'undefined' && window.innerWidth < (bp || 768);
    }

    window.SufiThemeViewportInit = function (dotNetRef, breakpointPx, id) {
        var bp = breakpointPx || 768;
        var current = getIsMobile(bp);
        function onResize() {
            var next = getIsMobile(bp);
            if (next !== current) {
                current = next;
                dotNetRef.invokeMethodAsync('OnViewportBreakpointChanged', next);
            }
        }
        if (id && listeners.has(id)) {
            window.removeEventListener('resize', listeners.get(id));
            listeners.delete(id);
        }
        window.addEventListener('resize', onResize);
        listeners.set(id, onResize);
        if (typeof window !== 'undefined' && window.setTimeout) {
            window.setTimeout(function () {
                var again = getIsMobile(bp);
                if (again !== current) {
                    current = again;
                    dotNetRef.invokeMethodAsync('OnViewportBreakpointChanged', again);
                }
            }, 150);
        }
        return current;
    };

    window.SufiThemeViewportDispose = function (id) {
        var onResize = listeners.get(id);
        if (onResize) {
            window.removeEventListener('resize', onResize);
            listeners.delete(id);
        }
    };
})();
