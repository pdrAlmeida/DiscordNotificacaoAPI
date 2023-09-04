using DSharpPlus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordNotification.API;

public class DiscordBot
{
    public DiscordClient Client { get; private set; }

    public DiscordBot(string token)
    {
        Client = new DiscordClient(new DiscordConfiguration
        {
            Token = token,
            TokenType = TokenType.Bot,
            ReconnectIndefinitely = true,
            GatewayCompressionLevel = GatewayCompressionLevel.Stream,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Debug
        });
    }

    public async Task IniciarBotAsync() =>  await Client.ConnectAsync();
}