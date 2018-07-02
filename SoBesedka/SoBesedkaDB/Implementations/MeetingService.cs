using SoBesedkaDB.Interfaces;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = System.DayOfWeek;
using System.Threading;

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
                MeetingName = model.MeetingName,
                MeetingDescription = model.MeetingDescription,
                MeetingTheme = model.MeetingTheme,
                CreatorId = model.CreatorId,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                RoomId = model.RoomId,
                RepeatingDays = model.RepeatingDays
            });
            foreach (var um in model.UserMeetings)
            {
                context.UserMeetings.Add(new UserMeeting
                {
                    MeetingId = model.Id,
                    UserId = um.UserId
                });
            }
            context.SaveChanges();


            List<string> emails = new List<string>();
            emails.Add(context.Users.FirstOrDefault(u => u.Id == model.CreatorId).UserMail);
            for (int i = 0; i < model.UserMeetings.Count; i++) {
                emails.Add(context.Users.FirstOrDefault(r => r.Id == model.UserMeetings[i].UserId).UserMail);
            }
            //emails = new List<string>(emails.Distinct());
            DateTime when = model.StartTime - TimeSpan.FromMinutes(15);
            String meetingName = model.MeetingName;
            String room = context.Rooms.FirstOrDefault(r => r.Id == model.RoomId).RoomName;
            DateTime now = DateTime.Now;
            for (int i = 0; i < emails.Count; i++)
            {
                MailService.SendEmail(emails[i], "Приглашение на мероприятие", "Мероприятие: " + meetingName + "\n Комната для переговоров: " + room + "\nВремя начала: " + model.StartTime);
                if (when <= now)
                {
                    MailService.SendEmail(emails[i], "Уведомление о начале мероприятия", "Мероприятие " + meetingName + " начнется через " + (model.StartTime - now).Minutes + " минут. \nКомната для переговоров: " + room);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        if (when > now)
                            Thread.Sleep(when - now);
                        MailService.SendEmail(emails[i], "Уведомление о начале мероприятия", "Мероприятие " + meetingName + " начнется через 15 минут. \nКомната для переговоров: " + room);
                    });
                }
            }
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
                    RoomId = element.RoomId,
                    RepeatingDays = element.RepeatingDays,
                    UserMeetings = context.UserMeetings.Select(um => new UserMeetingViewModel
                    {
                        Id = um.Id,
                        UserId = um.UserId,
                        MeetingId = um.MeetingId
                    })
                    .Where(um => um.MeetingId == element.Id)
                    .ToList()
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
                RoomId = rec.RoomId,
                UserMeetings = context.UserMeetings.Select(um => new UserMeetingViewModel
                {
                    Id = um.Id,
                    UserId = um.UserId,
                    MeetingId = um.MeetingId
                }).ToList()
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
            context.UserMeetings.RemoveRange(
                context.UserMeetings
                    .Where(um => um.MeetingId == model.Id)
            );
            foreach (var um in model.UserMeetings)
            {
                context.UserMeetings.Add(new UserMeeting
                {
                    UserId = um.UserId,
                    MeetingId = model.Id
                });
            }
            context.SaveChanges();
        }

        void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        bool MeetingIntersect(DateTime a, DateTime b, DateTime c, DateTime d)
        {
            if (a > b)  Swap(ref a, ref b);
            if (c > d)  Swap(ref c, ref d);
            if (a > c)
            {
                if (b < d)
                {
                    return a < b;
                }
                return a < d;
            }
            if (b < d)
            {
                return c < b;
            }
            return c < d;
        }

        public List<MeetingViewModel> GetListOfDay(int roomId, DateTime day)
        {
            var dayEnd = day.Date + TimeSpan.FromDays(1);
            List<MeetingViewModel> result = context.Meetings.Select(element => new MeetingViewModel
            {
                Id = element.Id,
                MeetingName = element.MeetingName,
                MeetingDescription = element.MeetingDescription,
                MeetingTheme = element.MeetingTheme,
                CreatorId = element.CreatorId,
                StartTime = element.StartTime,
                EndTime = element.EndTime,
                RoomId = element.RoomId,
                RepeatingDays = element.RepeatingDays,
                UserMeetings = context.UserMeetings.Select(um => new UserMeetingViewModel
                {
                    Id = um.Id,
                    UserId = um.UserId,
                    MeetingId = um.MeetingId
                })
                .Where(um => um.MeetingId == element.Id)
                .ToList()
            })
            .Where(m => m.RoomId == roomId && m.StartTime >= day.Date && m.EndTime < dayEnd && m.RepeatingDays == "0000000")
            .ToList();

            int c = result.Count;

            var rep = context.Meetings
            .Where(m => m.RoomId == roomId)
            .ToList();
            foreach (var meeting in rep)
            {
                if (meeting.RepeatingDays[(int) day.DayOfWeek] == '1')
                {
                    var meetingToAdd = new MeetingViewModel
                    {
                        Id = meeting.Id,
                        MeetingName = meeting.MeetingName,
                        MeetingDescription = meeting.MeetingDescription,
                        MeetingTheme = meeting.MeetingTheme,
                        CreatorId = meeting.CreatorId,
                        StartTime = day.Date + meeting.StartTime.TimeOfDay,
                        EndTime = day.Date + meeting.EndTime.TimeOfDay,
                        RoomId = meeting.RoomId,
                        RepeatingDays = meeting.RepeatingDays,
                        UserMeetings = context.UserMeetings.Select(um => new UserMeetingViewModel
                        {
                            Id = um.Id,
                            UserId = um.UserId,
                            MeetingId = um.MeetingId
                        })
                        .Where(um => um.MeetingId == meeting.Id)
                        .ToList()
                    };
                    var dontAdd = 0;
                    for (var i = 0; i < c; i++)
                    {
                        var added = result[i];
                        if (MeetingIntersect(meetingToAdd.StartTime, meetingToAdd.EndTime, 
                            added.StartTime, added.EndTime)) dontAdd++;
                    }
                    if (dontAdd == 0)
                    {
                        result.Add(meetingToAdd);
                        c++;
                    }
                }
            }

            c = result.Count;
            
            if (c > 0)
            {
                result.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
                if (result[c - 1].EndTime < day.Date.AddHours(24))
                    result.Add(new MeetingViewModel
                    {
                        StartTime = result[c - 1].EndTime,
                        EndTime = day.Date.AddHours(24)
                    });

                for (int i = 1; i < c; i++)
                {
                    if (result[i].StartTime > result[i - 1].EndTime)
                    {
                        result.Add(new MeetingViewModel
                        {
                            StartTime = result[i - 1].EndTime,
                            EndTime = result[i].StartTime,
                            MeetingName = string.Empty
                        });
                    }
                }
                if (result[0].StartTime > day.Date.AddHours(0))
                    result.Add(new MeetingViewModel
                    {
                        StartTime = day.Date.AddHours(0),
                        EndTime = result[0].StartTime
                    });

                result.Sort((a, b) => a.StartTime.CompareTo(b.StartTime));
            }
            else
            {
                result = new List<MeetingViewModel> {
                    new MeetingViewModel
                    {
                        StartTime = day.Date.AddHours(0),
                        EndTime = day.Date.AddHours(24)
                    }
                };
            }
            return result;
        }

        public List<MeetingViewModel> GetListUserInvites(int id)
        {
            List<UserMeetingViewModel> almostresult = context.UserMeetings
                .Where(rec => rec.UserId == id)
                .Select(rec => new UserMeetingViewModel
                {
                    MeetingId = rec.MeetingId
                }).ToList();
            List<MeetingViewModel> result = new List<MeetingViewModel>();
            for (int i = 0; i < almostresult.Count; i++)
            {
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
