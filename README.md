# ContosoCrafts / SuperNova

A comprehensive ASP.NET Core Blazor e-commerce web application developed as a quarter-long project for **CPSC 5110: Fundamentals of Software Engineering** during Fall Quarter 2025 at Seattle University.

This project demonstrates modern web development practices, Agile methodologies, and rigorous software engineering principles while serving as a practical learning vehicle for team-based development.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Coding Standards](#coding-standards)
- [Testing Standards](#testing-standards)
- [Accomplishments](#accomplishments)
- [Agile Methodology](#agile-methodology)
- [Contributors](#contributors)
- [License](#license)

---

## Project Overview

ContosoCrafts/SuperNova is a full-featured e-commerce web application built using ASP.NET Core Blazor Server. The application showcases product management, advanced search capabilities, user interaction features, and a polished neumorphic user interface design.

This project was developed following strict academic coding standards and Agile development practices, including weekly sprints, Kanban boards, user stories following INVEST criteria, and retrospectives using the Drop/Add/Keep/Improve format.

---

## Features

### Core Functionality

- **Product CRUD Operations**: Complete Create, Read, Update, and Delete functionality for product management
- **Product Catalog**: Browse and view detailed product information with images and descriptions
- **Star Rating System**: Interactive 5-star rating system with real-time visual feedback
- **Comments System**: Add and view user comments on products

### Advanced Search

- **Multi-Filter Search**: Search products by multiple criteria simultaneously
- **Cascading Filter Dropdowns**: Dynamic filter options that update based on selections
- **Real-Time Results**: Instant search results as filters are applied

### User Experience

- **Dark Mode Toggle**: System-wide dark/light theme switching with persistence
- **Theme Persistence**: User theme preferences saved across sessions
- **Product Sharing**: Share product links via social media and direct links
- **Responsive Design**: Mobile-friendly layout using CSS Grid and Flexbox
- **Neumorphic UI**: Modern soft-shadow design aesthetic throughout the application

### Data Validation

- **URL Validation**: HTTP verification for product image URLs
- **Defensive Programming**: Comprehensive input validation and malformed data protection
- **Fast Fail Pattern**: Early validation and error handling throughout the codebase

---

## Technology Stack

### Frontend

- ASP.NET Core Blazor Server
- Bootstrap 5
- Custom Neumorphism CSS
- CSS Grid and Flexbox

### Backend

- ASP.NET Core 8.0
- C# 12
- JSON File Persistence
- IHttpClientFactory for HTTP Operations
- Dependency Injection

### Testing

- **NUnit 4.4.0** - Unit testing framework
- **bUnit 2.0.66** - Blazor component testing
- **Moq 4.29.72** - Mocking framework

---

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- Git

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/ContosoCrafts.git
   ```

2. Navigate to the project directory:

   ```bash
   cd ContosoCrafts
   ```

3. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

4. Build the solution:

   ```bash
   dotnet build
   ```

5. Run the application:

   ```bash
   dotnet run --project src/ContosoCrafts.WebSite
   ```

6. Open your browser and navigate to `https://localhost:5001`

### Running Tests

```bash
dotnet test
```

---

## Project Structure

```
ContosoCrafts/
├── src/
│   └── ContosoCrafts.WebSite/
│       ├── Components/
│       │   └── Pages/
│       ├── Models/
│       ├── Services/
│       ├── wwwroot/
│       │   ├── css/
│       │   │   └── neumorphism.css
│       │   └── data/
│       │       └── products.json
│       └── Program.cs
├── UnitTests/
│   ├── Components/
│   ├── Models/
│   ├── Services/
│   └── Pages/
└── README.md
```

---

## Coding Standards

The following coding standards were strictly enforced throughout development as established by the course professor.

### Documentation Standards

| Standard | Description |
|----------|-------------|
| XML Comments | `///` comments required on all classes and methods, including unit tests |
| Inline Comments | `//` comments required on all attributes, properties, and variables |
| Blank Lines | Blank line before each comment block |
| Closing Braces | Blank line after each `}` |
| Logic Grouping | Blank lines to separate logical groups for readability |
| File Endings | Files end with `}`, no trailing blank line |

### Code Structure Standards

| Standard | Description |
|----------|-------------|
| If Statements | All `if` statements must be wrapped in `{}` |
| Else Avoidance | `else` statements should be avoided; use early returns instead |
| Fast Fail Pattern | Validate inputs early and return/throw immediately on failure |
| Conditional Logic | Avoid `\|\|` or `&&` in conditional statements; use separate conditions |
| Negation Avoidance | Minimize use of negation (`!`) in conditions |
| Try-Catch Avoidance | Remove try-catch blocks; handle errors through validation |
| Using Statements | Remove all unused `using` statements |

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Models | Suffix with "Model" | `ProductModel` |
| Enums | Include values, 0 for undefined | `enum Status { Undefined = 0, Active = 1 }` |
| Methods | Action words, PascalCase | `GetAllProducts()`, `ValidateInput()` |
| Global Variables | PascalCase | `ProductService` |
| Local Variables | camelCase | `productList` |
| CSS Classes | Kebab-case with dashes | `product-card`, `dark-mode-toggle` |

### Programming Principles

| Principle | Description |
|-----------|-------------|
| Single Responsibility | Methods should do one and only one thing |
| Defensive Programming | Protect against malformed input and URLs |
| One Variable Per Line | Declare each variable on its own line |
| Enum Usage | Use Enums instead of strings or integers for categorical values |

### HTML/CSS Standards

| Standard | Description |
|----------|-------------|
| No Inline Styles | No style attributes in HTML except for background images in foreach loops |
| CSS Naming | Consistent, well-named classes using kebab-case |
| Separation of Concerns | All styling in CSS files, not in HTML |

---

## Testing Standards

### Unit Test Template

All unit tests follow this structured template:

```csharp
/// <summary>
/// Description of what the test validates
/// </summary>
[Test]
public void Method_Condition_State_Reason_Expected()
{
    // Arrange
    var data = // setup test data

    // Act
    var result = // execute method under test

    // Reset
    // cleanup if necessary

    // Assert
    Assert.That(result, Is.EqualTo(expectedValue));
}
```

### Naming Convention

Tests follow the pattern: `Method_Condition_State_Reason_Expected`

**Examples:**

- `GetAllProducts_Valid_Test_Default_Should_Return_Product_List`
- `AddRating_Invalid_Test_Null_ProductId_Should_Return_False`
- `UpdateProduct_Valid_Test_Existing_Product_Should_Return_True`
- `DeleteProduct_Invalid_Test_NonExistent_Id_Should_Return_False`

### Variable Standards

| Purpose | Variable Name |
|---------|---------------|
| Arrange | `var data = ...` |
| Act | `var result = ...` |

### Assertion Standards

- **Preferred**: `Assert.That()` with constraint syntax
- **Minimize**: Legacy assertion methods like `Assert.AreEqual()`

**Examples:**

```csharp
// Preferred
Assert.That(result, Is.Not.Null);
Assert.That(result.Count, Is.EqualTo(5));
Assert.That(result, Is.True);
Assert.That(result, Is.Empty);

// Avoid
Assert.IsNotNull(result);
Assert.AreEqual(5, result.Count);
Assert.IsTrue(result);
```

### Testing Packages

| Package | Version | Purpose |
|---------|---------|---------|
| NUnit | 4.4.0 | Unit testing framework |
| bUnit | 2.0.66 | Blazor component testing |
| Moq | 4.29.72 | Mocking dependencies |

---

## Accomplishments

### Code Quality

- **100% Code Coverage**: Achieved complete unit test coverage across all components, services, and models
- **Zero Code Smells**: Maintained clean code throughout development with no technical debt
- **Full Standards Compliance**: All code adheres to established academic coding standards

### Technical Achievements

- **Comprehensive Unit Testing**: Implemented systematic testing using NUnit, bUnit, and Moq
- **JavaScript Interop Testing**: Successfully tested Blazor JS interop functionality
- **HTTP Client Mocking**: Implemented sophisticated HTTP client mocking for URL validation tests
- **Blazor Component Testing**: Mastered complex component testing with threading considerations
- **Precise Coverage Measurement**: Systematically identified and covered all code paths

### Development Practices

- **Agile Methodology**: Completed weekly sprints with user stories following INVEST criteria
- **Kanban Board Management**: Maintained organized task tracking throughout the quarter
- **Retrospective Implementation**: Conducted regular Drop/Add/Keep/Improve retrospectives
- **Defensive Programming**: Implemented comprehensive input validation and error handling

### UI/UX Achievements

- **Neumorphic Design System**: Created cohesive soft-shadow UI aesthetic
- **Responsive Layout**: Fully responsive design using CSS Grid and Flexbox
- **Accessibility Compliance**: Implemented proper form labeling and ARIA attributes
- **Theme Persistence**: User preferences maintained across sessions

---

## Agile Methodology

This project was developed using Agile practices as taught in CPSC 5110:

### Sprint Structure

- Weekly sprints with defined goals and deliverables
- Sprint planning sessions to estimate and assign user stories
- Daily progress tracking via Kanban board

### User Stories

All features were developed from user stories following INVEST criteria:

- **I**ndependent
- **N**egotiable
- **V**aluable
- **E**stimable
- **S**mall
- **T**estable

### Retrospectives

Regular retrospectives using the Drop/Add/Keep/Improve format:

- **Drop**: Practices to eliminate
- **Add**: New practices to adopt
- **Keep**: Successful practices to continue
- **Improve**: Areas needing enhancement

### Reference Materials

- Primary Textbook: "Engineering Software Products" by Ian Sommerville
- Course materials covering software engineering concepts from prototyping through DevOps

---

## Contributors

| Name | GitHub | Role |
|------|--------|------|
| Nassuel | [GitHub Profile](https://github.com/nassuel) | Developer |
| Aarti Dashore | [GitHub Profile](https://github.com/AartiDashore) | Contributor |

---

## Course Information

**Course**: CPSC 5110 - Fundamentals of Software Engineering

**Institution**: Seattle University

**Quarter**: Fall 2025

**Instructor**: [Professor Mike Koenig](https://github.com/koenigm-seattleu)

**TAs**: [Chengfei Jiang](https://github.com/CJgracee) and [Young Kim](https://github.com/youngwasd)

---

## License

This project was developed for educational purposes as part of CPSC 5110 at Seattle University.

---

## Acknowledgments

- Seattle University Computer Science Department
- CPSC 5110 Course Staff
- All contributors and classmates who provided feedback and support

---

*This README was last updated: January 2026*