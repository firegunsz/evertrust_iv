using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relation.ViewModels.Home
{
    /// <summary>
    /// 首頁 最新資訊 呈現更新
    /// </summary>
    public class HomeNewsVM
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string NewsTitle { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string NewsContent { get; set; }
    }
}
