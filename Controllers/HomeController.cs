using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Relation.BusinessLogic.Home;
using Relation.Commons.Attibute;
using Relation.Commons.Extensions;
using Relation.ViewModels;
//using Relation.ViewModels.Admin;
using Relation.ViewModels.Home;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static OnlineManager.ViewModels.Home.MenuVM;
using static Relation.ViewModels.Login.SsoVM;

namespace Relation.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="configuration"></param>
        public HomeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _config = configuration;
        }

        #region View

        [TypeFilter(typeof(AuthCheck))]
        public IActionResult Index()
        {
            return View();
        }

        #endregion

        /// <summary>
        /// 取得最新資訊列表(單筆/多筆)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [TypeFilter(typeof(AuthCheck))]
        public ActionResult<Result<List<HomeNewsVM>>> GetNewsList(HomeNewsVM request)
        {
            //return Success();

            // 取得服務回傳內容
            var response = new HomeNews(_config).GetList(request);

            // 檢查是否有回傳資料
            if (response.Count() > 0)
            {
                // 回傳 List Data
                return Success(response);
            }
            if (response.Count() == 0)
            {
                return Success();
            }
            else
            {
                return Fail("HC01", "取得最新資訊失敗");
            }
        }


        ///// <summary>
        ///// 取得Menu權限
        ///// </summary>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult<Result<List<JstreeList>>> GetJstreeData(MenuListRequest request)
        {
            request.EmpNo = Emp.empNo;

            // 取得服務回傳內容
            var response = new HomeNews(_config).GetJstreeData(request);

            response = OdData(response);

            // 檢查是否有回傳資料
            if (response.Count() > 0)
            {
                // 回傳 List Data
                return Success(response);
            }
            if (response.Count() == 0)
            {
                return Success();
            }
            else
            {
                return Fail("HC01", "取得最新資訊失敗");
            }
        }
        public List<JstreeList> OdData(List<JstreeList> list)
        {
            List<JstreeList> new_list = new List<JstreeList>();
            AddChlid("#", list, ref new_list);
            return new_list;
        }
        public string AddChlid(string k , List<JstreeList> list,ref List<JstreeList> new_list)
        {
            var cl = list.Where(x => x.parent == k);
            if (cl.Count() > 0)
            {
                foreach (var c in cl)
                {
                    new_list.Add(c);
                    if (list.Where(x => x.parent == c.id).Count() > 0) { 
                        AddChlid(c.id, list, ref new_list); 
                    }
                }
            }
            return "";
        }

    }
}
