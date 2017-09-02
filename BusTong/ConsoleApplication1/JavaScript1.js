/// <reference path="jquery-1.8.2.js" />
var chepic = null;
var lines = null
var counter = 0;
var timerId = null;
//
var whereList = new Array();
var points = new Array();
var globaldata;
var temppoint; // 全局变量用来保存原来的点的位置

function SearchStation(which) {
    var roadline = $("#roadline").val();
    var roadname = $("#roadname").val();
    var registrationmark = $("#registrationmark").val();
    if (which == "station") {
        window.location.href = "ShowData.aspx?which=station&roadline=" + roadline + "&roadname=" + roadname;
    }
    else if (which == "ad") {
        window.location.href = "ShowData.aspx?which=ad&roadline=" + roadline + "&registrationmark=" + registrationmark;
    }
    //$("#needShow").show();
    //$("#BtnSearch").click();

}

function sStation() {
    // $("#needShow").show();
    var roadline = $("#roadline").val();
    var roadname = $("#roadname").val();
    var html = "";
    var url = "Handler.ashx?Method=station&roadline=" + roadline + "&roadname=" + roadname;
    $.get(url, function (data) {
        var data = $.parseJSON(data);

        var jdata = data.data;
        if (jdata.length > 0) {
            for (var i = 0; i < jdata.length; i++) {
                html += "<li ><a href='StationBrand.aspx?TypeLine=Line&StopId=" + jdata[i].StopId + "' target='_blank'><p style='line-height:20px'>";
                html += ' <span style=" color:Orange">站名：' + jdata[i].StationName + '</span><br /><span>环域：' + jdata[i].Area + '</span>&nbsp;&nbsp;&nbsp;&nbsp;<span>区属：' + jdata[i].District + '</span><br /><span>路名：' + jdata[i].RoadName + '</span><br /><span>线路：' + jdata[i].RoadLine + '<%#Eval("RoadLine")%></span>&nbsp;&nbsp;&nbsp;&nbsp;<span>方向：' + jdata[i].PathDirection + '</span><br /><span>站址：' + jdata[i].StationAddress + '<%#Eval("StationAddress")%></span><br /><span>并线：' + jdata[i].lineList + '</span><br />';
                html += "</p></a>";
            }

        }
        else {
            alert("无查询数据！");
        }

        //$("#busCount").text(data.BusCount);
        $("#showStation").html(html);

    });
}

function searchDir() {
    if ($("#slRoadLine").val() != "") {
        $("#ToDirection").empty();
        var options = "<option value=''></option>";
        var str_where = "RoadLine='" + $("#slRoadLine").val() + "'";
        var jsondata = '{table_name:"tblRoadBaseInfo",str_field:"ToDirection",str_where:"' + str_where + '"}';
        var rqurl = '../WebService.asmx/GetFieldByWhere';
        var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
        var Jsonstr = eval('(' + callbackdata + ')');
        var item = Jsonstr.d;
        if (item == null) {
            return false;
        }
        for (var i = 0; i < item.length; i++) {
            options += "<option value=" + item[i] + ">" + item[i] + "</option>";
        }
        $(options).appendTo("#ToDirection");
    }
}

//通过路线名称 行驶方向 查找路线
function SearchRoadLine(dir) {

    //如果查询结果部分是关闭的则在这里打开
    var RoadLine = "";
    var ToDirection = "";
    if (window.location.search != "" && window.location.search != null) {
        RoadLine = getQueryString("slRoadLine");
        //方向
        // ToDirection = getQueryString("ToDirection");
        ToDirection = dir;
    }
    else {
        alert("无效参数");
        return false;
    }

    // $("#sslk").attr("checked", false);
    var TotalDiv = ""; //统计数据
    //线路名

    if (RoadLine == "") {
        window.alert("参数无效!");
        return false;
    }
    if (ToDirection == "") {
        window.alert("参数无效!");
        return false;
    }
    if (ToDirection == "up") {

        $("#up").removeClass("colorGray");
        $("#down").addClass("colorGray");
    }
    else {
        $("#up").addClass("colorGray");
        $("#down").removeClass("colorGray");
    }
    points = new Array();
    var jsondata = '{RoadLine:"' + RoadLine + '",ToDirection:"' + ToDirection + '"}';
    var rqurl = '../WebService.asmx/getRoadLineInfoByRoadLineForMobile';
    var callbackdata = Jquery_Ajax(jsondata, rqurl, false);
    var Jsonstr = $.parseJSON(callbackdata);
    var item = $.parseJSON(Jsonstr.d);
    if (item == null || item == "") {
        alert('当前路线不存在！');
        return false;
    }
    $("#rd").html(item.RoadLine);
    removeAllOverlays();
    globaldata = item;
    var tbl_temp = "";
    var k = 0;
    $("#result").hide();
    /*---将得到的地图数据放入表格中---*/
    for (var j = 0; j < item.StationPointInfos.length; j++) {
        var strLatLon = item.StationPointInfos[j].StrLatlon;
        var labl = item.StationPointInfos[j].PointName; //标签点的名称
        var SETime = item.StationPointInfos[j].SETime; //首末班车时间
        if (j == item.StationPointInfos.length - 1) {
            var RoadNum = item.RoadNum; //路线总数
            var BrandNum = item.BrandNum; //站牌总数
            var RoadCount = 0;
            if (item.RelationsRoadLine != null && item.RelationsRoadLine != "") {
                RoadCount = item.RelationsRoadLine.length
            }

        }
        if (strLatLon != "") {
            var lat = item.StationPointInfos[j].StrLatlon.split(',')[0]; //经度
            var lon = item.StationPointInfos[j].StrLatlon.split(',')[1]; //纬度
            var point = new MPoint(lat, lon); //坐标
            marker = new MMarker(point, new MIcon("../icons/1/icon_03.png", 8, 8));
            marker.setLabel(new MLabel(labl), true); //添加标签
            marker.label.setVisible(false)//隐藏标签

            ////
            var lineList = item.StationPointInfos[j].LineList.split("、");
            var strlineList = "";
            for (var i = 0; i < lineList.length; i++) {
                if (strlineList != "") {
                    //                    strlineList += "、<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                    strlineList += "、" + lineList[i];
                } else {
                    //                    strlineList += "<a href='javascript:;' style='text-decoration: none;' onclick='javascript:searchroadline(\"" + lineList[i] + "\",\"" + item.StationPointInfos[j].StopId + "\",\"" + strLatLon + "\")'>" + lineList[i] + "</a>"
                    strlineList += lineList[i];
                }
            }
            var CaptionNumberInfo = getCaptionNumberInfo(item.StationPointInfos[j]);
            var str_div = "<div class='RoundedCorner' style='font-size:12px; width:240px;'>"
                + "<table id='tablereg'><tr><td style='width:60px'>站址：</td><td >" + item.StationPointInfos[j].Address + "</td></tr><tr><td>并线：</td><td>" + strlineList + "</td></tr>"
                + "<tr ><td style='width:80px;'>设施编号：</td><td>" + CaptionNumberInfo + "</td></tr>"
                + "</tr></table>"
                + "</p><b class='rbottom'><b class='r4'></b><b class='r3'></b><b class='r2'></b><b class='r1'></b></b><div style='display:none;'>[" + labl + "]<div></div>";
            marker.info = new MInfoWindow("当前站点:" + labl, str_div, "20px", "20px");
            var linkecolor = "Black";
            var backgroundimage = "";
            if (j == 0) {
                marker.setIcon(new MIcon("../icons/起点.png", 16, 16), true);
                linkecolor = "Red";
                backgroundimage = "background-image: url(\"image/point.png\");background-repeat: no-repeat;";
            }
            else if (j == item.StationPointInfos.length - 1) {
                marker.setIcon(new MIcon("../icons/终点.png", 16, 16), true);
                linkecolor = "Blue";
            }


            MEvent.addListener(marker, "click", function (omarker) {
                omarker.openInfoWindow();
            });
            MEvent.addListener(marker, "mouseover", function (mk) {
                if (mk.label) mk.label.setVisible(true);
            })
            MEvent.addListener(marker, "mouseout", function (mk) {
                if (mk.label) mk.label.setVisible(false);
            })
            points.push(marker);
            if (k == 0) {
                maplet.addOverlay(marker);
            }
        }
        else {

        }
    }

    //用于存放当前点的集合的数组
    var strlinelatlon = "";
    if (item.LineLatLon != undefined) {

        var points_temp = new Array();
        var LineLatLon = item.LineLatLon.split(';')
        for (var i = 0; i < LineLatLon.length; i++) {

            if (LineLatLon[i] != null && LineLatLon[i] != "" && LineLatLon[i] != undefined) {
                if (i > 0) {
                    strlinelatlon += ";" + LineLatLon[i];
                }
                else {
                    strlinelatlon += LineLatLon[i];
                }
                var lat = LineLatLon[i].split(',')[0];
                var lon = LineLatLon[i].split(',')[1];
                var point = new MPoint(lat, lon);
                points_temp.push(point);
            }
        }
        //将地图移动到中心位置
        var lat = item.CenterPoint.split(',')[0];
        var lon = item.CenterPoint.split(',')[1]
        maplet.centerAndZoom(new MPoint(lat, lon), 10);
        $("#linecenter").attr("value", item.CenterPoint);
        //创建笔刷将所有的点连接起来
        var brush = new MBrush();
        brush.arrow = false;
        brush.stroke = 4;
        brush.fill = false;
        brush.color = 'blue';
        brush.bgcolor = 'red';
        var polyline = new MPolyline(points_temp, brush);
        maplet.addOverlay(polyline);
        //注释代码为线路轨迹回放代码
        //lines = item.LineLatLon.split(';');
        //startORI();

    }
    //$("#roadlines").val(strlinelatlon);

    //  $("#resultcontent").html(tbl_temp);
    // $("#showChice").hide();
    // $("#showMap").show();
    //显示 站名
    showstationname(-1);;
}

function getCaptionNumberInfo(oStop) {
    var stationinfo = "";
    var polesinfo = "";
    if (oStop.FacilitiesList != undefined) {
        var Facs = oStop.FacilitiesList;
        $(Facs).each(function (k) {
            if (Facs[k].FacilitiesType == "候车亭") {
                if (stationinfo != "") {
                    stationinfo += "、" + Facs[k].CapitalNumber;
                } else {
                    stationinfo += Facs[k].CapitalNumber;
                }

            }
            else {
                if (polesinfo != "") {
                    polesinfo += "、" + Facs[k].CapitalNumber;
                } else {
                    polesinfo += Facs[k].CapitalNumber;
                }
            }
        });
    }
    var CaptionNumberInfo = "";
    if (stationinfo != "") {
        CaptionNumberInfo += stationinfo + "（亭）"
    }
    if (polesinfo != "") {
        CaptionNumberInfo += polesinfo + "（牌）"
    }
    return CaptionNumberInfo;
}

//显示站名
function showstationname(showtype) {

    if (points != null) {
        for (var i = 0; i < points.length; i++) {
            var omk = points[i];
            if (showtype == "-1") {
                omk.label.setVisible(false);
            }
            else {
                var Station = $("#cbxStation").attr("checked");
                var Pole = $("#cbxPole").attr("checked");
                if (showtype == "0") {
                    var info = omk.info.content;
                    var lbl = info.substring(info.indexOf('[') + 1, info.indexOf(']'));
                    omk.setLabel(new MLabel(lbl));
                    omk.label.setVisible(true);
                }
                else if (showtype == "1") {
                    var info = omk.info.content;
                    var txt = info.replace(/<\/?.+?>/g, "").replace('资产编号', '*').replace("站点照片", "#");
                    var lbl = txt.substring(txt.indexOf('*') + 1, txt.indexOf('#'));
                    if ((Station && Pole) || (!Station && !Pole)) {
                        omk.setLabel(new MLabel(lbl));
                    }
                    else if (Station) {
                        if (lbl.indexOf('亭') > -1 && lbl.indexOf('牌') > -1) {
                            var infos = lbl.split('）');
                            if (infos[0].indexOf('亭') > -1) {
                                omk.setLabel(new MLabel(infos[0].replace('（亭', "")))
                            }
                            else if (infos[1].indexOf('亭') > -1) {
                                omk.setLabel(new MLabel(infos[1].replace('（亭', "")))
                            }
                        }
                        else {
                            omk.setLabel(new MLabel(lbl.replace('（亭）', "")));
                        }
                    }
                    else if (Pole) {
                        if (lbl.indexOf('亭') > -1 && lbl.indexOf('牌') > -1) {

                            var infos = lbl.split('）');
                            if (infos[0].indexOf('牌') > -1) {
                                omk.setLabel(new MLabel(infos[0].replace('（牌', "")))
                            }
                            else if (infos[1].indexOf('牌') > -1) {
                                omk.setLabel(new MLabel(infos[1].replace('（牌', "")))
                            }
                        }
                        else {
                            omk.setLabel(new MLabel(lbl.replace('（牌）', "")));
                        }
                    }

                    omk.label.setVisible(true);
                }
            }
        }
    }
    maplet.refresh();
}

//正则获取url的值
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    // name = name.replace("?", "").split('&');
    var r = window.location.search.substr(1).match(reg);
    // var r = name.split('=');
    if (r != null && r != "") return r[2]; return null;
}

//控制文本显示、隐藏
function ShowHiden(show, hide) {
    $("#" + show).css("display", "block");
    $("#" + show).val("");
    $("#" + hide).css("display", "none");
    $("#" + hide).val("");
}

function ShowHiden1(show, hide) {
    $("#" + show).css("display", "block");
    //$("#" + show).val("");
    $("#" + hide).css("display", "none");
    //  $("#" + hide).val("");
}


//function refesh() {

//    setInterval(load, 30000);
//}

//function load()
//{
//    location.reload();
//}
