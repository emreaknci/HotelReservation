using Business.Abstract;
using Business.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessService(this IServiceCollection services)
        {
            //services.AddFluentValidation(options =>
            //{
            //    // Validate child properties and root collection elements
            //    options.ImplicitlyValidateChildProperties = true;
            //    options.ImplicitlyValidateRootCollectionElements = true;

            //    // Automatic registration of validators in assembly
            //    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //});

            services.AddTransient<IHotelService, HotelService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddAutoMapper(typeof(BusinessServiceRegistration));
            return services;
        }
    }
}
