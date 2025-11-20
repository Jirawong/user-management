# User Management System

This project is a partially completed user management system built with:

- **Frontend:** Angular  
- **Backend:** .NET 9 Web API  
- **Database:** SQL Server / SQL Express  

Some UI/UX elements and error handling are still being improved.



## How to Run the Project

### 1. Database Setup (SQL Server / SQL Express)

All SQL scripts are located in the `/database` directory.

Run them in this order:

1. `create_tables.sql`  
2. `seed-samples-role-and-permission.sql`

**Database Name:** `user_management`


### 2. Backend Setup


`cd backend`

`dotnet restore`

Update appsettings.json to match your local SQL Express instance:

```bash
"ConnectionStrings": { "Default": "Server=localhost\\SQLEXPRESS;Database=user_management;Trusted_Connection=True;TrustServerCertificate=True;" }
```

`dotnet run`

### 3. Frontend Setup

`cd frontend` 

`npm install `

`ng serve`

### API Endpoints 

`GET /api/users`

Pagination + search 

`GET /api/users/{id} `

Get one user 

`POST /api/users `

Create user 

`PUT /api/users/{id} `

Update user 

`DELETE /api/users/{id} `

Delete user 

`GET /api/roles `

List roles 

`GET /api/permissions`

List permissions 

### Thank You 
If you have trouble running the project, feel free to contact me
