using SchoolAPI.DTOs.DiaLetivo;
using SchoolAPI.Models;
using SchoolAPI.Models.Enum;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DiaLetivoService
{
    private readonly IDiaLetivoRepository _repository;
    private readonly ITurmaDisciplinaProfessorRepository _turmaDisciplinaProfessorRepository;

    public DiaLetivoService(
        IDiaLetivoRepository repository,
        ITurmaDisciplinaProfessorRepository turmaDisciplinaProfessorRepository)
    {
        _repository = repository;
        _turmaDisciplinaProfessorRepository = turmaDisciplinaProfessorRepository;
    }

    public async Task<IEnumerable<DiaLetivoResponseDto>> GetByAnoLetivoAsync(
        int anoLetivoId, SegmentoEnum? segmento = null, int? turmaId = null, int? professorId = null)
    {
        var dias = await _repository.GetByAnoLetivoAsync(anoLetivoId, segmento);

        if (turmaId.HasValue && professorId.HasValue)
        {
            var diasSemana = await GetDiasSemanaDoProfessorAsync(turmaId.Value, professorId.Value, anoLetivoId);
            if (diasSemana != null)
                dias = dias.Where(d => diasSemana.Contains((int)d.Data.DayOfWeek));
        }

        return dias.Select(Map);
    }

    // Dias da semana em que o professor dá aula na turma, ou null quando não há restrição
    // (sem vínculo, ou algum vínculo sem dias definidos — ex.: professor regente).
    private async Task<HashSet<int>?> GetDiasSemanaDoProfessorAsync(int turmaId, int professorId, int anoLetivoId)
    {
        var vinculos = (await _turmaDisciplinaProfessorRepository
            .GetByTurmaEProfessorAsync(turmaId, professorId, anoLetivoId)).ToList();

        if (vinculos.Count == 0 || vinculos.Any(v => v.DiasSemana.Count == 0))
            return null;

        return vinculos.SelectMany(v => v.DiasSemana).ToHashSet();
    }

    public async Task<IEnumerable<DiaLetivoResponseDto>> CreateLoteAsync(DiaLetivoLoteRequestDto dto)
    {
        if (!Enum.IsDefined(typeof(SegmentoEnum), dto.Segmento))
            throw new ArgumentException("Segmento inválido.");

        var dias = dto.Datas.Select(data => new DiaLetivo
        {
            AnoLetivoId = dto.AnoLetivoId,
            Segmento = dto.Segmento,
            Data = data
        });

        var criados = await _repository.CreateLoteAsync(dias);
        return criados.Select(Map);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var dia = await _repository.GetByIdAsync(id);
        if (dia == null) return false;

        await _repository.DeleteAsync(dia);
        return true;
    }

    private static DiaLetivoResponseDto Map(DiaLetivo d) => new(
        d.Id,
        d.AnoLetivoId,
        d.AnoLetivo?.Ano ?? 0,
        d.Data,
        d.Segmento
    );
}
