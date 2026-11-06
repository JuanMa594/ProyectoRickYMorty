#  Rick and Morty Project - Juan Usuga

![Rick and Morty](https://tse3.mm.bing.net/th/id/OIP.B62kM7etxlckDEVuy9ULJQHaIX?r=0&rs=1&pid=ImgDetMain&o=7&rm=3)

## Overview

This project is an interactive web application developed in **Blazor .NET 10** that uses the [Rick and Morty API](https://rickandmortyapi.com/) to display information about the characters from the series. The application allows users to explore characters, filter them by different criteria, and vote using a likes/dislikes system.

## Table of Contents
- [Key Features](#key-features)
- [Installation](#instructions-for-running-the-app)
- [How the API Was Used](#how-the-api-was-used)
- [Code Structure](#code-structure)
- [Additional Features](#additional-features)
- [Technical Decisions](#technical-decisions)
- [Technologies Used](#technologies-used)
- [Screenshots](#screenshots)

---

### Key Features

- **Character gallery** in grid format
- **Voting system** (likes/dislikes) with real-time scoring
- **Advanced filters** by name, species, and status
- **Full pagination** with intuitive navigation
- **Modern design** with animations and visual effects
- **Clean architecture** following SOLID principles

---

## Instructions for Running the App

### Prerequisites

- **.NET SDK 10.0** or later
- A code editor (Visual Studio, VS Code, or Rider)
- A modern web browser

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/JuanMa594/ProyectoRickYMorty.git
   cd RickAndMortyProject
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Open in the browser:**
   ```
   https://localhost:5001
   ```
   Or the URL displayed in the console.


---

## How the API Was Used

###     Endpoint Used

The application consumes the **Characters** endpoint of the Rick and Morty API:

```
Base URL: https://rickandmortyapi.com/api/character
```

###     Implementation

#### 1. Configuring HttpClient

In `Program.cs`:

```csharp
builder.Services.AddHttpClient(“RickAndMortyApi”, client =>
{
    client.BaseAddress = new Uri(“https://rickandmortyapi.com/api/character”);
    client.DefaultRequestHeaders.Add(“Accept”, “application/json”);
});
```

#### 2. Abstraction Service

A service layer was implemented following the **Repository** pattern:

```csharp
public interface IRickAndMortyService
{
    Task<ApiResponse<Character>> GetCharactersAsync(int page = 1);
    Task<Character?> GetByIdAsync(int id);
    Task<List<Character>> GetMultipleCharactersAsync(params int[] ids);
    Task<ApiResponse<Character>> FilterCharactersAsync(...);
}
```
#### 3. Available Operations

| Method | Description | Endpoint |
|--------|-------------|----------|
| `GetCharactersAsync(page)` | Retrieves paginated characters | `GET /character?page={page}` |
| `GetByIdAsync(id)` | Retrieves a character by ID | `GET /character/{id}` |
| `GetMultipleCharactersAsync(ids)` | Retrieves multiple characters | `GET /character/{id1},{id2}` |
| `FilterCharactersAsync(...)` | Filters characters | `GET /character?name=...&status=...` |

#### 4. Error Handling

- **404**: Handled by returning `null` or empty lists
- **Network errors**: Captured with `try-catch` and logged
- **Pagination**: Page limit control

---

## Code Structure

```
RickYMortyProject/
│
├── Components/
│   ├── Layout/
│   │   ├── NavMenu.razor           # Side navigation menu
│   │   ├── MainLayout.razor        # Main layout
|   |   └── MainLayout.razor.css    # CSS customization for the main layout
│   │
│   ├── Pages/
│   │   ├── Home.razor              # Home page
│   │   ├── Characters.razor        # Character gallery (main page)
|   │   ├── Error.razor             # Error page, in case of an unexpected error
│   │   └── NotFound.razor          # 404 error
│   │
|   ├── _Imports.razor              # Global import handling
│   └── App.razor                   # Root component
|
├── Models/
│   ├── ApiResponse.cs              # Paginated response model and pagination information
│   └── Character.cs                # Character and location model
│
├── Services/
│   ├── IRickAndMortyService.cs     # Service interface
│   └── RickAndMortyService.cs      # Service implementation
│
├── wwwroot/
│   ├── css/
│   │   └── app.css                 # Styles and customization for the entire application
│   │
|   ├── lib/bootsrap/dist           # Bootstrap library and CSS
|   │
|   ├── Fonts/
|   |   └── get_scwifty.ttf         # Rick and Morty-style font
|   |
│   └── LogoPagina.png              # Logo used for the application
│
├── Program.cs                      # Application configuration
└── appsettings.json                # General configuration
```
*NOTE: This is where the project's general structure was set up, including the files and folders that were added, modified, or programmed—excluding additional files and folders that are primarily needed for the project to function correctly.*

###   Key Components

#### `RickAndMortyService.cs`
- Encapsulates all the logic for communicating with the API
- Handles errors and exceptions
- Configures JSON options (camelCase)
- Implements `IHttpClientFactory` for efficient connection management

#### `Characters.razor`
- Page displaying the characters from Rick and Morty
- Main component with interactive rendering (`@rendermode InteractiveServer`)
- Manages pagination state, filters, and ratings
- Implements UI using Bootstrap and custom CSS
- Handles user events (clicks, filters, navigation)

#### `NavMenu.razor`
- Side menu for navigating between pages
- Animations and hover effects
- “Liquid-glass” style design

---

## Additional Features

### 1. Advanced Filters

The application implements three types of filters that can be combined:

- **By Name**: Partial search (e.g., “rick” finds “Rick Sanchez”)
- **By Status**: Dropdown with options (Alive, Dead, Unknown)
- **By Species**: Partial search (e.g., “human,” “alien”)

**Features:**
- Independent and combinable filters
- “Search” button to apply filters and “Clear” button to reset
- Dynamic message displaying results and active filters
- Handling of empty results (404)

### 2. Smooth Animations and Custom Visual Style

#### Navigation Menu
- **Hover effect**: Slide to the right + background change
- **Ripple effect**: Ripple on click
- **Transitions**: `cubic-bezier` animation for smooth movement
- **Active state**: Side color bar with a dark green to aquamarine gradient

#### Character Cards
- **Hover**: Lift effect with `translateY(-5px)` and enhanced shadow
- **Smooth transitions**: 0.3s for all interactions
- **Button effects**: Scale and drop shadow on activation
- **Loading state**: Animated spinner

#### Pagination
- **Distinct buttons**: Previous/Next with dark gray background
- **Bordered buttons**: First/Last with thicker border and large icon
- **Hover states**: Smooth color changes
- **Active page**: Highlighted with a blue background

---

## Technical Decisions

### 1. **Blazor InteractiveServer vs. InteractiveAuto**

**Decision:** `@rendermode InteractiveServer` was used

**Reasons:**
- ✅ Simpler to configure in .NET 10
- ✅ Does not require downloading WASM to the client
- ✅ Lower initial application size
- ✅ Sufficient for the project’s needs (external API)
- ✅ Excellent for applications with moderate concurrent users
### 2. **HttpClientFactory Instead of Direct HttpClient**

**Decision:** Implement `IHttpClientFactory`

**Reasons:**
- ✅ Prevents socket exhaustion
- ✅ Automatic lifecycle management
- ✅ Better performance in production applications
- ✅ Facilitates testing with mock clients
- ✅ Centralized configuration

### 3. **Repository Pattern with Interface**

**Decision:** Create `IRickAndMortyService` + implementation

**Reasons:**
- ✅ Dependency inversion (SOLID)
- ✅ Facilitates unit testing
- ✅ Allows changing the implementation without affecting other components
- ✅ Encapsulates business logic
- ✅ Better maintainability

### 4. **State Management with Dictionaries**

**Decision:** Use `Dictionary<int, int>` for scores

**Reasons:**
- ✅ O(1) access by character ID (instant access)
- ✅ Persists throughout the browsing session
- ✅ Simple and efficient for local state
- ❌ Does not persist between page reloads (a conscious decision for simplicity; may be implemented in the future)

### 5. **Bootstrap + Custom CSS**

**Decision:** Combine Bootstrap with custom CSS

**Reasons:**
- ✅ Bootstrap provides a customized page foundation
- ✅ Custom CSS for a unique visual identity
- ✅ Avoids the “generic” look of frameworks
- ✅ Maintains consistency across components
- ✅ Easy to maintain and extend

### 6. **JsonSerializerOptions with camelCase**

**Decision:** Set `PropertyNamingPolicy.CamelCase`

### 6. **JsonSerializerOptions with camelCase**

**Decision:** Set `PropertyNamingPolicy.CamelCase`

**Reasons:**
- ✅ The Rick and Morty API uses camelCase
- ✅ Standard convention in REST APIs
- ✅ Prevents deserialization errors
- ✅ Centralized configuration within the service

### 7. **Client-Side Pagination**

**Decision:** Do not implement infinite scroll; use traditional pagination

**Reasons:**
- ✅ Better UX, due to the very large number of characters (826 characters)
- ✅ Explicit user control
- ✅ Lower memory consumption
- ✅ More accessible
- ✅ Aligns with API capabilities

### 8. **Filters Using New Requests vs. Local Cache**

**Decision:** Make a new request to the API for each filter

**Reasons:**
- ✅ Results are always up to date
- ✅ Lower memory usage on the client side
- ✅ Leverages native API capabilities
- ✅ Better for frequently changing data
- ❌ More network requests (acceptable since the API is free and fast)

---

## Technologies Used

| Technology | Version | Purpose |
|------------|---------|-----------|
| .NET | 10.0 | Main framework |
| Blazor | .NET 10 | Interactive frontend |
| Bootstrap | 5.3.8 | CSS framework |
| Bootstrap Icons | 1.11.0 | Icon set |
| C# | 12.0 | Programming language |
| Rick and Morty API | v1 | Data source |

---
## Screenshots

### Character Gallery
![Gallery](docs/Screenshot(1).png)

### Filters
![Filters](docs/Screenshot(2).png)

### Navigation Menu
![Menu](docs/Screenshot%20(3).png)

---

## Functional Tests
**A file named “Tests” has been added to the “docs” folder, where you can find the functional tests for the project**

---

## 🔮 Future Improvements

- [ ] Implement score persistence (LocalStorage)
- [ ] Add a character details page where users can access a page with each character’s details, more photos of the character, and the episode(s) in which they appear
- [ ] Add dark mode
- [ ] Implement PWA (Progressive Web App)
- [ ] Make it responsive for all types of devices

---

## Author

**Juan Manuel Usuga Galeano**
