/**
 * KomTheme JavaScript Module
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

// Track active animations to prevent rapid-click issues
const _activeAnimations = new WeakMap();

// Track all menu items for accordion behavior
const _menuRegistry = new Map();

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
    element.style.transition = 'height 0.25s ease';
}

/**
 * Unregister a menu item (cleanup on dispose)
 * @param {string} menuId - Menu item identifier
 */
export function unregisterMenuItem(menuId) {
    const item = _menuRegistry.get(menuId);
    if (item && item.parentId) {
        const parent = _menuRegistry.get(item.parentId);
        if (parent) {
            parent.children = parent.children.filter(id => id !== menuId);
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
 */
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

/**
 * Expand a submenu with smooth animation
 * @param {HTMLElement} element - The submenu container element
 */
function expandSubmenu(element) {
    if (!element) return;
    
    // Prevent animation if already animating
    if (_activeAnimations.get(element)) return;
    
    _activeAnimations.set(element, true);

    // Measure content height
    element.style.height = 'auto';
    const height = element.scrollHeight;
    element.style.height = '0';
    
    // Force reflow
    element.offsetHeight;
    
    // Animate to target height
    element.style.height = height + 'px';

    // After animation, set to auto for dynamic content
    const onTransitionEnd = () => {
        if (element.style.height !== '0px') {
            element.style.height = 'auto';
        }
        _activeAnimations.delete(element);
        element.removeEventListener('transitionend', onTransitionEnd);
    };
    element.addEventListener('transitionend', onTransitionEnd);
}

/**
 * Collapse a submenu with smooth animation
 * @param {HTMLElement} element - The submenu container element
 */
function collapseSubmenu(element) {
    if (!element) return;
    
    // Prevent animation if already animating
    if (_activeAnimations.get(element)) return;
    
    _activeAnimations.set(element, true);
    
    // Get current height
    const height = element.scrollHeight;
    element.style.height = height + 'px';
    
    // Force reflow
    element.offsetHeight;
    
    // Animate to 0
    element.style.height = '0';
    
    const onTransitionEnd = () => {
        _activeAnimations.delete(element);
        element.removeEventListener('transitionend', onTransitionEnd);
    };
    element.addEventListener('transitionend', onTransitionEnd);
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
        // Collapse all siblings first (accordion behavior)
        collapseAllSiblings(menuId);
        
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
    
    element.style.height = 'auto';
    element.style.overflow = 'hidden';
    element.style.transition = 'height 0.25s ease';
}

// Expose viewport helpers on window so Blazor can invoke them as global functions
// (Blazor's InvokeAsync on ES module reference often fails with "is not a function")
if (typeof window !== 'undefined') {
    window.KomThemeViewportInit = initViewportBreakpoint;
    window.KomThemeViewportDispose = disposeViewportBreakpoint;
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
