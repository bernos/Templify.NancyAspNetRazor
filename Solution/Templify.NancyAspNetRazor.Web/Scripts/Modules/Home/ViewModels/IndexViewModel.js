define(["config", "jquery", "../Models/MyModel"], function (config, $, MyModel) {

    console.log("IndexViewModel has loaded ", config);
    console.log($("body"))

    var m = new MyModel();
    console.log(m.name);

});