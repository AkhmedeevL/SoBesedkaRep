using System;
using System.Net.Mail;
using System.Threading;
using System.Web;
using SoBesedkaDB;
using SoBesedkaDB.Implementations;

namespace SoBesedkaRestAPI
{
    public class TimerModule : IHttpModule
    {
        /// <summary>
        /// Вам потребуется настроить этот модуль в файле Web.config вашего
        /// веб-сайта и зарегистрировать его с помощью IIS, чтобы затем воспользоваться им.
        /// см. на этой странице: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //удалите здесь код.
        }

        public void Init(HttpApplication context)
        {
            // Ниже приводится пример обработки события LogRequest и предоставляется 
            // настраиваемая реализация занесения данных
            context.LogRequest += new EventHandler(OnLogRequest);
            var ctx = new SoBesedkaDBContext();
            mservice = new MeetingService(ctx);
            rservice = new RoomService(ctx);
            uservice = new UserService(ctx);
            timer = new Timer(SendEmail, null, 0, interval);
        }

        #endregion

        private MeetingService mservice;
        private RoomService rservice;
        private UserService uservice;
        static Timer timer;
        long interval = 60000; //1 минута
        static object synclock = new object();

        private void SendEmail(object obj)
        {
            lock (synclock)
            {
                DateTime dd = DateTime.Now;
                var events = mservice.GetListOfRange(dd + TimeSpan.FromMinutes(10), dd + TimeSpan.FromMinutes(11));
                if (events.Count > 0)
                {
                    foreach (var ev in events)
                    {
                        var room = rservice.GetElement(ev.RoomId);
                        foreach (var um in ev.UserMeetings)
                        {
                            var user = uservice.GetElement(um.UserId);
                            MailService.SendEmail(user.UserMail, "Уведомление о начале мероприятия",
                                $"Мероприятие {ev.MeetingName} начнется через {(ev.StartTime - dd).Minutes} минут. \nМесто: {room.RoomName}, {room.RoomAdress}");
                        }
                    }
                }
            }
        }

        public void OnLogRequest(Object source, EventArgs e)
        {
            //здесь можно разместить логику занесения данных
        }
    }
}
