using SchoolAPI.DTOs.Disciplina;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DisciplinaService
{
    private readonly IDisciplinaRepository _repository;

    public DisciplinaService(IDisciplinaRepository repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<DisciplinaResponseDto> Data, int Total)> GetAllAsync(int page, int pageSize)
    {
        var (disciplinas, total) = await _repository.GetAllAsync(page, pageSize);
        var data = disciplinas.Select(d => new DisciplinaResponseDto(d.Id, d.Nome, d.Ativo));
        return (data, total);
    }

    public async Task<DisciplinaResponseDto?> GetByIdAsync(int id)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return null;
        return new DisciplinaResponseDto(disciplina.Id, disciplina.Nome, disciplina.Ativo);
    }

    public async Task<DisciplinaResponseDto> CreateAsync(DisciplinaRequestDto dto)
    {
        var disciplina = new Disciplina
        {
            Nome = dto.Nome,
            Ativo = true
        };

        var created = await _repository.CreateAsync(disciplina);
        return new DisciplinaResponseDto(created.Id, created.Nome, created.Ativo);
    }

    public async Task<DisciplinaResponseDto?> UpdateAsync(int id, DisciplinaRequestDto dto)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return null;

        disciplina.Nome = dto.Nome;

        var updated = await _repository.UpdateAsync(disciplina);
        return new DisciplinaResponseDto(updated.Id, updated.Nome, updated.Ativo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var disciplina = await _repository.GetByIdAsync(id);
        if (disciplina == null) return false;

        await _repository.DeleteAsync(disciplina);
        return true;
    }
}