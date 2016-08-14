using System;
using System.Web.Mvc;
using MailChimp.Net;
using MailChimp.Net.Models;
using MailChimp.Net.Core;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;

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

        public async Task<ActionResult> Detail(string id = "foo@bar.com")
        {
            try
            {
                var model = await Manager.Members.GetAsync("f809a0eba9", id);
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

        public async Task<ActionResult> Create()
        {
            var member = new Member
            {
                EmailAddress = "foo@bar.com",
                Status = Status.Pending,
                EmailType = "html",
                IpSignup = Request.UserHostAddress,
                TimestampSignup = DateTime.UtcNow.ToString("s"),
                MergeFields = new Dictionary<string, string>
                {
                    { "FNAME", "Foo" },
                    { "LNAME", "Bar" }
                }
            };

            try
            {
                var result = await Manager.Members.AddOrUpdateAsync("f809a0eba9", member);
                return RedirectToAction("Detail", new { id = result.EmailAddress });
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

        public async Task<ActionResult> Update(string id = "bar@foo.com")
        {
            var member = new Member
            {
                EmailAddress = id,
                MergeFields = new Dictionary<string, string>
                {
                    { "FNAME", "Bar" },
                    { "LNAME", "Foo" }
                }
            };

            try
            {
                var result = await Manager.Members.AddOrUpdateAsync("f809a0eba9", member);
                return RedirectToAction("Detail", new { id = result.EmailAddress });
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

        public async Task<ActionResult> Delete(string id = "bar@foo.com")
        {
            try
            {
                await Manager.Members.DeleteAsync("f809a0eba9", id);
                return RedirectToAction("Index");
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