DevExpress.ui.dxDataGrid.defaultOptions({
    device: { deviceType: "desktop" },
    options: {
        showBorders: true,
        editing: {
            mode: "popup",
            useIcons: true,
            popup: {
                showTitle: true,
                position: {
                    my: ["center", "top"],
                    at: ["center", "top"],
                },
            }
        },
    }
});