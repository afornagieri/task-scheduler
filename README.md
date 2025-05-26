💪 Task Scheduler - Publicador de Tarefas

Este projeto implementa um serviço de publicação de tarefas assíncronas em C# utilizando RabbitMQ. Ele simula o início de uma arquitetura de processamento assíncrono, focando na estrutura e na organização da solução conforme proposto no desafio técnico.

🔧 Tecnologias Utilizadas

.NET 6

RabbitMQ

Docker

MongoDB (previsto, mas não utilizado neste escopo)

Arquitetura limpa e injeção de dependência

Comunicação via fila de mensagens (mensageria assíncrona)

📦 Como Rodar o Projeto

Pré-requisitos

Docker e Docker Compose instalados

Passos:

git clone https://github.com/seu-usuario/task-scheduler.git
cd task-scheduler
docker-compose up --build

A aplicação ficará disponível em:

API: http://localhost:5000

RabbitMQ UI: http://localhost:15672 (login: guest, senha: guest)

📬 Como Usar a API

Criar uma nova tarefa

Endpoint:

POST /api/tasks

Exemplo de Payload:
```
{
  "type": "SendEmail",
  "payload": {
    "email": "usuario@exemplo.com",
    "mensagem": "Olá, bem-vindo!"
  }
}
```

Exemplo de resposta
```
{
  "id": "d88cd2ed-5649-4db4-bc37-e845eef81f38"
}
```

GET /api/tasks{id}

Exemplo de Payload:
```
/api/tasks/d88cd2ed-5649-4db4-bc37-e845eef81f38
```

Exemplo de resposta
```
{
  "id": "d88cd2ed-5649-4db4-bc37-e845eef81f38",
  "type": "SendEmail",
  "status": "Awaiting",
  "created_at": "2025-05-26T15:44:21.783Z",
  "payload": {
    "email": "usuario@exemplo.com",
    "mensagem": "Olá, bem-vindo!"
  }
}
```

🧠 Sobre a Arquitetura

Embora o processamento não tenha sido implementado, a estrutura já está preparada para:

Escalabilidade: utilizando RabbitMQ, é possível adicionar workers posteriormente de forma transparente

Concorrência segura: o design suporta múltiplos consumidores simultâneos

Sistema de re-tentativa: pode ser implementado posteriormente pelo consumer

Status de tarefas: pode ser persistido no MongoDB ou outro banco NoSQL

🚚 Deployment

A aplicação está totalmente containerizada via Dockerfile e docker-compose.yml, permitindo execução em qualquer ambiente com Docker.

✏️ Considerações Finais

Este projeto foca na estrutura inicial e publicação assíncrona, simulando o fluxo real de sistemas de mensageria distribuída.
