# E-Commerce Inventory Management System

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

