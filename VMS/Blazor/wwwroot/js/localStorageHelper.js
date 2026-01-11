window.setMainLayoutInstance = function (dotNetObj) {
    // Save a reference to MainLayout's .NET instance
    window.mainLayoutInstance = dotNetObj;
};

window.localStorageHelper = {
    notifyLoginChange: function () {
        // If MainLayout is active, trigger .NET method
        if (window.mainLayoutInstance) {
            window.mainLayoutInstance.invokeMethodAsync('NotifyLoginChange');
        }
    }
};
