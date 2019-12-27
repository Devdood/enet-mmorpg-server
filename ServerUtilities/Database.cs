using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace ServerUtilities
{
    public class DatabaseManager
    {
        public static string connectionString
        {
            get
            {
                return DatabaseUtils.connectionString;
            }
        }

        private static Thread updateThread;
        public static int lastInsertedItemId;
        public static int lastQuestId;
        public static int lastTitleId;
        public static int lastSkillId;
        public static int lastAbilityId;

        public static void Initialize()
        {
            try
            {
                DatabaseUtils.Initialize();

                updateThread = new Thread(new ThreadStart(Run));
                updateThread.Start();

                //lastInsertedItemId = GetLastInsertedId("items");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldnt load db config: " + ex.ToString());
            }
        }

        private static void Run()
        {
            while (true)
            {
                int playersCount = 0;

                /*if (CharactersManager.onlinePlayers.Entities.Count != 0)
                {
                    float startTime = Server.Time;

                    Dictionary<int, Client> online = new Dictionary<int, Client>(CharactersManager.onlinePlayers.Entities);
                    List<Player> charactersForUpdate = new List<Player>();

                    foreach (var player in online.Values.ToList())
                    {
                        charactersForUpdate.Add(player.Character);
                    }

                    if (charactersForUpdate.Count == 0)
                    {
                        return;
                    }

                    playersCount = charactersForUpdate.Count;

                    CharacterDB.SaveCharacters(charactersForUpdate);

                    ServerLogger.Log(null, "Updated database | Query time: " + (Server.Time - startTime).ToString(), LoggingLevel.PROD);*/
            

                    //InsertQuery(string.Format("INSERT INTO analytics(online_players) VALUES ({0})", playersCount));

                Thread.Sleep(120000);
            }
        }

        private static int GetLastInsertedId(string tableName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string lastInsertedQuestQuery = "SELECT `AUTO_INCREMENT`FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'mmotest' AND TABLE_NAME = '" + tableName + "'";
                MySqlCommand command = new MySqlCommand(lastInsertedQuestQuery, conn);
                command.ExecuteNonQuery();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public static string PackIds(HashSet<int> ids)
        {
            string result = "";
            foreach (var item in ids)
            {
                result += item + ",";
            }

            if (result.Length != 0)
            {
                result = result.Remove(result.Length - 1);
            }
            return "(" + result + ")";
        }

        public static string FloatToDouble(float f)
        {
            return System.Convert.ToDouble(f).ToString().Replace(",", ".");
        }

        public static DataTable ReturnQuery(string query)
        {
            return DatabaseUtils.ReturnQuery(query);
        }

        public static long InsertQuery(string query)
        {
            return DatabaseUtils.InsertQuery(query);
        }
    }
}