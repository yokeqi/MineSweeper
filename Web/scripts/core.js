


var offsetX = 0;// 左偏移量
var offsetY = 0;// 上偏移量
var num = 9;// 行列-多少格
var boxSize = 40;// 格子尺寸
var boomNum = 10;// 多少颗雷

var at = [[-1, -1], [0, -1], [+1, -1], [-1, 0], [+1, 0], [-1, +1], [0, +1], [+1, +1]];// 九宫格向量
var map = new Array();
var mark = new Array();

$(document).ready(function () {
    var width = $(window).width();
    $("#line").width(width).height(100);

    buildGame();

    timer();
    window.setInterval(timer, 1000);
})

function buildGame() {
    $("#panel").empty();
    $("#msg").hide("fast");
    createMap();// 初始化地图
    createUI();// 创建UI
    registerEvents();
}

function createMap() {
    //初始化为0
    for (var i = 0; i < num; i++) {
        map[i] = new Array();
        mark[i] = new Array();
        for (var j = 0; j < num; j++) {
            map[i][j] = 0;
            mark[i][j] = 0;
        }
    }

    var count = 0;
    // 生成雷
    while (count < boomNum) {
        var row = Math.floor(Math.random() * (num - 1));
        var col = Math.floor(Math.random() * (num - 1));
        if (isMine(row, col)) {
            // 这个位置已经有雷,重新生成位置
            continue;
        }
        count++;
        map[row][col] = 100;

        // 周围的雷数标记+1
        searchAround(row, col, function (posX, posY) {
            map[posX][posY]++;
        });
        //for (var i = 0; i < at.length; i++) {
        //    var posX = row + at[i][0];
        //    var posY = col + at[i][1];
        //    // 不要超出地图范围
        //    if (posX < 0 || posY < 0 || posX > map.length || posY > map[0].length)
        //        continue;

        //    map[posX][posY]++;
        //}
    }
}

function createUI() {
    // 生成界面
    for (var i = 0; i < num; i++) {
        for (var j = 0; j < num; j++) {
            var l = offsetX + i * boxSize;
            var t = offsetY + j * boxSize;
            // 创建容器Box
            var box = $("<div></div>").addClass("box").css({ "left": l + "px", "top": t + "px" }).attr({ "row": i, "col": j });
            // 创建遮罩层
            var cover = $("<div></div>").addClass("cover").width(boxSize - 1).height(boxSize - 1);// 遮罩层，这样就直接看不到底下是不是雷
            // 创建实际内容
            var content = $("<div></div>").addClass("content").width(boxSize - 1).height(boxSize - 1).hide();// 内容，可能是空白，可能是个提示数字，也可能就是个雷

            if (map[i][j] >= 100) {
                // 如果是雷，则把格子数据替换成一张Gif图。
                var boom = $("<img></img>").attr("src", "./imgs/boom.gif").width(40).height(40);
                content.css("margin", "0px").html(boom);
            } else if (map[i][j] > 0) {
                content.text(map[i][j]);
            }

            box.append(cover).append(content);
            $("#panel").append(box);
        }
    };
}

function registerEvents() {
    $(".box").hover(function () {
        var row = $(this).attr("row");
        var col = $(this).attr("col");
        if (mark[row][col] == 0)
            $(this).children(".cover").css("backgroundColor", "lightgray");
    });
    $(".box").mouseleave(function () {
        var row = $(this).attr("row");
        var col = $(this).attr("col");
        if (mark[row][col] == 0)
            $(this).children(".cover").css("backgroundColor", "black");
    });
    $(".box").dblclick(function () {
        // 挖格子，看运气是不是雷
        sweeper($(this));
    });

    $(".box").click(function () {
        var row = $(this).attr("row");
        var col = $(this).attr("col");
        mark[row][col] = mark[row][col] == 1 ? 0 : 1; // 旗子标记

        var color = mark[row][col] == 1 ? "red" : "black";
        $(this).children(".cover").css("backgroundColor", color);

        valid(); // 验证是否成功通关。
    });
}

function isMine(row, col) {
    return map[row][col] >= 100;
}

function sweeper(box) {
    var row = box.attr("row");
    var col = box.attr("col");
    if (isMine(row, col)) {
        $(".cover").fadeOut("slow").siblings(".content").fadeIn("slow");
        box.delay(1).queue(function () {
            $("#msg").text("Boom!!!游戏结束.").show("slow");
        });

        return;
    }

    // 如果遇到空白，则自动挖开以此为中心的九宫格，并且递归搜索。
    autoSweeper(row, col);

    valid();
}

function autoSweeper(row, col) {
    var cover = $(".box[row=" + row + "][col=" + col + "]").children(".cover");

    if (cover.is(":hidden"))
        return;

    cover.hide("fast").siblings(".content").show("fast", function () {
        if (map[row][col] != 0)
            return;

        // 如果挖到空白格子，则自动挖四周
        searchAround(row, col, autoSweeper);
    });
}

function searchAround(row, col, action) {
    for (var i = 0; i < at.length; i++) {
        var posX = parseInt(row) + at[i][0];
        var posY = parseInt(col) + at[i][1];
        // 不要超出地图范围
        if (posX < 0 || posY < 0 || posX >= map.length || posY >= map[0].length)
            continue;

        action(posX, posY);
    }
}

function valid() {

    var count = boomNum;
    for (var i = 0; i < num; i++) {
        for (var j = 0; j < num; j++) {
            if (mark[i][j] == 1 && map[i][j] >= 100) {
                count--;
            }
        }
    }

    if (count == 0) {
        $(".cover").fadeOut("fast").siblings(".content").fadeIn("fast");
        $(".cover").delay(1).queue(function () {
            $("#msg").text("恭喜你！成功过关.").show("slow");
        });
    }
}

function timer() {
    // 更新当前系统时间
    var now = new Date().toLocaleString();
    $("#now").text(now);
}