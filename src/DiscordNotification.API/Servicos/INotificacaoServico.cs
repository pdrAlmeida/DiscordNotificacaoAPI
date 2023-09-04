using DiscordNotification.API.DTOs;

namespace DiscordNotification.API.Services
{
    public interface INotificacaoServico
    {
        public void Notificar(NotificacaoDTO notification);
    }
}