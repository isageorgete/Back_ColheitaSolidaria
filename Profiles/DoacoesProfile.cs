using AutoMapper;
using Back_ColheitaSolidaria.DTOs.Doacoes;
using Back_ColheitaSolidaria.Models;

namespace Back_ColheitaSolidaria.Profiles
{
    public class DoacoesProfile : Profile
    {
        public DoacoesProfile()
        {
            CreateMap<Doacao, DoacaoResponseDto>();
            CreateMap<DoacaoCreateDto, Doacao>();
            CreateMap<DoacaoUpdateDto, Doacao>();
        }
    }
}
