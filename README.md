Backend (.Net 9) 

Prerequisites
.NET 9 installed

SQL Server running locally (or a connection string to a remote instance)

dotnet tool install --global dotnet-ef

Configuration
Connection String In appsettings.json, set your database connection string:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

Add Initial Migration

bash
dotnet ef migrations add InitialCreate
This generates migration files under Migrations/.

Features

JWT authentication with role/claim support

Registration and login endpoints

Task CRUD endpoints with server‑side pagination

Error handling with proper JSON responses

Run Backend

cd TaskManager.Api
dotnet restore
dotnet run


API runs at https://localhost:7169.

# TaskManager
