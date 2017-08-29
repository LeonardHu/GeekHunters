using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using GeekHunter.Data.Implementation.DTO;

namespace GeekHunter.Models
{
    public class CandidatesListModel
    {
        public List<SkillModel> Skills { get; set; }
        public List<CandidateObject> Candidates { get; set; }

        public CandidatesListModel()
        {
            Skills = new List<SkillModel>();
            Candidates = new List<CandidateObject>();
        }
    }
}