using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class DataAccessServiceRegistration
    {
        public static IServiceCollection AddDataAccessService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<HotelReservationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));


            services.AddScoped<IHotelDal, HotelDal>();
            services.AddScoped<IPaymentDal, PaymentDal>();
            services.AddScoped<IReservationDal, ReservationDal>();
            services.AddScoped<IRoomDal, RoomDal>();
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IRoomImageDal, RoomImageDal>();
            services.AddScoped<IHotelImageDal, HotelImageDal>();

            return services;
        }
    }
}
