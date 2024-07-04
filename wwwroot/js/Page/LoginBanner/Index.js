$(function () {
    Loadtype(true);
    //綁定search按鈕
    $("#search").click(() => {
        Search()
    })
})
function Loadtype() {
    LoadTable()
}
/**
 * 查詢按鈕
 * */
function Search() {
    if (!$.fn.DataTable.isDataTable('#table')) {
        LoadTable()
    } else {
        $("#table").DataTable().ajax.reload();
    }
}

function LoadTable() {
    $.post("/LoginBanner/GetLoginBannerList").done(rspData => {

        var l_pic = "";
        var l_pic_sub_name = "";
        var m_pic = "";
        var m_pic_sub_name = "";

        if (rspData && rspData.isSuccess) {
            Object.keys(rspData.data).forEach(i => {
                if (i == "l_pic" || i == "m_pic" || i == "l_pic_sub_name" || i == "m_pic_sub_name") {
                    if (i == "l_pic") {
                        l_pic = rspData.data[i];
                    } else if (i == "m_pic") {
                        m_pic = rspData.data[i];
                    } else if (i == "l_pic_sub_name") {
                        l_pic_sub_name = rspData.data[i];
                    } else if (i == "m_pic_sub_name") {
                        m_pic_sub_name = rspData.data[i];
                    }
                } else {
                    $(`#${i}`).val(rspData.data[i]);
                }
            })

            $("#l_pic").prop("src", "data:image/" + l_pic_sub_name + ";base64," + l_pic).show();
            $("#m_pic").prop("src", "data:image/" + m_pic_sub_name + ";base64," + m_pic).show();
        }
    })
}

function Update() {
    if (confirm("確定要更新?") && $("#EditForm").valid()) {

        let form = document.getElementById("EditForm");
        let reqData = new FormData();
        Array.from(form.elements).forEach(item => {
            reqData.append(item.name, item.type == "file" ? item.files[0] : item.value)
        })
        console.log("======== 001 ")
        console.log(reqData)
        $.ajax({
            url: "/LoginBanner/LoginBannerUpdate",
            type: "POST",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: reqData
        }).done(rspData => {
            if (rspData.isSuccess) {
                alert("更新成功")
                LoadTable()
            } else {
                alert(rspData.returnMessage)
            }

        }).fail(() => {
            alert("系統錯誤，請洽管理員")
        })
    }
}

function fileChange(e) {
    if (e.files[0]) {
        $(`label[for=${e.id}].custom-file-label`).text(e.files[0].name);
    } else {
        $(`label[for=${e.id}].custom-file-label`).text("請選擇");
    }
}