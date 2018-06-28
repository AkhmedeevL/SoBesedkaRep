
using SoBesedkaDB;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System.Collections.Generic;

namespace SoBesedkaDB.Interfaces
{
    public interface IUserService
    {
        List<User> GetList();

        UserViewModel GetElement(int id);

        void AddElement(User model);

        void UpdElement(User model);

        void DelElement(int id);
    }
}