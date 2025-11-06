# Universal Inventory Manager

A modern, multi-user web application for flexible inventory management built with **Blazor Server** and **.NET 8**.

## 🎯 Features

### Core Functionality
- **Dynamic Inventory Structure** - Create custom inventories with configurable fields
- **Custom ID Generation** - Configurable ID formats with date, random numbers, sequences, and GUIDs
- **Real-time Collaboration** - Live comments and discussions using SignalR
- **Drag-and-Drop Interface** - Reorder custom fields and ID format components
- **Cloud Image Storage** - Upload and manage images with Cloudinary CDN

### User Management
- **OAuth Authentication** - Google & Facebook login
- **Role-Based Access** - Owner, write-access users, and admin roles
- **User Administration** - Admin dashboard for user management

### Data Management
- **PostgreSQL Database** - Relational database with Entity Framework Core
- **Optimistic Locking** - Prevents data conflicts in multi-user scenarios
- **Full-Text Search** - Search across all inventories and items
- **Cascade Deletion** - Automatic cleanup of related data

## 🚀 Technology Stack

- **Frontend**: Blazor Server (.NET 8)
- **Backend**: ASP.NET Core
- **Database**: PostgreSQL (NeonDB)
- **ORM**: Entity Framework Core 8.0.4
- **Real-time**: SignalR
- **Authentication**: OAuth (Google, Facebook)
- **Image Storage**: Cloudinary
- **UI Framework**: Bootstrap 5

## 📦 Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL database (NeonDB recommended)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd InventoryManager
   ```

2. **Configure Environment**

   **⚠️ IMPORTANT: Security**
   - This project uses `appsettings.template.json` as a reference
   - **DO NOT** commit real credentials to git
   - Use environment variables or user-secrets for sensitive data
   - The `.gitignore` file is configured to exclude sensitive configuration files

   ```bash
   # Option A: Copy template and edit (for production)
   cp appsettings.template.json appsettings.json
   # Then edit appsettings.json with your credentials

   # Option B: Use user-secrets (recommended for development)
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
   dotnet user-secrets set "Authentication:Google:ClientId" "YOUR_GOOGLE_CLIENT_ID"
   dotnet user-secrets set "Authentication:Google:ClientSecret" "YOUR_GOOGLE_CLIENT_SECRET"
   # ... set other secrets
   ```

3. **Set Up External Services**

   **Database (PostgreSQL - NeonDB)**
   - Create account at https://neon.tech
   - Create new database
   - Copy connection string

   **OAuth Authentication**
   - Google: https://console.developers.google.com/
   - Facebook: https://developers.facebook.com/
   - Add your domain to authorized redirect URIs

   **Cloudinary (Image Storage)**
   - Create account at https://cloudinary.com
   - Get Cloud Name, API Key, and API Secret from dashboard

4. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```

5. **Build and Run**
   ```bash
   dotnet build --configuration Release
   dotnet run --configuration Release
   ```

## 📁 Project Structure

```
├── Components/
│   ├── Pages/              # Main pages (Home, InventoryItems, Settings, etc.)
│   ├── Simplified*.razor   # Reusable components
│   └── Layout/             # Layout components
├── Data/
│   └── AppDbContext.cs     # EF Core database context
├── Hubs/
│   └── CommentHub.cs       # SignalR hub for real-time comments
├── Models/                 # Entity models
├── Services/               # Business logic services
└── wwwroot/               # Static assets (CSS, JS, images)
```

## 🎨 Key Features Explained

### Custom Fields
Each inventory can define up to 3 custom fields per type (String, Integer, Boolean) with:
- Enable/disable fields
- Custom field names
- Drag-and-drop reordering

### Custom ID Formats
Build unique item IDs from components:
- Fixed text
- Random numbers (with min/max)
- Sequence numbers (with start/increment)
- Date/time (various formats)
- GUID

### Real-time Comments
- Live comment updates using SignalR
- Automatic fallback polling if SignalR disconnected
- Group-based messaging per inventory

## 🛠️ Development

### Build Commands
```bash
# Debug build
dotnet build

# Release build
dotnet build --configuration Release

# Run tests
dotnet test

# Database operations
dotnet ef migrations add <Name>
dotnet ef database update
```

### Code Style
- C# 12 with nullable reference types enabled
- Async/await patterns for all I/O operations
- Repository pattern for data access
- Dependency injection throughout

## 📄 Documentation

- `Checklist.md` - Project requirements checklist
- `Project description.md` - Detailed feature specifications

## 🔒 Security Features

- OAuth 2.0 authentication
- Role-based authorization
- SQL injection protection (EF Core)
- XSS protection (Blazor Server)
- CSRF protection

## 📊 Performance

- Optimized database queries (no N+1 problems)
- Cloudinary CDN for fast image delivery
- SignalR for real-time updates without polling
- Lazy loading for large inventories

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📝 License

This project is licensed under the MIT License.


Built with Blazor Server and modern .NET technologies.
