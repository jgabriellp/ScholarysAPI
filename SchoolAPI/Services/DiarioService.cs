using SchoolAPI.DTOs.Diario;
using SchoolAPI.Models.Enum;
using SchoolAPI.Repositories.Interfaces;

namespace SchoolAPI.Services;

public class DiarioService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IFrequenciaRepository _frequenciaRepository;
    private readonly INotaRepository _notaRepository;
    private readonly IDesenvolvimentoMaternalRepository _desenvolvimentoRepository;
    private readonly IDisciplinaRepository _disciplinaRepository;

    public DiarioService(
        IAlunoRepository alunoRepository,
        IFrequenciaRepository frequenciaRepository,
        INotaRepository notaRepository,
        IDesenvolvimentoMaternalRepository desenvolvimentoRepository,
        IDisciplinaRepository disciplinaRepository)
    {
        _alunoRepository = alunoRepository;
        _frequenciaRepository = frequenciaRepository;
        _notaRepository = notaRepository;
        _desenvolvimentoRepository = desenvolvimentoRepository;
        _disciplinaRepository = disciplinaRepository;
    }

    public async Task<DiarioMaternalDto?> GetDiarioMaternalAsync(int alunoId, int anoLetivoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);
        if (aluno == null) return null;

        var frequenciaGrid = await MontarGridFrequenciaAsync(alunoId, aluno.TurmaId, anoLetivoId);

        var desenvolvimentos = await _desenvolvimentoRepository.GetByAlunoAsync(alunoId, anoLetivoId);

        return new DiarioMaternalDto(
            aluno.Id,
            aluno.Nome,
            aluno.NumeroChamada,
            aluno.Turma?.Nome ?? "",
            aluno.AnoLetivo?.Ano ?? 0,
            frequenciaGrid,
            desenvolvimentos.Select(d => new DesenvolvimentoBimestralDto(d.Bimestre, d.Descricao))
        );
    }

    public async Task<DiarioFundamentalDto?> GetDiarioFundamentalAsync(int alunoId, int anoLetivoId)
    {
        var aluno = await _alunoRepository.GetByIdAsync(alunoId);
        if (aluno == null) return null;

        var frequenciaGrid = await MontarGridFrequenciaAsync(alunoId, aluno.TurmaId, anoLetivoId);

        var disciplinas = await _disciplinaRepository.GetAllAsync(1, 100);
        var notas = (await _notaRepository.GetByAlunoAsync(alunoId, anoLetivoId)).ToList();

        var notasPorDisciplina = disciplinas.Data.Select(d =>
        {
            decimal? GetNota(int unidade) => notas
                .FirstOrDefault(n => n.DisciplinaId == d.Id && n.Unidade == unidade)?.Valor;

            var u1 = GetNota(1);
            var u2 = GetNota(2);
            var u3 = GetNota(3);
            var u4 = GetNota(4);
            var u5 = GetNota(5);
            var u6 = GetNota(6);
            var rec = GetNota(7);

            decimal? media1Sem = (u1.HasValue && u2.HasValue && u3.HasValue)
                ? Math.Round((u1.Value + u2.Value + u3.Value) / 3, 2) : null;

            decimal? media2Sem = (u4.HasValue && u5.HasValue && u6.HasValue)
                ? Math.Round((u4.Value + u5.Value + u6.Value) / 3, 2) : null;

            decimal? mediaAnual = (media1Sem.HasValue && media2Sem.HasValue)
                ? Math.Round((media1Sem.Value + media2Sem.Value) / 2, 2) : null;

            decimal? mediaFinal = null;
            string? resultado = null;

            if (mediaAnual.HasValue)
            {
                if (mediaAnual.Value >= 6)
                {
                    mediaFinal = mediaAnual;
                    resultado = "Aprovado";
                }
                else if (rec.HasValue)
                {
                    mediaFinal = Math.Round((mediaAnual.Value + rec.Value) / 2, 2);
                    resultado = mediaFinal >= 6 ? "Aprovado" : "Reprovado";
                }
            }

            return new NotaDisciplinaDto(
                d.Id, d.Nome,
                u1, u2, u3, media1Sem,
                u4, u5, u6, media2Sem,
                mediaAnual, rec, mediaFinal, resultado
            );
        });

        // Resultado geral: reprovado se qualquer disciplina reprovada
        var resultadoGeral = notasPorDisciplina.Any(n => n.Resultado == "Reprovado")
            ? "Reprovado"
            : notasPorDisciplina.All(n => n.Resultado == "Aprovado")
                ? "Aprovado" : null;

        return new DiarioFundamentalDto(
            aluno.Id,
            aluno.Nome,
            aluno.NumeroChamada,
            aluno.Turma?.Nome ?? "",
            aluno.AnoLetivo?.Ano ?? 0,
            frequenciaGrid,
            notasPorDisciplina,
            null,
            resultadoGeral
        );
    }

    private async Task<GridFrequenciaDto> MontarGridFrequenciaAsync(int alunoId, int turmaId, int anoLetivoId)
    {
        var frequencias = (await _frequenciaRepository.GetByTurmaAsync(turmaId, anoLetivoId)).ToList();

        var mesesNome = new[]
        {
            "", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
            "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
        };

        var meses = Enumerable.Range(1, 12).Select(mes =>
        {
            var frequenciasMes = frequencias.Where(f => f.Data.Month == mes).ToList();
            var dias = new Dictionary<int, bool?>();

            foreach (var f in frequenciasMes)
            {
                var fa = f.FrequenciaAlunos.FirstOrDefault(x => x.AlunoId == alunoId);
                dias[f.Data.Day] = fa?.Presente;
            }

            var totalFaltasMes = dias.Values.Count(v => v == false);

            return new FrequenciaMesDto(mes, mesesNome[mes], dias, totalFaltasMes);
        });

        var totalFaltas = meses.Sum(m => m.TotalFaltas);
        var totalDias = frequencias.Count;
        var percentual = totalDias > 0
            ? Math.Round((decimal)(totalDias - totalFaltas) / totalDias * 100, 2)
            : 0;

        return new GridFrequenciaDto(meses, totalFaltas, percentual);
    }
}