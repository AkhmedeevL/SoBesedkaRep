
using SoBesedkaDB;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System.Collections.Generic;

namespace SoBesedkaDB.Interfaces
{
    public interface IMeetingService
    {
        List<MeetingViewModel> GetList();

        MeetingViewModel GetElement(int id);

        void AddElement(Meeting model);

        void UpdElement(Meeting model);

        void DelElement(int id);
    }
}