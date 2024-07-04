using System;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using static Relation.ViewModels.Login.SsoVM;

namespace OnlineManager.Commons.Attibute
{
    public class AuthAccess : ActionFilterAttribute
    {
        readonly string type;
        readonly string path;
        public AuthAccess(string _url, string _type)
        {
            this.path = _url;
            this.type = _type;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            var str_authData = Newtonsoft.Json.JsonConvert.DeserializeObject<SsoEmpData>(filterContext.HttpContext.Session.GetString("UserData")).empAuth;
            JArray jA = JArray.Parse(str_authData);
            var auth = jA.FirstOrDefault(x => x["url"]?.ToString() == path)?[type]?.ToString() == "Y";
        
            if (!auth)
            {
                var result = new ContentResult();
                result.Content = "您沒有此單元權限";
                filterContext.Result = result;
            }
      
            return;
        }

    }

}
