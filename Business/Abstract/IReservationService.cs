using Core.Entities;
using Core.Utils.Results;
using Entities.Hotels;
using Entities.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IReservationService
    {
        Result<List<Reservation>> GetAll();
        Task<Result<Reservation>> GetByIdAsync(int id);
        Task<Result<Reservation>> Reserve(CreateReservationDto reservation);
        Task<Result<Reservation>> Update(UpdateReservationDto reservation);
        Task<Result<Reservation>> Remove(RemoveReservationDto reservation);
        Task<Result<Reservation>> RemoveById(int id);
        Task<Result<List<Reservation>>> RemoveRange(List<RemoveReservationDto> reservation);
        Result<PaginationResult<Reservation>> GetAllPagination(BasePaginationRequest req);

    }
}
