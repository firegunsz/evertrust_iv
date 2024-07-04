using static OnlineManager.ViewModels.CommonVM;

namespace OnlineManager.ViewModels.Home
{
    public class MenuVM
    {
        public class JstreeList
        {           
            public string id { get; set; }
            public string parent { get; set; }
            public string text { get; set; }
            public string href { get; set; }
            public int  Sort { get; set; }
        }
        public class MenuListRequest : TableBase
        {
            public string EmpNo { get; set; }
            public string bank { get; set; }
            public string txt_code { get; set; }
            public string txt_name { get; set; }
        }
    }
}
