using GeekHunter.Data.Implementation.Entities;
using System.Linq;

namespace GeekHunter.Data.Implementation.Responsitories
{
    public class AccountResponsitory : BaseReponsitory
    {
        public AccountResponsitory() : base() { }
        public AccountResponsitory(GeekHunterEntities context) : base(context) { }

        public bool Login(string name, string password)
        {
            using (GeekHunterEntities ctx = new GeekHunterEntities())
            {
                var user = ctx.AdminUsers.FirstOrDefault(x => x.UserName == name && x.Password == password);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
