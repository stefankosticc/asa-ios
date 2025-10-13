# ASA

ASA is an iOS app for artists and enthusiasts to share, discover, and discuss artworks. It enables users to showcase their creations, connect with others, participate in auctions, and manage their profiles in a modern, user-friendly environment.

## Features

- **Artwork Sharing:** Upload and showcase your art with descriptions and tags.
- **Discovery:** Browse and search artworks by category, artist, or popularity.
- **Social Interaction:** Like and follow other artists.
- **Auctions:** Participate in live auctions to buy or sell art.
- **Profile Management:** Customize your profile, view your and others collections

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/stefankosticc/asa-ios.git
   ```
2. **Setup the Backend:**

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

3. **Open the project in Xcode:**
   - Navigate to the project directory and open `ios.xcodeproj`.
4. **Install dependencies:**
   - If using CocoaPods:
     ```bash
     pod install
     ```
   - If using Swift Package Manager, dependencies will resolve automatically.
5. **Build and run:**
   - Select a simulator or device and press `Run` in Xcode.

## Usage

- **Sign Up / Log In:** Create an account or log in to access all features.
- **Explore Artworks:** Use the discover and search page to discover new art.
- **Share Your Art:** Tap the add button to add new artwork.
- **Interact:** Like, follow and chat with artists to build your network.
- **Join Auctions:** Browse ongoing auctions and place bids.
