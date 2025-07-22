# 🟦 Connectied

A project developed as part of **On-the-Job Training (OJT)**.

## Getting Started

The following prerequisites are required to build and run the solution:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (latest version)
- [Node.js](https://nodejs.org/) (latest LTS)

### Steps to Run

1. Ensure that `Connectied.Server` is set as the startup project:  
   In Visual Studio, right-click on the project and select **"Set as Startup Project"**.

2. Open your terminal, navigate to the `connectied.client` directory, and install the dependencies:

    ```bash
    npm install
    ```

3. Run the project in Visual Studio using either:
   - **Start Debugging** (`F5`), or  
   - **Start Without Debugging** (`Ctrl + F5`)

## 🧱 Technology Stack

- **.NET 8.0** – Backend framework
- **React + TypeScript** – Frontend framework
- **Tailwind CSS** – Styling framework

---

## 📦 Backend Libraries

- **MediatR**  
  For managing commands and queries in a clean architecture style (CQRS).
  
- **Ardalis.GuardClauses**  
  For implementing guard clauses to validate inputs and ensure code correctness.

- **Ardalis.Result**  
  For handling results in a consistent manner, providing structured success/error flows.

- **Ardalis.Specification**  
  For building reusable, composable query logic using specifications.

- **FluentValidation**  
  For validating inputs with a fluent API.

- **Mapster**  
  For fast and efficient object-to-object mapping.

- **Entity Framework Core**  
  For database access and object-relational mapping.

- **Serilog**  
  For structured logging.

- **Swashbuckle (Swagger)**  
  For auto-generating interactive API documentation.

---
