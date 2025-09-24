# 🛒 Talabat Clone – E-Commerce Web API

## 📦 Description  
A full-stack e-commerce Web application inspired by Talabat's food delivery platform. It includes features such as order management, product catalog browsing, simulated payment workflows and an admin panel for the app's management. Built with **ASP.NET Core**, the project emphasizes clean architecture, modularity, and extensibility.

This solution consists of three main interconnected projects:

1. **.NET Core Web API**  
   Serves as the central backend interface for data access, business logic, and API endpoints. It exposes RESTful services consumed by both frontend applications.

2. **Angular Frontend (User Application)**  
   A client-facing web application built for interacting with the Web API to provide users with product browsing, account management, and other features. It serves as a consumer of the Web API and is used primarily for integration testing and endpoint validation. While it is **not authored by me**, it provides a valuable interface for verifying backend functionality.


3. **ASP.NET Core MVC Admin Panel**  
   A separate administrative interface designed for internal use. It provides granular control over the system, including:
   - Full CRUD operations for products, users, and roles
   - Role-based access control (RBAC) for managing user privileges
   - Secure authentication and authorization using ASP.NET Identity


## 🛠️ Technologies Used  
- **.NET 7** – Backend & MVC framework  
- **Entity Framework Core 7** – ORM for database management  
- **Microsoft Identity** – Security and user management with customized data models  
- **JWT (JSON Web Tokens)** – Authentication and authorization for the web api project
- **Cookie Based Authentication** - Authentication for the MVC admin panel project
- **Redis** – Distributed caching layer  
- **Stripe** – Integrated payment gateway (test mode)

## 🧠 Architecture & Design Patterns  
- **Onion Architecture** – Separation of concerns across Core, Infrastructure (The Repository layer), Services, API and MVC layers  
- **Generic Repository Pattern** – Abstracted data access logic  
- **Unit of Work Pattern** – Transactional consistency across repositories  
- **Specification Pattern** – Flexible query composition and filtering
- The **Web API** acts as the shared backend for both the Angular and MVC applications.
- Each frontend project is decoupled and communicates with the API via HTTP.
- This separation ensures scalability, maintainability, and clear boundaries between user-facing and administrative functionality.


## 🧱 Error Handling Middleware
The project includes a custom middleware that intercepts unhandled exceptions and returns structured error responses using ProblemDetails. This ensures:
- Centralized exception logging
- Predictable error formats for API consumers
- Clean separation of error-handling logic

## 🚀 Getting Started

### Prerequisites
Check the following:
- [.NET SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Stripe account](https://dashboard.stripe.com/register) (for test keys)
- Optional: Visual Studio

### 🔧 Setup Instructions
1. **Create a Stripe account and obtain your test API keys**
2. **Clone the repository**
   ```
   git clone https://github.com/HossamZaki-git/Talabat.git
   cd Talabat
   ```
3. **Create appsettings.json files inside the Talabat.WebAPI & the Admin Panel projects folders:**
   - Populate the appsettings.json file of the web api with the following content
      ```json
      {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
          }
        },
        "AllowedHosts": "*",
        "ConnectionStrings": {
          "ConnectionString": "Server = .; Database = Talabat; Trusted_Connection = true; encrypt = true; TrustServerCertificate = true; MultipleActiveResultSets = true;",
          "IdentityConnection": "Server = .; Database = TalabatIdentity; Trusted_Connection = true; encrypt = true; TrustServerCertificate = true; MultipleActiveResultSets = true;",
          "redis": "localhost"
        },
        "JWT": {
          "Secret": "",
          "Issuer": "",
          "Audience": "API consumer",
          "Duration": 2
        },
        "WebAPIBaseURL": "",
        "Stripe": {
          "Publishablekey": "",
          "Secretkey": ""
        },
        "AllowedOrigins": {
          "TalabatOrigin": ""
        }
      } 
      ```
   - Populate the appsettings.json the file of the admin panel with the following content
     ```json
        {
        "Logging": {
          "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
                      }
                    },
        "AllowedHosts": "*",
        "ConnectionStrings": {
          "ConnectionString": "Server = .; Database = Talabat; Trusted_Connection = true; encrypt = true; TrustServerCertificate = true; MultipleActiveResultSets = true;",
          "IdentityConnection": "Server = .; Database = TalabatIdentity; Trusted_Connection = true; encrypt = true; TrustServerCertificate = true; MultipleActiveResultSets = true;"
                             }
      } 
     ```
   - Put the values of the publishable key and the secret key in the Stripe.Publishablekey & Stripe.Secretkey
   - Assign at least 32 characters to JWT.Secret
   - Assign the base URL to JWT.Issuer & WebAPIBaseURL
4. **Restore the dependencies** `dotnet restore Talabat.sln`
5. **Start the Redis server using** `redis-server` .
6. **Run the web api project first to seed the data. Now the web api project is ready** You can test it using any API client like Postman or the MVC project.  
   If you want to integrate it with the frontend to test the payment process, download the Angular project and follow its setup instructions [The Angular Project](https://github.com/HossamZaki-git/Talabat-angular-client-).  
   Then, in the `appsettings.json` file, assign the frontend base URL to `AllowedOrigins.TalabatOrigin` to comply with the CORS policy.
7. **Run the MVC admin panel project. Admin account Credentials**  
- Email: `admin000@talabat.com`  
- Password: `Pa$$w0rd`

