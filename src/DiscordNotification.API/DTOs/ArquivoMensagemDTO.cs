using System.IO;

namespace DiscordNotification.API.DTOs
{
    public class ArquivoMensagemDTO
    {
        public ArquivoMensagemDTO(Stream stream, string path)
        {
            Stream = stream;
            CaminhoArquivo = path;
        }

        public Stream Stream { get; private set; }
        public string CaminhoArquivo { get; private set; }
    }
}