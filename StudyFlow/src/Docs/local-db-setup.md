The API reads the connection string from configuration using the standard `.NET` `ConnectionStrings` section.
Here is an example of how to set up the connection string in your `appsettings.json` file:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=localhost;Database=StudyFlowDB;User Id=your_username;Password=your_password;"
  }
}
```
Make sure to replace `your_username` and `your_password` with your actual PostgreSQL credentials.
You can then access this connection string in your application code as follows:
```csharp
var connectionString = Configuration.GetConnectionString("DefaultConnection");
```
This setup allows your application to connect to the local PostgreSQL database named `StudyFlowDB` using the specified credentials.
Ensure that your SQL server is running locally and that the database `StudyFlowDB` has been created before attempting to connect.

Program.cs (usage)

builder.Services.AddDbContext<StudyFlowDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

dotnet ef migrations add <MigrationName>
dotnet ef database update