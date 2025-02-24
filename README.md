# Challenge N5 Backend

Este proyecto permite levantar el entorno necesario utilizando Docker.

## Instrucciones

Para levantar el proyecto, simplemente ejecuta el siguiente comando:

```
docker-compose up -d
```
# Creacion del topico en Kafka            
### Crear topico de kafka para enviar mensajes
```
kafka-topics --create --topic permission-topic --bootstrap-server kafka:9092
```
