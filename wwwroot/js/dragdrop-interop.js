// JavaScript interop for drag-and-drop functionality using SortableJS

let sortableInstances = {};

window.initializeSortable = function(elementId, dotNetObject, callbackMethod) {
    console.log(`initializeSortable called for: ${elementId}`);

    // Get the element
    const element = document.getElementById(elementId);
    if (!element) {
        console.error(`Element with ID '${elementId}' not found`);
        return;
    }

    // Destroy existing instance first
    if (sortableInstances[elementId]) {
        console.log(`Destroying existing Sortable instance for: ${elementId}`);
        sortableInstances[elementId].destroy();
        delete sortableInstances[elementId];
    }

    // Initialize Sortable
    try {
        sortableInstances[elementId] = new Sortable(element, {
            animation: 150,
            handle: '.drag-handle',
            ghostClass: 'sortable-ghost',
            chosenClass: 'sortable-chosen',
            dragClass: 'sortable-drag',
            onEnd: function(evt) {
                console.log(`onEnd triggered for: ${elementId}`);
                try {
                    // Get the new order as an array
                    const items = Array.from(evt.from.children);
                    const orderedIds = items.map(item => item.getAttribute('data-field'));
                    console.log(`New order for ${elementId}:`, orderedIds);

                    // Call the Blazor method
                    if (dotNetObject && callbackMethod && typeof dotNetObject[callbackMethod] === 'function') {
                        dotNetObject[callbackMethod](orderedIds);
                    } else {
                        console.error(`Invalid dotNetObject or callbackMethod for ${elementId}`);
                    }
                } catch (error) {
                    console.error(`Error in onEnd for ${elementId}:`, error);
                }
            },
            onError: function(error) {
                console.error(`Sortable error for ${elementId}:`, error);
            }
        });

        console.log(`Sortable initialized successfully for element: ${elementId}`);
    } catch (error) {
        console.error(`Failed to initialize Sortable for ${elementId}:`, error);
    }
};

window.destroySortable = function(elementId) {
    if (sortableInstances[elementId]) {
        sortableInstances[elementId].destroy();
        delete sortableInstances[elementId];
        console.log(`Sortable destroyed for element: ${elementId}`);
    }
};
