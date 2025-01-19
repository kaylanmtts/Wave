# API REST para Gerenciamento de Músicas, Álbuns e Artistas 🎶🎤

Esta API REST foi projetada para gerenciar informações sobre **músicas**, **álbuns** e **artistas**. Ela permite realizar operações CRUD (Create, Read, Update, Delete) para cada um desses recursos. 

## Funcionalidades 🛠️

### 1. **Gerenciamento de Artistas** 🎸
- **Cadastro** de novos artistas.
- **Visualização** de informações sobre os artistas.
- **Atualização** de dados de artistas.
- **Exclusão** de artistas do sistema.

### 2. **Gerenciamento de Álbuns** 💿
- **Cadastro** de novos álbuns.
- **Visualização** de álbuns existentes.
- **Atualização** de informações de álbuns.
- **Exclusão** de álbuns.

### 3. **Gerenciamento de Músicas** 🎵
- **Cadastro** de novas músicas.
- **Visualização** de músicas no sistema.
- **Atualização** de dados das músicas.
- **Exclusão** de músicas.

### 4. **Relacionamento entre Músicas, Álbuns e Artistas** 🔗
- Músicas podem ser **associadas** a álbuns.
- Álbuns podem ser **associados** a artistas.
- Isso garante que cada música tenha um **contexto completo**, com sua relação com álbuns e artistas.

## Tecnologias Usadas ⚙️
- **C#** com **ASP.NET** para a construção da API.
- **Entity Framework** para interação com o banco de dados.
- **SQL Server** como banco de dados para armazenamento dos dados.
- **Docker** para rodar o SQL Server.
- **JWT (JSON Web Token)** para autenticação de usuários.
