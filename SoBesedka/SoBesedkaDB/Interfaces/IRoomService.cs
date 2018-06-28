
using SoBesedkaDB;
using SoBesedkaDB.Views;
using SoBesedkaModels;
using System.Collections.Generic;

namespace SoBesedkaDB.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetList();

        RoomViewModel GetElement(int id);

        void AddElement(Room model);

        void UpdElement(Room model);

        void DelElement(int id);
    }
}
