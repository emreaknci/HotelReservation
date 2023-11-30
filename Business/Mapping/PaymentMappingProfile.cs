using AutoMapper;
using Entities.Hotels;
using Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class PaymentMappingProfile:Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<CreatePaymentDto, Payment>().ReverseMap();
        }
    }
}
