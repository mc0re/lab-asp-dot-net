using Globomantics.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;


namespace Globomantics.Controllers
{
    public class ConferenceController : Controller
    {
        #region Fields

        private IConferenceService mConfService;

        #endregion


        #region INit and clean-up

        public ConferenceController(IConferenceService confService)
        {
            mConfService = confService;
        }

        #endregion


        #region Web API

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Conference overview";
            return View(await mConfService.GetAll());
        }


        public IActionResult Add()
        {
            ViewBag.Title = "Add new conference";
            return View(new ConferenceModel());
        }


        [HttpPost]
        public async Task<IActionResult> Add(ConferenceModel conf)
        {
            if (ModelState.IsValid)
                await mConfService.Add(conf);

            return RedirectToAction("Index");
        }

        #endregion
    }
}
