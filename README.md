# ✂️ GroomMate —  Men's Salon Appointment Platform

GroomMate is a modern, responsive, full-stack appointment booking and management platform built specifically for men's grooming salons. The system replaces traditional phone or walk-in booking methods with a streamlined digital portal for customers, staff, and administrators.

---

## 🌟 Core Features

### 👤 Customer Portal
* **Register & Login**: Custom Forms Authentication with validation rules.
* **Service Catalog**: Browse 9+ premium grooming services with prices, descriptions, and dynamic icons.
* **Customer Reviews**: View real customer feedback, ratings, and comment snippets for each service.
* **Interactive Booking**: Book slots using a date-time picker. Includes validation to block bookings on Tuesdays (closed days) and outside working hours (9:00 AM – 10:00 PM).
* **My Dashboard**: Track upcoming appointments, view pricing, cancel pending bookings, and leave star-ratings and comments on completed services.

### 💈 Staff Dashboard
* **My Schedule**: Log in as a stylist to view assigned appointments in real time.
* **Complete Service**: Mark active sessions as completed, making them eligible for customer feedback.

### 👑 Admin Dashboard
* **Full Overview**: View all appointments (upcoming, completed, and cancelled) in a unified dashboard.
* **Staff Assignment**: Dynamically assign or reassign stylists to pending bookings via live dropdowns.
* **Service Completion Tracking**: A dedicated column to see exactly when stylists completed and marked off services.
* **Customer Feedback Feed**: Monitor all customer ratings and feedback in a separate table.

### ✉️ Notification System
* **Automated Emails**: Sends instant confirmation and cancellation emails to customers.
* **Gmail SMTP Integration**: Configured to send emails directly to user inbox using Google App Passwords.
* **Local Pickup Directory**: Falls back to writing local `.eml` files on disk during offline testing.

---

## 🛠️ Technology Stack

* **Framework**: ASP.NET MVC 5 (.NET Framework 4.7.2)
* **Language**: C# 7.3
* **Database & ORM**: SQL Server + Entity Framework 6 (Code-First approach with migrations)
* **Frontend UI**: Bootstrap 5.2.3, Bootstrap Icons 1.10.5, Google Fonts (*Outfit* and *Playfair Display*)
* **Styles**: Custom Vanilla CSS (`Content/Site-Custom.css`)
* **Client Scripts**: jQuery 3.7.0 & jQuery Validation

---

## 📊 Database Schema & Relationships

The database utilizes Entity Framework 6 Code-First with the following main tables:

```
[Roles] (RoleID, RoleName)
   │
   └── (1-to-many) ──> [Users] (UserID, Username, Password, Email, FullName, RoleID, IsDeleted)
                          │
                          └── (1-to-many) ──> [Appointments] <── (many-to-1) ── [Services]
                                                     │
                                                     └── (1-to-1) ──> [Feedbacks] (Rating, Comments)
```

1. **`Roles`**: Seeds default roles (`Admin`, `Staff`, `Customer`).
2. **`Users`**: Enforces unique username checks and password matching.
3. **`Services`**: Stores salon catalog data (e.g. Price, IsActive, Description).
4. **`Appointments`**: Tracks time, status (`Pending`, `Confirmed`, `Completed`, `Cancelled`), customer ID, and assigned staff ID.
5. **`Feedbacks`**: Stores customer reviews linked to a completed appointment.

---

## 🚀 Quick Setup & Installation

### Prerequisites
* Windows OS
* Visual Studio 2022 (with ASP.NET and web development workload)
* Local SQL Server Express instance
* IIS Express

### Installation Steps

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/Balrajgopi/GroomMate.git
   cd GroomMate
   ```

2. **Configure Connection String**:
   Open `Web.config` and configure your database server name in `connectionStrings`:
   ```xml
   <connectionStrings>
       <add name="GroomMateConnectionString" 
            connectionString="Server=YOUR_SERVER_NAME;Database=GroomMateDB_New;Trusted_Connection=True;TrustServerCertificate=True;" 
            providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

3. **Database Migration & Seeding**:
   Open the **Package Manager Console** in Visual Studio and run:
   ```powershell
   Update-Database
   ```
   This automatically creates the database structure and seeds the initial services, roles, and default users.

4. **Build & Run**:
   * Restore NuGet packages.
   * Run the project using **IIS Express** (Port `54713` by default).

---

## ✉️ SMTP Email Setup

To enable real email notifications to Gmail, follow these steps:

1. Turn on **2-Step Verification** on your Google Account.
2. Visit **App Passwords** in your account security settings and generate a password for GroomMate.
3. Paste the credentials into `Web.config` inside `<appSettings>`:
   ```xml
   <add key="SmtpHost" value="smtp.gmail.com" />
   <add key="SmtpPort" value="587" />
   <add key="SmtpEnableSsl" value="true" />
   <add key="SmtpUsername" value="your-email@gmail.com" />
   <add key="SmtpPassword" value="your-16-character-app-password" />
   <add key="SmtpFromEmail" value="your-email@gmail.com" />
   ```
   *Note: If `SmtpHost` is left blank, the application will default to saving `.eml` logs inside the `emails/` directory on disk.*

---

## 👨‍💻 About the Creator

GroomMate was developed by **Balraj Gopi** as a showcase of premium ASP.NET MVC 5 application design, role-based authorization, and automated transaction services.

* **LinkedIn**: [linkedin.com/in/balraj-gopi](https://www.linkedin.com/in/balraj-gopi)
* **GitHub**: [github.com/Balrajgopi](https://github.com/Balrajgopi)
* **Email (Gmail)**: Balrajgopi0000@gmail.com
* **Email (Outlook)**: balrajgopi2005@outlook.com

---
*Developed with passion and precision. Feel free to clone, star, or contribute!*
