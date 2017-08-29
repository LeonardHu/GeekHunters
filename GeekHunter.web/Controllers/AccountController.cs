using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GeekHunter.Models;
using GeekHunter.Data.Implementation.Responsitories;
using GeekHunter.Data.Implementation.Entities;
using GeekHunter.Data.Implementation.DTO;

namespace GeekHunter.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
        {
        }
        
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var accontRep = new AccountResponsitory(new GeekHunterEntities());
            bool loginSuccess = accontRep.Login(model.UserName, model.Password);
            if (loginSuccess == true)
            {
                var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, model.UserName),
                        new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string")
                    },
                    DefaultAuthenticationTypes.ApplicationCookie);

                HttpContext.GetOwinContext().Authentication.SignIn(identity);

                return RedirectToAction("Index", "Manage");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }
        

        
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            var rep = new SkillResponsitory();
            model.Skills = rep.GetAll().Select(x => new SkillModel()
            {
                Id = x.Id,
                Name = x.Name,
                Selected = false
            }).ToList();
            return View(model);
        }

        //register candidate
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var rep = new CandidateRepository();
                var cadidate = ConvertCandidateModelToObject(model);
                try
                {
                    rep.Register(cadidate);
                    model.Success = true;
                    model.Message = "Candidate was register successfully.";
                }
                catch (Exception e)
                {
                    model.Success=false;
                    model.Message = "Failed to register.";
                }
                
            }
            
            return View(model);
        }


        private CandidateObject ConvertCandidateModelToObject(RegisterViewModel model)
        {
            var candidateObject = new CandidateObject();
            candidateObject.FirstName = model.FirstName;
            candidateObject.LastName = model.LastName;
            candidateObject.Skills = model.Skills.Where(x => x.Selected == true)
                                                .Select(x => new Skill()
                                                {
                                                    Name = x.Name,
                                                    Id = x.Id
                                                }).ToList();

            return candidateObject;
        }


        [HttpPost]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}