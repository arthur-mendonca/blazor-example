namespace Loja.UseCases.Base;

public abstract class ResultadoBase
{
    public bool Sucesso { get; set; }
    public List<MensagemRetorno> Mensagens { get; set; } = new List<MensagemRetorno>();
}

public class ResultadoVoid : ResultadoBase
{
}

public class ResultadoUnico<T> : ResultadoBase
{
    public T? Objeto { get; set; }
}

public class ResultadoLista<T> : ResultadoBase
{
    public IEnumerable<T> Objetos { get; set; } = new List<T>();
}