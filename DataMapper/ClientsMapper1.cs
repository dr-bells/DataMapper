using System;
using System.Collections;
using System.Configuration;
using Npgsql;

namespace DataMapper
{
    class ClientsMapper1
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public static ClientsMapper1 Instance { get; } = new ClientsMapper1();
        private ClientsMapper1() { }
        public void AddNewClient(Clients clients)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO Clients (client_id, first_name, last_name, birthday)  VALUES (@ClientID, @firstname, @lastname, @birthday)",conn))
                {
                    command.Parameters.AddWithValue("@ClientID", clients.ID);
                    command.Parameters.AddWithValue("@firstname", clients.Firstname);
                    command.Parameters.AddWithValue("@lastname", clients.Lastname);
                    command.Parameters.AddWithValue("@birthday", clients.Birthday);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void ClientsRentalHistory(int id)
        {
            ArrayList HistoricRentals = new ArrayList();
            ArrayList ActiveRentals = new ArrayList();
            string active = "active", historic = "historic";

            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT first_name, last_name FROM clients WHERE client_id = @id", conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n** Clients Rental History **\n");
                        Console.WriteLine("Client ID - {0}: {1} {2} \n", id, reader[0], reader[1]);

                        using (NpgsqlConnection conn2 = new NpgsqlConnection(CONNECTION_STRING))
                        {
                            conn2.Open();
                            using (var command2 = new NpgsqlCommand("SELECT title, date_of_rental, date_of_return FROM clients cl " +
                                "JOIN Rentals r on r.client_id = cl.client_id JOIN Copies c ON c.copy_id = r.copy_id " +
                                "JOIN movies m ON c.movie_id = m.movie_id WHERE cl.client_id = @id", conn2))
                            {
                                command2.Parameters.AddWithValue("@id", id);
                                NpgsqlDataReader reader2 = command2.ExecuteReader();
                                while (reader2.Read())
                                {
                                    if (Convert.ToDateTime(reader2[2]) > DateTime.Now)
                                    {
                                        string newActiveRental = reader2[0] + " - Rented On: " + reader2[1] + " Due date: " + reader2[2];
                                        ActiveRentals.Add(newActiveRental);
                                    }
                                    else
                                    {
                                        string newHistoricRental = reader2[0] + " - From: " + reader2[1] + " To: " + reader2[2];
                                        HistoricRentals.Add(newHistoricRental);
                                    }
                                }
                            }
                        }

                        Console.WriteLine("- Active Rentals -");
                        OtherMethods.RentalHistoryPrinting(ActiveRentals, active);
                        Console.WriteLine("\n- Historic Rentals -");
                        OtherMethods.RentalHistoryPrinting(HistoricRentals, historic);
                    }
                    else { Console.WriteLine($"There are no clients with the id: {id}"); }
                }
            }
        }
    }
}
