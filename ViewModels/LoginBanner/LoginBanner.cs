using System;
using System.Collections.Generic;
using static OnlineManager.ViewModels.CommonVM;
using Microsoft.AspNetCore.Http;

namespace OnlineManager.ViewModels.LoginBanner
{
    public class LoginBanner
    {
    } 
    public class LoginBannerList
    {
        public class GetLoginBannerResponse
        {
            public string id { set; get; }
            public string type { set; get; }
            /// <summary>
            /// 電腦版(ID)
            /// </summary>
            public string l_tbl_banner_pic_id { set; get; }
            /// <summary>
            /// 手機版(ID)
            /// </summary>
            public string m_tbl_banner_pic_id { set; get; }
            /// <summary>
            /// 電腦版(連結位置)
            /// </summary>
            public string l_url { set; get; }
            /// <summary>
            /// 手機版(連結位置)
            /// </summary>
            public string m_url { set; get; }
            public string cdate { set; get; }

            public string mod_user { set; get; }
            /// <summary>
            /// 電腦版(圖片名稱)
            /// </summary>
            public string l_pic_name { set; get; }
            /// <summary>
            /// 電腦版(副檔名)
            /// </summary>
            public string l_pic_sub_name { set; get; }
            /// <summary>
            /// 電腦版(圖片)(BASE64)
            /// </summary>
            public string l_pic { set; get; }
            /// <summary>
            /// 手機版(圖片名稱)
            /// </summary>
            public string m_pic_name { set; get; }
            /// <summary>
            /// 手機版(副檔名)
            /// </summary>
            public string m_pic_sub_name { set; get; }
            /// <summary>
            /// 手機版(圖片)(BASE64)
            /// </summary>
            public string m_pic { set; get; }
        }
    }
    public class LoginBannerAdd
    {
        public class AddDataRequest
        {
            /// <summary>
            /// 電腦版(連結位置)
            /// </summary>
            public string l_url { set; get; }
            /// <summary>
            /// 手機版(連結位置)
            /// </summary>
            public string m_url { set; get; }

            public string mod_user { set; get; }
            /// <summary>
            /// 電腦版(圖片名稱)
            /// </summary>
            public string l_pic_name { set; get; }
            /// <summary>
            /// 電腦版(副檔名)
            /// </summary>
            public string l_pic_sub_name { set; get; }
            /// <summary>
            /// 電腦版(圖片)(BASE64)
            /// </summary>
            public string l_pic { set; get; }
            /// <summary>
            /// 手機版(圖片名稱)
            /// </summary>
            public string m_pic_name { set; get; }
            /// <summary>
            /// 手機版(副檔名)
            /// </summary>
            public string m_pic_sub_name { set; get; }
            /// <summary>
            /// 手機版(圖片)(BASE64)
            /// </summary>
            public string m_pic { set; get; }

            public IFormFile l_pic_png { set; get; }
            public IFormFile m_pic_png { set; get; }
        }
    }
}
