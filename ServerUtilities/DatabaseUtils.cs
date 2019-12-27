using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Linq;

public class DatabaseUtils
{
    public static string connectionString;

    public static void Initialize()
    {
        try
        {
            XDocument configXml = XDocument.Load("config/db.xml");

            string hostname = configXml.Root.Element("hostname").Value;
            string username = configXml.Root.Element("username").Value;
            string password = configXml.Root.Element("password").Value;
            string port = configXml.Root.Element("port").Value;
            string database = configXml.Root.Element("database").Value;

            connectionString = string.Format("Server={0};Port={1};Database={2};User={3};Password={4};SslMode=none; convert zero datetime=True", hostname, port, database, username, password);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Couldnt load db config: " + ex.ToString());
        }
    }

    public static DataTable ReturnQuery(string query)
    {
        DataTable results = new DataTable("Results");
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query, conn);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    results.Load(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(query + " /// Return query error: " + ex.ToString());
            }
        }

        return results;
    }

    public static long InsertQuery(string query)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }
    }
}