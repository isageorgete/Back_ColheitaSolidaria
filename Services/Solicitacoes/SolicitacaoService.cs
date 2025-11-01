using AutoMapper;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs.Solicitacoes;
using Back_ColheitaSolidaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_ColheitaSolidaria.Services.Solicitacoes
{
    public class SolicitacaoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SolicitacaoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Listar todas as solicitações
        public async Task<List<SolicitacaoResponseDto>> GetAllAsync()
        {
            var solicitacoes = await _context.Solicitacoes.ToListAsync();
            return _mapper.Map<List<SolicitacaoResponseDto>>(solicitacoes);
        }

        // Buscar solicitação por Id
        public async Task<SolicitacaoResponseDto?> GetByIdAsync(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            return solicitacao == null ? null : _mapper.Map<SolicitacaoResponseDto>(solicitacao);
        }

        // Criar nova solicitação
        public async Task<SolicitacaoResponseDto> CreateAsync(SolicitacaoCreateDto dto)
        {
            var solicitacao = _mapper.Map<Solicitacao>(dto);
            solicitacao.Status = "Pendente";
            solicitacao.DataSolicitacao = DateTime.Now;

            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();

            return _mapper.Map<SolicitacaoResponseDto>(solicitacao);
        }

        // Atualizar status da solicitação
        public async Task<SolicitacaoResponseDto?> UpdateStatusAsync(int id, SolicitacaoUpdateDto dto)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null) return null;

            solicitacao.Status = dto.Status;
            await _context.SaveChangesAsync();

            return _mapper.Map<SolicitacaoResponseDto>(solicitacao);
        }

        // Aprovar solicitação
        public async Task<SolicitacaoResponseDto?> AprovarAsync(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null) return null;

            solicitacao.Status = "Aprovada";
            await _context.SaveChangesAsync();

            return _mapper.Map<SolicitacaoResponseDto>(solicitacao);
        }

        // Negar solicitação
        public async Task<SolicitacaoResponseDto?> NegarAsync(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null) return null;

            solicitacao.Status = "Negada";
            await _context.SaveChangesAsync();

            return _mapper.Map<SolicitacaoResponseDto>(solicitacao);
        }

        // Deletar solicitação
        public async Task<bool> DeleteAsync(int id)
        {
            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null) return false;

            _context.Solicitacoes.Remove(solicitacao);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
