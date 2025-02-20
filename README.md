# Challenge N5 Backend

Este proyecto permite levantar el entorno necesario utilizando Docker.

## Instrucciones

Para levantar el proyecto, simplemente ejecuta el siguiente comando:

```
docker-compose up -d
```
# Creacion del topico en Kafka            
### La unica tarea que no se pudo automatizar es la creacion del topico en Kafka. Para hacerlo, utiliza el siguiente comando:
```
kafka-topics --create --topic nombre-del-topico --bootstrap-server kafka:9092
```
