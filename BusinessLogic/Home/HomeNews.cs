using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
//using Relation.ViewModels.Admin;
using Relation.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OnlineManager.ViewModels.Home.MenuVM;

namespace Relation.BusinessLogic.Home
{
    /// <summary>
    /// 處理首頁最新資訊相關內容
    /// </summary>
    public class HomeNews
    {
        private readonly IConfiguration _config;
        private readonly string _conStr;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="configuration"></param>
        public HomeNews(IConfiguration configuration)
        {
            _config = configuration;
            _conStr = _config.GetConnectionString("Default");
        }

        /// <summary>
        /// 取得最新資訊列表(id為0代表全取)
        /// </summary>
        /// <param name="newsData"></param>
        /// <returns></returns>
        public List<HomeNewsVM> GetList(HomeNewsVM newsData)
        {
            // 建立連線並操作
            using (var conn = new SqlConnection(_conStr))
            {
                // DB Link 字串
                string Leave_LinkStr = _config.GetValue<string>("ConnectionDBLink:LEAVE");

                // 參數
                var para = new DynamicParameters();

                string sql = $@"SELECT NewsId,NewsTitle,NewsContent,NewsStatus,ModUser,ModTime
                                FROM OnlineManagerNews
                                WHERE NewsStatus <> 'D' AND NewsStatus <> 'H' ";

                // 執行Query
                var result = conn.Query<HomeNewsVM>(sql, para);

                return result.ToList();
            }
        }

        /// <summary>
        /// 取得Menu權限
        /// </summary>
        /// <param name="newsData"></param>
        /// <returns></returns>
        public List<JstreeList> GetJstreeData(MenuListRequest Data)
        {
            // 建立連線並操作
            using (var conn = new SqlConnection(_conStr))
            {
                // DB Link 字串
                string Leave_LinkStr = _config.GetValue<string>("ConnectionDBLink:LEAVE");

                // 參數
                var para = new DynamicParameters();

                string sql = $@" SELECT 
                                	  a.prg_id as id,
                                      a.parent as parent,
                                      a.des1 as text,
                                      a.url as href,
                                      a.order_no as Sort
                                 FROM tbl_emp_menu a
                                 INNER JOIN  (
                                 SELECT prg_id FROM ecusermenu
                                 WHERE user_id = (select Top 1 join_group from ecusermenu where user_id = @empNo)
                                 ) b on a.prg_id = b.prg_id
                                 order by a.parent,a.order_no asc ";

                para.Add("empNo", Data.EmpNo, System.Data.DbType.String);

                // 執行Query
                var result = conn.Query<JstreeList>(sql, para);

                return result.ToList();
            }
        }       
    }
}
