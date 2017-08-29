using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using GeekHunter.Data.Implementation.Entities;
using System.Data;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using GeekHunter.Data.Implementation.Responsitories;

namespace GeekHunter.Tests
{
    [TestClass]
    public class CandidateTest
    {
        private Mock<DbSet<T>> MapData<T>(IQueryable<T> data) where T:class
        {
            var mockSetSkill = new Mock<DbSet<T>>();
            mockSetSkill.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSetSkill.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSetSkill.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSetSkill.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSetSkill;
        }

        private GeekHunterEntities GetContext()
        {
            var mockContext = new Mock<GeekHunterEntities>();
            
            var skills = new List<Skill>
            {
                new Skill { Name = "C#",Id=1 },
                new Skill { Name = "JavaScript",Id=2 },
                new Skill { Name = "Sql",Id=3 },
            }.AsQueryable();
            
            var skillObj = MapData<Skill>(skills);
            mockContext.Setup(c => c.Skills).Returns(skillObj.Object);

            var candidates= new List<Candidate>
            {
                new Candidate {
                    FirstName = "Tom",
                    LastName ="Green",
                    Id =1,
                    CandidateSkills =new List<CandidateSkill>() {
                        new CandidateSkill { Id=1,CandidateId=1,SkillId=1 },
                        new CandidateSkill { Id=2, CandidateId=1, SkillId=2 }
                    }
                },
                new Candidate {
                    FirstName = "Jack",
                    LastName ="Green",
                    Id =2,
                    CandidateSkills =new List<CandidateSkill>() {
                        new CandidateSkill { Id=3, CandidateId=2, SkillId=1 },
                    }
                },
                new Candidate { FirstName= "John", LastName = "Green", Id=3 },
            }.AsQueryable();

            var candidateObj = MapData<Candidate>(candidates);
            mockContext.Setup(c => c.Candidates).Returns(candidateObj.Object);

            return mockContext.Object;
        }

        [TestMethod]
        public void SkillGetValidate()
        {
            var ctx = GetContext();

            var skillReponsitory = new SkillResponsitory(ctx);
            var skills = skillReponsitory.GetAll();

            Assert.AreEqual(3, skills.Count);
            Assert.AreEqual("C#", skills[0].Name);
            Assert.AreEqual("JavaScript", skills[1].Name);
            Assert.AreEqual("Sql", skills[2].Name);
        }

        [TestMethod]
        public void CandidateGetAllValidate()
        {
            var ctx = GetContext();
            var rep = new CandidateRepository(ctx);

            var allCandidates = rep.GetAll();
            Assert.AreEqual(3, allCandidates.Count);
            Assert.AreEqual("John", allCandidates[2].FirstName);
        }

        [TestMethod]
        public void CandidateGetBySkillValidate()
        {
            var ctx = GetContext();
            var rep = new CandidateRepository(ctx);

            //find by c#
            List<int> skillIds = new List<int>() { 1 };
            var candidates = rep.GetBySkills(skillIds);
            Assert.AreEqual(2, candidates.Count);

            //find by javascript
            skillIds = new List<int>() { 2 };
            candidates = rep.GetBySkills(skillIds);
            Assert.AreEqual(1, candidates.Count);
            Assert.AreEqual("Tom", candidates[0].FirstName);

            //find by c# and javascript
            skillIds = new List<int>() { 1, 2 };
            candidates = rep.GetBySkills(skillIds);
            Assert.AreEqual(1, candidates.Count);
            Assert.AreEqual("Tom", candidates[0].FirstName);

            //find by sql
            skillIds = new List<int>() { 3 };
            candidates = rep.GetBySkills(skillIds);
            Assert.AreEqual(0, candidates.Count);

            //find by no skill
            skillIds = new List<int>() { };
            candidates = rep.GetBySkills(skillIds);
            Assert.AreEqual(1, candidates.Count);
            Assert.AreEqual("John", candidates[0].FirstName);
        }
    }
}
