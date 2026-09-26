Site de Receitas

Aplicação ASP.NET Core (Razor Pages) com SQL Server.

Alterações recentes
1. Senha removida do código-fonte

A connection string estava fixa em IngredientRepository e UserRepository, com o texto Password=${PASSWORD}. Em C#, ${PASSWORD} não é substituído por nada: o texto era enviado literalmente ao SQL Server.

Agora a connection string é lida da configuração do .NET (ConnectionStrings:Receita):

Localmente: via User Secrets, fora da pasta do projeto.
No GitHub Actions: via variável de ambiente ConnectionStrings__Receita.
2. Repositórios recebem a connection string pelo construtor

IngredientRepository e UserRepository agora têm o construtor:

csharp
public UserRepository(string connString)

O Program.cs lê a configuração uma vez e registra os repositórios com uma factory:

csharp
var connString = builder.Configuration.GetConnectionString("Receita")
    ?? throw new InvalidOperationException("ConnectionStrings:Receita não configurada.");

builder.Services.AddScoped(_ => new UserRepository(connString));
builder.Services.AddScoped(_ => new IngredientRepository(connString));

Se a connection string não estiver configurada, a aplicação falha logo na inicialização, com uma mensagem clara.

3. UserRepository
GetUsers passou a usar using em SqlConnection, SqlCommand e SqlDataReader, então a conexão é fechada mesmo em caso de exceção.
AddUser foi implementado (antes lançava NotImplementedException), com INSERT parametrizado.
Foram removidos a connection string comentada e o using System.Security.Cryptography.X509Certificates que não era usado.
4. Senhas de usuário com hash

A coluna password da tabela [USER] deve guardar apenas o hash, nunca a senha em texto puro. O hash é gerado no UserService com PasswordHasher<User> (ASP.NET Core Identity):

csharp
using Microsoft.AspNetCore.Identity;

private readonly PasswordHasher<User> hasher = new();

public void Register(User user, string plainPassword)
{
    user.Password = hasher.HashPassword(user, plainPassword);
    userRepository.AddUser(user);
}

public bool CheckPassword(User user, string plainPassword)
{
    var result = hasher.VerifyHashedPassword(user, user.Password, plainPassword);
    return result != PasswordVerificationResult.Failed;
}

No login, use CheckPassword em vez de comparar as senhas diretamente.

Configuração local
1. Registrar a connection string nos User Secrets

Na pasta do projeto do site (onde está o Program.cs):

bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Receita" "Server=localhost,1433;Database=Receita;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;"

Para conferir:

bash
dotnet user-secrets list

O valor fica em:

Linux/macOS: ~/.microsoft/usersecrets/<UserSecretsId>/secrets.json
Windows: %APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json

O <UserSecretsId> adicionado ao .csproj pode ser commitado; ele não contém a senha.

Os User Secrets só são carregados automaticamente quando ASPNETCORE_ENVIRONMENT=Development, que é o padrão do dotnet run com o launchSettings.json do template.

2. Ajustar a coluna de senha

O hash tem cerca de 84 caracteres:

sql
ALTER TABLE [USER] ALTER COLUMN password NVARCHAR(256) NOT NULL;

Usuários cadastrados antes dessa mudança (com senha em texto puro) não passam na verificação e precisam ser recadastrados.

3. Rodar
bash
dotnet run
GitHub Actions

Crie o secret DB_CONNECTION_STRING no repositório (Settings → Secrets and variables → Actions) com a connection string completa e exponha-o no workflow:

yaml
env:
  ConnectionStrings__Receita: ${{ secrets.DB_CONNECTION_STRING }}

O __ é convertido em : pelo .NET, então o mesmo GetConnectionString("Receita") funciona localmente e no CI.

Segurança
A senha antiga do sa ficou no histórico do Git e deve ser trocada.
Recomenda-se criar um usuário SQL próprio da aplicação, com permissão apenas no banco Receita, em vez de usar o sa.
Nunca retorne o campo Password em listagens ou telas.