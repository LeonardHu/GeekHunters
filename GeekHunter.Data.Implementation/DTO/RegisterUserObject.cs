using GeekHunter.Data.Implementation.Entities;
using System.Collections.Generic;

namespace GeekHunter.Data.Implementation.DTO
{
    public class CandidateObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Skill> Skills { get; set; }

        public CandidateObject()
        {
            Skills = new List<Skill>();
        }
    }
}
