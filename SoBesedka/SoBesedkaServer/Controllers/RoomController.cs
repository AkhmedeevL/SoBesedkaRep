
using SoBesedkaDB.Interfaces;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SoBesedkaRestAPI.Controllers
{
    public class RoomController : ApiController
    {
        private readonly IRoomService _service;

        public RoomController(IRoomService service)
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

        public IHttpActionResult GetAvailableRooms(string start, string end)
        {
            DateTime startTime = DateTime.Parse(start),
                endTime = DateTime.Parse(end);
            var list = _service.GetAvailableRooms(startTime, endTime);
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
        public void AddElement(Room model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(Room model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(Room model)
        {
            _service.DelElement(model.Id);
        }
    }
}
