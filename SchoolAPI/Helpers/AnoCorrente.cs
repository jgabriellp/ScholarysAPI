namespace SchoolAPI.Helpers;

/// <summary>
/// Regra do "ano letivo corrente" (sem coluna no banco):
/// 1) o ano letivo igual ao ano atual no horário de Brasília;
/// 2) senão, o mais recente que não esteja no futuro;
/// 3) senão (só existem anos futuros), o mais próximo.
/// Assim, cadastrar o ano seguinte com antecedência não muda o ano em uso.
/// </summary>
public static class AnoCorrente
{
    // Brasília é UTC-3 fixo (sem horário de verão desde 2019).
    public static int AnoAtual() => DateTime.UtcNow.AddHours(-3).Year;

    // Nas consultas EF a ordenação usada é:
    //   OrderBy(ano <= anoAtual ? anoAtual - ano : ano)
    // Anos até o atual ficam com chave pequena (0 = ano atual), anos futuros com chave = o próprio ano
    // (sempre maior), então o primeiro resultado é o ano corrente pela regra acima.
}
