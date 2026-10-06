/**
 * SufiTheme - Viewport breakpoint (classic script for Blazor global invocation).
 * Loaded via script bundle; attaches SufiThemeViewportInit and SufiThemeViewportDispose to window.
 */
(function () {
    'use strict';
    var listeners = new Map();

    function getIsMobile(bp) {
        return typeof window !== 'undefined' && window.innerWidth <= (bp || 768);
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
        // Rotation can report the previous width, then settle. Recheck after layout.
        function onOrientation() {
            onResize();
            if (typeof window !== 'undefined' && window.setTimeout) {
                window.setTimeout(onResize, 150);
            }
        }
        function detach() {
            window.removeEventListener('resize', onResize);
            window.removeEventListener('orientationchange', onOrientation);
        }
        if (id && listeners.has(id)) {
            listeners.get(id)();
            listeners.delete(id);
        }
        window.addEventListener('resize', onResize);
        window.addEventListener('orientationchange', onOrientation);
        listeners.set(id, detach);
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
        var detach = listeners.get(id);
        if (typeof detach === 'function') {
            detach();
            listeners.delete(id);
        }
    };
})();
