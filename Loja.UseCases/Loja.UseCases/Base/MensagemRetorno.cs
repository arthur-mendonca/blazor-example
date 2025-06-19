namespace Loja.UseCases.Base;

public class MensagemRetorno
{
    public string Texto { get; set; } = default!;
    public EOrigem Origem { get; set; }

    public MensagemRetorno(string texto, EOrigem origem = EOrigem.Info)
    {
        Texto = texto;
        Origem = origem;
    }

    public enum EOrigem
    {
        Info,
        Aviso,
        Erro
    }
}