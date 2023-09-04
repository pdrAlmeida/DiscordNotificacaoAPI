using DiscordNotification.API;
using DiscordNotification.API.Configuration;
using DiscordNotification.API.DTOs;
using DiscordNotification.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));

var discordBot = new DiscordBot(builder.Configuration.GetSection("Config").Get<Config>().TokenDiscordBot);

builder.Services.AddSingleton(discordBot.Client);
builder.Services.AddScoped<INotificacaoServico, NotificacaoServico>();

var app = builder.Build();

app.MapPost("/Notificacao", (INotificacaoServico notificacaoServico, NotificacaoDTO notificacao) =>
{
    try
    {
        notificacaoServico.Notificar(notificacao);
        return Results.Ok("Notificação enviada com sucesso!");
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.ToString());
    }
});
app.UseHttpsRedirection();

await discordBot.IniciarBotAsync();

app.Run();