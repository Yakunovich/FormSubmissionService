# FormSubmission Application

A simple form submission application with a .NET backend API and a Vue.js frontend.

## Requirements

- .NET 9.0 SDK
- Node.js (v14 or newer)
- npm

## Quick Start

### Backend API

1. Navigate to the API project directory:
   ```
   cd FormSubmissionService
   ```

2. Run the API:
   ```
   dotnet run --project FormSubmission.API/FormSubmission.API.csproj
   ```

3. The API will be available at:
   - https://localhost:5001
   - http://localhost:5000

### Frontend Client

1. Navigate to the client directory:
   ```
   cd FormSubmissionClient
   ```

2. Install dependencies (first time only):
   ```
   npm install
   ```

3. Start the development server:
   ```
   npm run dev
   ```
   
## API Endpoints

- `GET /api/forms` - Get all forms
- `GET /api/forms/{id}` - Get form by ID
- `POST /api/forms` - Create a new form
- `POST /api/forms/search` - Search forms

## Configuration

The application uses an in-memory database by default. Configuration can be adjusted in:
- `FormSubmission.API/appsettings.json`
- `FormSubmission.API/appsettings.Development.json` 