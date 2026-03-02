# comandas.api
Api de gerenciamento de comandas de restaurante

## Resumo
`comandas.api` é uma API RESTful para gerenciar comandas em estabelecimentos de alimentação. A aplicação permite cadastrar produtos, abrir e listar comandas, e gerenciar pedidos associados a cada comanda — tudo pensado para suportar o fluxo de atendimento em bares e restaurantes.

## Funcionalidades principais
- Cadastro e atualização de produtos (nome, descrição, preço, categoria).
- Abertura, listagem e fechamento de comandas.
- Criação, listagem e atualização de pedidos vinculados a uma comanda (quantidade, observações, status).
- Cálculo automático do total da comanda com base nos pedidos.
- Endpoints RESTful simples para integração com front-end (web, tablet, PDV).

## Modelos principais (resumo)
- `Produto`: id, nome, descrição, preço, categoria.
- `Comanda`: id, número/mesa, status (aberta/fechada), total, data de abertura/fechamento.
- `Pedido`: id, comandaId, produtoId, quantidade, observações, status (pendente/entregue).

## Endpoints sugeridos
- `POST /api/produtos` — criar produto
- `GET /api/produtos` — listar produtos
- `POST /api/comandas` — abrir nova comanda
- `GET /api/comandas` — listar comandas
- `GET /api/comandas/{id}/pedidos` — listar pedidos de uma comanda
- `POST /api/comandas/{id}/pedidos` — adicionar pedido à comanda

## Fluxo básico de uso
1. Cadastrar os produtos disponíveis.
2. Abrir uma comanda ao iniciar o atendimento.
3. Adicionar pedidos à comanda conforme clientes fazem os pedidos.
4. Consultar/editar pedidos e, quando necessário, fechar a comanda gerando o total para cobrança.

## Tecnologias (exemplo)
- .NET (API Web)
- Entity Framework Core para persistência
- SQL Server / outro banco relacional
- JSON sobre HTTP para comunicação

## Como rodar localmente (rápido)
1. Ajuste a `ConnectionString` no arquivo de configuração.
2. Execute `dotnet build` e `dotnet run` a partir do diretório do projeto.
3. Use ferramentas como Postman ou um front-end para testar os endpoints.

## Contribuição
Contribuições são bem-vindas — abra uma issue ou envie um pull request com melhorias.

---

