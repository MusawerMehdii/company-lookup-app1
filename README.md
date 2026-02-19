# Secure Company Lookup App

React (SPA) + ASP.NET Core Web API + OIDC (Auth0)

---

## Overview

This project is a full-stack web application where a user can:

- Log in using OAuth2 / OpenID Connect
- Search for Norwegian companies
- View company details
- Access secured backend endpoints

---

## Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Clean service-layer architecture
- JWT Bearer Authentication
- HttpClient
- Dependency Injection

### Frontend
- React (Vite)
- TypeScript
- Auth0 React SDK
- Axios
- Responsive CSS Grid UI

### Authentication
- Auth0 (OIDC)
- Authorization Code Flow with PKCE
- JWT access tokens
- Refresh tokens enabled

### External Data Source
- Brønnøysundregistrene – Enhetsregisteret (Open Data API)

---

# Architecture

company-lookup-app/
│
├── backend/
│ └── CleanCompanyApi/
│
└── frontend/
└── company-frontend/


Important:

- The frontend never calls the Norwegian open API directly.
- All external API calls go through the backend.
- All backend endpoints are protected using JWT Bearer authentication.

---

# Backend Setup

## Prerequisites

- .NET SDK 8.0
- Visual Studio 2022 or dotnet CLI

## Run Backend

```bash
cd backend/CleanCompanyApi
dotnet restore
dotnet run
API runs on:

https://localhost:7242
Backend Authentication
All endpoints are protected with:

[Authorize]
The frontend sends:

Authorization: Bearer <access_token>
The backend validates:

Issuer

Audience

Signature

Expiration

Backend Endpoints
GET /api/me
Returns JWT claims of the authenticated user.

GET /api/companies?query=<text>
Searches companies by name.

Returns 400 if query is missing.

GET /api/companies/{orgnr}
Fetches company by organization number.

Error Handling
External API failure → 502 Bad Gateway

External API timeout → 504 Gateway Timeout

Clear error messages returned

No caching used

Frontend Setup (React + TypeScript)
Prerequisites
Node.js 18+

npm

Run Frontend
cd frontend/company-frontend
npm install
npm run dev
Frontend runs on:

http://localhost:5173
TypeScript Setup
If TypeScript is not installed:

npm install --save-dev typescript @types/react @types/react-dom
Files using TypeScript:

App.tsx

CompanySearch.tsx

main.tsx

types.ts

Auth0 Configuration
Create a .env file inside:

frontend/company-frontend/
Add:

VITE_AUTH0_DOMAIN=YOUR_AUTH0_DOMAIN
VITE_AUTH0_CLIENT_ID=YOUR_CLIENT_ID
VITE_AUTH0_AUDIENCE=https://company-api
Do NOT commit .env.

Ensure backend appsettings.json contains:

"Auth0": {
  "Domain": "YOUR_AUTH0_DOMAIN",
  "Audience": "https://company-api"
}
Authentication Flow
User logs in via Auth0

Frontend requests access token with correct audience

Token is attached to API calls

Backend validates JWT

Secure endpoints return data

Example Authenticated Request
curl -k \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  https://localhost:7242/api/companies?query=test
What Works End-to-End
OAuth2 / OIDC login (Authorization Code Flow + PKCE)

JWT-secured backend

Protected endpoints

Norwegian open data API integration

React SPA consuming secured backend

Access + refresh tokens handled client-side

Search by name

Search by organization number

TypeScript frontend

Clean service-layer backend architecture

Improvements (Future Work)
Add pagination

Add dedicated company details page

Improve UI accessibility

Add automated tests (unit + integration)

Move token storage to in-memory with refresh fallback

Add structured logging

Add Docker support

Notes
Swagger disabled in production scenarios

Backend root (/) intentionally returns 404

All API interaction is done through secured endpoints
