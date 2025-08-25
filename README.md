# Talabat Clone – E-Commerce Web API

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
- **Onion Architecture** – Separation of concerns across Core, Infrastructure, Services, and API layers  
- **Generic Repository Pattern** – Abstracted data access logic  
- **Unit of Work Pattern** – Transactional consistency across repositories  
- **Specification Pattern** – Flexible query composition and filtering
