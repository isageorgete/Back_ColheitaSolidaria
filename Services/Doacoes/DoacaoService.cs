using AutoMapper;
using Back_ColheitaSolidaria.Data;
using Back_ColheitaSolidaria.DTOs.Doacoes;
using Back_ColheitaSolidaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Supabase;
using Supabase.Storage;
using System.IO;

namespace Back_ColheitaSolidaria.Services.Doacoes
{
    public class DoacaoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly Supabase.Client _supabaseClient;

        public DoacaoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            // Inicializa o Supabase Client
            _supabaseClient = new Supabase.Client("https://pyjqpkkscqlokgmdtslk.supabase.co", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InB5anFwa2tzY3Fsb2tnbWR0c2xrIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDkwNDM5ODUsImV4cCI6MjA2NDYxOTk4NX0.9Kx1mTVvz7ZMH6Xi_e6ZBnQjNgaYQO28-cktBNE-Fso");
            _supabaseClient.InitializeAsync().Wait();
        }

        // Listar todas as doações
        public async Task<List<DoacaoResponseDto>> GetAllAsync()
        {
            var doacoes = await _context.Doacoes.ToListAsync();
            return _mapper.Map<List<DoacaoResponseDto>>(doacoes);
        }

        // Buscar doação por ID
        public async Task<DoacaoResponseDto?> GetByIdAsync(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            return doacao == null ? null : _mapper.Map<DoacaoResponseDto>(doacao);
        }

        public async Task<DoacaoResponseDto> CreateAsync(DoacaoCreateDto dto)
        {
            try
            {
                Console.WriteLine("=== SERVICE: INICIANDO CREATE ===");
                string urlImagem = null;

                if (dto.Imagem != null)
                {
                    Console.WriteLine("Fazendo upload da imagem...");
                    var nomeArquivo = $"{Guid.NewGuid()}_{dto.Imagem.FileName}";
                    var bucket = _supabaseClient.Storage.From("doacoes");

                    using var stream = dto.Imagem.OpenReadStream();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    Console.WriteLine($"Bytes do arquivo: {fileBytes.Length}");

                    var options = new Supabase.Storage.FileOptions
                    {
                        CacheControl = "3600",
                        Upsert = false
                    };

                    await bucket.Upload(fileBytes, nomeArquivo, options);
                    urlImagem = bucket.GetPublicUrl(nomeArquivo);
                    Console.WriteLine($"Upload concluído. URL: {urlImagem}");
                }

                var doacao = _mapper.Map<Doacao>(dto);
                doacao.ImagemUrl = urlImagem;

                Console.WriteLine("Salvando no banco de dados...");
                _context.Doacoes.Add(doacao);
                await _context.SaveChangesAsync();
                Console.WriteLine("Salvo com sucesso!");

                return _mapper.Map<DoacaoResponseDto>(doacao);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO NO SERVICE: {ex}");
                throw;
            }
        }

        public async Task<DoacaoResponseDto?> UpdateAsync(int id, DoacaoUpdateDto dto)
        {
            var doacao = await _context.Doacoes.FindAsync(id);
            if (doacao == null) return null;

            if (dto.NovaImagem != null)
            {
                var nomeArquivo = $"{Guid.NewGuid()}_{dto.NovaImagem.FileName}";
                var bucket = _supabaseClient.Storage.From("doacoes");

                using var stream = dto.NovaImagem.OpenReadStream();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var options = new Supabase.Storage.FileOptions
                {
                    CacheControl = "3600",
                    Upsert = false
                };

                await bucket.Upload(fileBytes, nomeArquivo, options);
                doacao.ImagemUrl = bucket.GetPublicUrl(nomeArquivo);
            }

            // Mapeia apenas os dados básicos (ignora NovaImagem e UrlImagem)
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
    }
}