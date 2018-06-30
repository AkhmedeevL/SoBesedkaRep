﻿using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            User elementEmail = context.Users.FirstOrDefault(rec =>
            rec.UserMail == model.UserMail && rec.Id != model.Id);
            if (elementEmail != null)
            {
                throw new Exception("Этот E-mail уже занят!");
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
            User elementEmail = context.Users.FirstOrDefault(rec =>
                        rec.UserMail == model.UserMail && rec.Id != model.Id);
            if (elementEmail != null)
            {
                throw new Exception("Этот E-mail уже занят!");
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

        public User ConvertViewToUser(UserViewModel view) {
            return new User {
                Id = view.Id,
                UserFIO = view.UserFIO,
                UserMail = view.UserMail,
                UserLogin = view.UserLogin,
                UserPassword = view.UserPassword,
                isAdmin = view.isAdmin
            };
        }

        public bool SignIn(string login, string password, out UserViewModel outUser)
        {
            List<UserViewModel> list = GetList();
            foreach (var user in list) {
                if (user.UserLogin == login && user.UserPassword == password) {
                    outUser = user;
                    return true;
                }
            }
            outUser = null;
            return false;
        }

        public void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress("sobesedkaapp@yandex.ru");
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.yandex.ru", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential("sobesedkaapp@yandex.ru","teamb123");

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
