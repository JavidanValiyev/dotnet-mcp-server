using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using Serilog;

namespace mcp_mssql_server;

[McpServerToolType]
public class SqlTools
{
    [McpServerTool,Description("Runs a SELECT query")]
    public static async Task<string> Select(string sql,[FromServices]IOptions<AppSettings> options)
    {
        Log.Information("Sql query running with connection string: {ConnectionString}",options.Value.ConnectionString);

        if (!sql.TrimStart().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
            return ("Error: Only SELECT is allowed.");
        try {
            using var conn = new SqlConnection(options.Value.ConnectionString);
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();
        
            var rows = new List<Dictionary<string, object>>();
            while (await reader.ReadAsync()) {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.GetValue(i);
                rows.Add(row);
            }
            return System.Text.Json.JsonSerializer.Serialize(rows);
        }
        catch (Exception ex) {
            return ($"SQL Error: {ex.Message}");
        }
    }
}

[McpServerResourceType]
public class SqlResource
{ 
    [McpServerResource, Description("Get tables and columns from the sql server")]
    public static async Task<string> GetSchema([FromServices]IOptions<AppSettings> options)
    {
        Log.Information("Getting schema with connection string: {ConnectionString}",options.Value.ConnectionString);
        await using var conn = new SqlConnection(options.Value.ConnectionString);
        await conn.OpenAsync();
    
        var cmd = new SqlCommand($"SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS", conn);
        var schema = new List<object>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            schema.Add(new { Table = reader[0], Column = reader[1] });
        }
        return System.Text.Json.JsonSerializer.Serialize(schema);
    }
}