Secure Company Lookup App

React (SPA) + ASP.NET Core Web API + OIDC

Overview

This project is a small full-stack web application where a user can:

Log in using OAuth2 / OpenID Connect

Search for Norwegian companies

View basic company information

The solution consists of:

Frontend: React (Vite SPA)

Backend: ASP.NET Core Web API

Authentication: Auth0 (OIDC, Authorization Code Flow with PKCE)

External Data Source: Brønnøysundregistrene (Enhetsregisteret – Open Data)

Architecture
company-lookup-app/
│
├── backend/                 # ASP.NET Core Web API
│   └── CleanCompanyApi/
│
└── frontend/
    └── company-frontend/    # React SPA (Vite)


The frontend never calls the Norwegian open API directly.

All external API calls go through the backend.

All backend endpoints are protected using JWT Bearer authentication.

Prerequisites
Backend

.NET SDK 8.0

Visual Studio 2022 or dotnet CLI

Frontend

Node.js 18+

npm (comes with Node)

Authentication (OIDC)
Provider

Auth0 was used as the OpenID Connect provider.

Flow

Authorization Code Flow with PKCE (recommended for SPAs)

JWT access tokens

Refresh tokens enabled

Tokens obtained

access_token – used to call the backend API

refresh_token – used to silently refresh the access token

Token storage (client-side)

Tokens are stored using the Auth0 React SDK

Storage location: localStorage

Trade-offs:

✅ Enables silent token refresh

❌ Slightly less secure than in-memory storage 

Test Users

Two test users are configured in Auth0.

Example:

(mentioned on email)

(Passwords are configured in the Auth0 dashboard.)

How to log in

Open the frontend

Click Login

Authenticate using one of the test users

After login, you are redirected back to the app

Backend (ASP.NET Core Web API)
How to run
cd backend/CleanCompanyApi
dotnet restore
dotnet run


The API runs on:

https://localhost:7242

Authentication & Authorization

All API endpoints are protected with JWT Bearer authentication

The frontend sends:

Authorization: Bearer <access_token>


The backend validates:

Issuer

Audience

Signature

Expiration

API Endpoints
GET /api/me

Returns basic information from the authenticated JWT.

Example response:

{
  "authenticated": true,
  "claims": [
    { "type": "iss", "value": "https://<auth0-domain>/" },
    { "type": "sub", "value": "<user-id>" },
    { "type": "aud", "value": "https://company-api" }
  ]
}

GET /api/companies?query=<text>

Searches for Norwegian companies.

Query parameter is required

Returns 400 if missing or empty

Calls Brønnøysundregistrene internally

Maps the response to a simplified DTO

Example response:

[
  {
    "organizationNumber": "934045769",
    "name": "TEST DA",
    "organizationForm": "Ansvarlig selskap med delt ansvar",
    "municipality": "DRAMMEN"
  }
]

GET /api/companies/{orgnr}

Fetches details for a single company by organization number.

Error Handling

External API failure → 502 Bad Gateway

External API timeout → 504 Gateway Timeout

Clear error messages are returned to the client

No caching is used (per requirements)

Norwegian Open Data API

Source: Brønnøysundregistrene – Enhetsregisteret

No registration required

Public open data API

Endpoints used:

GET /api/enheter

GET /api/enheter/{orgnr}

The backend normalizes external fields into a stable internal DTO before returning data to the frontend.

Frontend (React SPA)
How to run
cd frontend/company-frontend
npm install
npm run dev


Frontend runs on:

http://localhost:5173

Functionality

Login / Logout using OIDC

Fetches access_token silently

Calls backend with Authorization: Bearer <token>

Company search UI:

Search input

Loading state

Error state

Results list

Displayed fields:

Company name

Organization number

Organization form

Municipality

Example Authenticated Request
curl -k \
  -H "Authorization: Bearer <ACCESS_TOKEN>" \
  https://localhost:7242/api/companies?query=test

What Works End-to-End

✅ OAuth2 / OIDC login (Authorization Code Flow + PKCE)

✅ JWT-secured backend

✅ Protected endpoints

✅ Norwegian open data API integration

✅ React SPA consuming secured backend

✅ Access + refresh tokens handled client-side

What I Would Improve With More Time

Add pagination to company search

Add a dedicated company details page

Improve UI styling and accessibility

Add automated tests (unit + integration)

Move token storage to in-memory with refresh fallback

Add structured logging and correlation IDs

Notes

Swagger is disabled in production scenarios

The backend root (/) intentionally returns 404

All API interaction is done through secured endpoints

Update V0.1



# Clean Company API

Full-stack company lookup application using:

- ASP.NET Core Web API
- Auth0 JWT Authentication
- React (Vite)
- TypeScript
- Axios

---

## 🔧 Backend Setup

### 1️⃣ Navigate to backend folder

```bash
cd backend
2️⃣ Install dependencies
dotnet restore
3️⃣ Run the API
dotnet run
Backend runs on:

https://localhost:7242
💻 Frontend Setup (TypeScript + Vite)
1️⃣ Navigate to frontend folder
cd frontend/company-frontend
2️⃣ Install dependencies
npm install
3️⃣ Install TypeScript (if not installed)
npm install --save-dev typescript @types/react @types/react-dom
4️⃣ Run development server
npm run dev
Frontend runs on:

http://localhost:5173
🔐 Auth0 Configuration
In main.tsx:

<Auth0Provider
  domain="YOUR_AUTH0_DOMAIN"
  clientId="YOUR_CLIENT_ID"
  authorizationParams={{
    redirect_uri: window.location.origin,
    audience: "https://company-api"
  }}
>
Make sure backend appsettings.json contains:

"Auth0": {
  "Domain": "YOUR_AUTH0_DOMAIN",
  "Audience": "https://company-api"
}
🔎 Features
Search companies by name

Search by organization number

Secure JWT-protected endpoints

Clean service-layer architecture

TypeScript frontend

Responsive UI

📦 Tech Stack
Backend:

ASP.NET Core 8

JWT Bearer Authentication

HttpClient

Clean architecture service pattern

Frontend:

React

Vite

TypeScript

Auth0 React SDK

Axios

🚀 Production Notes
Enable strict ValidateAudience = true

Move secrets to environment variables

Enable HTTPS in production

Add error logging middleware