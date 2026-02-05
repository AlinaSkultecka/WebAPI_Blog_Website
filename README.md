# Lab 2 – Community Web API

## 📌 Description
This project is **Lab 2** in the .NET Cloud Development course.  
The task was to build a **RESTful Web API** for a simple community where users can create blog posts, comment on posts, and search content. All data is stored in a database and accessed using **ASP.NET Web API** and **Entity Framework Core**.

---

## 🛠 Technologies
- ASP.NET Core Web API  
- C#  
- Entity Framework Core  
- SQL Database  
- JWT Authentication  
- Postman  

---

## 📂 Features
- User registration, login (JWT), update, and delete
- Logged-in users can create, update, and delete their own posts
- Posts include title, text, and category
- Categories are stored in a separate table
- Logged-in users can comment on other users’ posts (not their own)
- Search posts by title or category
- Authorization ensures users can only modify their own content

---

## 🧪 Testing
All endpoints are tested using **Postman**.  
Protected endpoints require a valid JWT token. Swagger is included for API documentation.

---

## ▶️ Run Instructions
1. Clone the repository  
2. Open in Visual Studio  
3. Configure database connection in `appsettings.json`  
4. Run EF migrations  
5. Start the application and test with Postman

---

## ✅ Result
The project meets all requirements for **Lab 2**, including object-oriented design, database integration, authentication, and RESTful API principles.
