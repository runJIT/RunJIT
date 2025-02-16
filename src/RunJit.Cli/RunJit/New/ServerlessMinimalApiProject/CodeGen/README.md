# Siemens.Data.Cloud.Core

### How to setup?
1. Download Docker
2. Install [amazon/dynamo-local](https://hub.docker.com/r/amazon/dynamodb-local)
3. Run installed dynamo with this command 
```docker run -d -p 8001:8000 --name dynamodb-local amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb```
    1. Please note that you can also run the dynamo without sharedDb Option, but running with gives you the possibility to check everything also in console or DataGrip to see the actual items.
4. If you change the ports (feel free to do so), you can adapt the environment variables of your local startup.

[LICENSE:](./siemens-data-cloud-core/LICENSE.md)