using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoBesedkaRestAPI.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(User model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(User model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(User model)
        {
            _service.DelElement(model.Id);
        }

        [HttpGet]
        public IHttpActionResult SignIn(string login, string password)
        {
            var element = _service.GetByLogin(login);
            if (element != null && element.UserPassword != password)
            {
                element = null;
            }
            return Ok(element);
        }
    }
}