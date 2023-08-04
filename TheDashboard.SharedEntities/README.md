# Shared Entities

In a modern microservice architecture we try to decouple everything. To achieve this, an Event Sourcing pattern is used. 
This means that every microservice has its own database and its own data model. This is great, but it also means that we have to duplicate data. 
For example, if we have a `User` entity, we have to duplicate it in every microservice that needs it. 
If we have to change something in the `User` entity, we have to change it in every microservice. 
This is not only a lot of work, but it also means that we have to deploy every microservice. 
This is not a problem if we only have a few microservices, but if we have a lot of them, it can become a problem. 

The task is to address this issue without loosing the decoupling. The solution created here uses OpenAPI definition files to generate the code for the entities.
These generated entities are used for both, sending commands through the event pipeline and to query data from the services.

> OpenAPI is used out of the box, but it is also possible to use other definition files like gGRPC or GraphQL. The used generator is NSwag.

