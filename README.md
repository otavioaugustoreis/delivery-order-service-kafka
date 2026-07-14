# Sistema de Inserção de Pedidos

## Descrição curta
Sistema de inserção de pedidos para um aplicativo de varejo com foco em mensageria via Kafka.  
Fluxo principal: cliente cria pedido no app → pedido é persistido no MongoDB → evento é publicado no Kafka → serviço de notificação consome a mensagem e envia notificações.

## Arquitetura do sistema

| Componente | Tecnologia | Responsabilidade |
|---|---|---|
| API de Pedidos | .NET | Recebe requisições, valida e grava pedido no MongoDB |
| Broker de Mensagens | Kafka | Transporte confiável de eventos de pedido |
| Serviço de Notificação | .NET | Consome tópico Kafka e envia notificações |
| Banco de Dados | MongoDB | Persistência de pedidos e metadados |

## Principais conceitos e garantias

### Atomicidade
- Recomenda-se o padrão **Outbox** para garantir consistência entre persistência e publicação.
- Alternativas:
  - transações MongoDB quando aplicável;
  - gravação em coleção outbox seguida de worker que publica no Kafka e marca como enviado.

### Observabilidade
- Logs estruturados
- Métricas:
  -  publicação/consumo.
- Tracing distribuído para correlacionar:
  - requisição HTTP → gravação DB → publicação Kafka → consumo.
- Health checks para API, Kafka e MongoDB.
###e Resiliência
- Consumidor com deduplicação via chave única no MongoDB ou cache.
- Retry exponencial e DLQ (dead-letter topic) para falhas persistentes.

## Tecnologias usadas
- **Linguagem e runtime:** .NET (C#)
- **Mensageria:** Apache Kafka
- **Banco de dados:** MongoDB
- **Observabilidade:** OpenTelemetry, Prometheus, Grafana, ELK (opcional)
- **Infra local:** Docker e docker-compose

## Como rodar localmente

### Pré-requisitos
- Docker e Docker Compose
- .NET SDK compatível com o projeto

### Exemplo de `docker-compose` mínimo
