using System.Web;
using System.Web.Mvc;

namespace _23DH110152_PhamNguyenQuocAnh_KTBE
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
