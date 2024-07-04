using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relation.ViewModels
{
    /// <summary>
    /// Payload Data Result 介面
    /// </summary>
    /// <typeparam name="T">Payload 型別</typeparam>
    public interface IResult<T>
    {
        /// <summary>
        /// 表示回傳狀態
        /// </summary>
        bool IsSuccess { get; set; }

        /// <summary>
        /// 錯誤代碼
        /// </summary>
        string ReturnCode { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        string ReturnMessage { get; set; }

        /// <summary>
        /// 回傳資料內容
        /// </summary>
        T Data { get; set; }
    }
}
