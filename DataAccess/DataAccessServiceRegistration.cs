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

            //services.AddTransient<IUserDal, UserDal>();
            //services.AddTransient<IRoleDal, RoleDal>();
            //services.AddTransient<IWriterDal, WriterDal>();
            //services.AddTransient<ICategoryDal, CategoryDal>();
            //services.AddTransient<IBlogDal, BlogDal>();
            //services.AddTransient<ICommentDal, CommentDal>();
            return services;
        }
    }
}
