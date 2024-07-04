using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Relation.ViewModels
{
    /// <summary>
    /// 自訂義回傳Result
    /// </summary>
    [Serializable]
    public class Result<T> : IResult<T> where T : class
    {
        /// <summary>
        /// 表示回傳狀態
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 回傳代碼
        /// </summary>
        public string ReturnCode { get; set; }

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string ReturnMessage { get; set; }

        /// <summary>
        /// 回傳資料內容
        /// </summary>
        public T Data { get; set; }
    }

    [Serializable]
    public class Result : Result<object>
    {
    
    }

}