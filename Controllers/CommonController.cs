using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineManager.BusinessLogic;
using Relation.Controllers;
using Relation.ViewModels;
using System.Collections.Generic;

namespace OnlineManager.Controllers
{
    public class CommonController : BaseController
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="configuration"></param>
        public CommonController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _config = configuration;
        }
    }
}
