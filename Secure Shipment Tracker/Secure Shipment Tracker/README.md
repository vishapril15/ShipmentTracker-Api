# Shipment Tracking System

A secure ASP.NET Core Web API that manages shipments with JWT authentication, role-based authorization (Admin/Staff), and status transition validation.

## Features
- JWT Authentication
- Role-Based Authorization
- Shipment Status Validation
- SQL Server Database
- Clean Layered Architecture

## Technologies Used
- ASP.NET WEB API
- SQL Server Express / LocalDB
- Entity Framework Core
- JWT Bearer Authentication

 ##  Role-Based Access Rules

| Role   | Permissions |
|--------|------------|
| Admin  | Create Shipment, Update Shipment Status, View All Shipments, Register Users |
| Staff  | Create Shipment, View Shipments, Cannot mark shipment as Delivered |


##  Api EndPoints

### 🔐 Authentication
**Register**
- ***POST*** `/api/Auth/register`
- Access : Admin

**Login**
- ***POST*** `/api/Auth/login`
-  **Access** : Public
## 📦 Shipments

**Create Shipment**
- ***POST*** `/api/Shipment/createShipment`
-  **Access**  : Admin,Staff

**Get All Shipments**
- ***GET*** `/api/Shipment/GetAllShipments`
-   **Access** : Admin 

**Get Staff Shipments**
- ***GET*** `/api/Shipment/GetAllShipmentsByStaff`
-  **Access** : Staff

**Update Shipment Status**
- ***PUT*** `/api/Shipment/UpdateShipmentStatus`
-  **Access** : Admin,Staff
- **Note:** Staff cannot update status to Delivered
## Shipment Status Flow

Created → Dispatched  
Dispatched → InTransit  
InTransit → Delivered  
Invalid transitions not allowed
## Security Practices
- Password hashing (no plain text passwords)  
- JWT Authentication  
- Role-Based Authorization  
- Business rule validation in Service layer  
- Proper HTTP status codes 
