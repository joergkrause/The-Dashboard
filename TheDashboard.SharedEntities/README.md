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

## Setup a New Service

To create additional functionality, you ususally need a new service that handles queries and commands. You also may need UI in the frontend.

### Create a New Service

First, you create a new openapi.json that contains the definition of the entities that you want to share. Also it defined endpoints for querying. This happens in the "TheDashboard.SharedEntities" project.

Then, add the Nswag calls to the build process. This happens in the "TheDashboard.SharedEntities" project file.

```xml
<Exec Command="$(NSwagExe_Net60) run nswag.json /runTime:net60 /variables:Client=<MyNew>Client,Namespace=TheDashboard.SharedEntities,InputSwagger=openapi/<mynew>_openapi.json,ConfigHelper=IConfiguration" />
<Exec Command="$(NSwagExe_Net60) openapi2cscontroller /input:openapi/<mynew>_openapi.json /classname:<MyNew>Base /namespace:TheDashboard.SharedEntities /output:Controllers/<MyNew>Base.cs /dateTimeType:System.DateTime /jsonLibrary:SystemTextJson " />
```

Replace all occurences of '<MyNew>' and add the parts to the appropriate section in *.csproj* file.

Compile the project and check if the generated files are correct. You now have:

* A controller base class to handle REST queries
* A proxy (client) to call these controllers from UI
* A set of shared entities (DTOs) used by all parts of the project

As the architecture is event based, you also need to create a new event handler. This happens in the service's project. The commands send by the UI are handled here. The definition shall be in the "TheDashboard.SharedEntities" project.

A command looks like this:

~~~csharp
public record TileUpdated(int TileId, TileDto Item) : Command;
~~~

Follow the notion, <Entity><Action> and use the DTOs from the shared entities project. Use past tense for the verb. Use the DTOs that you have created in the previous step.

The event handler looks like this:

~~~csharp
public class TileUpdatedHandler : ICommandHandler<TileUpdated>
{
    private readonly ITileStore _tileStore;

    public TileUpdatedHandler(ITileStore tileStore)
    {
        _tileStore = tileStore;
    }

    public async Task Handle(TileUpdated command)
    {
        var tile = await _tileStore.Get<Tile>(command.TileId);
        tile.Update(command.Item);
        await _tileStore.Save(tile);
    }
}
~~~

> Not that this code is just an example. Your service shall provide some abstraction for the required actions, such as a repository for database access. In the example this is the *ITileStore* interface.

This event handler is registered in the service's startup class. This is done in the respective service's project. It goes usually in a folder named 'Infrastructure/Integration'.

### Use the New Service

After this and the basic setup of Masstransit, the commands are send from Frontend to Proxy, forwarded to Eventbus and handled in the services. Queries are send from Frontend to Proxy and forwarded to the REST controllers.

The given archticture assures that you have pure business logic and local storage activities in your service. Everything around shall be handled by the provided infrastructure made available by various containers.
