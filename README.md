# CashFlow

![badge-dot-net]
![badge-rider]
![badge-sqlserver]
![badge-swagger]

## About the Project

This project, developed using **.NET 8**, adopts the principles of **Domain-Driven Design (DDD)** to offer a structured and effective solution for managing personal expenses. The main objective is to allow users to register their expenses with informations such as title, date, description, amount and payment type. All data is stored securely in a **SQL Server** database.

The **API** architecture is based on **REST**, using standard **HTTP methods** for simple and efficient communication. It is also complemented by **Swagger** documentation, which provides an interactive graphical interface for developers to explore and test endpoints in an easy way.

### Nuget Packages

- **AutoMapper:** is responsible for mapping between domain and request/response objects, reducing the need for repetitive and manual code.
- **FluentAssertions:** is used in unit tests to make verifications more readable, helping to write clear and understandable tests.
- **FluentValidation:** is used to implement validation rules in a simple and intuitive way in the request classes, keeping the code clean and easy to maintain.
- **EntityFramework:** acts as an ORM (Object-Relational Model) that simplifies interactions with the database by allowing the use of .NET objects to manipulate data directly, without the need to deal with SQL queries.

### Features
- **Domain-Driven Design (DDD):** Modular structure that facilitates understanding and maintenance of the application domain.
- **Unit Testing:** Comprehensive testing with FluentAssertions to ensure functionality and quality.
- **Reporting:** Ability to export detailed reports to PDF and Excel, providing visual and effective analysis of expenses.
- **RESTful API with Swagger Documentation:** Documented interface that facilitates integration and testing by developers.

## Getting Started

For a local copy to work properly, follow the steps below.

### Requirements

- Visual Studio version 2022+ or Visual Studio Code.
- Windows 10+ or Linux/MacOS with [.NET SDK][dot-net-sdk] installed.
- Microsoft SQL Server.

### Installation
1. Clone the repository: 
```sh
git clone https://github.com/thrsouza/CashFlow.git
```
2. Fill in the information in the `appsettings.Development.json` file.
3. Run the API and enjoy your test. :)


<!-- Links -->
[dot-net-sdk]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0

<!-- Badges -->
[badge-dot-net]: https://img.shields.io/badge/.NET-000?logo=dotnet&logoColor=fff&style=for-the-badge
[badge-rider]: https://img.shields.io/badge/Rider-000?logo=rider&logoColor=fff&style=for-the-badge
[badge-sqlserver]: https://img.shields.io/badge/SQL%20Server-000?logo=windows&logoColor=fff&style=for-the-badge
[badge-swagger]: https://img.shields.io/badge/Swagger-000?logo=swagger&logoColor=fff&style=for-the-badge
