using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace sqltest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "127.0.0.1,1434",
                TrustServerCertificate = true,
                UserID = "sa",
                Password = "QAZ123wsxzx",
                InitialCatalog = "MainData"
            };

            var connectionString = builder.ConnectionString;

            try
            {
                await using var connection = new SqlConnection(connectionString);
                Console.WriteLine("\nMedicine Database Query:");
                Console.WriteLine("=========================================\n");

                await connection.OpenAsync();

                var sql = "SELECT [Name],[Strength], [Indication] FROM [MainData].[dbo].[medicine] WHERE Form = 'Cream' and Indication = 'Virus' and Manufacturer LIKE 'Roche%';";
                await using var command = new SqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Console.WriteLine("{0} {1} {2} {3} {4}", reader.GetString(0), "|" ,reader.GetString(1), "|" , reader.GetString(2));
                }
            }
            catch (SqlException e) when (e.Number == 404)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }
    }
}