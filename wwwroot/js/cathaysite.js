// 根據時間戳 取得uuid
function get_uuid() {

    var d = Date.now();

    if (typeof performance !== 'undefined' && typeof performance.now === 'function') {
        d += performance.now(); //use high-precision timer if available
    }

    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {

        var r = (d + Math.random() * 16) % 16 | 0;

        d = Math.floor(d / 16);

        return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);

    });
}

// 取得網址根路徑
function getRootPath() {

    var pathName = window.location.pathname.substring(1);

    var webName = pathName == '' ? '' : pathName.substring(0, pathName.indexOf('/'));

    // true=本機TEST用 只會有Localhost根目錄
    LocalHost = true;

    if (LocalHost) {

        return window.location.protocol;
    }
    // =================================

    if (webName == "") {
        return window.location.protocol + '//' + window.location.host;
    }
    else {
        return window.location.protocol + '//' + window.location.host + '/' + webName;
    }
}

// 確認cookie是否存在 並回傳內容函式
function chkCookie(name) {
    var match = document.cookie.match(RegExp('(?:^|;\\s*)' + name + '=([^;]*)')); return match ? match[1] : null;
}

// 設定 cookie value (過期時間應該改小時)
function setCookie(cname, cvalue, exdays) {

    const d = new Date();

    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));

    let expires = "expires=" + d.toUTCString();

    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

// ======================================================

// 日期檢核
function JsCheckDate(data) {

    var date = data.split('/');

    var Year = date[0];
    var Month = date[1];
    var Day = date[2];

    if (Year.length != 4) { return false; }
    if (Month.length != 2) { return false; }
    if (Day.length != 2) { return false; }

    // 純數字
    var reg = /^[0-9\s]*$/;
    if (!reg.test(Year)) { return false; }
    if (!reg.test(Month)) { return false; }
    if (!reg.test(Day)) { return false; }

    // 範圍
    if (Number(Year) < 1911) { return false; }
    if (Number(Year) > 3000) { return false; }
    if (Number(Month) < 1) { return false; }
    if (Number(Month) > 12) { return false; }
    if (Number(Day) < 1) { return false; }
    if (Number(Day) > 31) { return false; }

    // TODO 檢查大小月 閏年

    return true;
}

// 數字檢核
function JsCheckNumber(data) {

    var regex = /^[0-9\s]*$/;
    if (regex.test(data)) { return false; }

    return true;
}

// 英數字檢核
function JsCheckEngNum(data) {

    var regex = new RegExp("^[a-zA-Z0-9 ]+$");
    if (regex.test(data)) { return false; }

    return true;
}

// 欄位不可為空
function JsCheckEmpty(data) {

    if (!$.trim(data.replace(/\　/g, ""))) {
        return false;
    }

    return true;
}

// E-mail
function JsCheckEmail(data) {

    const re = /^(([.](?=[^.]|^))|[\w_%{|}#$~`+!?-])+@(?:[\w-]+\.)+[a-zA-Z.]{2,63}$/;
    if (!re.test(data)) {
        return false;
    }

    return true;
}

// 身分證字號
function JsCheckID(data) {

    //建立字母分數陣列(A~Z)
    var city = new Array(1, 10, 19, 28, 37, 46, 55, 64, 39, 73, 82, 2, 11, 20, 48, 29, 38, 47, 56, 65, 74, 83, 21, 3, 12, 30)

    data = data.toUpperCase();

    //使用「正規表達式」檢驗格式
    if (data.search(/^[A-Z](1|2)\d{8}$/i) == -1) {

        return false;
    }
    else {
        //將字串分割為陣列(IE必需這麼做才不會出錯)
        data = data.split('');

        //計算總分
        var total = city[data[0].charCodeAt(0) - 65];
        for (var i = 1; i <= 8; i++) {
            total += eval(data[i]) * (9 - i);
        }

        //補上檢查碼(最後一碼)
        total += eval(data[9]);

        //檢查比對碼(餘數應為0);
        return ((total % 10 == 0));
    }

    return true;
}


//共用，call後台預覽畫面
function PreviewMD(id, type) {

    $.post("/Notify/preview", {
        id: id,
        type: type
    })
        .done((data, textStatus, jqXHR) => {
            var newtab = window.open("", '_blank');
            newtab.document.write(data);
        })

}

jQuery.ajaxSetup({
    beforeSend: function (xhr, setting) {
    
        let url = new URL(window.location.href);
        if (setting.dataType == "script") {
  

        } else {
            setting.url = URLJoin(url.origin, root, setting.url)
        }

    },
    dataFilter: function (data, type) {

        let url = new URL(window.location.href);

        if (data.indexOf('您沒有此單元權限') >= 0) {
            location.href = URLJoin(url.origin, root, "/HandlePage/error2");
        } else {
            return data;
        }

    }
});
const URLJoin = (...args) =>
    args
        .join('/')
        .replace(/[\/]+/g, '/')
        .replace(/^(.+):\//, '$1://')
        //   .replace(/^file:/, 'file:/')
        .replace(/\/(\?|&|#[^!])/g, '$1')
        .replace(/\?/g, '&')
        .replace('&', '?');