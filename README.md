# SleekFlow

#### Required

- Visual Studio 2022
- A SQL server database

#### How to start local development
- Clone the git repository
- Open the 'SleekFlow' solution in Visual Studio
- Change the 'SleekFlowDbConnection' connection string property in the 'appsettings.json' and 'appsettings.Development.json' to point to your SQL server database
- Open the Package manager console and set 'SleekFlow.Infrastructure' as the default project within the console
- Run the command 'update-database' which tells EntityFramework Core to apply the migrations (The migrations include setting up the database and seeding the tables with data).
- Ensure 'SleekFlow.Api' project is set as the start up project
- Run the solution

#### How to run the test cases
- Open the 'SleekFlow' solution in Visual Studio
- Right click on the 'SleekFlow.Test' project and select 'Run Tests'
