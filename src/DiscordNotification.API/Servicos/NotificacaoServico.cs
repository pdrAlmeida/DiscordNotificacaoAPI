using DiscordNotification.API.Configuration;
using DiscordNotification.API.DTOs;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordNotification.API.Services
{
    public class NotificacaoServico : INotificacaoServico
    {
        private readonly string _caminhoArquivoTemp;
        private readonly DiscordClient _discord;

        public NotificacaoServico(DiscordClient discordCliente, IOptions<Config> configuracoes)
        {
            _discord = discordCliente;
            _caminhoArquivoTemp = configuracoes.Value.CaminhoArquivoTemporario;
        }

        public async void Notificar(NotificacaoDTO notificacao)
        {
            var servidor = ObterServidor(notificacao.NomeServidor);

            var canal = await ObterCanal(notificacao.NomeCanal, servidor, notificacao.CriarCanalCasoNaoExista);

            if (notificacao.ExibirComoTopico)
                canal = await ObterTopico(notificacao.Titulo, canal);


            string mensagem = EscreverMensagem(notificacao);


            if (MensagemExcedeuLimiteDeCaracteres(mensagem.Length, notificacao.Formato))
                await EnviarMensagemReduzidaComArquivo(mensagem, notificacao, canal);

            else
                await EnviarMensagem(mensagem, notificacao.Titulo, notificacao.Formato, canal);
        }


        private DiscordGuild ObterServidor(string nomeServidor) 
            => _discord.Guilds.FirstOrDefault(s => s.Value.Name.ToUpper() == nomeServidor.ToUpper()).Value
                ?? throw new Exception($"O servidor {nomeServidor} não encontrado!");
        private static async Task<DiscordChannel> ObterCanal(string nomeCanal, DiscordGuild servidor, bool criarCanalCasoNaoExista)
        {
            var canal = servidor.Channels.FirstOrDefault(c => c.Value.Name.ToLower() == nomeCanal.ToLower()).Value;

            return canal is not null
                ? canal
                : criarCanalCasoNaoExista
                    ? await servidor.CreateChannelAsync(nomeCanal, ChannelType.Text)
                    : throw new Exception($"O canal {nomeCanal} não foi encontrado!");
        }
        private static async Task<DiscordThreadChannel> ObterTopico(string titulo, DiscordChannel canal)
        {
            return canal.Threads.FirstOrDefault(t => t.Name.ToLower() == titulo.ToLower()) ??
                await canal.CreateThreadAsync(titulo, AutoArchiveDuration.Day, ChannelType.PublicThread);
        }


        private async Task EnviarMensagemReduzidaComArquivo(string mensagem, NotificacaoDTO notificacao, DiscordChannel canal)
        {
            var arquivo = CriarArquivoDeMensagem(mensagem);

            var mensagemReduzida = EscreverMensagemReduzida(notificacao);

            await canal.SendMessageAsync(new DiscordMessageBuilder()
                .WithContent(mensagemReduzida)
                .WithFiles(new Dictionary<string, Stream>() { { $"{notificacao.Titulo}.txt", arquivo.Stream } }));

            arquivo.Stream.Close();
            File.Delete(arquivo.CaminhoArquivo);
        }
        private static async Task EnviarMensagem(string mensagem, string titulo, FormatoNotificacao formatoNotificacao, DiscordChannel canal)
        {
            if (formatoNotificacao == FormatoNotificacao.Embed)
            {
                await canal.SendMessageAsync(new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Aquamarine,
                    Description = mensagem,
                    Title = titulo
                });
            }
            else
                await canal.SendMessageAsync(mensagem);
        }


        private ArquivoMensagemDTO CriarArquivoDeMensagem(string mensagem)
        {
            string caminhoArquivo = $"{_caminhoArquivoTemp}\\{Guid.NewGuid()}.txt";

            using (StreamWriter file = File.CreateText(caminhoArquivo))
            {
                file.Write(mensagem);
                file.Close();
            };
            return new ArquivoMensagemDTO(File.OpenRead(caminhoArquivo), caminhoArquivo);
        }


        private static string EscreverMensagem(NotificacaoDTO notification)
        {
            var str = new StringBuilder();
            if (notification.Formato == FormatoNotificacao.String)
            {
                str.AppendLine($"__**{notification.Titulo}**__");
                str.AppendLine();
            }
            str.AppendLine($"**MENSAGEM:** {notification.Mensagem}");

            if (notification.Link != null)
            {
                str.AppendLine();
                str.AppendLine($"**LINK:** {notification.Link}");
            }

            if (notification.Exception != null)
            {
                str.AppendLine();
                str.AppendLine($"**EX:** {notification.Exception}");
            }

            return str.ToString();
        }
        private static string EscreverMensagemReduzida(NotificacaoDTO notification)
        {
            var strB = new StringBuilder();

            strB.AppendLine($"__**{notification.Titulo}**__");
            strB.AppendLine();

            strB.AppendLine($"**MENSAGEM:** {notification.Mensagem}");

            if (notification.Link != null)
            {
                strB.AppendLine();
                strB.AppendLine($"**LINK:** {notification.Link}");
            }

            if (notification.Exception != null)
            {
                strB.AppendLine();
                strB.AppendLine($"**EX:** {notification.Exception}");
            }

            string str = strB.ToString();
            if (str.Length > 2000)
            {
                str = str[..(2000 - 54)];
                str += "... `Veja a notificação completa no seguinte arquivo:`";
            }

            return str;
        }


        private static bool MensagemExcedeuLimiteDeCaracteres(int tamanhoMensagem, FormatoNotificacao formatoNotificacao) 
            => (tamanhoMensagem > 2000 && formatoNotificacao == FormatoNotificacao.String)
                ||
               (tamanhoMensagem > 4096 && formatoNotificacao == FormatoNotificacao.Embed);
    }
}