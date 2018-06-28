using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoBesedkaDB.Implementations
{
    public class UserService : IUserService
    {
        private SoBesedkaDBContext context;

        public UserService(SoBesedkaDBContext context)
        {
            this.context = context;
        }

        public void AddElement(User model)
        {
            User element = context.Users.FirstOrDefault(rec => rec.UserLogin == model.UserLogin);
            if (element != null)
            {
                throw new Exception("Этот логин уже занят!");
            }
            context.Users.Add(new User
            {
                Id = model.Id,
                UserFIO = model.UserFIO,
                UserMail = model.UserMail,
                UserLogin = model.UserLogin,
                UserPassword = model.UserPassword,
                isAdmin = model.isAdmin,
                UserMeetings = null,
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            User element = context.Users.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Users.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Пользователь не найден");
            }
        }

        public UserViewModel GetElement(int id)
        {
            User element = context.Users.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new UserViewModel
                {
                    Id = element.Id,
                    UserFIO = element.UserFIO,
                    UserMail = element.UserMail,
                    UserLogin = element.UserLogin,
                    UserPassword = element.UserPassword,
                    isAdmin = element.isAdmin
                };
            }
            throw new Exception("Пользователь не найден");
        }

        public List<UserViewModel> GetList()
        {
            List<UserViewModel> result = context.Users.Select(rec => new UserViewModel
            {
                Id = rec.Id,
                UserFIO = rec.UserFIO,
                UserMail = rec.UserMail,
                UserLogin = rec.UserLogin,
                UserPassword = rec.UserPassword,
                isAdmin = rec.isAdmin
            })
                .ToList();
            return result;
        }

        public void UpdElement(User model)
        {
            User element = context.Users.FirstOrDefault(rec =>
                                    rec.UserLogin == model.UserLogin && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Этот логин уже занят!");
            }
            element = context.Users.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }

            element.UserLogin = model.UserLogin;
            element.Id = model.Id;
            element.UserFIO = model.UserFIO;
            element.UserMail = model.UserMail;

            element.UserPassword = model.UserPassword;
            element.isAdmin = model.isAdmin;

            context.SaveChanges();
        }


        
    }
}
