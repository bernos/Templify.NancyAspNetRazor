({
    mainConfigFile: "Scripts/common.js",
    baseUrl: "./Scripts",
    findNestedDependencies:true,
    modules: [
        {
            name: "common"
        },
        {
            name: "Modules/Home/index",
            exclude: ["common"]
        }
    ],

    dir: "Scripts-built",
    removeCombined: true,

    skipDirOptimize: true
})