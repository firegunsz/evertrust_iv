using CsApiBaseHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog;
using Relation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Relation.Middlewares
{
    /// <summary>
    /// 處理系統所有的Exception
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        private readonly ICsExceptionEmail _csExceptionEmail;

        private readonly Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger("FileLog");

        public ExceptionMiddleware(RequestDelegate next , IConfiguration configuration, ICsExceptionEmail csExceptionEmail)
        {
            _config = configuration;
            _next = next;
            _csExceptionEmail = csExceptionEmail;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ExceptionLogAndMail(httpContext, ex);

            httpContext.Response.ContentType = "application/json";
            //httpContext.Response.StatusCode = 200;

            //var defaultError = new Result
            //{
            //    IsSuccess = false,
            //    ReturnCode = "9999",
            //    ReturnMessage = ex.Message.ToString()
            //};
            if (_config.GetValue<string>("Environment") != "LOCAL")
            {
      
                httpContext.Response.Redirect($"{_config.GetValue<string>("IISRoot")}/HandlePage/maintain");
            }
            else
            {
                httpContext.Response.Redirect("/HandlePage/maintain");
            }
        }

        private void ExceptionLogAndMail(HttpContext context, Exception ex)
        {
            string exceptionTime = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            var errMsg = new List<string>();

            errMsg.Add(Environment.NewLine);

            errMsg.Add(string.Format("[DateTime.Now:{0}]", exceptionTime));

            // 取得 Host IP
            errMsg.Add(string.Format("[Server IP:{0}]", GetHostIp() ));

            // 取得 Host NAME
            errMsg.Add(string.Format("[Server Name:{0}]", Dns.GetHostName()));

            foreach (var item in context.Items)
            {
                errMsg.Add(string.Format("[{0}:{1}]", item.Key, item.ToString()));
            }

            errMsg.Add("[錯誤訊息]" + ex.Message);

            errMsg.Add("[錯誤堆疊]" + ex.StackTrace);

            // 寫LOG
            logger.Error(string.Join(Environment.NewLine, errMsg));

            //發信
            _csExceptionEmail.SendMailByNetMail(string.Join("<br />", errMsg));
            // SendSmtpMail(content);

            //Notify
        }

        /// <summary>
        /// 取得 Server IP
        /// </summary>
        /// <returns></returns>
        private string GetHostIp() 
        {
            string localIP = string.Empty;

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);

                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;

                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }
    }
}
