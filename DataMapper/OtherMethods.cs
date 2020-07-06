using System;
using System.Collections;
using System.Configuration;
using Npgsql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper
{
    class OtherMethods
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public static void MenuDisplay()
        {
            Console.Clear();
            Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
            Console.WriteLine("MENU\n1. Check Current Offers. \n2. Check a Client's Rental History. \n3. New Rental. \n4. Return a copy. " +
                "\n5. Add a new user. \n6. Create a new Movie. \n7. Rental Statistics. \n8. Overdue rentals.\n9. Quit.\n\n");
            Console.Write("Please enter your menu choice: ");
        }
        public static string MenuRepeatOption()
        {
            string userRepeatChoice;
            Console.Write("\nDo you want to do anything else [Y/N]: ");
            userRepeatChoice = Console.ReadLine().ToUpper();
            if (userRepeatChoice != "Y")
            {
                userRepeatChoice = "N";
                Console.Clear();
                Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n** GOODBYE **");
            }

            return userRepeatChoice;
        }
        public static void RentalHistoryPrinting(ArrayList arrayList, string rentalList)
        {
            if (arrayList.Count == 0)
            {
                Console.WriteLine("There are no {0} rentals.", rentalList);
            }
            else
            {
                foreach (var item in arrayList)
                {
                    Console.WriteLine(item);
                }
            }
        }
        public static int GenerateNewUniqueId(string whichID)
        {
            int newUniqueId = 0;
            switch (whichID)
            {
                case "copy_id":
                    using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                    {
                        conn.Open();
                        using (var command = new NpgsqlCommand("SELECT MAX(copy_id) FROM Copies", conn))
                        {
                            NpgsqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                newUniqueId = (int)reader[0];
                            }
                        }
                    }
                    break;

                case "movie_id":
                    using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                    {
                        conn.Open();
                        using (var command = new NpgsqlCommand("SELECT MAX(movie_id) FROM Movies", conn))
                        {
                            NpgsqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                newUniqueId = (int)reader[0];
                            }
                        }
                    }
                    break;

                case "client_id":
                    using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                    {
                        conn.Open();
                        using (var command = new NpgsqlCommand("SELECT MAX(client_id) FROM Clients", conn))
                        {
                            NpgsqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                reader.Read();
                                newUniqueId = (int)reader[0];
                            }
                        }
                    }
                    break;
            }
            return newUniqueId + 1;
        }
        public static void AddNewCopies(int numberOfNewCopies, int movieID)
        {
            for (int i = 0; i < numberOfNewCopies; i++)
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("INSERT INTO copies (copy_id, available, movie_id) " +
                        "VALUES (@CopyID, 'true', @MovieID)", conn))
                    {
                        command.Parameters.AddWithValue("@CopyID", GenerateNewUniqueId("copy_id"));
                        command.Parameters.AddWithValue("@MovieID", movieID);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
