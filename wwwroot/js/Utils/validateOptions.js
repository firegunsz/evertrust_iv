var va_default =
{
    rules: {
        name: {
            required: true,
            minlength: 2
        },
        username: {
            required: true,
            range: [6, 12]
        },
        password: {
            required: true,
            minlength: 6
        },
        confirm_password: {
            required: true,
            minlength: 6,
            equalTo: "#password"
        },
        email: {
            required: true,
            email: true
        },
        dropdown: {
            required: true
        },
        textbox: {
            required: true
        }
    },
    messages: {
        name: {
            required: "姓名為必填",
            minlength: "姓名至少要有兩個字"
        },
        username: {
            required: "帳號為必填",
            range: $.validator.format("您的帳號必須 {0} 到 {1} 個字元")
        },
        password: {
            required: "密碼為必填",
            minlength: $.validator.format("您的密碼必須至少 {0} 個字元")
        },
        confirm_password: {
            required: "確認密碼為必填",
            minlength: $.validator.format("您的密碼必須至少 {0} 個字元"),
            equalTo: "請輸入與以上一樣的密碼"
        },
        email: "請輸入正確的 E-mail 地址",
        dropdown: "請選擇一個",
        textbox:"必填"

    }
}