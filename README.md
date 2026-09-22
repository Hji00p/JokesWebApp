# 🎭 Jokes & Riddles Web App

A full-stack ASP.NET Core MVC web application designed for sharing, managing, and browsing jokes and riddles with built-in authentication and author-level security.

---

## ✨ Features

- **Full CRUD Operations:** Create, view, edit, and delete jokes.
- **Spoiler-Free Punchlines:** The answers and punchlines are hidden in the main overview table and revealed only on demand via the details page.
- **Search Engine:** Filter jokes by keywords through a dedicated search interface (`ShowSearchForm` / `ShowSearchResults`).
- **User Authentication:** Built with ASP.NET Core Identity (user registration, login, session management).
- **Author-Based Access Control:**
  - Authenticated users are automatically assigned as the author of their submissions.
  - Only the original author can edit or delete their own jokes (enforced via UI checks and server-side controller guards).
  - Unauthenticated visitors have read-only access.
- **Custom Dark UI:** Clean, modern dark theme built on top of Bootstrap 5.

---

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core MVC (.NET 8 / .NET 9)
- **Database & ORM:** Entity Framework Core, SQL Server / LocalDB
- **Authentication:** ASP.NET Core Identity
- **Frontend:** Razor Views (C#), Bootstrap 5, Custom CSS
