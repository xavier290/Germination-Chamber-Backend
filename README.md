# MQTT Server with ASP.NET Core

This project demonstrates how to set up an MQTT server using ASP.NET Core. It includes user authentication with a custom API and integrates MQTT functionality.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
- [Usage](#usage)
  - [Running the Server](#running-the-server)
  - [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This project provides a simple MQTT server implementation using ASP.NET Core. It includes basic user authentication using a custom API with JWT tokens, and integrates MQTT messaging capabilities.

## Features

- MQTT server with WebSocket support
- Custom user authentication API
- JWT token-based authentication
- Entity Framework Core for database operations
- Cors support for frontend integration

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Node.js](https://nodejs.org/) (for frontend integration if needed)

### Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/your-username/your-repo.git
    cd your-repo
    ```

2. Install the required NuGet packages:

    ```sh
    dotnet restore
    ```

### Database Setup

1. Configure your database connection string in `appsettings.json`:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=MqttDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AllowedHosts": "*"
    }
    ```

2. Create and apply migrations:

    ```sh
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

## Usage

### Running the Server

1. Start the server:

    ```sh
    dotnet run
    ```

2. The server should now be running at `http://localhost:5085` with MQTT support on port `1883`.

### API Endpoints

#### User Registration

- **URL:** `/api/auth/register`
- **Method:** `POST`
- **Body:**

    ```json
    {
      "UserName": "your_username",
      "Name": "your_name",
      "LastName": "your_lastname",
      "Email": "your_email",
      "Password": "your_password"
    }
    ```

#### User Login

- **URL:** `/api/auth/login`
- **Method:** `POST`
- **Body:**

    ```json
    {
      "Email": "your_email",
      "Password": "your_password"
    }
    ```

- **Response:**

    ```json
    {
      "token": "your_jwt_token"
    }
    ```

### MQTT Messaging

Use an MQTT client (like [MQTT.fx](https://mqttfx.jensd.de/)) to connect to your server at `ws://localhost:5085/mqtt` for WebSocket connections or `tcp://localhost:1883` for TCP connections.

## Contributing

We welcome contributions!

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
