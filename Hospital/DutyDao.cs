using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserLibrary;

namespace Hospital
{
    public class DutyDao
    {
        public static List<Duty> duties = new List<Duty>(); //główna lista dyzurów
        public static List<Duty> userDuties = new List<Duty>(); //pomocnicza lista dyżurów

        public static void dutyEditByUser(User user) //Metoda obslugujaca menu edycji dyzuru
        {
            bool cancel = false;
            while (!cancel)
            {
                Console.Clear();
                Console.WriteLine("1. Pokaż dyżury");
                Console.WriteLine("2. Usuń dyżur");
                Console.WriteLine("3. Dodaj nowy dyżur");
                Console.WriteLine("4. Anuluj");
                try
                {
                    string numerString = Console.ReadLine(); //Pobranie numeru od uzytkownika
                    int numer = int.Parse(numerString); //sprawdzenie czy to na pewno liczba

                    switch (numer)
                    {//menu odpowiada liczbom po "case"
                        case 1:
                            Console.Clear();
                            showDutiesByUser(user); //Wyświetla wszytskie dyżury użytkownika
                            Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true;
                            break;

                        case 2:
                            removeDutyByUser(user); //Przechodzi do menu usuwania dyzuru
                            Console.Clear();
                            showDutiesByUser(user); //wyświetla wszytskie dyzury pracownika po edycji
                            Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true;
                            break;

                        case 3:
                            addNewDutyToUser(user); //przechodzi do menu dodawania nowego dyzuru
                            Console.Clear();
                            showDutiesByUser(user); //Wyświetla wszytskie dyzury
                            Console.WriteLine(HospitalDao.Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true;
                            break;

                        case 4: //wychodzi z menu - ustawia zmienna na zakonczenie petli
                            cancel = true;
                            break;

                        default: //Jeśli spoza zakresu menu
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine(HospitalDao.Err_Select_Menu_Number);
                }
            }
        }

        public static void removeDutyByUser(User user)
        {
            Console.WriteLine("Wybierz numer dyżuru do usunięcia");
            showDutiesByUser(user); //wyświetla wysztskie dyzury usera
            bool correct = false;
            while (!correct)
            {
                int numer = UserDao.getIntNumber(); //pobiera liczeb od uzytkownika ktory dyzur usunac
                numer--; //ponieważ numeracja od 1. a na liscie od 0
                if (numer < userDuties.Count()) //jeśli numer sie miesci w dostępnych dyzurach
                {
                    Duty duty = userDuties[numer]; //Pobieram wybrany dyzur z pomocniczej listy
                    duties.Remove(duty); //Usuwa wybrany z głównej listy dyzuroów
                    correct = true; //zmienna do zakonczenia petli
                }
                else // jeśli nie ma takiego numeru na liscie
                {
                    Console.WriteLine(HospitalDao.Err_Select_Menu_Number);
                }
            }
        }

        public static void addNewDutyToUser(User user)
        {
            DateTime date = new DateTime(); //Tworzy nowy obiekt daty
            Console.WriteLine("Wpisz datę dyżuru (format dd/mm/yyyy):");

            bool correct = false;
            while (!correct)
            {
                string dateString = Console.ReadLine(); //Pobiera z klawiatury datę 
                if (DateTime.TryParse(dateString, out date)) //Próbuje zamienić to na obiekt daty 
                {
                    if (checkDutyByDate(date, user)) //Sprawdzenie czy spełnia kryteria podane w zadaniu
                    {
                        duties.Add(new Duty(date, user.id)); //Jeśli jest wszytsko okej zapisuje dyżur na głównej liście
                    }
                    correct = true;
                }
                else //Jesli data źle wpisana
                {
                    Console.WriteLine(HospitalDao.Err_std);
                }
            }
        }

        public static bool checkDutyByDate(DateTime date, User user)
        {
            int[] months = new int[13] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //inicjalizacja liczby dyzurów w ciagu 12 miesiecy dla wybranego usera
            foreach (Duty duty in duties) //sprawdza wszystkie dyżury
            {
                if (duty.day.Equals(date)) //Jeśli nowy ma datę taką jak inny
                {
                    if (duty.userID.Equals(user.id)) //Jeśli id usera jest identyczne
                    {
                        Console.WriteLine("Ten użytkownik już ma zaplanowany taki dyżur.");
                        Console.WriteLine("\nNacisnij dowolny przycisk");
                        Console.ReadKey();
                        return false;
                    }
                    if (typeof(Doctor).IsInstanceOfType(user)) //Jeśli sprawdzany jest lekarzem
                    {
                        User dutyUser = HospitalDao.getUserById(duty.userID); //pobiera usera po id z dyzuru aby sprawdzic pozniej jego specjalizacje
                        if (dutyUser != null
                            && typeof(Doctor).IsInstanceOfType(dutyUser) // i ten z dyżuru jest lekarzem
                            && ((Doctor)user).speciality.Equals(((Doctor)dutyUser).speciality)) // i ma tę samą specjalizację
                        {
                            Console.WriteLine("W tym dniu jest już zaplanowany dyżur lekarza z taką specjalizacją");
                            Console.WriteLine("\nNacisnij dowolny przycisk");
                            Console.ReadKey();
                            return false;
                        }
                    }
                }
                if (duty.userID.Equals(user.id) && (duty.day.Equals(date.AddDays(1)) || duty.day.Equals(date.AddDays(-1)))) //jeśli ten sam user ma zaplanowany dyzur dzien wczesniej albo pozniej
                {
                    Console.WriteLine("Nie można zaplanować dyżuru dzień po dniu.");
                    Console.WriteLine("\nNacisnij dowolny przycisk");
                    Console.ReadKey();
                    return false;
                }
                months[duty.day.Month]++; //Przy każdym znalezionym dyzurze dla wybranego usera zwiększa liczbę dyżurów w danym miesiacu w tablicy
                if (months[date.Month] >= 10) // i jesli jest 10 lub wiecej dyzurów nie pozwala dodac nowego w ty miesiacu
                {
                    Console.WriteLine("Nie można zaplanować dyżuru więcej niż 10 razy w miesiącu.");
                    Console.WriteLine("\nNacisnij dowolny przycisk");
                    Console.ReadKey();
                    return false;
                }
            }
            return true;
        }

        public static void showDutiesByUser(User user) //Wyświetlanie dyżurów dla podaneego użytkownika
        {
            userDuties.Clear(); //Wyczyszczenie pomocniczej listy 
            duties.Sort((p, q) => p.day.CompareTo(q.day)); // sortowanie na podstawie daty
            int numer = 0;
            foreach (Duty duty in duties)
            {
                if (duty.userID.Equals(user.id)) //Jeśli dyżur ma id podanego użytkownika
                {
                    numer++;
                    userDuties.Add(duty); //dodanie dużuru do listy pomocniczej - później będzie potrzebna
                    printDuty(duty, numer, user); //Wypisanie dyżuru na ekran
                }
            }
        }

        public static void printDuty(Duty duty, int number, User user) //Wypisanie sformatowanej wersji danych o dyżurze
        {
            string sNumber = number + "."; //zamiana liczby na tekst i dodana kropka
            //Poniżej formatowanie tekstu numery w nawiasach klamrowych odpowiadają kolejno zmiennym po przecinku
            //Metoda PadRight ustawia długość wyrazu, aby w konsoli wyrównać tekst
            //Widoczny format daty można analogicznie zmieniać
            Console.WriteLine(String.Format("{0} {1} {2} {3}", sNumber.PadRight(3), duty.day.Date.ToString("dd/MM/yyyy, dddd").PadRight(22), user.name, user.surname));
        }
    }
}
