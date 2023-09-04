using System.Collections.Generic;

namespace DiscordNotification.API.DTOs
{
    public record NotificacaoDTO(
        string Mensagem,
        string Titulo,
        string NomeCanal,
        string NomeServidor,
        bool ExibirComoTopico,
        bool CriarCanalCasoNaoExista,
        FormatoNotificacao Formato,
        string? Exception,
        string? Link);

    public enum FormatoNotificacao
    {
        String = 0,
        Embed = 1
    }
}