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
    public class RoomService : IRoomService
    {
        private SoBesedkaDBContext context;

        public RoomService(SoBesedkaDBContext context)
        {
            this.context = context;
        }
        public List<RoomViewModel> GetList()
        {
            List<RoomViewModel> result = context.Rooms.Select(rec => new RoomViewModel
            {
                Id = rec.Id,
                RoomName = rec.RoomName,
                RoomAdress = rec.RoomAdress,
                Description = rec.Description,
                Meetings = context.Meetings.Select(m => new MeetingViewModel {
                    Id = m.Id,
                    MeetingName = m.MeetingName,
                    MeetingTheme = m.MeetingTheme,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    CreatorId = m.CreatorId,
                    MeetingDescription = m.MeetingDescription,
                    RoomId = m.RoomId
                }).ToList()
            })
            .ToList();
            return result;
        }

        public RoomViewModel GetElement(int id)
        {
            Room element = context.Rooms.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new RoomViewModel
                {
                    Id = element.Id,
                    RoomAdress = element.RoomAdress,
                    Description = element.Description,
                    RoomName = element.RoomName,
                    Meetings = context.Meetings.Select(m => new MeetingViewModel
                    {
                        Id = m.Id,
                        MeetingName = m.MeetingName,
                        MeetingTheme = m.MeetingTheme,
                        StartTime = m.StartTime,
                        EndTime = m.EndTime,
                        CreatorId = m.CreatorId,
                        MeetingDescription = m.MeetingDescription,
                        RoomId = m.RoomId
                    }).ToList()
                };
            }
            throw new Exception("Помещение не найдено");
        }

        public void AddElement(Room model)
        {
            Room element = context.Rooms.FirstOrDefault(rec => rec.RoomName == model.RoomName);
            if (element != null)
            {
                throw new Exception("Уже есть помещение с таким названием");
            }
            context.Rooms.Add(new Room
            {
                Id = model.Id,
                RoomName = model.RoomName,
                RoomAdress = model.RoomAdress,
                Description = model.Description,
                Meetings = null,
            });
            context.SaveChanges();
        }

        public void UpdElement(Room model)
        {
            Room element = context.Rooms.FirstOrDefault(rec =>
                        rec.RoomName == model.RoomName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть помещение с таким названием");
            }
            element = context.Rooms.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Id = model.Id;
            element.RoomName = model.RoomName;
            element.RoomAdress = model.RoomAdress;
            element.Description = model.Description;
            element.Meetings = model.Meetings;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Room element = context.Rooms.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Rooms.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Помещение не найдено");
            }
        }
    }
}
