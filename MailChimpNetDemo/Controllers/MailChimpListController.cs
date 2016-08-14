﻿using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MailChimpNetDemo.Controllers
{
    public class MailChimpListController : Controller
    {
        private static MailChimpManager Manager = new MailChimpManager();

        public async Task<ActionResult> Index()
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

        public async Task<ActionResult> Create()
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
                    FromEmail = "mailchimp@dougv.com",
                    FromName = "Doug Vanderweide",
                    Subject = "Email message from dynamically created List",
                    Language = "en-us"
                },
                EmailTypeOption = true
            };

            try
            {
                var model = await Manager.Lists.AddOrUpdateAsync(list);
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

        public async Task<ActionResult> Update()
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
                    FromName = "Jane Smith"
                },
                EmailTypeOption = true
            };

            try
            {
                var model = await Manager.Lists.AddOrUpdateAsync(list);
                return RedirectToAction("Detail");
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

        public async Task<ActionResult> Delete()
        {
            try
            {
                await Manager.Lists.DeleteAsync("f809a0eba9");
                return RedirectToAction("Detail");
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