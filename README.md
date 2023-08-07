# The Dashboard

The Dashboard is a boilerplate project for modern service architecture. It is designed to build a Dashboard App, a web application that allows users to view and manage their data. The project is inspired by the work of Amitpnk and jasontaylordev.

## Setup

To set up the project, run the docker compose file. Make sure to check the SQL server volume path to assure persistence.

In the 'docker-compose.yml' file identify the section that defines the volume:

~~~
    volumes:
     - type: bind
       source: "/D/Volumes/sqldata"
       target: /var/opt/mssql/data
~~~

Change the source path to a folder on your local machine. This folder will be used to store the SQL server data. Make sure the folder exists.

The same procedure is required for the event store database:

~~~
    volumes:
      - type: bind
        source: "/D/Volumes/eventstore/data"
        target: /var/lib/eventstore
      - type: bind
        source: "/D/Volumes/eventstore/logs"
        target: /var/log/eventstore
~~~

Change both paths according to your system. Make sure the folders exist.

> Keep an eye on the slashes. Even on a Windows system it's strongly recommended to use forward slashes only.

### Prerequisites

You need to have docker desktop including docker compose installed. Then just run the docker compose file. The compose file will create all the necessary containers and databases. It will also seed the databases with some data.

All batches use "https://localhost:7500" as preferred URL. If you want to use a different URL, you need to change the batches and the port bindings in 'docker-compose.yml' accordingly.
Identify the ports in the service named 'frontend' and see the port bindings:

~~~
    ports:
      - "5500:80"
      - "7500:443"
~~~

The easist way to run outside of Visual Studio is just using the start batches:

### Authentication

Currently the app authenticates users and app services against Azure AD B2C. The setup includes:

* A free Azure subscription
* A tenant with one Azure AD B2C instance
* A user flow for sign up and sign in
* A client application registration (for frontend)

Not all these data and update the appropriate section in 'appsettings.json' of the project named 'TheDashboard.Frontend':

~~~
  "AzureAdB2C": {
    "Instance": "https://<your-tenant-name>.b2clogin.com",
    "Domain": "<your-tenant-name>.onmicrosoft.com",
    "TenantId": "<your-tenant-id>",
    "ClientId": "<your-client-id>",
    "Domain": "<your-tenant-name>.onmicrosoft.com",
    "SignUpSignInPolicyId": "<your-sign-up-sign-in-policy-id>",
    "ResetPasswordPolicyId": "<your-reset-password-policy-id>",
    "EditProfilePolicyId": "<your-edit-profile-policy-id>"
  }
~~~

### Start the App Locally

The app is supported on all OS.

#### Linux and macOS

~~~
start.sh
~~~

> This also supports WSL (Windows Subsystem for Linux) on Windows box.

#### Windows

~~~
start.cmd
~~~

#### Other and Universal (PowerShell)

~~~
start.ps1
~~~

### Visual Studio 2022

Just hit F5. The docker compose setting is predefined. The project will start in debug mode.

### Visual Studio Code

There is currently no debugging setup included. You can run the project in VS Code by running the following command in the root folder:

~~~
docker compose --profile all up --build
~~~

## Features (MVP - minimum viable product)

The Dashboard App offers the following features:

* Supported through Azure AD B2C:
  * User can sign up
  * User can sign in
  * User can sign out
  * User can view their profile data
* Distinct features
  * Add dashboards
  * Add tiles
  * Add data sources and schedules
  * Assign tiles to dashboards
  * Publish dashboard for public viewing

### Planned Development Path

Assign dashboards to a closed user audience.
Publish device dependent versions (such as mobile/PWA/...).
Edit dashboard configurations as JSON.
Add more tile types.
Add more data sources.
Add more authentication providers (such as Google, Facebook, ...).

## Architecture

In general, the architecture follows the eventourcing pattern. The project is based on the CQRS pattern, specifically.

The app is split in multiple micro services based on that CQRS pattern, it has an event store, receiving all the commands, and this store distributes the events to all the services. 
Each service can (if it makes sense) store it's own "projected data" in database for efficient querying. 
The services will have only GET methods to retrieve data, but all insert/update/delete operations will come through the event messaging as a command. 

### Overview

#### Querying Data

Frontend (DMZ) <-- Proxy (YARP) <-- Services (WebAPi, inner zone) <-- Database

#### Invoke Commands

Frontend (DMZ) --> Proxy (YARP) --> Command Store (EventStore, inner zone) --> Queue (RabbitMQ, inner zone) --> Services (WebAPi, inner zone) --> Database

Because of the complete asynchronisity of the system, the frontend will receive updates as SignalR messages through a message hub. The messages are just simple "new data available" messages,
the frontend will then query the data from the services.

### Basics

In an event sourcing architecture note that all changes to the application state are stored as a sequence of events in the event store. The event store is indeed the single source of truth 
regarding the history of these state changes.

However, reconstructing the current state of an entity from the event history every time can be expensive and slow, especially as the number of events grow over time. 
To alleviate this, a common pattern is to use a "Read Model" or a "Projection". We use MongoDb here to have a simple and easy to use store. In case the deployement shall make use of
an Azure based infrastructure, we can use CosmosDB with the MongoDB API. This is the same reason for using Masstransit as a message broker, as it can be easily make use of Azure Service Bus.

Read Models (also known as materialized views) are typically denormalized representations of the current state, optimized for specific queries. They are populated and kept updated by subscribing 
to the events in the event store. For example, you might have a read model for "current user status", which is updated every time a "user logged in" or "user logged out" event is fired. 
This read model can be stored in a database which is different from your event store, and this is the database you'd typically query for displaying data to users or for other "read" operations.

The process of updating the Read Model from the event store is often done by a separate service or component in your system, sometimes called a Projection. The Projection subscribes to the 
events from the event store (usually via a message bus like RabbitMQ or Kafka), and updates the Read Model accordingly. The choice of database for the Read Model depends on the specific queries 
it needs to support - it can be a SQL database, a NoSQL database like MongoDB, a search index like Elasticsearch, or even a cache like Redis.

So in summary, while the event store is the single source of truth in terms of the history of state changes, you'd typically also have a separate database (or multiple databases) acting 
as the Read Model to store the current state for efficient querying. And the process of updating this Read Model from the events in the event store is typically done by a Projection.

This pattern of having a separate Read Model that's updated by a Projection is also a key part of the CQRS (Command Query Responsibility Segregation) architectural pattern, which is often used 
in conjunction with Event Sourcing. The "Command" part corresponds to writing events to the event store, and the "Query" part corresponds to reading from the Read Model.

### Project Structure

The project of the project is divided into frontend and backend. The frontend is a Blazor Server application. The backend consists of several APIs:

* DashboardService: Create a dashboard, manage Dashboards, assign Tiles to dashboard.
* TileService: Manage Tiles independently from Dashboards. Define data sources and views.
* DataConsumerService: Pull data from datasource and establish Websocket connection to tiles.
* UiInfoService: Manage UI information, such as available Tiles, available views, etc.

Authentication is handled by Azure AD B2C with default policies. The frontend is protected by a proxy (YARP) that handles authentication and forwards requests to the backend. The backend is protected by Azure AD B2C as well, but the proxy forwards the authentication token to the backend. The backend then validates the token and extracts the user information from it. The backend is not accessible from the outside, only the proxy can access it.

The project also creates and needs several databases.

### Shared Libraries

In a modern microservice architecture we try to decouple everything. To achieve this, an Event Sourcing pattern is used. 
This means that every microservice has its own database and its own data model. This is great, but it also means that we have to duplicate data. 
For example, if we have a `User` entity, we have to duplicate it in every microservice that needs it. 
If we have to change something in the `User` entity, we have to change it in every microservice. 
This is not only a lot of work, but it also means that we have to deploy every microservice. 
This is not a problem if we only have a few microservices, but if we have a lot of them, it can become a problem. 

The task is to address this issue without loosing the decoupling. The solution created here uses OpenAPI definition files to generate the code for the entities.
These generated entities are used for both, sending commands through the event pipeline and to query data from the services.

> OpenAPI is used out of the box, but it is also possible to use other definition files like gGRPC or GraphQL. The used generator is NSwag.

To address changes or additions, just edit the xxx_openapi.json files with the appropriate OpenAPI definitions, DTOs, and security settings. Then the code can be generated with the NSwag tool. The generated code is then referenced to the appropriate projects.
In the service projects and implementation must exist, that handles the actual requests. The generated code is only used to send the requests and to receive the responses.

### Frontend

The frontend is a Blazor app that has graphical support for managing dashboard and for viewing / exporting dashboards.

### Languages

The project is primarily written in C# (87.6%), with HTML (5.8%), TSQL (2.7%), CSS (2.4%), Dockerfile (1.4%), and JavaScript (0.1%) also used.

### References and Packages

The project makes use of these specific packages or libraries:

* MassTransit (to handle RabbitMQ)
* Automapper (to map between DTOs and Entities)
* NSwag (to generate C# client code from Swagger documentation)
* Quartz (to handle scheduled jobs) in the data source service

> Swashbuckle (to generate Swagger documentation) is not being used, as the OpenAPI documentation is created and maintained manually. It's now a first class citizen - contract first principle.
 
#### Frontend Only 

* LigerShark (to optimize Web pages)
* Bytex (Cookie banner generator)
* Blazorise (Blazor component library)
* Blazorise.Bootstrap (Blazor component library with adoption of Bootstrap CSS)

However, it uses standard libraries and packages for .NET, Blazor, and Azure AD B2C.

### Project Structure

The project follows a standard structure for a .NET solution with multiple projects. 
The solution file (TheDashboard.sln) is in the root directory, along with Docker-related files and a README.md. 
The individual services (DashboardService, TileService, etc.) are organized into their own projects within the solution.

For more detailed information, please visit the project page.

# Further Reading

The purpose of the event store in this architecture is multifold:

* Single Source of Truth: All changes to the application state are stored as a series of events. This makes it the authoritative source of what has happened in your system. Each event is a fact that has happened at a particular time and cannot be changed or deleted. This gives you a strong audit log out-of-the-box.
* Event Sourcing: Your application state at any point in time can be derived by replaying the events from the event store. This is in contrast to traditional CRUD-based systems, where you overwrite the state and don't keep the history of changes.
* Temporal Queries: Because each event has a timestamp, you can query how the system looked at any point in time. This can be useful for debugging or understanding user behavior.

The data flow in such a system would look something like this:

1. A user interacts with your Blazor server app, causing it to dispatch a command (for example, "AddItem").
2. The command is handled by the command handler, which generates an event (for example, "ItemAdded") and stores it in the event store. The same event is then published to the message bus.
3. Microservices subscribed to the "ItemAdded" event receive it from the message bus. Each service may update its own read model (stored in its own database) based on the event. For example, an inventory service might decrement the quantity of the added item, while an analytics service might increment a counter of total items added.
4. When a user queries data (for example, to view a list of items), the query is handled by the appropriate microservice, which serves the data from its read model.

As you've probably noted, the same event might be stored in multiple places (once in the event store, and again in each service's read model), which might seem redundant. 
However, this is a trade-off that's made in order to decouple the services from each other and allow each to optimize its own data storage for the queries it needs to serve.

It's also worth noting that in this architecture, the event store and the read models serve different purposes: the event store is the source of truth for what has happened, 
while the read models are optimized for querying the current state of the system.
