using BusinessLogicLayer.Model;
using System.Linq;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        User GetByName(string name);

        IQueryable<User> GetAll();
        //   IQueryable<User> GetByName(string name);
        User GetById(int id);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
