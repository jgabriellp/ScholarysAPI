using Microsoft.EntityFrameworkCore;
using SchoolAPI.Models;
using SchoolAPI.Models.Enum;

namespace SchoolAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<AnoLetivo> AnosLetivos => Set<AnoLetivo>();
    public DbSet<Turma> Turmas => Set<Turma>();
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Disciplina> Disciplinas => Set<Disciplina>();
    public DbSet<TurmaDisciplinaProfessor> TurmaDisciplinaProfessores => Set<TurmaDisciplinaProfessor>();
    public DbSet<Frequencia> Frequencias => Set<Frequencia>();
    public DbSet<FrequenciaAluno> FrequenciaAlunos => Set<FrequenciaAluno>();
    public DbSet<Nota> Notas => Set<Nota>();
    public DbSet<DesenvolvimentoMaternal> DesenvolvimentosMaternal => Set<DesenvolvimentoMaternal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enums como string no banco
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<User>()
            .Property(u => u.Ativo)
            .HasColumnType("boolean");

        modelBuilder.Entity<Turma>()
            .Property(t => t.Segmento)
            .HasConversion<string>();

        // Índice único: um professor não pode ter a mesma disciplina na mesma turma/ano
        modelBuilder.Entity<TurmaDisciplinaProfessor>()
            .HasIndex(t => new { t.TurmaId, t.DisciplinaId, t.AnoLetivoId })
            .IsUnique();

        // Índice único: uma turma não pode ter duas frequências no mesmo dia
        modelBuilder.Entity<Frequencia>()
            .HasIndex(f => new { f.TurmaId, f.Data })
            .IsUnique();

        // Índice único: nota por aluno/disciplina/unidade/ano
        modelBuilder.Entity<Nota>()
            .HasIndex(n => new { n.AlunoId, n.DisciplinaId, n.Unidade, n.AnoLetivoId })
            .IsUnique();

        // Índice único: desenvolvimento por aluno/bimestre/ano
        modelBuilder.Entity<DesenvolvimentoMaternal>()
            .HasIndex(d => new { d.AlunoId, d.Bimestre, d.AnoLetivoId })
            .IsUnique();
    }
}