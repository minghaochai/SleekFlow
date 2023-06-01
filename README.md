# SleekFlow Backend

#### Required

- Visual Studio 2022
- A SQL server database

#### How to start local development
- Clone the git repository
- Navigate to the 'backend/src' folder and open the 'SleekFlow' solution in Visual Studio
- Change the 'Cors:Origins' string property in the 'appsettings.json' and 'appsettings.Development.json' to your client url
- Change the 'SleekFlowDbConnection' connection string property in the 'appsettings.json' and 'appsettings.Development.json' to point to your SQL server database
- Open the Package manager console and set 'SleekFlow.Infrastructure' as the default project within the console
- Run the command 'update-database' which tells EntityFramework Core to apply the migrations (The migrations include setting up the database and seeding the tables with data).
- Ensure 'SleekFlow.Api' project is set as the start up project
- Run the solution

#### How to run the test cases
- Navigate to the 'backend/src' folder and open the 'SleekFlow' solution in Visual Studio
- Quickway to setup test database. Within the 'SleekFlow.Api' project, change the 'SleekFlowDbConnection' connection string property in the 'appsettings.json' and 'appsettings.Development.json' to point to your SQL server test database. Run the command 'update-database' which tells EntityFramework Core to apply the migrations (rememeber to change the connection string property back to the developement SQL server database after)
- Within the 'SleekFlow.IntegrationTest' project open the 'CustomWebApplicationFactory.cs' file. At line 15 replace the dictionary string value to your test database connection string. Reason is to workaround problem stated in this thread https://github.com/dotnet/aspnetcore/issues/37680
- Right click on the 'SleekFlow.IntegrationTest' project and select 'Run Tests' to run the integration tests
- Right click on the 'SleekFlow.UnitTest' project and select 'Run Tests' to run the unit tests

# SleekFlow Frontend

#### Required

- Visual Studio Code

#### How to start local development
- Open the 'frontend' folder with visual studio
- Run "npm i" to install the dependencies
- In the .env file, change the 'REACT_APP_API_BASE_URL' property to point to the backend api url
- Run "npm start" to start the client
