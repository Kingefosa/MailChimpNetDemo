using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MailChimpNetDemo.Controllers
{
    public class MailChimpCampaignController : Controller
    {
        private static MailChimpManager Manager = new MailChimpManager();

        public async Task<ActionResult> Index()
        {
            var options = new CampaignRequest
            {
                ListId = "f809a0eba9",
                Status = CampaignStatus.Save,
                SortOrder = CampaignSortOrder.DESC,
                Limit = 10
            };

            ViewBag.ListId = "f809a0eba9";

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

        public async Task<ActionResult> Detail()
        {
            var model = await Manager.Campaigns.GetAsync("d4688e21b2");
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var campaign = new Campaign
            {
                Type = CampaignType.Regular,
                Recipients = new Recipient
                {
                    ListId = "f809a0eba9"
                },
                Settings = new Setting
                {
                    SubjectLine = $"Campaign created by program at {DateTime.UtcNow.ToString("s")}",
                    Title = $"Dynamic campaign {Guid.NewGuid()}",
                    FromName = "Doug Vanderweide",
                    ReplyTo = "mailchimp@dougv.com"
                },
                Tracking = new Tracking
                {
                    Opens = true,
                    HtmlClicks = true,
                    TextClicks = true
                },
                SocialCard = new SocialCard
                {
                    ImageUrl = "http://cdn.smosh.com/sites/default/files/legacy.images/smosh-pit/122010/lolcat-link.jpg",
                    Description = "I'm learning how to make dynamic MailChimp campaigns via the API.",
                    Title = "Using the MailChimp API in .NET via the MailChimp.NET.V3 wrapper"
                },
                ReportSummary = new ReportSummary(),
                DeliveryStatus = new DeliveryStatus()
            };

            try
            {
                await Manager.Campaigns.AddOrUpdateAsync(campaign);
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
            var campaign = new Campaign
            {
                Id = "d4688e21b2",
                Settings = new Setting
                {
                    Title = $"Dynamic campaign {Guid.NewGuid()} modified {DateTime.UtcNow.ToString("s")}"
                }
            };

            try
            {
                await Manager.Campaigns.AddOrUpdateAsync(campaign);
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

        public async Task<ActionResult> SetContentRaw()
        {
            var content = new ContentRequest
            {
                PlainText = "Hello world! I am testing Doug Vanderweide's MailChimp.NET.V3 demo, at https://www.dougv.com",
                Html = "<!doctype html><html lang=\"en\"><head><meta charset=\"utf-8\"><title>title</title></head><body><p>Hello world! I am testing Doug Vanderweide's MailChimp.NET.V3 demo, at <a href=\"https://www.dougv.com\">https://www.dougv.com</a></body></html>"
            };

            try
            {
                await Manager.Content.AddOrUpdateAsync("d4688e21b2", content);
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

        public async Task<ActionResult> SetContentTemplate()
        {
            var content = new ContentRequest
            {
                Template = new Template
                {
                    Id = 123456
                }
            };

            try
            {
                await Manager.Content.AddOrUpdateAsync("d4688e21b2", content);
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

        public async Task<ActionResult> Test()
        {
            try
            {
                await Manager.Campaigns.TestAsync("d4688e21b2", new CampaignTestRequest
                {
                    Emails = new string[] { "foo@bar.com", "bar@foo.com" },
                    EmailType = "html"
                });

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

        public async Task<ActionResult> Send()
        {
            try
            {
                await Manager.Campaigns.SendAsync("d4688e21b2");
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

        public async Task<ActionResult> Delete()
        {
            try
            {
                await Manager.Campaigns.DeleteAsync("d4688e21b2");
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