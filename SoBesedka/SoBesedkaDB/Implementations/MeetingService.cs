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
    public class MeetingService : IMeetingService
    {
        private SoBesedkaDBContext context;

        public MeetingService(SoBesedkaDBContext context)
        {
            this.context = context;
        }

        public void AddElement(Meeting model)
        {
            context.Meetings.Add(new Meeting
            {
                Id = model.Id,
                MeetingName = model.MeetingName,
                MeetingDescription = model.MeetingDescription,
                MeetingTheme = model.MeetingTheme,
                CreatorId = model.CreatorId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                RoomId = model.RoomId,
                UserMeetings = null
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Meeting element = context.Meetings.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Meetings.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Событие не найдено");
            }
        }

        public MeetingViewModel GetElement(int id)
        {
            Meeting element = context.Meetings.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new MeetingViewModel
                {
                    Id = element.Id,
                    MeetingName = element.MeetingName,
                    MeetingDescription = element.MeetingDescription,
                    MeetingTheme = element.MeetingTheme,
                    CreatorId = element.CreatorId,
                    StartTime = element.StartTime,
                    EndTime = element.EndTime,
                    RoomId = element.RoomId
                };
            }
            throw new Exception("Событие не найдено");
        }

        public List<MeetingViewModel> GetList()
        {
            List<MeetingViewModel> result = context.Meetings.Select(rec => new MeetingViewModel
            {
                Id = rec.Id,
                MeetingName = rec.MeetingName,
                MeetingDescription = rec.MeetingDescription,
                MeetingTheme = rec.MeetingTheme,
                CreatorId = rec.CreatorId,
                StartTime = rec.StartTime,
                EndTime = rec.EndTime,
                RoomId = rec.RoomId
            })
                .ToList();
            return result;
        }

        public void UpdElement(Meeting model)
        {
            Meeting element = context.Meetings.FirstOrDefault(rec =>
                                    rec.MeetingName == model.MeetingName && rec.Id != model.Id);

            element = context.Meetings.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Событие не найдено");
            }
            element.Id = model.Id;
            element.MeetingName = model.MeetingName;
            element.MeetingDescription = model.MeetingDescription;
            element.MeetingTheme = model.MeetingTheme;
            element.CreatorId = model.CreatorId;
            element.StartTime = model.StartTime;
            element.EndTime = model.EndTime;
            element.RoomId = model.RoomId;

            context.SaveChanges();
        }


    }
}
