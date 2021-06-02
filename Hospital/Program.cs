using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserLibrary;

namespace Hospital
{
    public class Program
    {
        private static User loggedUser;

        public static void Main(string[] args)
        {
            bool PROGRAM_ON = false; //zmienna zezwalająca na uruchomienie systemu
            HospitalDao.initializeData(); //Wczytanie danych z plików .xml - Deserializacja
            if (HospitalDao.users.Any()) //Jeślli istnieją uzytkownicy
            {
                PROGRAM_ON = true;
                login(); // logowanie
                Console.Clear();
                Console.WriteLine("Zalogowano jako: " + loggedUser.name + " " + loggedUser.surname + "\n");
            }
            else
            {
                Console.WriteLine("Brak dostępnych użytkowników");
            }

            while (PROGRAM_ON) //główna pętla programu
            {
                if (loggedUser.right.Equals(User.Right.HIGH)) //Menu użytkownika z wysokimi uprawnieniami
                {
                    HospitalDao.showUserList(loggedUser); //Wyświetla listę użytkowników
                    Console.WriteLine(HospitalDao.Key_to_back_to_menu); //Wypisuje ustandaryzowane polecenie na ekran (tekst zapisany w zmiennej)
                    Console.ReadKey();
                    Console.Clear();

                    while (PROGRAM_ON) // menu główne
                    {
                        Console.WriteLine("Wpisz numer z menu:");
                        Console.WriteLine("1. Edytuj dane pracownika");
                        Console.WriteLine("2. Dodaj nowego użytkownika");
                        Console.WriteLine("3. Wyświetl listę użytkowników");
                        Console.WriteLine("4. Wyjście");
                        try
                        {
                            string odp = Console.ReadLine(); //wczytuje liczbę 
                            int option = int.Parse(odp); //sprawdza czy to na pewno liczba
                            switch (option)
                            {
                                // Poniższe numery po "case" to numery z menu
                                case 1:
                                    HospitalDao.userEdit(loggedUser); //otwiera menu edycji uzytkownika
                                    HospitalDao.showUserList(loggedUser); //wyświetla użytkowników
                                    Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case 2:
                                    HospitalDao.addNewUser(loggedUser); //otwiera menu dodawania nowego użytkownika
                                    break;

                                case 3:
                                    Console.Clear();
                                    HospitalDao.showUserList(loggedUser); //Wyświetla wszytskich użytkowników
                                    Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case 4:
                                    PROGRAM_ON = false;
                                    break;

                                default:
                                    Console.Clear();
                                    Console.WriteLine(HospitalDao.Err_Select_Menu_Number);
                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Blędne dane!\n");
                        }
                    }
                }
                else // Menu użytkownika z niskimi uprawnieniami
                {
                    Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                    Console.ReadKey();
                    Console.Clear();

                    while (PROGRAM_ON) //Podobne menu do tego powyzej tylko inne opcje
                    {
                        Console.WriteLine("Wpisz numer z menu:");
                        Console.WriteLine("1. Wyświetl listę użytkowników");
                        Console.WriteLine("2. Wyświetl listę dyżurów");
                        Console.WriteLine("3. Wyjście");
                        try
                        {
                            string odp = Console.ReadLine(); //odczyt numeru z klawiatury
                            int option = int.Parse(odp);
                            switch (option)
                            {
                                case 1:
                                    Console.Clear();
                                    HospitalDao.showUserList(loggedUser); //Wyświetla listę uzytkownikow dostępnych dla tego usera
                                    Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case 2:
                                    Console.Clear();
                                    HospitalDao.showUserList(loggedUser); //Wyświetla listę uzytkownikow dostępnych dla tego usera
                                    Console.WriteLine("\n\nWpisz id użytkownika, którego dane chcesz wyświetlić:");
                                    long id = UserDao.getLongNumber(); //przygotowana metoda pobierania numeru od użytkownika
                                    User userToEdit = HospitalDao.getUserById(id); //Pobiera uzytkownika po podanym wyzej ID
                                    if (userToEdit != null) //jeśli poprzednia metoda nie pobrała użytkownika
                                    {
                                        Console.Clear();
                                        userToEdit.printUser(1); //Wypisuje wybranego użytkownika
                                        Console.WriteLine();
                                        DutyDao.showDutiesByUser(userToEdit); //Wyświetla listę dyżurów dla wybranego użytkownika
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nie ma użytkownika o zadanym ID");
                                    }
                                    Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;

                                case 3:
                                    PROGRAM_ON = false; //przestawienie zmiennej włączonego programu, aby wyjść z pętli i zakończyć program
                                    break;

                                default: //Jeśli wybrany numer spoza zakresu menu
                                    Console.Clear();
                                    Console.WriteLine(HospitalDao.Err_Select_Menu_Number);
                                    break;
                            }
                        }
                        catch (Exception) //Jeśli wpisany tekst zamiast numeru
                        {
                            Console.Clear();
                            Console.WriteLine(HospitalDao.Err_std);
                        }
                    }
                }
                break;
            }
            HospitalDao.saveData(); //Zapisuje dane do plików podczas zamykania programu - Serializacja
        }

        private static void login()
        {
            loggedUser = null;
            while (loggedUser == null)
            {
                Console.WriteLine("Witaj w systemie SuperHospital!\n");
                Console.WriteLine("Zaloguj się, aby kontynuować");
                Console.WriteLine("Login:");
                string nazwaU = Console.ReadLine();
                Console.WriteLine("Haslo:");
                string password = Console.ReadLine();
                loggedUser = HospitalDao.getUserByUsernamePassword(nazwaU, password); //Pobieranie użytkownika o zadanym loginie i haśle
                if (loggedUser == null) //Jeśli powyższa metoda nie zwróciła żadnego użytkownika
                {
                    Console.Clear();
                    Console.WriteLine("Podano zły login, lub hasło!\n");
                }
            } 
        }


    }
}
