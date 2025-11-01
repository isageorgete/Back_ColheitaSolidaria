using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;

namespace Back_ColheitaSolidaria.Profiles
{
    public class SolicitacaoProfile : Profile
    {
        public SolicitacaoProfile()
        {
            CreateMap<Solicitacao, SolicitacaoResponseDto>();

            CreateMap<SolicitacaoCreateDto, Solicitacao>();

            CreateMap<SolicitacaoUpdateDto, Solicitacao>();
        }
    }
}
