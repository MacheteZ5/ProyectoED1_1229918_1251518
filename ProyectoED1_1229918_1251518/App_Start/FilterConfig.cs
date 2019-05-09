using System.Web;
using System.Web.Mvc;

namespace ProyectoED1_1229918_1251518
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
