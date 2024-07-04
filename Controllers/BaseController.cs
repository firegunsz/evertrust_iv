using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Relation.Commons.Extensions;
using Relation.Commons.Settings;
using Relation.ViewModels;

using static Relation.ViewModels.Login.SsoVM;

namespace Relation.Controllers
{
    public class BaseController : Controller
    {
        public SystemConfig sysConfig { set; get; } = new SystemConfig();
        public readonly SsoEmpData Emp;

        /// <summary>
        /// BaseController 建構函式 處理Config取值
        /// </summary>
        /// <param name="configuration"></param>
        public BaseController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            // 取得正式與測試環境連線字串 EMPTRAN
            sysConfig.ConnStr.Add("Default", configuration.GetSection("ConnectionStrings").GetValue<string>("Default"));

            // 取得目前環境設定資訊
            sysConfig.ConfigStr.Add("Env", configuration.GetValue<string>("Environment"));
            sysConfig.ConfigStr.Add("IISRoot", configuration.GetValue<string>("IISRoot"));
            // 資料庫前綴 LinkServer
            sysConfig.ConfigStr.Add("LEAVE", configuration.GetSection("ConnectionDBLink").GetValue<string>("LEAVE"));
            sysConfig.ConfigStr.Add("EMPTRAN", configuration.GetSection("ConnectionDBLink").GetValue<string>("EMPTRAN"));
            //掛入使用者資訊
      
            Emp = (httpContextAccessor.HttpContext?.Session.TryGetValue("UserData", out byte[] a) ?? false) ? httpContextAccessor.HttpContext.Session.GetObject<SsoEmpData>("UserData") : new SsoEmpData();

        }

        #region 基本回傳格式

        /// <summary>
        /// 回傳帶有成功狀態的結果
        /// </summary>
        /// <returns></returns>
        protected OkObjectResult Success()
        {
            return Ok(new Result { IsSuccess = true });
        }
        /// <summary>
        /// 回傳帶有成功狀態的結果
        /// </summary>
        /// <returns></returns>
        protected OkObjectResult Success(string returnMessage)
        {
            return Ok(new Result
            {
                IsSuccess = true,
                ReturnMessage = returnMessage
            });
        }
        /// <summary>
        /// 回傳帶有成功狀態且自訂訊息的Payload結果
        /// </summary>
        /// <param name="data">Payload</param>
        /// <returns></returns>
        protected ActionResult<Result<TData>> Success<TData>(TData data) where TData : class
        {
            return Ok(new Result<TData> { IsSuccess = true, Data = data });
        }

        /// <summary>
        /// 回傳帶有失敗狀態且自訂訊息的Payload結果
        /// </summary>
        /// <param name="returnCode">回傳代碼</param>
        /// <param name="returnMessage">回傳訊息</param>
        /// <returns></returns>
        protected OkObjectResult Fail(string returnCode, string returnMessage)
        {
            return Ok(new Result
            {
                IsSuccess = false,
                ReturnCode = returnCode,
                ReturnMessage = returnMessage
            });
        }
        protected OkObjectResult Fail(string returnMessage)
        {
            return Ok(new Result
            {
                IsSuccess = false     ,
                ReturnMessage = returnMessage
            });
        }
        #endregion
    }
}
