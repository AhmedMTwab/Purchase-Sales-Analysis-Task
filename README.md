# Purchase & Sales Analysis Task

A C#/.NET Core application for efficient bulk analysis and management of purchase and sales data, supporting both Excel (XLSX) and CSV file uploads. The application is designed with clean architecture, dependency injection, and high-performance data processing using bulk database operations.

---

## Features

- Upload and analyze large datasets of purchases and sales from XLSX or CSV files
- High-performance bulk insert/update using [EFCore.BulkExtensions](https://github.com/borisdj/EFCore.BulkExtensions)
- Batch processing for improved memory efficiency
- Automatic product detection and creation during sales upload
- Query top sold products, deadstock, and profits
- Clean separation of concerns (Core, Domain, Infrastructure, API)
- Extensible via dependency injection

---

## Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- EFCore.BulkExtensions
- CsvHelper (for CSV parsing)
- OfficeOpenXml (EPPlus for XLSX parsing)
- Dependency Injection

---

## Project Structure

```
SRC/
├── Purchase&Sales_API/           # Web API Layer
├── Purchase&Sales_Core/          # Application/Core Services
├── Purchase&Sales_Domain/        # Domain Models and Interfaces
├── Purchase&Sales_Infrastructure/# Infrastructure (EF, DB, Repositories)
```

---

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (or change connection string as needed)

### Setup

1. **Clone the repository:**
    ```sh
    git clone https://github.com/AhmedMTwab/Purchase-Sales-Analysis-Task.git
    cd Purchase-Sales-Analysis-Task
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

3. **Apply database migrations:**
    ```sh
    dotnet ef database update --project SRC/Purchase&Sales_Infrastructure
    ```

4. **Run the application:**
    ```sh
    dotnet run --project SRC/Purchase&Sales_API
    ```

5. **Access API docs:**  
    Navigate to `https://localhost:5001/swagger` (or the port shown in your console) for Swagger UI.

---

## Usage

- **Upload XLSX or CSV:**  
  Use the API endpoints to upload purchase or sales data files.
- **Batch Size:**  
  Large files are processed in batches (default: 20,000 rows per batch).
- **Automatic Product Creation:**  
  Sales upload will auto-create products not found in the database.
- **Profit Analysis:**  
  EndPoints for Most Sold Products & Products profit.
- **Error Handling:**  
  All unhandled errors are caught by a global error handling middleware and returned as structured JSON.

---

## Author

[AhmedMTwab](https://github.com/AhmedMTwab)