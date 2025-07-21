# RecipeBook - WIP

Este é um projeto que desenvolvi para colocar em prática os conteúdos aprendidos durante meus estudos em .NET. Trata-se de um livro de receitas no qual é possível criar e visualizar receitas, além de compartilhá-las com outras pessoas. O projeto foi pensado para ter uma base sólida e facilmente escalável, seguindo os princípios do DDD (Domain-Driven Design). Ele contém testes para cada parte do código e suporte a vários idiomas.

## ✨ Recursos Principais

- Cadastro, edição e remoção de receitas
- Organização por categorias e ingredientes
- Busca eficiente por nome, ingrediente ou categoria
- Padrão de código limpo, modular e documentado

## 🚀 Tecnologias Utilizadas

- **Linguagem principal:** C#
- **Frameworks:** Aps.Net
- **Banco de dados:** MySql
- **Documentação:** Swagger

## 🏗️ Padrões e Boas Práticas

- Estrutura de pastas seguindo padrão DDD e separação de responsabilidades
- Uso de controle de versão via Git com convenções de branch e commits
- Testes automatizados e validação de regras de negócio

## 📦 Instalação

1. Clone o repositório:
   ```bash
   git clone https://github.com/cleytonbernardino/RecipeBook.git
   ```
2. Acesse a pasta do projeto:
   ```bash
   cd RecipeBook
   ```
3. Restaure as dependências
	```bash
	cd src/Backend/RecipeBook.API
	dotnet restore
	```
4. Configure o projeto
	- Navegue até appsetting.json (src -> backend -> RecipeBook.API)
	- Espando o menu ecolhido para revelar o appsetings.Devolopment.json e appsetings.Test.json
	- Preencha as informalções pedidas dentro de cada arquivo
	- **SigningKey** deve ser a mesma no devolopment e test, é recomedado que não use o valor padrão.

 5. **Opcional** -> Execute os testes antes de executar o projeto para conferir se tudo foi devidademente configurado.
	```bash
	dotnet test
	```

## ▶️ Executando o Projeto
```bash
dotnet build
dotnet run
```
Acesse via navegador em [https://localhost:7088](https://localhost:7088) (caso tenha modificado a porta modifique aqui também).

## 🧪 Executando Testes

```bash
dotnet test
```