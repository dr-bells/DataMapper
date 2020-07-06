using System;
using System.Configuration;
using Npgsql;

namespace DataMapper
{
    class RentalsMapper : IMapper<Rentals>
    {
        private static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Rental"].ToString();
        public static RentalsMapper Instance { get; } = new RentalsMapper();
        private RentalsMapper() { }
        public void GetByID(CollectId collectId, int? id)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT r.*, c.* , t.* , p.* FROM rentals r INNER JOIN clients c ON r.client_id = c.client_id  " +
                    "INNER JOIN copies t on r.copy_id = t.copy_id INNER JOIN movies p ON t.movie_id = p.movie_id where  c.client_id = @client_id"))
                {
                    command.Parameters.AddWithValue("@Client_id", collectId.ClientId);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void Save(Rentals rental)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO rentals(client_id, copy_id, date_of_rental, date_of_return) " +
                    "VALUES (@client_id, @copy_id, @date_of_rental, @date_of_return)", conn))
                {
                    command.Parameters.AddWithValue("@client_id", rental.Clientld);
                    command.Parameters.AddWithValue("@copy_id", rental.CopyId);
                    command.Parameters.AddWithValue("@date_of_rental", rental.DateOfRental);
                    command.Parameters.AddWithValue("@date_of_return", rental.DateOfReturn);
                    command.ExecuteNonQuery();
                }
                RentalsAndRetturns(rental.CopyId, "rental");
                Console.WriteLine("\nClient Number: {0} has successfuly rented the movie with the Copy ID: {1} and " +
                    "it is due to be returned on : {2}.", rental.Clientld, rental.CopyId, DateTime.Now.AddDays(14));
            }
        }
        public void History(DateTime dateTime)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT title, SUM (Price) FROM Movies m JOIN Copies c ON m.movie_id = c.movie_id " +
                    "JOIN Rentals r on r.copy_id = c.copy_id WHERE date_of_rental > @dateTime GROUP BY title", conn))
                {
                    command.Parameters.AddWithValue("@dateTIme", dateTime);
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.Write("{0}\t${1} \n", reader[0], reader[1]);
                    }
                }
            }
        }
        public void RentalsAndRetturns(int copyID, string option)
        {
            if (option == "return")
            {

                using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("Update Copies SET  available ='t' where copy_id = @copy_id", conn))
                    {
                        command.Parameters.AddWithValue("@copy_id", copyID);
                        command.ExecuteNonQuery();
                    }
                }
                using (NpgsqlConnection conn2 = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn2.Open();
                    using (var command = new NpgsqlCommand("Update Rentals SET  date_of_return = @dateOfReturn where copy_id = @copy_id", conn2))
                    {
                        command.Parameters.AddWithValue("@copy_id", copyID);
                        command.Parameters.AddWithValue("@dateOfReturn", DateTime.Now);
                        command.ExecuteNonQuery();
                    }
                }
                Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n-- Return a copy --\n\n");
                Console.WriteLine("You have successfully returned the movie with the copy id: " + copyID);
            }
            if (option == "rental")
            {

                using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand("Update Copies SET  available ='false' where copy_id = @copy_id", conn))
                    {
                        command.Parameters.AddWithValue("@copy_id", copyID);
                        command.ExecuteNonQuery();
                    }
                }
                Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n-- Return a copy --\n\n");
            }
        }
        public void OverdueRentals()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var command = new NpgsqlCommand("SELECT copy_id, DATE_PART('day', date_of_return - date_of_rental)" +
                    "FROM rentals WHERE(SELECT DATE_PART('day', date_of_return - date_of_rental)) > 14", conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Copy ID\tNumber of Days\tAmount Due ( $0.5/Day )");
                        while (reader.Read())
                        {
                            Console.Write("{0}\t{1}\t\t${2} \n", reader[0], reader[1], (double)reader[1] * 0.5);
                        }
                    }
                    else {  Console.WriteLine("There are no overdue rentals."); }
                }
            }
        }
        public void Delete(Rentals t)
        {
            throw new NotImplementedException();
        }
        public Rentals GetByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
