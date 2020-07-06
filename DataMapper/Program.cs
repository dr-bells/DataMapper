using System;

namespace DataMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            int menuChoice;
            bool correctMenuChoice;
            string userRepeatChoice = "Y";

            while (userRepeatChoice == "Y")
            {
                OtherMethods.MenuDisplay();
                correctMenuChoice = int.TryParse(Console.ReadLine(), out menuChoice);
                while (!correctMenuChoice || 0 > menuChoice || menuChoice > 9)
                {
                    Console.Clear();
                    Console.WriteLine("You entered a wrong choice.\n\nPress any key to continue......");
                    Console.ReadLine();
                    OtherMethods.MenuDisplay();
                    correctMenuChoice = int.TryParse(Console.ReadLine(), out menuChoice);
                }

                switch (menuChoice)
                {
                    case 1: //1. Check Current Offers.
                        #region 1. Check Current Offers.
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
                        MovieMapper.Instance.AvailableCopies();

                        Console.WriteLine("Current Available Offers:   " + DateTime.Today.ToLongDateString() + "\n");
                        foreach (var item in MovieMapper.Instance.availableCopiesList)
                        {
                            Movie movie = MovieMapper.Instance.GetByID(item);
                            Console.WriteLine(movie);
                        }
                        userRepeatChoice =OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 2: //2. Check a Client's Rental History.
                        #region 2. Check a Client's Rental History.
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n** Clients Rental History **\n");
                        int inputedClientId;

                        Console.Write("Please enter the client's id: ");
                        inputedClientId = int.Parse(Console.ReadLine());
                        ClientsMapper1.Instance.ClientsRentalHistory(inputedClientId);
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 3: //3. New Rental.
                        #region New Rental.
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n-- New Rental --\n\n");
                        int client_id, copy_id;
                        DateTime dateOfRental, dateOfReturn;

                        Console.Write("Please enter the Client's Id: ");
                        client_id = int.Parse(Console.ReadLine());
                        Console.Write("Please enter the Copy Id: ");
                        copy_id = int.Parse(Console.ReadLine());
                        dateOfRental = DateTime.Now;
                        dateOfReturn = dateOfRental.AddDays(14);
                        var newrental = new Rentals(client_id, null, copy_id, dateOfRental, dateOfReturn);
                        RentalsMapper.Instance.Save(newrental);
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 4: //4. Return a copy. 
                        #region 4. Return a copy.
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n-- Return a copy --\n\n");
                        int returningCopyID;
                        Console.Write("Please enter the copy id of the movie you want to return: ");
                        returningCopyID = int.Parse(Console.ReadLine());
                        RentalsMapper.Instance.RentalsAndRetturns(returningCopyID, "return");
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 5: //5. Add a new user.
                        #region 5. Add a new user.
                        string firstname, lastname;
                        DateTime birthday;

                        Console.Write("Please enter the firstname of the client: "); firstname = Console.ReadLine();
                        Console.Write("Please enter the lastname of the client: "); lastname = Console.ReadLine();
                        Console.Write("Please enter the birthday of the client [yyyy-mm-dd]: "); birthday = DateTime.Parse(Console.ReadLine());
                        Clients clients = new Clients(OtherMethods.GenerateNewUniqueId("client_id"), firstname, lastname, birthday);
                        ClientsMapper1.Instance.AddNewClient(clients);
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 6: //6.Create a new Movie.
                        #region 6.Create a new Movie.
                        string movieTitle;
                        int newMovieId = OtherMethods.GenerateNewUniqueId("movie_id"), year, numberOfCopies;
                        double price;

                        Console.Write("Please Enter the title of the movie: ");
                        movieTitle = Console.ReadLine();
                        Console.Write("Please enter the year the movie was produced: ");
                        year = int.Parse(Console.ReadLine());
                        Console.Write("Please enter the price of the movie: ");
                        price = double.Parse(Console.ReadLine());
                        Console.Write("Please enter the number of copies available: ");
                        numberOfCopies = int.Parse(Console.ReadLine());
                        Movie movie2 = new Movie(newMovieId, movieTitle, year, price);
                        MovieMapper.Instance.Save(movie2);
                        OtherMethods.AddNewCopies(numberOfCopies, newMovieId);
                        Console.WriteLine(MovieMapper.Instance.GetByID(newMovieId).ToString());
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 7: //7. Rental Statistics.
                        #region 7. Rental Statistics.  **                                      
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
                        Console.WriteLine("** Rental Statistics **\n\n");
                        Console.Write("Please enter the starting date [yyyy:mm:dd] : ");
                        DateTime startingDate = DateTime.Parse(Console.ReadLine());
                        RentalsMapper.Instance.History(startingDate);
                        userRepeatChoice = OtherMethods.MenuRepeatOption();            
                        #endregion
                        break;
                    case 8: //8. Overdue rentals.
                        #region 8. Overdue rentals.
                        Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
                        Console.WriteLine("** Overdue Rentals (More than 2 weeks) **\n\n");
                        RentalsMapper.Instance.OverdueRentals();
                        userRepeatChoice = OtherMethods.MenuRepeatOption();
                        #endregion
                        break;
                    case 9: //Quit.
                        Console.Clear(); 
                        Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
                        Console.Write("You selected QUIT. Are you sure you want to quit? [Y/N]: ");
                        string repeatQuit = Console.ReadLine().ToUpper();
                        if (repeatQuit == "Y")
                        {
                            userRepeatChoice = "N";
                            Console.Clear();
                            Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n ** GOODBYE * *");
                        }
                        else { userRepeatChoice = "Y"; }
                        break;
                }
            }
        }
    }
}
