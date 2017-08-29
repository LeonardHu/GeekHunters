using GeekHunter.Data.Implementation.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GeekHunter.Data.Implementation.Responsitories
{
    public class SkillResponsitory:BaseReponsitory
    {
        public SkillResponsitory() : base() { }
        public SkillResponsitory(GeekHunterEntities context) : base(context) { }

        public List<Skill> GetAll()
        {
            return ctx.Skills.ToList();
        }
    }
}
