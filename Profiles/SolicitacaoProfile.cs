using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Models;
using AutoMapper;

namespace Back_ColheitaSolidaria.Profiles
{
    public class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            // Mapear modelo para resposta DTO
            CreateMap<Solicitacao, SolicitacaoResponseDto>()
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.RecebedorId));

            // Mapear create DTO para modelo
            CreateMap<SolicitacaoCreateDto, Solicitacao>()
                .ForMember(dest => dest.RecebedorId, opt => opt.MapFrom(src => src.UsuarioId))
                .ForMember(dest => dest.QuantidadeSolicitada, opt => opt.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DataSolicitacao, opt => opt.Ignore());

            // Mapear update DTO
            CreateMap<SolicitacaoUpdateDto, Solicitacao>();
        }
    }
}
