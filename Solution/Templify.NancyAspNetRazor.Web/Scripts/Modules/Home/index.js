require(["../../config"], function (config) {

    require(["jquery"], function($) {
        console.log("index.js loaded ", config);
        console.log($("body"))
    });

});