using SchoolAPI.Models;

namespace SchoolAPI.Repositories.Interfaces;

public interface IDesenvolvimentoMaternalRepository
{
    Task<IEnumerable<DesenvolvimentoMaternal>> GetByAlunoAsync(int alunoId, int anoLetivoId);
    Task<DesenvolvimentoMaternal?> GetByAlunoEBimestreAsync(int alunoId, int bimestre, int anoLetivoId);
    Task<DesenvolvimentoMaternal?> GetByIdAsync(int id);
    Task<DesenvolvimentoMaternal> CreateAsync(DesenvolvimentoMaternal desenvolvimento);
    Task<DesenvolvimentoMaternal> UpdateAsync(DesenvolvimentoMaternal desenvolvimento);
    Task DeleteAsync(DesenvolvimentoMaternal desenvolvimento);
}