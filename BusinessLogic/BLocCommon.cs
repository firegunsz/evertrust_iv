using Dapper;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace OnlineManager.BusinessLogic
{
    public class BLocCommon
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="configuration"></param>
        public BLocCommon(IConfiguration configuration)
        {
            _config = configuration;
        }
    }
}
