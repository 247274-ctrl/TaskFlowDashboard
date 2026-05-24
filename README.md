# Application 8 Documentation

# TaskFlow Dashboard with Database Integration using Blazor and SQL Server

---

# Introduction

The **TaskFlow Dashboard** is a professional Blazor web application integrated with a SQL Server database using **Entity Framework Core**.

This application demonstrates:

- CRUD Operations
- Database Connectivity
- Entity Framework Core
- Real-Time UI Updates
- State Management
- Data Persistence
- Interactive Dashboard Design
- SQL Server Integration

The system allows users to:

- Add tasks
- Edit tasks
- Delete tasks
- Clear all tasks
- Store tasks permanently in database

All changes performed from the browser are automatically reflected inside the SQL Server database.

---

# Problem Statement

Connect the ToDo Blazor application with SQL Server database so that:

- All tasks are stored in database
- Add, Update, Delete, and Clear operations update database
- Database changes reflect instantly in UI
- Application demonstrates full CRUD functionality

---

# Objectives

The objectives of this application are:

- To connect Blazor with SQL Server
- To implement Entity Framework Core
- To perform CRUD operations
- To synchronize UI with database
- To understand DbContext architecture
- To implement persistent task storage

---

# Technologies Used

| Technology | Purpose |
|---|---|
| ASP.NET Core Blazor | Frontend Framework |
| Razor Components | UI Development |
| C# | Backend Logic |
| SQL Server | Database |
| Entity Framework Core | ORM |
| LINQ | Database Queries |
| Bootstrap | UI Styling |

---

# Project Architecture

| Layer | Purpose |
|---|---|
| UI Layer | Displays dashboard and user interaction |
| Model Layer | Defines database entity |
| Data Layer | Handles database connection |
| Database Layer | Stores tasks permanently |

---

# Database Information

| Database Name | TodoDB |
|---|---|
| Table Name | Tasks |
| Database Type | SQL Server |
| ORM Used | Entity Framework Core |

---

# SQL Query Used

## Code

```sql
USE TodoDB;
GO

SELECT * FROM dbo.Tasks;
```

---

# Explanation

### Purpose

This query is used to:

- Open TodoDB database
- Display all records from Tasks table

### Output

Shows all tasks currently stored in SQL Server database.

---

# Database Connection Screenshot

<img width="975" height="888" alt="image" src="https://github.com/user-attachments/assets/882216e2-8cd1-42b3-beac-d1c8ebbe733b" />


---
# Packages Installed(Database updated)

<img width="975" height="987" alt="image" src="https://github.com/user-attachments/assets/398fd138-2625-455d-aeb3-18663b0c2254" />

---
# Database Table Screenshot
<img width="975" height="789" alt="image" src="https://github.com/user-attachments/assets/bf4c95a7-dfa0-4a19-a5c4-f36756f34a02" />


---

# SQL Query Output Screenshot

<img width="1405" height="657" alt="image" src="https://github.com/user-attachments/assets/6e7444e6-a554-409c-bf1e-fea56d5d0df8" />


---

# Model Class

# Complete Code

```csharp
using System.ComponentModel.DataAnnotations;

namespace TaskFlowDashboard.Models
{
    public class TodoTask
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
```

---

# TodoTask Model Explanation

## Purpose

This class represents the structure of the Tasks table in database.

---

# Properties Explanation

| Property | Type | Purpose |
|---|---|---|
| Id | int | Primary key |
| Title | string | Stores task title |
| IsCompleted | bool | Stores completion status |

---

# Required Validation

## Code

```csharp
[Required]
public string Title { get; set; }
```

---

## Explanation

### Purpose

Ensures task title cannot be empty.

### Importance

Prevents invalid task records from being stored in database.

---

# Database Context Class

# Complete Code

```csharp
using Microsoft.EntityFrameworkCore;
using TaskFlowDashboard.Models;

namespace TaskFlowDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoTask> Tasks { get; set; }
    }
}
```

---

# AppDbContext Explanation

## Purpose

Acts as bridge between application and SQL Server database.

---

# DbSet Property

## Code

```csharp
public DbSet<TodoTask> Tasks { get; set; }
```

---

## Explanation

### Functionality

Maps TodoTask model with Tasks database table.

### Concept

Entity Framework automatically creates communication between:

Model ↔ Database Table

---

# Database Context Factory

# Complete Code

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TaskFlowDashboard.Data;

namespace TaskFlowDashboard.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLExpress;Database=TodoDB;Trusted_Connection=True;TrustServerCertificate=True;");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
```

---

# AppDbContextFactory Explanation

## Purpose

Provides database connection configuration during development and migrations.

---

# SQL Server Connection String

## Code

```csharp
optionsBuilder.UseSqlServer(
"Server=localhost\\SQLExpress;Database=TodoDB;Trusted_Connection=True;TrustServerCertificate=True;");
```

---

## Explanation

### Connection Details

| Setting | Purpose |
|---|---|
| Server | SQL Server instance |
| Database | TodoDB database |
| Trusted_Connection | Windows Authentication |
| TrustServerCertificate | Allows secure local connection |

---

# Todo Dashboard Component

# Complete Code

```razor
@page "/todo-dashboard"
@rendermode InteractiveServer

@using TaskFlowDashboard.Models
@using TaskFlowDashboard.Data
@inject AppDbContext db

<PageTitle>TaskFlow Dashboard</PageTitle>

<div class="dashboard-wrapper">

    <div class="dashboard-header">
        <h1>TaskFlow Dashboard</h1>
        <p>Manage your tasks efficiently and stay productive.</p>
    </div>

    <!-- ADD TASK -->
    <div class="task-input-section">

        <InputText @bind-Value="newTask"
                   placeholder="Enter your task..."
                   class="task-input" />

        <button class="add-task-btn" @onclick="AddTask">
            Add Task
        </button>

        <button class="btn btn-danger"
                @onclick="ClearAllTasks">
            Clear All
        </button>

    </div>

    <!-- TASK LIST -->
    <div class="task-list">

        @if (tasks.Count == 0)
        {
            <div class="empty-state">
                No tasks available
            </div>
        }
        else
        {
            @foreach (var task in tasks)
            {
                <div class="task-item">

                    @if (editingTaskId == task.Id)
                    {
                        <InputText @bind-Value="editText"
                                   class="task-input" />

                        <div class="action-buttons">

                            <button class="btn-save"
                                    @onclick="() => UpdateTask(task.Id)">
                                Save
                            </button>

                            <button class="btn-cancel"
                                    @onclick="CancelEdit">
                                Cancel
                            </button>

                        </div>
                    }
                    else
                    {
                        <span class="task-title">@task.Title</span>

                        <div class="action-buttons">

                            <button class="btn-edit"
                                    @onclick="() => StartEdit(task)">
                                Edit
                            </button>

                            <button class="btn-delete"
                                    @onclick="() => DeleteTask(task.Id)">
                                Delete
                            </button>

                        </div>
                    }

                </div>
            }
        }

    </div>

</div>

@code {

    private string newTask = "";
    private List<TodoTask> tasks = new();

    private int? editingTaskId = null;
    private string editText = "";

    protected override void OnInitialized()
    {
        LoadTasks();
    }

    private void LoadTasks()
    {
        tasks = db.Tasks.ToList();
    }

    private void AddTask()
    {
        if (!string.IsNullOrWhiteSpace(newTask))
        {
            db.Tasks.Add(new TodoTask
                {
                    Title = newTask,
                    IsCompleted = false
                });

            db.SaveChanges();

            newTask = "";
            LoadTasks();
        }
    }

    private void DeleteTask(int id)
    {
        var task = db.Tasks.Find(id);

        if (task != null)
        {
            db.Tasks.Remove(task);
            db.SaveChanges();
            LoadTasks();
        }
    }

    private void StartEdit(TodoTask task)
    {
        editingTaskId = task.Id;
        editText = task.Title;
    }

    private void CancelEdit()
    {
        editingTaskId = null;
        editText = "";
    }

    private void UpdateTask(int id)
    {
        var task = db.Tasks.Find(id);

        if (task != null)
        {
            task.Title = editText;
            db.SaveChanges();

            editingTaskId = null;
            editText = "";

            LoadTasks();
        }
    }

    private void ClearAllTasks()
    {
        var allTasks = db.Tasks.ToList();

        db.Tasks.RemoveRange(allTasks);
        db.SaveChanges();

        LoadTasks();
    }
}
```

---

# Todo Dashboard Explanation

## Purpose

This component manages the complete task system and performs all database operations.

---

# Dependency Injection

## Code

```razor
@inject AppDbContext db
```

---

## Explanation

### Purpose

Injects database context into component.

### Functionality

Allows component to directly interact with SQL Server database.

---

# State Variables

## Code

```csharp
private string newTask = "";
private List<TodoTask> tasks = new();

private int? editingTaskId = null;
private string editText = "";
```

---

## Explanation

| Variable | Purpose |
|---|---|
| newTask | Stores new task input |
| tasks | Stores task list |
| editingTaskId | Tracks editing task |
| editText | Stores edited text |

---

# Loading Data from Database

## Code

```csharp
private void LoadTasks()
{
    tasks = db.Tasks.ToList();
}
```

---

## Explanation

### Purpose

Fetches all tasks from database.

### Flow

Database  
↓  
Entity Framework  
↓  
List<TodoTask>  
↓  
UI Rendering

---

# Add Task Functionality

## Code

```csharp
private void AddTask()
{
    if (!string.IsNullOrWhiteSpace(newTask))
    {
        db.Tasks.Add(new TodoTask
        {
            Title = newTask,
            IsCompleted = false
        });

        db.SaveChanges();

        newTask = "";
        LoadTasks();
    }
}
```

---

# Add Task Explanation

## Step-by-Step Flow

1. User enters task
2. Add button clicked
3. Validation checks empty input
4. New object created
5. Data inserted into database
6. SaveChanges() commits data
7. UI refreshes automatically

---

# Delete Task Functionality

## Code

```csharp
private void DeleteTask(int id)
{
    var task = db.Tasks.Find(id);

    if (task != null)
    {
        db.Tasks.Remove(task);
        db.SaveChanges();
        LoadTasks();
    }
}
```

---

# Delete Task Explanation

## Functionality

- Finds task using ID
- Removes task from database
- Updates UI automatically

---

# Edit Task Functionality

# Start Edit

## Code

```csharp
private void StartEdit(TodoTask task)
{
    editingTaskId = task.Id;
    editText = task.Title;
}
```

---

## Explanation

Enables edit mode for selected task.

---

# Update Task

## Code

```csharp
private void UpdateTask(int id)
{
    var task = db.Tasks.Find(id);

    if (task != null)
    {
        task.Title = editText;
        db.SaveChanges();

        editingTaskId = null;
        editText = "";

        LoadTasks();
    }
}
```

---

## Explanation

### Functionality

- Finds task in database
- Updates task title
- Saves changes
- Refreshes UI

---

# Clear All Functionality

## Code

```csharp
private void ClearAllTasks()
{
    var allTasks = db.Tasks.ToList();

    db.Tasks.RemoveRange(allTasks);
    db.SaveChanges();

    LoadTasks();
}
```

---

## Explanation

### Purpose

Deletes all tasks from database.

### Flow

Get All Tasks  
↓  
RemoveRange()  
↓  
SaveChanges()  
↓  
Database Updated  
↓  
UI Refreshed

---

# CRUD Operations Summary

| Operation | Database Action |
|---|---|
| Create | INSERT |
| Read | SELECT |
| Update | UPDATE |
| Delete | DELETE |

---

# Entity Framework Workflow

```text
Blazor Component
       ↓
DbContext
       ↓
Entity Framework Core
       ↓
SQL Server Database
       ↓
Tasks Table
```

---

# Application Workflow

## Step 1

Application loads existing tasks from database.

---

## Step 2

User adds task from browser.

---

## Step 3

Task stored in SQL Server database.

---

## Step 4

Updated task list displayed automatically.

---

## Step 5

User edits or deletes task.

---

## Step 6

Database updates instantly.

---

## Step 7

UI reflects latest database state.

---

# Output Screenshots

# Application Home Output

<img width="1782" height="855" alt="image" src="https://github.com/user-attachments/assets/845d53e7-15c5-4e93-9d07-ebf67ced5340" />


---
# Dashboard
<img width="1882" height="848" alt="image" src="https://github.com/user-attachments/assets/38e685ea-f70e-4e3c-93a6-f37b55c6ffd8" />

---
# Add Task Output

<img width="1886" height="848" alt="image" src="https://github.com/user-attachments/assets/3b89b958-1004-4810-96c3-b2fb2c8e2172" />


---

# Edit Task Output

<img width="1896" height="858" alt="image" src="https://github.com/user-attachments/assets/6857974b-c680-496d-bf7d-ab09365842af" />


---

# Delete Task Output

<img width="1890" height="848" alt="image" src="https://github.com/user-attachments/assets/881c61d4-8bc0-4e96-865b-465f022b4fe1" />


---

# Clear All Output

<img width="1885" height="857" alt="image" src="https://github.com/user-attachments/assets/444c74e6-171b-4966-b254-291b0381b4be" />


---

# Database Connection Output

<img width="975" height="888" alt="image" src="https://github.com/user-attachments/assets/9d5810e3-5c2d-4859-9d02-e1182bae6b9b" />


---


# Tasks Table When new task(software asg added)

<img width="975" height="743" alt="image" src="https://github.com/user-attachments/assets/5b0fc69d-84b0-4a5d-9d5d-322369b1aecd" />


---

# Concepts Implemented

| Concept | Description |
|---|---|
| Entity Framework Core | ORM for database operations |
| SQL Server Integration | Database connectivity |
| CRUD Operations | Create, Read, Update, Delete |
| Dependency Injection | Injecting DbContext |
| Data Persistence | Permanent data storage |
| Interactive UI | Dynamic frontend |
| State Management | UI synchronization |
| Two-Way Binding | Input synchronization |

---

# Advantages of the Application

- Real database integration
- Persistent task storage
- Professional architecture
- Real-time updates
- Clean UI workflow
- Full CRUD support
- Scalable structure

---

# Conclusion

The TaskFlow Dashboard successfully demonstrates:

- Blazor and SQL Server integration
- Entity Framework Core implementation
- Full CRUD operations
- Real-time database synchronization
- Persistent data management
- Professional dashboard architecture

This application represents a complete full-stack Blazor project where frontend UI and backend database work together dynamically in real time.
