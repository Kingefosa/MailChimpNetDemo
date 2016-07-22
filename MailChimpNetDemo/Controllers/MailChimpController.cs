using MailChimp.Net;
using MailChimp.Net.Core;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MailChimpNetDemo.Controllers
{
    public class MailChimpController : Controller
    {
        private static readonly MailChimpManager Manager = new MailChimpManager();

        public async Task<ActionResult> SentCampaigns()
        {
            var options = new CampaignRequest
            {
                ListId = "rrnef25ixy",
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
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.ServiceUnavailable, ex.Message);
            }
        }
    }
}