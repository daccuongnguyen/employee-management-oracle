Employee Management (C# ASP.NET Core) - Oracle 11g sample
--------------------------------------------------------

This is a minimal ASP.NET Core Web API sample for managing Departments and Employees (CRUD) using Oracle 11g via Oracle.ManagedDataAccess.Core ADO.NET.

Structure:
- Program.cs
- appsettings.json (edit connection string)
- Controllers/
- Models/
- Data/OracleDbService.cs (simple helper)
- Services/DepartmentService.cs, EmployeeService.cs

How to use:
1. Edit appsettings.json: set Oracle connection string (user/schema must exist).
2. Create required tables in Oracle (SQL scripts provided in /sql).
3. `dotnet restore`
4. `dotnet run`
5. Swagger UI: http://localhost:5000/swagger (or printed URL)

Notes:
- This example uses plain ADO.NET (Oracle.ManagedDataAccess.Core). It is intentionally simple and synchronous for clarity.
- For production, add validation, error handling, authentication, and use migrations/EF Core if desired.
