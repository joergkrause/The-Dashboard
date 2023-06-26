# Dashboard Service

Boilerplate project for modern service architecture.

## Purpose

Build a Dashboard App.

Dashboard App is a web application that allows users to view and manage their data.

## Setup

Run docker compose file. Check SQL server volume path to assure persistence.

## Features

- [x] User can sign up
- [ ] User can sign in
- [ ] User can sign out
- [ ] User can view their data
- [ ] User can manage their data
- [ ] User can view their data in a chart
- [ ] User can view their data in a table
- [ ] User can view their data in a map

## Architecture

- [ ] Frontend (Blazor Server)
- [ ] Backend
  - [x] API: DashboardService
  - [x] API: TileService
  - [ ] API: DataConsumerService
  - [ ] API: UiInfoService
  - [ ] API: AuthService
- [ ] Database

### Details

#### DashboardService

Create a dashboard, manage Dashboards, assign Tiles to dashboard.

#### TileService

Manage Tiles independently from Dashboards. Define data sources and views.

#### DataConsumerService

Pull data from datasource and establish Websocket connection to tiles.

#### UiInfoService

Manage UI information, such as available Tiles, available views, etc. 

#### AuthService

Manage user authentication and authorization. This is using Azure AD B2C.

# Frontend

This is a blazor app that has graphical support for managing dashboard and for viewing / exporting dashboards.
