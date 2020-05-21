var MapLib = window.MapLib = MapLib || {};
(function () {
    /**
    * 地球半径
    */
    var EARTHRADIUS = 6370996.81;

    /** 
    *  判断一个点是否在多边形内部 
    *  @param {Array[]} points 多边形坐标集合 
    *  @param {Array} testPoint 测试点坐标 
    *  返回true为真，false为假 
    *  */
    MapLib.pointInsidePolygon = function insidePolygon(points, testPoint) {
        var x = testPoint[0], y = testPoint[1];
        var inside = false;
        for (var i = 0, j = points.length - 1; i < points.length; j = i++) {
            var xi = points[i][0], yi = points[i][1];
            var xj = points[j][0], yj = points[j][1];

            var intersect = ((yi > y) != (yj > y))
                && (x < (xj - xi) * (y - yi) / (yj - yi) + xi);
            if (intersect) inside = !inside;
        }
        return inside;
    }

    /** 
     *  判断一个点是否在圆的内部 
     *  @param point  点坐标 
     *  @param circle 圆心坐标 
     *  @param r 圆半径 
     *  返回true为真，false为假 
     *  */
    MapLib.pointInsideCircle = function pointInsideCircle(point, circle, r) {
        if (r === 0) return false
        point[0] = _getLoop(point[0], -180, 180);
        point[1] = _getRange(point[1], -74, 74);
        circle[0] = _getLoop(circle[0], -180, 180);
        circle[1] = _getRange(circle[1], -74, 74);
        var x1, x2, y1, y2;
        x1 = MapLib.degreeToRad(point[0]);
        y1 = MapLib.degreeToRad(point[1]);
        x2 = MapLib.degreeToRad(circle[0]);
        y2 = MapLib.degreeToRad(circle[1]);
        var distance = EARTHRADIUS * Math.acos((Math.sin(y1) * Math.sin(y2) + Math.cos(y1) * Math.cos(y2) * Math.cos(x2 - x1)));
        return distance < r;
    }
    /**
     * 将v值限定在a,b之间，经度使用
     */
    function _getLoop(v, a, b) {
        while (v > b) {
            v -= b - a
        }
        while (v < a) {
            v += b - a
        }
        return v;
    }
    /**
     * 将v值限定在a,b之间，纬度使用
     */
    function _getRange(v, a, b) {
        if (a != null) {
            v = Math.max(v, a);
        }
        if (b != null) {
            v = Math.min(v, b);
        }
        return v;
    }
    /**
    * 将度转化为弧度
    * @param {degree} Number 度
    * @returns {Number} 弧度
    */
    MapLib.degreeToRad = function (degree) {
        return Math.PI * degree / 180;
    }
})();//闭包结束