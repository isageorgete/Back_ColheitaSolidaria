using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Models;
using AutoMapper;

namespace Back_ColheitaSolidaria.Profiles
{
    public class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            // Mapeamento do modelo para DTO de resposta
            CreateMap<Solicitacao, SolicitacaoResponseDto>()
                .ForMember(dest => dest.RecebedorNome, opt => opt.MapFrom(src => src.Recebedor.NomeCompleto))
                .ForMember(dest => dest.DoacaoNome, opt => opt.MapFrom(src => src.Doacao.Nome))
                .ForMember(dest => dest.DoacaoDescricao, opt => opt.MapFrom(src => src.Doacao.Descricao));

            // Mapeamento do create DTO para modelo
            CreateMap<SolicitacaoCreateDto, Solicitacao>()
                .ForMember(dest => dest.RecebedorId, opt => opt.MapFrom(src => src.RecebedorId))
                .ForMember(dest => dest.QuantidadeSolicitada, opt => opt.MapFrom(src => src.Quantidade))
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.DataSolicitacao, opt => opt.Ignore());

            // Mapeamento do update DTO
            CreateMap<SolicitacaoUpdateDto, Solicitacao>();
        }
    }
}
