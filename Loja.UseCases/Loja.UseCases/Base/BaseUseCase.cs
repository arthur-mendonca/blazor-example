namespace Loja.UseCases.Base;

public abstract class BaseUseCase
{
    protected long idUsuarioLogado = 0;

    public void IdentificarAcesso(long idUsuario)
    {
        idUsuarioLogado = idUsuario;
    }

    protected ResultadoVoid Sucesso()
    {
        return new ResultadoVoid { Sucesso = true };
    }

    protected ResultadoVoid Falha(List<MensagemRetorno> mensagens)
    {
        return new ResultadoVoid { Sucesso = false, Mensagens = mensagens };
    }

    protected ResultadoUnico<T> SucessoObjeto<T>(T objeto)
    {
        return new ResultadoUnico<T> { Sucesso = true, Objeto = objeto };
    }

    protected ResultadoUnico<T> FalhaObjeto<T>(List<MensagemRetorno> mensagens)
    {
        return new ResultadoUnico<T> { Sucesso = false, Mensagens = mensagens };
    }

    protected ResultadoLista<T> SucessoLista<T>(IEnumerable<T> objetos)
    {
        return new ResultadoLista<T> { Sucesso = true, Objetos = objetos };
    }

    protected ResultadoLista<T> FalhaLista<T>(List<MensagemRetorno> mensagens)
    {
        return new ResultadoLista<T> { Sucesso = false, Mensagens = mensagens };
    }
}