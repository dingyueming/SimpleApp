//distinct
Array.prototype.distinct = function () {
    var arr = [];
    for (var i = 0; i < this.length; i++) {
        if (arr.indexOf(this[i]) == -1) {
            arr.push(this[i]);
        }
    }
    return arr;
}

//递归单位，返回所选单位树下的所有ID
function getUnitChildrenId(checkedNode) {
    var ids = [];
    ids.push(checkedNode.id);
    if (checkedNode.children != undefined) {
        checkedNode.children.forEach((option) => {
            ids.push(option.id);
            getUnitChildrenId(option).forEach((x) => { ids.push(x);})
        });
    }
    return ids.distinct();
}

function recuTreeSelectNodes(id, options) {
    var checkedNode = null;
    options.forEach((node) => {
        if (node.id == id) {
            checkedNode = node;
        }
        if (checkedNode == null && node.children != null) {
            checkedNode = recuTreeSelectNodes(id, node.children);
        }
    });
    return checkedNode;
}
