using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relation.ViewModels.Login
{
    public class SsoVM
    {
        /// <summary>
        /// SSO API REQUEST
        /// </summary>
        public class SsoApiReq
        {
            /// <summary>
            /// 
            /// </summary>
            public string empno { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string pd { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string systemid { get; set; }
        }

        /// <summary>
        /// SSO API RESPONSE
        /// </summary>
        public class SsoApiRes
        {
            /// <summary>
            /// 建構函式
            /// </summary>
            public SsoApiRes()
            {
                // 產生物件
                data = new SsoEmpData();
            }

            /// <summary>
            /// 伺服器時間
            /// </summary>
            public DateTime serverTime { get; set; }

            /// <summary>
            /// 回傳碼 S001 表示成功
            /// </summary>
            public string code { get; set; }

            /// <summary>
            /// SSO 登入 錯誤回傳訊息 (正確為空)
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// 員工資訊
            /// </summary>
            public SsoEmpData data { get; set; }
        }


        /// <summary>
        /// SSO API ERRORTIMES REQUEST
        /// </summary>
        public class SsoApiErrorTimesReq
        {
            /// <summary>
            /// 
            /// </summary>
            public string empno { get; set; }
        }

        /// <summary>
        /// SSO API ERRORTIMES RESPONSE
        /// </summary>
        public class SsoApiErrorTimesRes
        {
            /// <summary>
            /// 主機時間
            /// </summary>
            public DateTime serverTime { get; set; }

            /// <summary>
            /// Error代碼
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// Error回傳訊息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 錯誤次數
            /// </summary>
            public object data { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        public class SsoEmpData
        {
            /// <summary>
            /// 
            /// </summary>
            public string adAccount { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string empNo5 { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string empNo { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string deptNo { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string deptName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string email { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string systemid { get; set; }

            /// <summary>
            /// API KEY
            /// </summary>
            public string apiKey { get; set; }

            //===============================================================

            /// <summary>
            /// 
            /// </summary>
            public string positionName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string empId { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string empBirthday { get; set; }

            public string empAuth { get; set; }
        }
    }
}