# Euvic Internship Task

This is a web API written in .NET 7 that allows users to register and login to their accounts using JWT tokens. The API supports two user roles: an administrator who has access to a complete list of users and their detailed data in the database, and a regular user who can only access their own data.

The API and database are hosted on the Azure platform. We use Azure App Service to host the API and Azure SQL Database to host the database. Data between the API and database is encrypted in transit.

The API uses NLog for logging and Swagger for API documentation. NLog is a flexible and efficient logging framework for .NET, while Swagger provides a user-friendly interface for exploring and testing the API endpoints.

## Database
Database contains all information about accounts. Password are hassed.

## Endpoints

### /api/account/register
Allowes to register new users

### /api/account/login
Allowes to login to account by giving email and password

### /api/account/all
Gives list of all users in database. Only admin can use it

### /api/account/{id}
Gives details about accout with given id. Only admin and owner of accout can use it.
