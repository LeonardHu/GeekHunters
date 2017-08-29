using GeekHunter.Data.Implementation.Entities;
using GeekHunter.Data.Implementation.DTO;
using System.Collections.Generic;
using System.Linq;

namespace GeekHunter.Data.Implementation.Helpers
{
    public class CandidateHelper
    {
        public static CandidateObject ConvertCanidateObj(IEnumerable<Skill> skills, Candidate candidate)
        {
            var candidateObject = new CandidateObject();
            candidateObject.Skills = skills.ToList();
            candidateObject.Id = candidate.Id;
            candidateObject.FirstName = candidate.FirstName;
            candidateObject.LastName = candidate.LastName;

            return candidateObject;
        }        
    }
}