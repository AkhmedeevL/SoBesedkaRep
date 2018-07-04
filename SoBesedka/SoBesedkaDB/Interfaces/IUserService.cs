
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System.Collections.Generic;

namespace SoBesedkaDB.Interfaces
{
    public interface IUserService
    {
        List<UserViewModel> GetList();

        UserViewModel GetElement(int id);

        void AddElement(User model);

        void UpdElement(User model);

        void DelElement(int id);

        User ConvertViewToUser(UserViewModel view);

        UserViewModel GetByLoginOrEmail(string login);

        User RestoringPassword(string email);
    }
}