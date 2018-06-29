using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Collections;
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
                UserMeetings = model.UserMeetings
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

        public List<MeetingViewModel> GetListOfDay(int roomId, DateTime day)
        {
            var dayEnd = day.Date + TimeSpan.FromDays(1);
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
            .Where(m => m.RoomId == roomId && m.StartTime >= day.Date && m.EndTime < dayEnd)
            .ToList();

            

            
            int c = result.Count;
            if (c > 0)
            {
                result.Sort(delegate (MeetingViewModel a, MeetingViewModel b)
                {
                    return a.StartTime.CompareTo(b.StartTime);
                });
                if (result[c - 1].EndTime < day.Date.AddHours(17))
                    result.Add(new MeetingViewModel
                    {
                        StartTime = result[c - 1].EndTime,
                        EndTime = day.Date.AddHours(17)
                    });

                for (int i = 1; i < c; i++)
                {
                    if (result[i].StartTime > result[i - 1].EndTime)
                    {
                        result.Add(new MeetingViewModel
                        {
                            StartTime = result[i - 1].EndTime,
                            EndTime = result[i].StartTime,
                            MeetingName = String.Empty
                        });
                    }
                }
                if (result[0].StartTime > day.Date.AddHours(8))
                    result.Add(new MeetingViewModel
                    {
                        StartTime = day.Date.AddHours(8),
                        EndTime = result[0].StartTime
                    });
                
                result.Sort(delegate (MeetingViewModel a, MeetingViewModel b)
                {
                    return a.StartTime.CompareTo(b.StartTime);
                });
            }
            else
            {
                result = new List<MeetingViewModel> {
                    new MeetingViewModel
                    {
                        StartTime = day.Date.AddHours(8),
                        EndTime = day.Date.AddHours(17)
                    }
                };
            }
            return result;
        }

        public List<MeetingViewModel> GetListUserInvites(int id)
        {
            List<UserMeetingViewModel> almostresult = context.UserMeetings
                .Where(rec => rec.UserId == id)
                .Select(rec => new UserMeetingViewModel {
                    MeetingId = rec.MeetingId
                }).ToList();
            List<MeetingViewModel> result = new List<MeetingViewModel>();
            for (int i = 0; i < almostresult.Count; i++) {
                result.Add(GetElement(almostresult[i].MeetingId));
            }
            return result;
        }

        public List<MeetingViewModel> GetListUserCreatedMeetings(int id)
        {
            List<MeetingViewModel> result = context.Meetings
                .Where(rec => rec.CreatorId == id)
                .Select(rec => new MeetingViewModel
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
    }
}
