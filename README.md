# E-Commerce Inventory Management System

## Project Overview
Create a RESTful web API that manages product inventory for an online store. The system should handle user orders, inventory updates, and loyalty program management.

## Core Features

### 1. user Management
**Registration & Login:**
- users can create accounts and authenticate
- users should be able to access their account from multiple devices simultaneously (phone, laptop, tablet)
- When logging out, users can choose to:
  - Log out from current device only
  - Log out from all devices

**Example:** Anna logs in on her phone and laptop. She can work on both devices. When she calls logout endpoint on her phone, she should be able to specify logout scope (current device or all devices).

### 2. Product Management
**Inventory System:**
- Products have stock quantities
- Multiple users can try to purchase the same product simultaneously
- System must correctly handle multiple users attempting to purchase the final available items of a product at the same time.
- Stock levels must remain accurate even under heavy concurrent access
- *Important:* Database constraints (like CHECK constraints on stock quantities) are forbidden - you must handle concurrency through application-level mechanisms

**Example:** iPhone has 3 units in stock. At exactly 2:00 PM, 50 users call the purchase API for this iPhone. Only 3 orders should succeed, 47 should receive "out of stock" response.

### 3. Order Processing with Payment
**Payment Integration:**
Use this payment method in your order processing:
```csharp
public async Task<bool> ProcessPaymentAsync(decimal amount)
{
    // Simulate payment processing time
    await Task.Delay(TimeSpan.FromMinutes(2));
    
    // Simulate 90% success rate
    return Random.Shared.NextDouble() > 0.1;
}
```

**Order Workflow:**
- users can place orders for products
- Each order must process payment using the provided method
- Handle the 2-minute payment processing appropriately
- System should remain responsive and handle multiple concurrent payments

**Example:** user places order for $100 item. Payment processing starts and takes 2 minutes. During this time, other users should be able to place orders without being blocked.

### 4. Loyalty Program
**Event Publishing System:**
- users earn virtual coins for completed orders
- The Loyalty Program service is owned by another team - you do NOT need to implement it
- Your system must publish events when users complete orders
- These events will be consumed by the external Loyalty Program service
- Event publishing must be reliable - events cannot be lost even during system failures or database issues

**Example:** When John completes a $50 order, your system must publish an "OrderCompleted" event. This event publishing must be guaranteed to succeed even if the external Loyalty Program service is temporarily unavailable or if there are database transaction failures.

### 5. Discount System
**Discount Cards:**
- Support multiple types of discount cards (Percentage, Fixed Amount)
- Each card type has different calculation rules
- System should easily support adding new discount card types
- Cards can be applied during order checkout

**Example:**
- Silver Card: 10% discount on orders
- Gold Card: $20 off orders over $100
- Future: VIP Card with complex tiered discounts

## Technical Requirements

### Mandatory Features
- OpenAPI/Swagger documentation - fully documented API
- Docker deployment - complete application runnable with `docker-compose up`
- No pre-built authentication libraries (no Identity, no JWT libraries)
- Database with proper schema and migrations

### Performance Requirements
- System should handle 1000+ concurrent users
- Database operations must remain consistent under heavy load
- Payment processing should not block other operations
- All transactions must be reliable and atomic

## Expected Scenarios

**Scenario 1 - Concurrent Orders:**
At Black Friday 12:00 AM, 500 users simultaneously try to buy limited edition items. System should handle this gracefully without overselling or data corruption.

**Scenario 2 - Payment Processing:**
Multiple users start payment processing simultaneously. Each payment takes 2 minutes. System should handle all payments concurrently without blocking.

**Scenario 3 - Multi-device Usage:**
Business owner authenticates from office computer and mobile app. Both sessions should work independently, with option to terminate sessions selectively.

**Scenario 4 - Event Publishing Reliability:**
During high traffic, all completed orders must publish events reliably to the external Loyalty Program service, even if that service is slow, temporarily unavailable, or if database transactions fail.

## Deliverables

### Required Files
- Complete .NET Web API project
- docker-compose.yml with all services (API + Database)
- Dockerfile for the application
- Database migrations/setup scripts
- README.md with setup instructions

### API Requirements
- RESTful API design
- Complete OpenAPI/Swagger documentation
- Proper HTTP status codes and error handling
- Authentication and authorization endpoints

## Setup Requirements
The project must be runnable with these commands:
```bash
git clone [your-repository]
cd [project-folder]
docker-compose up
```

## Evaluation Criteria
- **Security:** Proper credential handling and session management
- **Concurrency:** Correct handling of simultaneous operations and race conditions
- **Reliability:** System resilience and data consistency
- **Architecture:** Clean code structure and extensible design
- **API Design:** Well-designed RESTful endpoints with proper documentation
- **Deployment:** Working Docker setup with all dependencies

## Submission
- You will be provided with a private GitHub repository
- Include all source code and configuration files in a Visual Studio solution
- Provide clear README with API usage examples
- Ensure `docker-compose up` works on fresh clone

---

Build a production-ready API that demonstrates your understanding of web service fundamentals, security, concurrency, and system reliability.

# 📦 EcommerceInventory API

EcommerceInventory is a sample **e-commerce backend** built with **.NET 8**, **PostgreSQL**, and **RabbitMQ**.  
It supports user authentication, product management, discounts, and order processing.

---

## 🚀 Getting Started

### Run with Docker Compose
```bash
docker-compose up --build
```

The API will be available at:  
👉 `http://localhost:8080`

Swagger UI:  
👉 `http://localhost:8080/swagger`

---

## ⚙️ Environment Variables

| Variable              | Default Value       | Description                  |
|------------------------|---------------------|------------------------------|
| POSTGRE_HOST          | postgres            | PostgreSQL host              |
| POSTGRE_PORT          | 5432                | PostgreSQL port              |
| POSTGRE_DB            | EcommerceInventory  | Database name                |
| POSTGRE_USER          | postgres            | DB username                  |
| POSTGRE_PASSWORD      | godika              | DB password                  |
| RabbitMQ__HostName    | rabbitmq            | RabbitMQ host                |
| RabbitMQ__Port        | 5672                | RabbitMQ port                |
| RabbitMQ__Exchange    | loyalty-events      | RabbitMQ exchange name       |

---

## 📖 API Endpoints

### 🧑 User
#### Register
`POST /api/users/register`

```json
{
  "username": "john",
  "password": "Pass123!"
}
```

#### Login
`POST /api/users/login`

```json
{
  "username": "john",
  "password": "Pass123!",
  "deviceId": "device-001"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

#### Logout
`POST /api/users/logout`

```json
{
  "userId": "guid-here",
  "sessionId": "guid-here"
}
```

#### Logout All
`POST /api/users/logout-all`

```json
{
  "userId": "guid-here"
}
```

---

### 📦 Products
#### Create Product
`POST /api/products/Create`

```json
{
  "name": "Laptop",
  "price": 1200.50,
  "stock": 10
}
```

#### Update Product
`PUT /api/products/Update/{id}`

```json
{
  "name": "Gaming Laptop",
  "price": 1500.00,
  "stock": 8
}
```

#### Get Product by Id
`GET /api/products/{id}`

#### Get All Products
`GET /api/products`

---

### 🎟️ Discounts
#### Create Discount Rule
`POST /api/Discount/Create`

```json
{
  "cardType": "Gold",
  "type": 0,
  "discountPercentage": 10,
  "minimumPurchaseAmount": 100,
  "validFrom": "2025-10-01T00:00:00",
  "validTo": "2025-12-31T23:59:59"
}
```

#### Update Discount Rule
`PUT /api/Discount/Update`

```json
{
  "id": "guid-here",
  "cardType": "Gold",
  "type": 1,
  "fixedAmount": 50,
  "minimumPurchaseAmount": 200,
  "validFrom": "2025-10-01T00:00:00",
  "validTo": "2025-12-31T23:59:59",
  "active": true
}
```

#### Get Discount by Id
`GET /api/Discount/{id}`

#### Get All Discounts
`GET /api/Discount/GetAll`

---

### 🛒 Orders
#### Place Order
`POST /api/Order/PlaceOrder/{userId}?discountCard=optional`

```json
[
  {
    "productId": "guid-here",
    "quantity": 2
  }
]
```

Response:
```json
{
  "orderId": "guid-here",
  "status": "Created"
}
```

---

## 🗂️ Tech Stack
- .NET 8
- PostgreSQL
- Entity Framework Core (Migration )
- RabbitMQ (Event-driven messaging)
- Docker & Docker Compose

