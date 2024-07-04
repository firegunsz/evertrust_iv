using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using static OnlineManager.ViewModels.LoginBanner.LoginBanner;
using static OnlineManager.ViewModels.LoginBanner.LoginBannerList;
using static OnlineManager.ViewModels.LoginBanner.LoginBannerAdd;
using System.Diagnostics;

namespace OnlineManager.BusinessLogic.LoginBanner
{
    public class LoginBanner
    {
        private readonly IConfiguration _config;
        private readonly string _conn;
        private readonly IWebHostEnvironment _host;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="configuration"></param>
        public LoginBanner(IConfiguration configuration, IWebHostEnvironment host)
        {
            _config = configuration;
            _host = host;
            _conn = configuration.GetConnectionString("Default");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public GetLoginBannerResponse GetLoginBanner()
        {
            var conn = new SqlConnection(_conn);
            var data = conn.QueryFirst<GetLoginBannerResponse>(@"select top 1 a.*, b.pic_name as l_pic_name, b.pic_sub_name as l_pic_sub_name, b.pic as l_pic,
                    c.pic_name as m_pic_name, c.pic_sub_name as m_pic_sub_name, c.pic as m_pic 
                    from tbl_banner a
                    left join tbl_banner_pic b on a.l_tbl_banner_pic_id = b.id
                    left join tbl_banner_pic c on a.m_tbl_banner_pic_id = c.id
                    order by a.cdate desc");
            return data;
        }

        public bool LoginBannerUpdate(AddDataRequest reqData)
        {
            string l_pic_id = "";
            string m_pic_id = "";

            string sql = @"
                Declare @l_pic_id nvarchar(50)
                Declare @m_pic_id nvarchar(50)
                set @l_pic_id = @l_pic_id_in
                set @m_pic_id = @m_pic_id_in

                insert into tbl_banner (id, type, l_tbl_banner_pic_id, m_tbl_banner_pic_id, l_url, m_url, cdate, mod_user)
                    values(NEWID(), 'login_page', 
                        case when LEN(@l_pic_id) <= 0 then (select top 1 l_tbl_banner_pic_id from tbl_banner order by cdate desc) else @l_pic_id end ,
                        case when LEN(@m_pic_id) <= 0 then (select top 1 m_tbl_banner_pic_id from tbl_banner order by cdate desc) else @m_pic_id end ,
                        @l_url, @m_url, GETDATE(), @mod_user)
                ";
            string sql2 = @"
                insert into tbl_banner_pic (id, pic_name, pic_sub_name, pic, cdate, mod_user)
                    values(@id, @pic_name, @pic_sub_name, @pic, GETDATE(), @mod_user)
                ";
            try
            {

                if (reqData.l_pic_png != null && reqData.l_pic_png.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        reqData.l_pic_png.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        reqData.l_pic = s;
                        Guid myuuid = Guid.NewGuid();
                        l_pic_id = myuuid.ToString();
                        // act on the Base64 data
                    }
                }
                if (reqData.m_pic_png != null && reqData.m_pic_png.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        reqData.m_pic_png.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        reqData.m_pic = s;
                        Guid myuuid = Guid.NewGuid();
                        m_pic_id = myuuid.ToString();
                        // act on the Base64 data
                    }
                }
                var param = new DynamicParameters();
                param.Add("l_pic_id_in", l_pic_id);
                param.Add("m_pic_id_in", m_pic_id);

                param.Add("l_url", reqData.l_url);
                param.Add("m_url", reqData.m_url);
                param.Add("mod_user", reqData.mod_user);

                var param_l_pic = new DynamicParameters();
                if (l_pic_id.Length > 0) { 
                    param_l_pic.Add("id", l_pic_id);
                    param_l_pic.Add("pic_name", reqData.l_pic_png.FileName);
                    param_l_pic.Add("pic_sub_name", Path.GetExtension(reqData.l_pic_png.FileName));
                    param_l_pic.Add("pic", reqData.l_pic);
                    param_l_pic.Add("mod_user", reqData.mod_user);
                }

                var param_m_pic = new DynamicParameters();
                if (m_pic_id.Length > 0)
                {
                    param_m_pic.Add("id", m_pic_id);
                    param_m_pic.Add("pic_name", reqData.m_pic_png.FileName);
                    param_m_pic.Add("pic_sub_name", Path.GetExtension(reqData.m_pic_png.FileName));
                    param_m_pic.Add("pic", reqData.m_pic);
                    param_m_pic.Add("mod_user", reqData.mod_user);
                }

                using (var conn = new SqlConnection(_conn))
                {
                    if (l_pic_id.Length > 0) { conn.Execute(sql2, param_l_pic); }
                    if (m_pic_id.Length > 0) { conn.Execute(sql2, param_m_pic); }
                    conn.Execute(sql, param);
                }

                
            }
            catch (System.Exception)
            {

                return false;
            }
            return true;
        }
    }
}
