using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            int menuChoice;
            bool correctMenuChoice;
            Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
            Console.WriteLine("MENU\n1. Check Current Offers. \n2. Check a Client's Rental History. \n3. New Rental. \n4. Return a copy. " +
                "\n5. Add a new user. \n6. Create a new Movie. \n7. Rental Statistics. \n8. Overdue rentals.\n\n");
            Console.Write("Please enter your menu choice: ");
            correctMenuChoice = int.TryParse(Console.ReadLine(), out menuChoice);

            switch (menuChoice)
            {
                case 1:
                    Console.Clear(); Console.WriteLine("|-----------------------------|\n| ** DR BELLS Rental Store ** |\n|-----------------------------|\n\n");
                    MovieMapper.Instance.AvailableCopies();
                    Console.WriteLine("Current Available Offers:   " + DateTime.Today.ToLongDateString() + "\n");
                    foreach (var item in MovieMapper.Instance.intList)
                    {
                        Movie movie = MovieMapper.Instance.GetByID(item);
                        Console.WriteLine(movie);
                    }
                    Console.WriteLine("\n");
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                default:
                    break;
            }

            //// Simple fetch from database
            //Movie movie = MovieMapper.Instance.GetByID(2);
            //Console.WriteLine(movie.ToString());
            //// This time MovieMapper will get the movie from cache, instead of getting it from database
            //movie = MovieMapper.Instance.GetByID(2);
            //Console.WriteLine(movie.ToString());

            //// New object is created
            //movie = new Movie(123, "The Last Samurai", 2003, 10);
            //// Before the object was only in the memory. We need to save it, to store it in the persistence layer
            //MovieMapper.Instance.Save(movie);
            //Console.WriteLine(MovieMapper.Instance.GetByID(123).ToString());

            //// We adjust the price
            //movie.Price = 4.5;
            //// We need to Save the object, or else price change will not be reflected in the database
            //MovieMapper.Instance.Save(movie);
            //// Price is changed both in the in-memory object and in the persistence layer
            //Console.WriteLine(MovieMapper.Instance.GetByID(123).ToString());



            //// Object is removed from the database
            //try
            //{
            //    MovieMapper.Instance.Delete(movie);
            //    if (MovieMapper.Instance.GetByID(123) == null)
            //    {
            //        Console.WriteLine("Object is removed from the database");
            //    }
            //} catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
}
