# The Dashboard

The Dashboard is a boilerplate project for modern service architecture. It is designed to build a Dashboard App, a web application that allows users to view and manage their data. The project is inspired by the work of Amitpnk and jasontaylordev.

## Setup

To set up the project, run the docker compose file. Make sure to check the SQL server volume path to assure persistence.

## Features

The Dashboard App offers the following features:

 - User can sign up
 - User can sign in
 - User can sign out
 - User can view their data
 - User can manage their data
 - User can view their data in a chart
 - User can view their data in a table
 - User can view their data in a map


## Architecture

In general, the architecture follows the eventourcing pattern. The project is based on the CQRS pattern, specifically.

The app is split in multiple micro services based on that CQRS pattern, it has an event store, receiving all the commands, and this store distributes the events to all the services. 
Each service can (if it makes sense) store it's own "projected data" in database for efficient querying. 
The services will have only GET methods to retrieve data, but all insert/update/delete operations will come through the event messaging as a command. 

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
* AuthService: Manage user authentication and authorization. This is using Azure AD B2C.

The project also creates and needs several databases.

### Frontend

The frontend is a Blazor app that has graphical support for managing dashboard and for viewing / exporting dashboards.

### Languages

The project is primarily written in C# (87.6%), with HTML (5.8%), TSQL (2.7%), CSS (2.4%), Dockerfile (1.4%), and JavaScript (0.1%) also used.

### References and Packages

The project makes use of these specific packages or libraries:

* MassTransit (to handle RabbitMQ)
* Automapper (to map between DTOs and Entities)
* Swashbuckle (to generate Swagger documentation)
* NSwag (to generate C# client code from Swagger documentation)
 
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
