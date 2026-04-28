using SchoolAPI.DTOs.Frequencia;
using SchoolAPI.Models;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class FrequenciaService
{
    private readonly IFrequenciaRepository _repository;

    public FrequenciaService(IFrequenciaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FrequenciaResponseDto>> GetByTurmaAsync(int turmaId, int anoLetivoId)
    {
        var frequencias = await _repository.GetByTurmaAsync(turmaId, anoLetivoId);
        return frequencias.Select(Map);
    }

    public async Task<FrequenciaResponseDto?> GetByTurmaEDataAsync(int turmaId, DateOnly data)
    {
        var frequencia = await _repository.GetByTurmaEDataAsync(turmaId, data);
        if (frequencia == null) return null;
        return Map(frequencia);
    }

    public async Task<FrequenciaResponseDto?> GetByIdAsync(int id)
    {
        var frequencia = await _repository.GetByIdAsync(id);
        if (frequencia == null) return null;
        return Map(frequencia);
    }

    public async Task<FrequenciaResponseDto> LancarAsync(FrequenciaRequestDto dto)
    {
        // Se já existe frequência para essa turma/data, atualiza
        var existente = await _repository.GetByTurmaEDataAsync(dto.TurmaId, dto.Data);

        if (existente != null)
        {
            foreach (var item in dto.Alunos)
            {
                var fa = existente.FrequenciaAlunos.FirstOrDefault(x => x.AlunoId == item.AlunoId);
                if (fa != null)
                    fa.Presente = item.Presente;
                else
                    existente.FrequenciaAlunos.Add(new FrequenciaAluno
                    {
                        AlunoId = item.AlunoId,
                        Presente = item.Presente
                    });
            }

            var updated = await _repository.UpdateAsync(existente);
            return Map(updated);
        }

        // Cria nova frequência
        var frequencia = new Frequencia
        {
            TurmaId = dto.TurmaId,
            AnoLetivoId = dto.AnoLetivoId,
            Data = dto.Data,
            FrequenciaAlunos = dto.Alunos.Select(a => new FrequenciaAluno
            {
                AlunoId = a.AlunoId,
                Presente = a.Presente
            }).ToList()
        };

        var created = await _repository.CreateAsync(frequencia);
        return Map(created);
    }

    // Calcula frequência de um aluno em uma turma/ano
    public async Task<decimal> CalcularFrequenciaAsync(int alunoId, int turmaId, int anoLetivoId)
    {
        var frequencias = await _repository.GetByTurmaAsync(turmaId, anoLetivoId);
        var totalDias = frequencias.Count();
        if (totalDias == 0) return 0;

        var diasPresente = frequencias
            .SelectMany(f => f.FrequenciaAlunos)
            .Count(fa => fa.AlunoId == alunoId && fa.Presente);

        return Math.Round((decimal)diasPresente / totalDias * 100, 2);
    }

    private static FrequenciaResponseDto Map(Frequencia f) => new(
        f.Id,
        f.TurmaId,
        f.Data,
        f.FrequenciaAlunos.Select(fa => new FrequenciaAlunoResponseDto(
            fa.AlunoId,
            fa.Aluno?.Nome ?? "",
            fa.Presente
        )).ToList()
    );
}