using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikiSoft.UniversalPaymentGateway.API.Dto;
using TikiSoft.UniversalPaymentGateway.Domain.Model;


namespace TikiSoft.UniversalPaymentGateway.API.Mapping
{
    public class ModelToDtoProfile : Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<TransactionRequest, TransactionDto>();
            //.ForMember(d=>d.TransType,o=>o.resol);
            CreateMap<TransItem, TransItemDto>();
        }
    }
}
