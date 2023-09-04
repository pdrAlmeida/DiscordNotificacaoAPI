# API de Notificações Discord

Esta é uma API simples para enviar notificações para servidores do Discord. Ela permite que você envie mensagens formatadas ou embutidas para canais específicos em servidores do Discord. Abaixo, você encontrará informações sobre como configurar e usar esta API.

## Configuração

Antes de começar a usar esta API, siga estas etapas de configuração:

### 1. Crie um Bot no Discord

Para usar esta API, você precisará criar um bot no Discord e obter o token dele. Siga os passos abaixo:

1. Acesse o [Painel de Desenvolvedor do Discord](https://discord.com/developers/applications).
2. Clique em "New Application" e dê um nome à sua aplicação.
3. No painel da sua nova aplicação, vá para a guia "Bot" no menu lateral esquerdo.
4. Clique em "Add Bot" e confirme a criação.
5. Em seguida, encontre a seção "Token" e clique em "Copy" para copiar o token do seu bot.

### 2. Adicione o Bot ao seu Servidor Discord

Para que o bot possa enviar mensagens para o servidor Discord desejado, você precisa adicioná-lo ao servidor. Siga os passos abaixo:

1. Ainda no painel da sua aplicação Discord, vá para a guia "OAuth2" no menu lateral esquerdo.
2. Na seção "OAuth2 URL Generator", selecione as permissões necessárias para o seu bot. Para este projeto, você precisa de permissões para "bot".
3. Role para baixo até a seção "OAuth2 URL" gerada e clique no link gerado. Isso abrirá uma página da web que permitirá adicionar o bot a um servidor.
4. Selecione o servidor para o qual deseja adicionar o bot e siga as instruções para concluir o processo.

### 3. Preencha o Caminho do Arquivo Temporário

Esta API pode criar arquivos temporários para mensagens longas. Certifique-se de configurar o caminho do arquivo temporário no arquivo `appsettings.json` usando a chave `CaminhoArquivoTemporario`.

## Como Usar

Para usar esta API, faça uma solicitação POST para `/Notificacao` com um corpo JSON contendo os detalhes da notificação. Os seguintes campos são obrigatórios:

- `Mensagem`: Texto principal da mensagem (obrigatório).
- `Titulo`: Título da mensagem (caso a propriedade "ExibirComoTopico" seja true, o tópico será criado com este nome) (obrigatório).
- `NomeCanal`: Nome do canal onde a mensagem será enviada (obrigatório).
- `NomeServidor`: Nome do servidor onde a mensagem será enviada (obrigatório).

Outros campos disponíveis no corpo da mensagem incluem:

- `ExibirComoTopico`: Se true, a mensagem será enviada em um tópico do Discord com o mesmo nome do título (opcional, padrão = false).

   **O que é um tópico no Discord?** No Discord, um tópico é uma funcionalidade que permite criar uma área de discussão separada dentro de um canal existente. É semelhante a ter uma subseção de conversa dedicada a um tópico específico dentro de um canal maior. Quando você define `ExibirComoTopico` como verdadeiro, a mensagem será enviada como parte desse tópico, e o nome do tópico será o mesmo que o título da mensagem. Se o tópico não existir, ele será criado automaticamente no canal.

- `CriarCanalCasoNaoExista`: Se true e o canal informado não existir no servidor, um novo canal de texto será criado com o nome informado (opcional, padrão = false).
- `Formato`: Modo como a mensagem será enviada no Discord (0 = String, 1 = Embed) (opcional, padrão = Texto).

   **O que é um embed no Discord?** No Discord, um "embed" é uma maneira de formatar mensagens de forma mais rica e visualmente atraente. Em uma mensagem de "embed," você pode incluir informações adicionais, como títulos, descrições, imagens, links e muito mais. Isso permite criar mensagens mais informativas e agradáveis visualmente.

   - **Formato 0 (String)**: Se você selecionar "Texto," a mensagem será enviada como um texto simples, sem formatação adicional. É adequado para mensagens de texto básicas.

   - **Formato 1 (Embed)**: Se você selecionar "Embed," a mensagem será enviada em um formato enriquecido, onde você pode personalizar a aparência da mensagem, adicionando títulos, descrições, cores de fundo e muito mais. Isso é útil quando você deseja destacar informações importantes ou tornar a mensagem mais visualmente atraente.

- `Exception`: Espaço para texto de uma exceção gerada (opcional).
- `Link`: Espaço para adicionar um link e enviá-lo junto com a mensagem (opcional).

## Como Consumir a Rota da API

Para consumir a rota da API que permite enviar notificações para o Discord, você pode seguir estas etapas:

### Método HTTP e URL

A rota da API é configurada para responder a solicitações POST no seguinte endpoint:

```http
POST /Notificacao
```

### Corpo da Solicitação

O corpo da solicitação deve ser um objeto JSON que contém os detalhes da notificação que você deseja enviar. Aqui está um exemplo de corpo de solicitação DTO:

```json
{
  "mensagem": "Esta é uma mensagem de exemplo.",
  "titulo": "Título da Mensagem",
  "nomeCanal": "canal-exemplo",
  "nomeServidor": "servidor-exemplo"
}
```
Lembre-se de que os quatro primeiros campos (mensagem, título, nomeCanal e nomeServidor) são obrigatórios para o funcionamento da mensagem.

### Exemplo de Requisição cURL

Você pode usar a ferramenta cURL para fazer uma solicitação POST para a rota da API. Substitua os valores no corpo da solicitação pelo seu próprio conteúdo:

```
curl -X POST "https://sua-api.com/Notificacao" -H "Content-Type: application/json" -d 
'{
  "mensagem": "Esta é uma mensagem de exemplo.",
  "titulo": "Título da Mensagem",
  "nomeCanal": "canal-exemplo",
  "nomeServidor": "servidor-exemplo"
}'
```

### Resposta da API

A API responderá com uma mensagem de sucesso ou erro, dependendo do resultado da operação. Você receberá uma resposta JSON que pode ser tratada em seu aplicativo, dependendo de suas necessidades.

## Autores

- [Pedro Almeida](https://github.com/pdrAlmeida) - Desenvolvedor principal

## Feito Em

- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) - Linguagem de programação
- [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) - Integração com o Discord
