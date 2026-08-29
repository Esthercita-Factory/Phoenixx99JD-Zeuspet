# Phoenixx99JD-Zeuspet · ZeusPet Veterinary Clinic 🐾

Management system for a **veterinary clinic** in which the **patient is the pet** and the **Client is its owner**. An owner can have one or several pets.

The project has **two versions** that share the same business logic:

| Version | Project | Type | Description |
|---------|---------|------|-------------|
| **Console** | `Phoenixx99JD-Zeuspet` | .NET console application | The original course version, with clients and pets CRUD. |
| **Web** | `Phoenixx99JD-Zeuspet.Web` | Interactive Blazor Server + Web API | The full evolution with a web interface, roles, schedules, community and statistics. |

> Both versions are written in **C#** on **.NET 10**. The Web version reuses and extends the data model and the `VeterinariaService` of the console version.

---

## 📁 Repository structure

```
Phoenixx99JD-Zeuspet/            ← Console version (.NET console)
├── Models/                      ← Animal, Cliente, Mascota, ServicioVeterinario, IRegistrable
├── Services/                    ← VeterinariaService, GeneradorId
├── UI/                          ← ConsolaUI (terminal menus and tables)
└── Program.cs                   ← Main menu entry point

Phoenixx99JD-Zeuspet.Web/        ← Web version (Blazor Server + Web API)
├── Components/
│   ├── Layout/                  ← MainLayout, ZeusNavbar
│   ├── Pages/                   ← +10 pages (see routes section below)
│   └── Shared/                  ← ZeusModal, RolRequerido, BarraEcualizador
├── Controllers/                 ← REST API: clients, pets, services, statistics
├── Models/                      ← Extended domain models
├── Services/                    ← VeterinariaService, SesionService
└── Program.cs                   ← Web application configuration and startup

Phoenixx99JD-Zeuspet.Tests/      ← Console logic tests (xUnit)
Phoenixx99JD-Zeuspet.Web.Tests/  ← Web logic tests (xUnit)
docs/DiagramaUML.md              ← UML diagram (Mermaid) of the classes
```

---

## ⚙️ Prerequisites

- **.NET SDK 10** or higher → https://dotnet.microsoft.com/download/dotnet/10.0
- Optional: JetBrains Rider, Visual Studio or Visual Studio Code (for development)

You can verify your installation with:

```bash
dotnet --version
```

---

## 🖥️ Console version

The original version focused on learning **object-oriented programming** and **LINQ** in C#.

### Features

- **Client management** (CRUD): add, list, search by name, update and delete.
- **Pet management** (CRUD): add, list all or by client, update and delete.
- **Encapsulation** in the properties of the classes (`Animal`, `Cliente`, `Mascota`).
- **Inheritance**: `Mascota` inherits from `Animal`.
- **Polymorphism**: `EmitirSonido()` varies according to the species.
- **Abstraction**: `ServicioVeterinario` with `ConsultaGeneral` and `Vacunacion`.
- **Interface** `IRegistrable` implemented by `Cliente` and `Mascota`.
- **Asynchronous programming**: methods using `async`/`await`, `Task`, `Task.WhenAll` and `Task.WhenAny` (for example, exporting and generating clinic reports).
- Loads sample data on startup ("Zeus", "Luna", "Rocky", etc.).

### How to run it

From the repository root:

```bash
dotnet run --project Phoenixx99JD-Zeuspet/Phoenixx99JD-Zeuspet.csproj
```

Or, entering the project folder:

```bash
cd Phoenixx99JD-Zeuspet
dotnet run
```

On startup a menu is shown in the terminal:

```
=== CLINICA VETERINARIA ZEUSPET ===

  [1] Gestionar Clientes
  [2] Gestionar Mascotas
  [3] Salir

  Opcion:
```

### How to run its tests

```bash
dotnet test Phoenixx99JD-Zeuspet.Tests/Phoenixx99JD-Zeuspet.Tests.csproj
```

---

## 🌐 Web version

Evolution of the project with an **interactive web interface (Blazor Server)** and a **REST API** built on the same business logic.

### Features

- **Role-based sign in**: `Usuario` (pet owner) and `Veterinario`.
- **Dashboard** with a general summary of the clinic.
- **Client** and **pet** management (clinical record, weight, notes, photo).
- **Appointments schedule**: request, confirm, reject and mark as completed.
- **Activities** and **reminders** per pet.
- **Quality of life evaluations** with tracking (timeline).
- **ZeusPet Community**: posts, likes and comments.
- **Reports** and clinic **statistics**.
- **REST API** under `/api/*` (clients, pets, services, statistics).

### Main routes

| Route | Page |
|-------|------|
| `/` | Welcome / role selection |
| `/dashboard` | Main panel |
| `/clientes` | Client management |
| `/mascotas` | Pet management |
| `/agenda` | Appointments schedule |
| `/actividades` | Activities and routine |
| `/evaluaciones` | Wellbeing evaluations |
| `/comunidad` | ZeusPet Community |
| `/reportes` | Reports and statistics |

### How to run it

From the repository root:

```bash
dotnet run --project Phoenixx99JD-Zeuspet.Web/Phoenixx99JD-Zeuspet.Web.csproj
```

Or:

```bash
cd Phoenixx99JD-Zeuspet.Web
dotnet run
```

Then open in the browser the URL shown in the console (usually `http://localhost:5000` or `https://localhost:5001`). Choose your role on the welcome page to sign in.

> The data is sample data loaded in memory when the application starts (it does not persist between restarts).

### How to run its tests

```bash
dotnet test Phoenixx99JD-Zeuspet.Web.Tests/Phoenixx99JD-Zeuspet.Web.Tests.csproj
```

---

## 🧪 Run all the repository tests

```bash
dotnet test
```

---

## 📌 Technologies

- **C#** / **.NET 10**
- **LINQ** (method and query syntax)
- **Object-oriented programming** (inheritance, abstraction, polymorphism, encapsulation)
- **Asynchronous programming** (`async`/`await`, `Task`)
- **Blazor Server** (server-side interactivity)
- **ASP.NET Core Web API** (REST controllers)
- **xUnit** (testing)

---

## 📚 Documentation

- `docs/DiagramaUML.md` — Class diagram (Mermaid) and relationships of the domain model.

---

> Learning project. The console version is the original course base and the web version is its evolution with a graphical interface.
