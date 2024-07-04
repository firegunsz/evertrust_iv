using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog;
using Relation.Commons.Extensions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static Relation.ViewModels.Login.SsoVM;

namespace Relation.Middlewares
{
    // to  do header  sessionid ip sysid  / servername post路徑 request內容 reponse內容 / token內容 => 8碼員編 islogin / request time  response time  (看能否記成一筆不要分兩筆)


    public class LogMiddleware
    {
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        Logger logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger("DbReqLog");


        public LogMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _config = configuration;
            _next = next;
        }

        /// <summary>
        /// USE NLOG WRITE LOG INTO TXT(NLOG SETTING) AND DATABASE
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            string requestBody = await GetRequestBody(httpContext.Request);

            if (!httpContext.Items.ContainsKey("ClientIP"))
            {
                httpContext.Items.Add("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
            }

            if (!httpContext.Items.ContainsKey("RequestHeaders"))
            {
                httpContext.Items.Add("RequestHeaders", System.Text.Json.JsonSerializer.Serialize(httpContext.Request.Headers));
            }

            if (!httpContext.Items.ContainsKey("RequestBody"))
            {
                httpContext.Items.Add("RequestBody", requestBody);
            }

            var EMPNO = httpContext.Session.GetObject<SsoEmpData>("UserData")?.empNo;
            if (EMPNO == null) { EMPNO = ""; }

            if (!httpContext.Items.ContainsKey("EmpNo"))
            {
                httpContext.Items.Add("EmpNo", EMPNO);
            }

            // Remove 密碼資訊
            if (requestBody.Contains("inputPass")) { requestBody = requestBody.Substring(0, 16); } // 只取帳號 

            // 處理上傳檔案log
            if(requestBody.Contains("filename=")) { requestBody = "UploadFile"; }

            logger.WithProperty("ClientIP", httpContext.Items.ContainsKey("ClientIP") ? httpContext.Items["ClientIP"].ToString().Trim() : "")
                  .WithProperty("SessionID", httpContext.Items.ContainsKey("SessionID") ? httpContext.Items["SessionID"].ToString().Trim() : "")
                  .WithProperty("UserAgent", httpContext.Items.ContainsKey("UserAgent") ? httpContext.Items["UserAgent"].ToString().Trim() : "")
                  .WithProperty("RequestUrl", string.Format($"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}"))
                  .WithProperty("Method", httpContext.Request.Method)
                  .WithProperty("Headers", System.Text.Json.JsonSerializer.Serialize(httpContext.Request.Headers).Trim())
                  .WithProperty("RequestBody", requestBody)
                  .WithProperty("ResponseBody", "") // 先給一個Default空值 回傳時再寫入實際內容
                  .WithProperty("ServerName", httpContext.Request.Host)
                  .WithProperty("SysId", httpContext.Items.ContainsKey("sysid") ? httpContext.Items["sysid"].ToString() : "Relation")
                  .WithProperty("EmpNo", httpContext.Items.ContainsKey("EmpNo") ? httpContext.Items["EmpNo"].ToString() : "")
                  .Info("Request");

            httpContext.Request.Body.Position = 0;



            var originalBodyStream = httpContext.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                //var response = httpContext.Response;

                //response.Body = responseBody;

                await _next(httpContext);

                //string responseBodyContent = null;

                //responseBodyContent = await FormatResponse(response);

                //Logger logger_all = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetLogger("FileLog");

                //logger_all
                //    .WithProperty("RequestUrl", string.Format("{0}://{1}{2}{3}", httpContext.Request.Scheme, httpContext.Request.Host, httpContext.Request.Path, httpContext.Request.QueryString))
                //    .WithProperty("Path", string.Format($"{httpContext.Request.Method} - {httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}"))
                //    .WithProperty("Body", responseBodyContent)
                //    .WithProperty("Headers", System.Text.Json.JsonSerializer.Serialize(httpContext.Request.Headers))
                //    .Info("Response");

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }


        private async Task<string> GetRequestBody(HttpRequest request)
        {
            var body = request.Body;
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;
            return bodyAsText;
        }


        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}
