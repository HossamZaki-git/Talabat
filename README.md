# 🛒 Talabat Clone – E-Commerce Web API

## 📦 Description  
A full-stack e-commerce Web API inspired by Talabat's food delivery platform. It includes features such as order management, product catalog browsing, and simulated payment workflows. Built with **ASP.NET Core**, the project emphasizes clean architecture, modularity, and extensibility.

## 🛠️ Technologies Used  
- **.NET 7** – Backend framework  
- **Entity Framework Core 7** – ORM for database management  
- **Microsoft Identity** – Security and user management with customized data models  
- **JWT (JSON Web Tokens)** – Authentication and authorization  
- **Redis** – Distributed caching layer  
- **Stripe** – Integrated payment gateway (test mode)

## 🧠 Architecture & Design Patterns  
- **Onion Architecture** – Separation of concerns across Core, Infrastructure (The Repository layer), Services, and API layers  
- **Generic Repository Pattern** – Abstracted data access logic  
- **Unit of Work Pattern** – Transactional consistency across repositories  
- **Specification Pattern** – Flexible query composition and filtering

## 🧱 Error Handling Middleware
The project includes a custom middleware that intercepts unhandled exceptions and returns structured error responses using ProblemDetails. This ensures:
- Centralized exception logging
- Predictable error formats for API consumers
- Clean separation of error-handling logic

## 🚀 Getting Started

### Prerequisites
Ensure the following are installed:
- [.NET SDK 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Stripe account](https://dashboard.stripe.com/register) (for test keys)
- Optional: Visual Studio

### 🔧 Setup Instructions
1. **Make a stripe user & account**
2. **Clone the repository**
   ```
   git clone https://github.com/HossamZaki-git/Talabat.git
   cd Talabat
   ```
3. **Fill the appsettings.json file values:**
     - Put the values of the publishable key and the secret key in the Stripe.Publishablekey & Stripe.Secretkey
     - Assign JWT.Secret to at least 32 characters
4. **Restore the dependencies** `dotnet restore`
5. Now the backend is ready. You can test it using any API client like Postman.  
   If you want to integrate it with the frontend to test the payment process, download the Angular project and follow its setup instructions.  
   Then, in the `appsettings.json` file, assign the frontend base URL to `AllowedOrigins.TalabatOrigin` to comply with the CORS policy.

