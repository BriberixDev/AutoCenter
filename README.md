# AutoCenter

AutoCenter is a web platform for **car sales, rentals, and automotive news**.  
Built with **ASP.NET Core (Razor Pages)** and **Entity Framework Core**.

## Features
- Car marketplace (listings with filtering)  
- Car rentals with booking system  
- Automotive news section  
- Basic user roles (Buyer, Seller, Admin)  

## Tech Stack
- ASP.NET Core 8 (Razor Pages)  
- EF Core + SQLite (dev), PostgreSQL (prod)  

## Getting Started
```bash
git clone https://github.com/BriberixDev/AutoCenter.git
cd AutoCenter
dotnet ef database update
dotnet run --project AutoCenter.Web
