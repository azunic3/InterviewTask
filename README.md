# Medicine Checker

Medicine Checker is a full-stack web application that allows users to search for medicines, review safety information, check potential allergy risks, and verify local availability in a pharmacy system.

The application integrates public drug data with a custom backend and database.

You can find the application on following link: https://agile-courage-production-0160.up.railway.app/ 
To access Swagger, use the following link: https://interviewtask-production-95d4.up.railway.app/swagger/index.html 

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
- Docker Desktop

---

## Running the Backend
**To run project locally, checkout to dev branch (git checkout dev)**

1. Run docker compose up --build
2. Run the backend
   dotnet run
3. The API will be available at: https://localhost:7091

## Running the Frontend
1. Navigate to the frontend folder: 
  cd frontend
2. Install dependencies: npm install
   npm install chart.js ng2-charts (required for Chart)
4. Start the Angular development server:
   ng serve
5. The frontend will be available at: http://localhost:4200

---

# Deployment Guide (Railway)

This project is deployed using **Railway**, with separate services for the **backend**, **frontend**, and **database**.

---

## Deployment Architecture

- **Backend**: ASP.NET Core Web API (Railway service)
- **Frontend**: Angular production build served via Node + `serve`
- **Database**: Managed PostgreSQL (Railway)
- **Communication**: Frontend → Backend via HTTPS REST API

---

## Backend Deployment (ASP.NET Core)

### Service Setup
- Backend is deployed from the **root of the repository**
- Railway automatically builds and runs the application

### Database
- PostgreSQL is provisioned as a Railway service
- The backend uses the Railway-provided `DATABASE_URL`
- The URL is converted at runtime into an Npgsql-compatible connection string

---

## Mailgun Notes (Email Notifications)

Email notifications will work only after the recipient email address is verified in Mailgun.
