using AutoMapper;
using Back_ColheitaSolidaria.Models;
using Back_ColheitaSolidaria.DTOs;

namespace Back_ColheitaSolidaria.Profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            // Entidade -> DTO (para leitura/resposta)
            CreateMap<Admin, AdminResponseDto>();

            // DTO -> Entidade (para cadastro)
            CreateMap<AdminRegisterDto, Admin>();

            // DTO -> Entidade (para atualização)
            CreateMap<AdminUpdateDto, Admin>();
        }
    }
}
