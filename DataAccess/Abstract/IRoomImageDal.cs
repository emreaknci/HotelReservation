using Core.DataAccess;
using Entities.RoomImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IRoomImageDal : IGenericRepository<RoomImage>
    {
        bool RemoveAllByRoomId(int roomId);
    }
}
