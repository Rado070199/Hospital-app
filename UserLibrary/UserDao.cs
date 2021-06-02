using System;

namespace UserLibrary
{
    public static class UserDao
    {
        public static int getIntNumber() //metoda pobierająca liczbę od użytkownika różnica taka od tej niżej ze tu jest typu int
        {
            while (true)
            {
                try
                {
                    int number = int.Parse(Console.ReadLine());//tutaj pobiera i od razu sprawdza czy na pewno jest liczbą
                    return number;
                }
                catch (Exception)
                {
                    Console.WriteLine("Nieprawidłowe dane! Wpisz poprawne:");
                }
            }
        }

        public static long getLongNumber() //metoda pobierająca liczbę od użytkownika różnica taka od tej wyżej ze tu jest typu long
        {
            while (true)
            {
                try
                {
                    long number = long.Parse(Console.ReadLine()); //tutaj pobiera i od razu sprawdza czy na pewno jest liczbą
                    return number;
                }
                catch (Exception)
                {
                    Console.WriteLine("Nieprawidłowe dane! Wpisz poprawne:");
                }
            }
        }
    }
}
