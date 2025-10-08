# 🧾 Colheita Solidária – Backend (API)

A **API Colheita Solidária** é o núcleo do sistema, responsável por gerenciar usuários, doações, recebedores e solicitações, integrando o frontend React com o banco de dados.  

Segue o padrão de **Clean Architecture**, garantindo organização, escalabilidade e facilidade de manutenção.

---

## 📚 Sobre o Projeto

Desenvolvido como parte do projeto Colheita Solidária, idealizado no 4º semestre do curso de **Desenvolvimento de Software Multiplataforma – Fatec Matão**.  

Objetivo: conectar agricultores com excedente de alimentos a pessoas em situação de vulnerabilidade, promovendo solidariedade e combate ao desperdício por meio da tecnologia.

---

## ⚙️ Funcionalidades Principais

- ✅ Autenticação e autorização com JWT  
- ✅ Cadastro e login de usuários (Admin, Agricultor, Recebedor)  
- ✅ CRUD completo de doações, recebedores e solicitações  
- ✅ Upload de imagens das doações (integração com Supabase Storage)  
- ✅ Documentação da API com Swagger  
- ✅ Testes de integração e unidade com xUnit  
- ✅ Integração com Firebase Cloud Functions para envio de e-mails automáticos

---

## 🧩 Arquitetura do Projeto

Camadas da Clean Architecture:

- **Domain** – Entidades e regras de negócio  
- **Application** – DTOs, interfaces e serviços  
- **Infrastructure** – Acesso ao banco de dados, contexto (EF Core)  
- **API** – Controllers e endpoints REST

---

## 🛠 Tecnologias Utilizadas

- **Linguagem:** C#  
- **Framework:** .NET 8 / ASP.NET Core  
- **Banco de Dados:** SQL Server (Azure)  
- **ORM:** Entity Framework Core  
- **Autenticação:** JWT  
- **Documentação:** Swagger  
- **Testes:** xUnit  
- **Integrações:** Supabase e Firebase Cloud Functions  
- **Hospedagem:** SQl

---

## 🧱 Principais Entregas (GitHub Projects)

- Configuração inicial da API (estrutura de pastas e pacotes NuGet)  
- AppDbContext e configuração do SQL Server  
- Implementação do Swagger, CORS e autenticação JWT  
- Entidades, DTOs e mapeamento com AutoMapper  
- Controllers e serviços: User, Doacao, Recebedor e Solicitacao  
- Testes de integração (UsersController e TokenController)  
- Deploy no Azure

---

## 🧪 Testes

- Testes unitários dos serviços (`UserService`, `DoacaoService`, etc.)  
- Testes de integração dos controladores (`UsersController`, `TokenController`)  
- Ambiente simulado com `CustomWebApplicationFactory`

---

## 👥 Equipe

- Miriam Silva Corrêa – Testes, integração da API e autenticação JWT  
- Isadora Georgete – Estrutura e documentação do backend  
- Caroline Oliveira Silva – Configuração e integração com frontend

---

## 📈 Critérios de Sucesso

- API funcional e estável, com 100% das rotas principais testadas  
- Integração completa com o frontend React  
- Deploy realizado com sucesso no Azure

---

## 🌐 Links

- 🔗 **Frontend do Projeto:** [Colheita Solidária (React)](https://github.com/miriam-silva/ColheitaSolidaria)  
- 💻 **Repositório Backend (API):** [GitHub](#)
