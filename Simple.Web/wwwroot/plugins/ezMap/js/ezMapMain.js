document.write("<script language=javascript src='../../plugins/ezmap/js/ezMapCommon.js'></script>");
var sMap = {

};

//实例化地图
function mapInit(containerId) {
    //引入地图库文件
    addStyles('http://39.104.61.84:9001/EzServerClient/lib/EzServerClient.min.css');
    addScript('http://39.104.61.84:9001/EzServerClient/lib/EzMapAPI.js');
    addScript('http://39.104.61.84:9001/EzServerClient/lib/EzServerClient.min.js');
    /**
    * 这里的map为上文HTML中新建的地图容器ID号；
    * 下面一行代码便可以加载地图到browser上,前提是
    * 在EzMapAPI.js中设置了相应的参数（具体查看EzMapAPI.js）
    */
    var map = new EzMap('map');
    // 添加地图控件 (级别控制条，也可以选择不添加)
    map.showStandMapControl();
    return map;
}

class SimpleMap {
    //等价于prototype的构造器
    constructor() {
        //引入地图库文件
        addStyles('http://39.104.61.84:9001/EzServerClient/lib/EzServerClient.min.css');
        addScript('http://39.104.61.84:9001/EzServerClient/lib/EzMapAPI.js');
        addScript('http://39.104.61.84:9001/EzServerClient/lib/EzServerClient.min.js');
    }

    mapInit() {
        var map = new EzMap('map');
        return map;
    }
}