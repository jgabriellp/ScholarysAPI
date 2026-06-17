using SchoolAPI.DTOs.Disciplina;
using SchoolAPI.Models;
using SchoolAPI.Models.Enum;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DisciplinaService
{
    private readonly IDisciplinaRepository _repository;

    public DisciplinaService(IDisciplinaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<DisciplinaResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize, SegmentoEnum? segmento)
    {
        var (disciplinas, total) = await _repository.GetAllAsync(page, pageSize, segmento);
        var data = disciplinas.Select(Map);
        return (data, total);
    }

    public async Task<DisciplinaResponseDto?> GetByIdAsync(int id)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return null;
        return Map(disciplina);
    }

    public async Task<DisciplinaResponseDto> CreateAsync(DisciplinaRequestDto dto)
    {
        var disciplina = new Disciplina
        {
            Nome = dto.Nome,
            Segmento = dto.Segmento,
            Ativo = true
        };

        var created = await _repository.CreateAsync(disciplina);
        return Map(created);
    }

    public async Task<DisciplinaResponseDto?> UpdateAsync(int id, DisciplinaRequestDto dto)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return null;

        disciplina.Nome = dto.Nome;
        disciplina.Segmento = dto.Segmento;

        var updated = await _repository.UpdateAsync(disciplina);
        return Map(updated);
    }

    private static DisciplinaResponseDto Map(Disciplina d) => new(d.Id, d.Nome, d.Segmento, d.Ativo);

    public async Task<bool> DeleteAsync(int id)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return false;

        await _repository.DeleteAsync(disciplina);
        return true;
    }
}