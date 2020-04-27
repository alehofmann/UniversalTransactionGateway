using AutoMapper;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Domain.Model;

namespace TikiSoft.UniversalPaymentGateway.API.Mapping
{
    public class DtoToModelProfile : Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<TransactionDto, TransactionRequest>();
            CreateMap<TransItemDto, TransItem>();
        }

    }
}
