requirejs.config({
    baseUrl: "Scripts",

    paths: {
        "jquery": "jquery-1.10.2.min"
        //    "knockout": "knockout-3.0.0",
        //    "knockout.mapping": "knockout.mapping-latest",
        //    "moment": "moment"
    }
});

require(["jquery", "config"]);