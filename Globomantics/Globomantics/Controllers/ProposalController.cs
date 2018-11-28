using Globomantics.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;

namespace Globomantics.Controllers
{
    public class ProposalController : Controller
    {
        #region Fields

        private IConferenceService mConfService;

        private IProposalService mPropService;

        #endregion


        #region Init and clean-up

        public ProposalController(IConferenceService confService, IProposalService propService)
        {
            mConfService = confService;
            mPropService = propService;
        }

        #endregion


        #region Web API

        public async Task<IActionResult> Index(int conferenceId)
        {
            var conf = await mConfService.GetById(conferenceId);
            ViewBag.Title = $"Proposals for conference {conf.Name} {conf.Location}";
            ViewBag.ConferenceId = conferenceId;

            return View(await mPropService.GetAll(conferenceId));
        }


        public IActionResult Add(int conferenceId)
        {
            ViewBag.Title = "Add new proposal";
            return View(new ProposalModel { ConferenceId = conferenceId });
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProposalModel proposal)
        {
            if (ModelState.IsValid)
                await mPropService.Add(proposal);

            return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
        }


        public async Task<IActionResult> Approve(int proposalId)
        {
            var proposal = await mPropService.Approve(proposalId);
            return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
        }

        #endregion
    }
}
