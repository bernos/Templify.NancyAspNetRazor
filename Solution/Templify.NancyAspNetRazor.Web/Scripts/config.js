/**
 * Application configuration settings. If you are looking for requirejs configuration settings
 * you'll find them in ~/Scripts/require-config.js
 */
define(function () {
    return {
        
        /**
         * Proxy access through to the appsettings provided by the 
         * Nancy.ClientAppSettings
         */
        appSettings: Settings
    };
});