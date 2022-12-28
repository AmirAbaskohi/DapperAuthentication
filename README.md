# MessageReceiver

A project built with ASP.NET C# that receives and processes messages from users.

## Features

- Authenticates API requests using a JWT token
- Reads messages from users
- Writes messages to a RabbitMQ queue
- Reads messages from the RabbitMQ queue and writes them to a database using Dapper
- Uses Docker to create a RabbitMQ instance
- A Quartz job updates the messages by reading them from the database using a StoredProcedure
- Logs the messages it reads in a file

## Getting Started

1. Clone the repository:

```bash
git clone https://github.com/AmirAbaskohi/MessageReceiver
```

2. Navigate to the project directory:

```bash
cd MessageReceiver
```

3. Install dependencies
```bash
dotnet restore
```

4. Set up the RabbitMQ instance with Docker:
```bash
docker compose -d
```

5. Set up the database (instructions specific to your database system will vary):
* Create a new database
* Run the appropriate SQL scripts to create the necessary tables

6. Update the database connection string in the `appsettings.json` file with your own database connection details.

7. Run the API project:
```bash
cd API
dotnet run
```

8. Run the Console project:
```bash
cd Console
dotnet run
```

## License
This project is licensed under the MIT License - see the LICENSE file for details.
