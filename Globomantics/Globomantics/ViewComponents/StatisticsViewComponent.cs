using Globomantics.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Globomantics.ViewComponents
{
    public class StatisticsViewComponent : ViewComponent
    {
        private readonly IConferenceService mService;


        public StatisticsViewComponent(IConferenceService service)
        {
            this.mService = service;
        }


        public async Task<IViewComponentResult> InvokeAsync(string caption)
        {
            ViewBag.Caption = caption;
            return View(await mService.GetStatistics());
        }
    }
}
