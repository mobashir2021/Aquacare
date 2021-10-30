using System.Web.Mvc;

namespace OnDemandService.Areas.Professional
{
    public class ProfessionalAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Professional";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Professional_default",
                url: "Professional/{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "LoginProfessional", id = UrlParameter.Optional },
                namespaces: new[] { "OnDemandService.Areas.Professional.Controllers" }
            );
        }
    }
}