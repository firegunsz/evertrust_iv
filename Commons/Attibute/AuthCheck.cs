using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Relation.Commons.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Relation.ViewModels.Login.SsoVM;

namespace Relation.Commons.Attibute
{
    public class AuthCheck : IActionFilter
    {
        private readonly IConfiguration _config;

        public AuthCheck(IConfiguration configuration)
        {
            _config = configuration;
        }
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool validate = true;

            // 檢查 EmpNO 內容是否存在且長度8碼不為空
            var empData = filterContext.HttpContext.Session.GetObject<SsoEmpData>("UserData");

            if (empData != null)
            {
                if (!(empData.empNo.ToString().Trim().Length == 8))
                {
                    validate = false;
                }
            }
            else
            {
                validate = false;
            }

            if (!validate)
            {
                var values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Login"
                });

                filterContext.HttpContext.Session.SetObject("LoginError", "登入狀態異常，請重新登入。");

                filterContext.Result = new RedirectToRouteResult(values);
            }           
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

    }
}
