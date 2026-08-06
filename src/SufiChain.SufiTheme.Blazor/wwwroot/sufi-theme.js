/**
 * SufiTheme JavaScript Module
 * Sidebar menu helpers, viewport breakpoint, and other theme utilities
 */

// ============================================
// Viewport breakpoint (mobile detection for dual sidebar layout)
// ============================================

const _viewportListeners = new Map();

/**
 * Initialize viewport breakpoint detection. Returns current isMobile (innerWidth < breakpointPx).
 * When viewport crosses the breakpoint, invokes dotNetRef.invokeMethodAsync('OnViewportBreakpointChanged', isMobile).
 * @param {DotNetObjectReference} dotNetRef - Blazor component reference
 * @param {number} breakpointPx - Width threshold (default 768)
 * @param {string} id - Unique id for this listener (for disposal)
 * @returns {boolean} true if current viewport is below breakpoint (mobile)
 */
export function initViewportBreakpoint(dotNetRef, breakpointPx, id) {
    const bp = breakpointPx ?? 768;
    function getIsMobile() {
        return typeof window !== 'undefined' && window.innerWidth < bp;
    }
    let current = getIsMobile();
    function onResize() {
        const next = getIsMobile();
        if (next !== current) {
            current = next;
            dotNetRef.invokeMethodAsync('OnViewportBreakpointChanged', next);
        }
    }
    if (id && _viewportListeners.has(id)) {
        window.removeEventListener('resize', _viewportListeners.get(id));
        _viewportListeners.delete(id);
    }
    window.addEventListener('resize', onResize);
    _viewportListeners.set(id, onResize);
    return current;
}

/**
 * Remove viewport breakpoint listener.
 * @param {string} id - Same id passed to initViewportBreakpoint
 */
export function disposeViewportBreakpoint(id) {
    const onResize = _viewportListeners.get(id);
    if (onResize) {
        window.removeEventListener('resize', onResize);
        _viewportListeners.delete(id);
    }
}

// ============================================
// Sidebar Menu - Smooth height animations
// ============================================

const SUBMENU_TRANSITION_MS = 250;
const SUBMENU_ANIMATION_FALLBACK_MS = SUBMENU_TRANSITION_MS + 150;

// Track in-flight submenu height animations (WeakMap avoids leaks when nodes are removed)
const _activeAnimations = new WeakMap();

// Track all menu items for accordion behavior
const _menuRegistry = new Map();

function finishSubmenuAnimation(element) {
    const state = _activeAnimations.get(element);
    if (!state) {
        return;
    }

    if (state.onTransitionEnd) {
        element.removeEventListener('transitionend', state.onTransitionEnd);
    }
    if (state.fallbackTimer) {
        clearTimeout(state.fallbackTimer);
    }

    _activeAnimations.delete(element);
}

function runHeightAnimation(element, applyAnimation, afterComplete) {
    if (!element) {
        return;
    }

    finishSubmenuAnimation(element);

    let finished = false;
    const finish = () => {
        if (finished) {
            return;
        }
        finished = true;
        if (typeof afterComplete === 'function') {
            afterComplete();
        }
        finishSubmenuAnimation(element);
    };

    const onTransitionEnd = (event) => {
        if (event.target === element && event.propertyName === 'height') {
            finish();
        }
    };

    const fallbackTimer = setTimeout(finish, SUBMENU_ANIMATION_FALLBACK_MS);

    _activeAnimations.set(element, { onTransitionEnd, fallbackTimer });
    element.addEventListener('transitionend', onTransitionEnd);

    applyAnimation(finish);
}

/**
 * Register a menu item for accordion behavior
 * @param {string} menuId - Unique identifier for this menu item
 * @param {HTMLElement} element - The submenu container element
 * @param {number} level - Nesting level (0, 1, 2, 3...)
 * @param {string} parentId - Parent menu item ID (null for top level)
 */
export function registerMenuItem(menuId, element, level, parentId) {
    if (!element) return;
    
    _menuRegistry.set(menuId, {
        element,
        level,
        parentId,
        isExpanded: false,
        children: []
    });
    
    // Link to parent
    if (parentId && _menuRegistry.has(parentId)) {
        const parent = _menuRegistry.get(parentId);
        if (!parent.children.includes(menuId)) {
            parent.children.push(menuId);
        }
    }
    
    // Initialize collapsed state
    element.style.overflow = 'hidden';
    element.style.height = '0';
    element.style.transition = `height ${SUBMENU_TRANSITION_MS}ms ease`;
}

/**
 * Unregister a menu item (cleanup on dispose)
 * @param {string} menuId - Menu item identifier
 */
export function unregisterMenuItem(menuId) {
    const item = _menuRegistry.get(menuId);
    if (item) {
        finishSubmenuAnimation(item.element);

        if (item.parentId) {
            const parent = _menuRegistry.get(item.parentId);
            if (parent) {
                parent.children = parent.children.filter(id => id !== menuId);
            }
        }
    }
    _menuRegistry.delete(menuId);
}

/**
 * Collapse all children recursively
 * @param {string} menuId - Parent menu item ID
 */
function collapseAllChildren(menuId) {
    const item = _menuRegistry.get(menuId);
    if (!item || !item.children.length) return;
    
    for (const childId of item.children) {
        const child = _menuRegistry.get(childId);
        if (child && child.isExpanded) {
            // Collapse this child
            collapseSubmenu(child.element);
            child.isExpanded = false;
            
            // Recursively collapse its children
            collapseAllChildren(childId);
        }
    }
}

/**
 * Collapse all siblings at the same level
 * @param {string} menuId - Current menu item ID
 *
 * Disabled: accordion behavior was causing sibling menus to get stuck after repeated
 * expand/collapse clicks (Blazor CSS state and JS height animations desynced).
 */
/*
function collapseAllSiblings(menuId) {
    const item = _menuRegistry.get(menuId);
    if (!item) return;
    
    // Find all items at the same level with the same parent
    for (const [otherId, otherItem] of _menuRegistry.entries()) {
        if (otherId !== menuId && 
            otherItem.level === item.level && 
            otherItem.parentId === item.parentId &&
            otherItem.isExpanded) {
            // Collapse sibling
            collapseSubmenu(otherItem.element);
            otherItem.isExpanded = false;
            
            // Recursively collapse its children
            collapseAllChildren(otherId);
        }
    }
}
*/

/**
 * Expand a submenu with smooth animation
 * @param {HTMLElement} element - The submenu container element
 */
function expandSubmenu(element) {
    if (!element) {
        return;
    }

    runHeightAnimation(element, (finish) => {
        element.style.height = 'auto';
        const height = element.scrollHeight;
        element.style.height = '0';

        // Force reflow before animating
        element.offsetHeight;

        if (height === 0) {
            finish();
            return;
        }

        element.style.height = height + 'px';
    }, () => {
        if (element.style.height !== '0px') {
            element.style.height = 'auto';
        }
    });
}

/**
 * Collapse a submenu with smooth animation
 * @param {HTMLElement} element - The submenu container element
 */
function collapseSubmenu(element) {
    if (!element) {
        return;
    }

    runHeightAnimation(element, (finish) => {
        const height = element.scrollHeight;
        element.style.height = height + 'px';

        // Force reflow before animating
        element.offsetHeight;

        if (height === 0) {
            element.style.height = '0';
            finish();
            return;
        }

        element.style.height = '0';
    });
}

/**
 * Toggle submenu with accordion behavior
 * @param {string} menuId - Unique menu item identifier
 * @param {HTMLElement} element - The submenu container element
 * @param {boolean} shouldExpand - Whether to expand or collapse
 */
export function toggleSubmenu(menuId, element, shouldExpand) {
    if (!element || !menuId) return;
    
    const item = _menuRegistry.get(menuId);
    if (!item) return;
    
    if (shouldExpand) {
        // Accordion disabled — allow multiple sibling menus to stay open
        // collapseAllSiblings(menuId);
        
        // Then expand this item
        expandSubmenu(element);
        item.isExpanded = true;
    } else {
        // Collapse this item and all its children
        collapseSubmenu(element);
        item.isExpanded = false;
        collapseAllChildren(menuId);
    }
}

/**
 * Set submenu to expanded state without animation (for initial load)
 * @param {string} menuId - Menu item identifier
 * @param {HTMLElement} element - The submenu container element
 */
export function setSubmenuExpanded(menuId, element) {
    if (!element || !menuId) return;
    
    const item = _menuRegistry.get(menuId);
    if (item) {
        item.isExpanded = true;
    }
    
    finishSubmenuAnimation(element);

    element.style.height = 'auto';
    element.style.overflow = 'hidden';
    element.style.transition = `height ${SUBMENU_TRANSITION_MS}ms ease`;
}

// Expose viewport helpers on window so Blazor can invoke them as global functions
// (Blazor's InvokeAsync on ES module reference often fails with "is not a function")
if (typeof window !== 'undefined') {
    window.SufiThemeViewportInit = initViewportBreakpoint;
    window.SufiThemeViewportDispose = disposeViewportBreakpoint;
}

// Default export for consumers that use the module namespace
export default {
    initViewportBreakpoint,
    disposeViewportBreakpoint,
    registerMenuItem,
    unregisterMenuItem,
    toggleSubmenu,
    setSubmenuExpanded
};
