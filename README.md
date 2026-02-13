# MCP SQL Server

A Model Context Protocol (MCP) server implementation for Microsoft SQL Server, built with .NET 10. This server enables AI assistants like Claude to interact with SQL Server databases through standardized MCP tools and resources.

## Overview

This MCP server provides read-only access to SQL Server databases, allowing AI models to query database schemas and execute SELECT queries. It implements the Model Context Protocol specification using the `ModelContextProtocol` preview NuGet package.
I developed this server to testing and learning purposes, so it may not be suitable for production use.

## Features

- **Schema Discovery**: Retrieve database table and column information via MCP resources
- **Safe Query Execution**: Execute SELECT queries with built-in protection against non-SELECT statements

## Technology Stack

- **.NET 10.0**: Latest .NET framework
- **Microsoft.Data.SqlClient**: SQL Server connectivity
- **ModelContextProtocol 0.8.0-preview.1**: MCP implementation
## MCP Tools & Resources

### Tools

- **Select**: Executes SELECT queries on the configured database
  - Validates queries to ensure only SELECT statements are allowed
  - Returns results as JSON-serialized data
  - Parameters: `sql` (string) - The SELECT query to execute

### Resources

- **GetSchema**: Retrieves all tables and columns from the database
  - Queries `INFORMATION_SCHEMA.COLUMNS`
  - Returns table and column information as JSON

## Installation

1. Clone the repository
2. Ensure you have .NET 10.0 SDK installed
3. Configure your database connection string in `appsettings.json`:

```json
{
  "ConnectionString": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
}
```

Alternatively, set the connection string via environment variable:
```bash
ConnectionString="Server=your_server;Database=your_database;..."
```

## Usage

### Running the Server

```bash
cd mcp-mssql-server
dotnet run
```

The server uses STDIO transport and will communicate via standard input/output streams.

### Integration with Claude Desktop

Add the following configuration to your Claude Desktop MCP settings:

```json
{
  "mcpServers": {
    "mssql": {
      "command": "dotnet",
      "args": ["run", "--project", "path/to/mcp-mssql-server/mcp-mssql-server.csproj"]
    }
  }
}
```

### Testing with MCP Inspector

```bash
npx @modelcontextprotocol/inspector dotnet run --project mcp-mssql-server/mcp-mssql-server.csproj
```

## Project Structure

```
mcp-mssql-server/
├── mcp-mssql-server/
│   ├── Program.cs          # Application entry point and MCP server setup
│   ├── SqlTools.cs         # MCP tools and resources implementation
│   ├── AppSettings.cs      # Configuration model
│   ├── appsettings.json    # Configuration file
│   └── mcp-mssql-server.csproj
├── LICENSE                 # MIT License
└── README.md
```

## Security Considerations

⚠️ **Important**: This server is designed for development and testing purposes only. Do not use in production environments without additional security measures.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with the [Model Context Protocol SDK](https://github.com/modelcontextprotocol)
- Tested with Claude Desktop and MCP Inspector tools
