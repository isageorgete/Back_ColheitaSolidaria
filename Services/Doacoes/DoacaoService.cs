using AutoMapper;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs.Doacoes;
using Back_ColheitaSolidaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_ColheitaSolidaria.Services.Doacoes
{
    public class DoacaoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DoacaoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Buscar doação por ID
        public async Task<DoacaoResponseDto?> GetByIdAsync(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            return doacao == null ? null : _mapper.Map<DoacaoResponseDto>(doacao);
        }

        // Criar doação (sem upload, apenas com a URL)
        // Criar doação (com imagem ou imagemUrl)
        public async Task<DoacaoResponseDto> CreateAsync(DoacaoCreateDto dto, string userEmail)
        {
            if (string.IsNullOrWhiteSpace(dto.ImagemUrl))
                throw new Exception("A URL da imagem é obrigatória.");

            // Mapeia o DTO para entidade
            var doacao = _mapper.Map<Doacao>(dto);
            doacao.ImagemUrl = dto.ImagemUrl;

            // 🔹 Vincula o usuário logado, se houver
            var colaborador = await _context.Colaboradores
                .FirstOrDefaultAsync(c => c.Email == userEmail);

            if (colaborador == null)
                throw new Exception("Colaborador não encontrado para este e-mail.");

            doacao.UsuarioId = colaborador.Id;

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();

            return _mapper.Map<DoacaoResponseDto>(doacao);
        }




        // Atualizar doação
        public async Task<DoacaoResponseDto?> UpdateAsync(int id, DoacaoUpdateDto dto)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return null;

            _mapper.Map(dto, doacao);
            await _context.SaveChangesAsync();

            return _mapper.Map<DoacaoResponseDto>(doacao);
        }

        // Deletar doação
        public async Task<bool> DeleteAsync(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return false;

            _context.Doacoes.Remove(doacao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Doacao>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Doacoes
                .Where(d => d.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<List<DoacaoResponseDto>> GetAllAsync()
        {
            var doacoes = await _context.Doacoes
                .Include(d => d.Usuario) // agora o EF sabe que é Colaborador
                .Select(d => new DoacaoResponseDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Descricao = d.Descricao,
                    Quantidade = d.Quantidade,
                    Validade = d.Validade,
                    ImagemUrl = d.ImagemUrl,
                    UsuarioId = d.UsuarioId,
                    NomeColaborador = d.Usuario != null ? d.Usuario.NomeCompleto : "Desconhecido"
                })
                .ToListAsync();

            return doacoes;
        }



    }
}
