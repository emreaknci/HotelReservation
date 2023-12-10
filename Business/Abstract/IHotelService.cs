using Core.Entities;
using Core.Utils.Results;
using Entities.Hotels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IHotelService
    {
        Result<List<Hotel>> GetAll();
        Result<List<HotelDetailDto>> GetAllWithImages(); 
        Task<Result<Hotel>> GetByIdAsync(int id);
        Result<HotelDetailDto> GetByIdWithImages(int id);
        Task<Result<Hotel>> AddAsync(CreateHotelDto hotel);
        Task<Result<List<Hotel>>> AddRangeAsync(List<CreateHotelDto> hotels);
        Task<Result<Hotel>> Update(UpdateHotelDto hotel);
        Task<Result<List<Hotel>>> UpdateRange(List<UpdateHotelDto> hotels);
        Task<Result<Hotel>> Remove(RemoveHotelDto hotel);
        Task<Result<Hotel>> RemoveById(int id);
        Task<Result<List<Hotel>>> RemoveRange(List<RemoveHotelDto> hotels);
        Result<PaginationResult<Hotel>> GetAllPagination(BasePaginationRequest req);
        Task<Result<bool>> ChangeHotelStatus(int hotelId);
    }
}
