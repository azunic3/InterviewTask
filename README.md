# Medicine Checker

Medicine Checker is a full-stack web application that allows users to search for medicines, review safety information, check potential allergy risks, and verify local availability in a pharmacy system.

The application integrates public drug data with a custom backend and database.

---

## Features

- Drug search using public openFDA data
- Allergy screening based on user-provided allergens
- Local availability check
- Email notification when a medicine becomes available via email
- Similar medicine suggestions
- Popular searches tracking

---

## Tech Stack

### Frontend
- Angular
- SCSS
- REST API communication

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- Mailgun integration

### Database
- SQL Server

---

## External APIs & Services

- **openFDA API** – public drug label data  
  https://open.fda.gov/

- **Mailgun** – email notifications  
  https://www.mailgun.com/

---

## Project Structure
- /Controllers - API endpoints
- /Services - Business logic and external API integrations
- /Data - Entity Framework DbContext
- /Model - Database entities
- /Dtos - Data transfer objects
- /Enums - Application enums
- /Helpers - Utility and helper classes
- /Migrations - Entity Framework Core migrations
- /frontend - Angular frontend application

---

## Prerequisites

- .NET 7 SDK (or compatible)
- Node.js (v18+ recommended)
- Angular CLI
- SQL Server (LocalDB or full instance)
- Mailgun account (for email notifications)

---

## Running the Backend

1. Configure the database connection string in `appsettings.json`.
2. Apply Entity Framework migrations:
   dotnet ef database update
3. Run the backend
   dotnet run
4. The API will be available at: https://localhost:7091

## Running the Frontend
1. Navigate to the frontend folder: 
  cd frontend
2. Install dependencies: npm install
   npm install chart.js ng2-charts (required for Chart)
4. Start the Angular development server:
   ng serve
5. The frontend will be available at: http://localhost:4200

---

## Mailgun Notes (Email Notifications)

Email notifications will work only after the recipient email address is verified in Mailgun.
