using System;
using System.Web.Mvc;
using MailChimp.Net;
using MailChimp.Net.Models;
using MailChimp.Net.Core;
using System.Threading.Tasks;
using System.Net;

namespace MailChimpNetDemo.Controllers
{
    public class MailChimpMemberController : Controller
    {
        private static MailChimpManager Manager = new MailChimpManager();

        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.ListId = "f809a0eba9";
                var model = await Manager.Members.GetAllAsync("f809a0eba9", new MemberRequest { Limit = 100, Status = Status.Subscribed });
                return View(model);
            }
            catch (MailChimpException mce)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway, mce.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
        }

        public async Task<ActionResult> Detail()
        {
            try
            {
                var model = await Manager.Members.GetAsync("f809a0eba9", "dougvanderweide@gmail.com");
                return View(model);
            }
            catch (MailChimpException mce)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway, mce.Message);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
        }
    }
}