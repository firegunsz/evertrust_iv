using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relation.Commons.Settings
{
    public class TokenPayload
    {
        /// <summary>
        /// 
        /// </summary>
        public string EmpNo { get; set; }
  
        /// <summary>
        /// 是否登入成功
        /// </summary>
        public bool IsLogin { get; set; }

        /// <summary>
        /// Token 過期時間
        /// </summary>
        public int ExpireTime { get; set; }

        /// <summary>
        /// Token狀態 正常為空值
        /// </summary>
        public string Status { get; set; }
    }
}
