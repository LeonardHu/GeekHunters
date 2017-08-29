using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GeekHunter.Models;
using GeekHunter.Data.Implementation.Responsitories;
using GeekHunter.Data.Implementation.Entities;

namespace GeekHunter.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController()
        {
        }


        public ActionResult Index()
        {
            var candidateRepository = new CandidateRepository();
            var candidates = candidateRepository.GetAll();

            var skillRepository = new SkillResponsitory();
            var skills = skillRepository.GetAll();

            var model = new CandidatesListModel();

            model.Candidates = candidates;
            model.Skills = skills.Select(x => new SkillModel()
            {
                Id = x.Id,
                Name = x.Name,
                Selected = true
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CandidatesListModel model)
        {
            var skillIds = model.Skills.Where(x => x.Selected == true).Select(x => x.Id).ToList();
            var candidateRepository = new CandidateRepository();
            var candidates = candidateRepository.GetBySkills(skillIds);
            model.Candidates = candidates;
            return View(model);
        }
    }
}