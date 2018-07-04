using SoBesedkaDB.Interfaces;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoBesedkaRestAPI.Controllers
{
    public class MeetingController : ApiController
    {
        private readonly IMeetingService _service;

        public MeetingController(IMeetingService service)
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
        public IHttpActionResult GetListOfDay(int roomId, string day)
        {
            var list = _service.GetListOfDay(roomId, DateTime.Parse(day, CultureInfo.InvariantCulture));
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetListUserCreatedMeetings(int id)
        {
            var list = _service.GetListUserCreatedMeetings(id);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetListUserInvites(int id)
        {
            var list = _service.GetListUserInvites(id);
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
        public void AddElement(Meeting model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(Meeting model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(Meeting model)
        {
            _service.DelElement(model.Id);
        }
    }
}
