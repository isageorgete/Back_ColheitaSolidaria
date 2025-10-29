using AutoMapper;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;

namespace Back_ColheitaSolidaria.Profiles
{
    public class ColaboradorProfile : Profile
    {
        public ColaboradorProfile()
        {
            // Entidade -> DTO (para leitura/resposta)
            CreateMap<Colaborador, ColaboradorResponseDto>();

            // DTO -> Entidade (para cadastro)
            CreateMap<ColaboradorRegisterDto, Colaborador>();

            // DTO -> Entidade (para atualização)
            CreateMap<ColaboradorUpdateDto, Colaborador>();
        }
    }
}

