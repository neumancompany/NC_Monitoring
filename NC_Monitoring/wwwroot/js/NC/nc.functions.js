function NCNotifySuccess(message)
{
    DevExpress.ui.notify({
        message: message,
        width: 450
    }, "success", 2000);
}

function NCNotifyError(message) {
    DevExpress.ui.notify({
        message: message,
        width: 450
    }, "error", 2000);
}