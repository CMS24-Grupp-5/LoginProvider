# 🔐 gRPC Login Provider – .NET

This project implements a **gRPC-based login provider** for user authentication, built using ASP.NET Core and gRPC. It defines a clean service interface for validating user credentials and returning standardized login responses.

---

## 📦 Features

* ✅ `LoginController` HTTP endpoint for user login
* 🔁 gRPC integration with `AccountGrpcService`
* 🔐 Email + Password validation via gRPC
* 📄 Model-based input/output with validation

---

## 🚀 Quick Overview

### ✅ Endpoint

`POST /api/login/login`

#### Request Body:

```json
{
  "email": "user@example.com",
  "password": "YourPassword123"
}
```

#### Successful Response:

```json
{
  "success": true,
  "message": "Login successful",
  "userId": "abc123"
}
```

#### Failure Response:

```json
{
  "success": false,
  "message": "Invalid credentials"
}
```

---



### 🧾 Models

* `LoginForm`: Required email and password
* `LoginResult`: Result status, message, and optional `UserId`

---
