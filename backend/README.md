# ArtSharingApp

ArtSharingApp is a full-stack platform for artists and enthusiasts to share, discover, and discuss digital artworks. It enables users to showcase their creations, connect with others, participate in auctions, and manage their profiles in a modern, user-friendly environment.

## Features

- **User Authentication:** Secure sign up, login, and profile management.
- **Artwork Management:** Upload, edit, and display artworks with rich descriptions.
- **Social Interaction:** Follow/unfollow users, view followers/following, and send direct messages.
- **Notifications:** Stay updated on likes, follows, and other interactions.
- **Auctions & Sales:** List artworks for sale or auction, bid on items, and view analytics.
- **Discovery & Search**: Comprehensive search functionality for users, artworks, cities, and galleries. Includes discovery features for trending artworks and top artists.

## Technologies

- **Frontend:** React, TypeScript
- **Backend:** ASP.NET Core, Entity Framework
- **Database:** PostgreSQL (or specify your DB)
- **Other:** RESTful APIs, SignalR

## Getting Started

### Prerequisites

- Node.js & npm (for frontend)
- .NET 9 SDK (for backend)
- PostgreSQL (or your chosen DB)

### Running Locally

#### 1. Clone the repository

```bash
git clone https://github.com/stefankosticc/art-sharing-app.git
cd ArtSharingApp
```

#### 2. Setup the Backend

```bash
cd ArtSharingApp.Backend
dotnet restore
```

- **Configure Application Settings**

  Update the `appsettings.json` file with your local PostgreSQL connection string.

  `appsettings.json`**:**

  ```json
  {
    "ConnectionStrings": {
      "ArtSharingAppContext": "Host=localhost;Port=5432;Username=postgres;Password=your_password;Database=art-db;"
    },
    "Jwt": {
      "Issuer": "ArtSharingApp",
      "Audience": "ArtSharingApp",
      "Token": "YOUR_SUPER_SECRET_JWT_KEY_SHOULD_BE_IN_USER_SECRETS"
    }
  }
  ```

- **Apply Database Migrations**
  The application uses EF Core migrations to set up the database schema. Run the following command from the `ArtSharingApp.Backend` directory:

  ```bash
  dotnet ef database update
  ```

- **Run the Application**

  ```bash
  dotnet run
  ```

  The API will be available at `http://localhost:5125`.

#### 3. Setup the Frontend

```bash
cd artsharingapp-frontend
```

Create a `.env` file in `artsharingapp-frontend` and set:

```
REACT_APP_BACKEND_URL=http://localhost:5125
```

```
npm install
npm start
```

The frontend runs at [http://localhost:3000](http://localhost:3000).

## Folder Structure

- `/ArtSharingApp.Backend` - ASP.NET Core backend
- `/artsharingapp-frontend` - React frontend
- `/ArtSharingApp.Tests` - Backend tests
