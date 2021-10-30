using System.Web.Mvc;

namespace OnDemandService.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { AreaName = "Admin",  controller = "User", action = "LoginProfessional", id = UrlParameter.Optional },
                namespaces: new[] { "OnDemandService.Areas.Admin.Controllers" }
            );
        }
    }
}