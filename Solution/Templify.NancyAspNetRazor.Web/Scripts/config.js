requirejs.config({
    //baseUrl: "Scripts",
    paths: {
        "jquery": "jquery-1.10.2.min",
        //    "knockout": "knockout-3.0.0",
        //    "knockout.mapping": "knockout.mapping-latest",
        //    "moment": "moment"
    }
});

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