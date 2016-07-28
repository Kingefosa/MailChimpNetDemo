using System;
using System.Web.Mvc;
using MailChimp.Net;
using System.Threading.Tasks;
using MailChimp.Net.Core;
using System.Net;
using MailChimp.Net.Models;

namespace MailChimpNetDemo.Controllers
{
    public class MailChimpController : Controller
    {
        private static MailChimpManager Manager = new MailChimpManager();

        public async Task<ActionResult> SentCampaigns()
        {
            var options = new CampaignRequest
            {
                ListId = "f809a0eba9",
                Status = CampaignStatus.Sent,
                SortOrder = CampaignSortOrder.DESC,
                Limit = 10
            };

            try
            {
                var model = await Manager.Campaigns.GetAllAsync(options);
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

        public async Task<ActionResult> CreateList()
        {
            var list = new List
            {
                Name = "API Created List",
                Contact = new Contact
                {
                    Company = "Foo Inc.",
                    Address1 = "123 Main St.",
                    City = "Anytown",
                    State = "ME",
                    Zip = "04000",
                    Country = "US"
                },
                PermissionReminder = "You were added to this list as part of an automated process that also created this list.",
                CampaignDefaults = new CampaignDefaults
                {
                    FromEmail = "me@myserver.com",
                    FromName = "John Doe",
                    Subject = "Email message from dynamically created List",
                    Language = "en/us"
                },
                EmailTypeOption = true
            };

            try
            {
                var model = await Manager.Lists.AddOrUpdateAsync(list);
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

        public async Task<ActionResult> UpdateList()
        {
            var list = new List
            {
                Id = "f809a0eba9",
                Name = "API Created List - EDITED",
                Contact = new Contact
                {
                    Company = "FooBar Inc.",
                    Address1 = "123 Main St.",
                    City = "Anytown",
                    State = "ME",
                    Zip = "04000",
                    Country = "US"
                },
                PermissionReminder = "You were added to this list as part of an automated process that also created this list.",
                CampaignDefaults = new CampaignDefaults
                {
                    FromEmail = "you@yourserver.com",
                    FromName = "Jane Smith",
                    Subject = "Email message from dynamically created List",
                    Language = "en-us"
                },
                EmailTypeOption = true
            };

            try
            {
                var model = await Manager.Lists.AddOrUpdateAsync(list);
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

        public async Task<ActionResult> GetLists()
        {
            var options = new ListRequest
            {
                Limit = 10
            };

            try
            {
                var model = await Manager.Lists.GetAllAsync(options);
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

        public async Task<ActionResult> GetList()
        {
            try
            {
                var model = await Manager.Lists.GetAsync("f809a0eba9");
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

        public async Task<ActionResult> DeleteList()
        {
            try
            {
                await Manager.Lists.DeleteAsync("f809a0eba9");
                return RedirectToAction("GetLists");
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