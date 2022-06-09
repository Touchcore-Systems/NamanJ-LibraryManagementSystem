# NamanJ-LibraryManagementSystem
 > Library Management System - Assessment project made in .NET core and Angular
 
## Technologies implemented
 - ASP.NET 5.0 (with .NET Core 5.0)
 - ASP.NET WebApi Core with JWT Bearer Authentication
 - ASP.NET Identity Core
 - Entity Framework Core
 - Angular
 - Azure Functions

## Installation

Clone the repo

```
git clone --depth https://github.com/Touchcore-Systems/NamanJ-LibraryManagementSystem.git
```

### Server Setup

Change the value of database connection string present in ```LmsApi/appsettings.json```. Install dependencies for server, run the command
```
cd LmsApi
dotnet restore
dotnet database update
```
and run server locally by ``` dotnet run ``` command


### Client Setup

Install dependencies of client by running command,
```
cd app-ui
npm install
```
Run ```ng serve``` for a dev server. Navigate to http://localhost:4200/. The app will automatically reload if you change any of the source files.

