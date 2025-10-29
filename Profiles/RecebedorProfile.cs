using AutoMapper;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;

namespace Back_ColheitaSolidaria.Profiles
{
    public class RecebedorProfile : Profile
    {
        public RecebedorProfile()
        {
            // Entidade -> DTO (para leitura/resposta)
            CreateMap<Recebedor, RecebedorResponseDto>();

            // DTO -> Entidade (para cadastro)
            CreateMap<RecebedorRegisterDto, Recebedor>();

            // DTO -> Entidade (para atualização)
            CreateMap<RecebedorUpdateDto, Recebedor>();
        }
    }
}

