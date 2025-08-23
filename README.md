# BizStockApplication
BizStock is a modern inventory and stock management system built with ASP.NET Core, Entity Framework Core, and PostgreSQL. It is designed to help businesses manage products, brands, categories, users, and transactions efficiently.

🚀 Features

🔑 Authentication & Authorization

JWT-based authentication

Role-based access (Admin, Supplier, Customer, Inventory Manager, etc.)

Two-Factor Authentication (2FA)

🛍 Product Management

Add, update, delete products

Assign products to brands and categories

Image upload & storage

SKU, Barcode & QR code support

🏷 Brand & Category Management

Create and organize categories (with parent/child relationships)

Manage brands with logo, website, and description

📦 Inventory Management

Stock adjustments & transfers

Recently viewed products tracking (session & user-based)

Unit of Measure support

💬 Complaint & Chat System

Customers can submit complaints

Real-time chat between customers and officers via SignalR

Read/unread message tracking

🔔 Notifications

Real-time system notifications using SignalR

account summary, and system health alerts

📑 Audit Logs & Monitoring

Activity logs stored in Elasticsearch

System health monitoring

Search using PostgreSQL Full-Text Search

📨 Communication

Email & SMS notifications (Termii integration for SMS)

Templated email support

🛠 Tech Stack

Backend: ASP.NET Core (C#)

Database: PostgreSQL + Entity Framework Core

Real-time: SignalR

Messaging Queue: RabbitMQ with MassTransit

Search & Logs: Elasticsearch

Authentication: JWT, 2FA (WebAuthn)

Frontend: HTML, CSS, JavaScript
