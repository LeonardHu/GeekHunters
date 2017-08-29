using GeekHunter.Data.Implementation.Entities;
using GeekHunter.Data.Implementation.Helpers;
using GeekHunter.Data.Implementation.DTO;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Transactions;

namespace GeekHunter.Data.Implementation.Responsitories
{
    public class CandidateRepository:BaseReponsitory
    {
        public CandidateRepository() : base() { }
        public CandidateRepository(GeekHunterEntities context) : base(context) { }

        public void Register(CandidateObject user)
        {
            var candidate = new Candidate()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            using (TransactionScope trans = new TransactionScope())
            {
                ctx.Candidates.Add(candidate);

                ctx.SaveChanges();
                

                foreach (var skill in user.Skills)
                {
                    var candidateSkill = new CandidateSkill();
                    candidateSkill.SkillId = skill.Id;
                    candidateSkill.CandidateId = candidate.Id;

                    ctx.CandidateSkills.Add(candidateSkill);
                    ctx.SaveChanges();
                }

                trans.Complete();
            }
        }

        public List<CandidateObject> GetAll()
        {
            var candidates = new List<CandidateObject>();

            foreach (var candidate in ctx.Candidates)
            {
                var skills = candidate.CandidateSkills.Select(x => x.Skill);
                candidates.Add(CandidateHelper.ConvertCanidateObj(skills, candidate));
            }

            return candidates;
        }

        public List<CandidateObject> GetBySkills(List<int> SkillIds)
        {
            var candidates = new List<CandidateObject>();

            var selectedCandidates = ctx.Candidates.Where(x => (
                            x.CandidateSkills.Count(cs =>
                            SkillIds.Contains(cs.SkillId)) == SkillIds.Count && SkillIds.Count > 0
                        )
                        || (
                            SkillIds.Count == 0 && x.CandidateSkills.Count == 0
                        )
                    );

            foreach (var selectedCandidate in selectedCandidates)
            {
                var skills = selectedCandidate.CandidateSkills.Select(x => x.Skill);
                candidates.Add(CandidateHelper.ConvertCanidateObj(skills, selectedCandidate));
            }

            return candidates;
        }
    }
}
