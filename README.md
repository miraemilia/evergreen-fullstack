# Fullstack Project

![TypeScript](https://img.shields.io/badge/TypeScript-v.4-green)
![SASS](https://img.shields.io/badge/SASS-v.4-hotpink)
![React](https://img.shields.io/badge/React-v.18-blue)
![Redux toolkit](https://img.shields.io/badge/Redux-v.1.9-brown)
![.NET Core](https://img.shields.io/badge/.NET%20Core-v.7-purple)
![EF Core](https://img.shields.io/badge/EF%20Core-v.7-cyan)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-v.14-drakblue)

This is the final project of Integrify Academy which involves creating a Fullstack project with React and Redux in the frontend and ASP.NET Core 7 in the backend. The result is an indoor plant e-commerce site called Evergreen which features basic user functionalities (registering, authentication, browsing through products, shopping cart, ordering) as well as admin functionalities for managing users, products and orders.

[Link to deployed project]()

## Table of Contents

1. [Technologies](#technologies)
2. [Functionalities](#functionalities)
3. [Frontend](#frontend)
4. [Backend](#backend)
   - [Design](#design)
   - [Deployment](#deployment)
   - [Running the project](#running-the-project)

## Technologies

- Frontend: SASS, TypeScript, React, Redux Toolkit
- Backend: ASP.NET Core, Entity Framework Core, PostgreSQL

## Functionalities

### User

1. User Management: a user is able to...
   - register for a user account (not admin account)
   - log in and out
   - (backend only: edit certain properties of their account)
   - (backend only: unregister)
2. Products: a user is able to...
   - view all available products
   - view a single product
   - search and sort products
3. Cart: a user is able to...
   - add products to a shopping cart
   - manage their shopping cart
   - (backend only: checkout the shopping cart / place an order)
4. Order Management: a user is able to...
   - (backend only: view their order history)
   - (backend only: track the status of their order)
   - (backend only: cancel their order while it is processing)

### Admin

1. User Management: an admin is able to...
   - view users
   - delete users
   - edit user roles
   - (backend only: create new users and admins)
2. Product Management: an admin is able to...
   - view products in admin mode
   - add products
   - edit products
   - delete products
3. Order Management: an admin is able to...
   - (backend only: view all orders)
   - (backend only: view order details)
   - (backend only: update order status)
   - (backend only: cancel orders while it is processing)

## Frontend

The frontend code and documentation are found in [this repository]().

## Backend

### General

- CLEAN architecture
- complies with Rest API
- EF core: code-first database (with seed data)
- error handling middleware
- authentication and authorization
- unit testing for service layer (xunit)
- documentation (README.md and Swagger)
- backend and database deployed on live servers

### Design

#### Database design

![ERD](readmeImages/erd.png)

#### Architecture

![Clean architecture layers](readmeImages/layers.png)

#### Endpoints

![Endpoints](readmeImages/endpoints.png)

### Deployment

[Link to backend deployment](https://evergreenbotanics.azurewebsites.net/)

### Running the backend locally

#### Requirements
- [.NET](https://dotnet.microsoft.com/en-us/download)

#### Instructions
- clone the project
- run the project with `dotnet watch --project Evergreen.WebAPI`
- run tests with `dotnet test`
