using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UserLibrary;

namespace Hospital
{
    public static class HospitalDao
    {
        public static List<User> users = new List<User>(); //główna lista użytkowników

        public static string Key_to_back_to_menu = "\nNaciśnij dowolny przycisk, aby zobaczyć do menu...";
        public static string Err_Select_Menu_Number = "Błąd! Wybierz numer z menu!\n";
        public static string Err_std = "Błędne dane!";

        public static void initializeData() //Wczytanie danych - metody wenątrz są wyjaśnione poniżej
        {
            users = readXML(users);
            DutyDao.duties = readXML(DutyDao.duties);
        }
        
        public static void saveData() //zapis danych - metody wenątrz są wyjaśnione poniżej
        {
            writeXML(users);
            writeXML(DutyDao.duties);
        }

        public static void writeXML<T>(List<T> data) //Serializacja przy pomocy metody generycznej to znaczy obojętnie jakiego typu będzie lista
        {
            String[] c = typeof(T).ToString().Split('.'); //Pobranie nazwy Typu serializowanych danych
            XmlSerializer writer = new XmlSerializer(typeof(List<T>)); //Utworzenie obiektu serializatora
            var path = Directory.GetCurrentDirectory() + "\\Data\\" + c[1] + ".xml"; //Utworzenie ścieżki do której zapisany będzie plik - Kombinacja bieżącego folderu\Data\[Typ].xml

            FileStream file = File.Create(path); //Utworzenie pliku
            writer.Serialize(file, data); //Serializacja - zapisanie danych przy pomocy znaczników xml
            file.Close(); //zamknięcie pliku
        }

        public static List<T> readXML<T>(List<T> data) //Deserializacja analogicznie do powyższej
        {
            String[] c = typeof(T).ToString().Split('.'); //Pobranie nazwy Typu serializowanych danych
            XmlSerializer reader = new XmlSerializer(typeof(List<T>)); //Utworzenie obiektu serializatora
            try
            {
                StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + "\\Data\\" + c[1] + ".xml"); //Otwarcie pliku z danymi - Kombinacja bieżącego folderu\Data\[Typ].xml
                data = (List<T>)reader.Deserialize(file); // Deserializacja - odczyt z pliku
                file.Close();
            }
            catch (FileNotFoundException e) //Ten wyjątek obsługiwany kiedy nie ma pliku
            {
                Console.Clear();
                Console.WriteLine("Plik " + c[1] + ".xml nie istnieje!");
                Console.ReadKey();
            }
            catch (DirectoryNotFoundException e) //Ten wyjątek obsługiwany kiedy nie ma katalogu
            {
                Console.Clear();
                Console.WriteLine("Nie odnaleziono katalogu: Dane");
                Console.WriteLine("Utwórz katalog: Dane i umieść w nim pliki .xml");
                Console.ReadKey();
            }
            catch (Exception e) //Tutaj jest obsługiwana reszta wyjątków
            {
                Console.WriteLine("Wystąpił nieoczekiwany błąd! Skontaktuj się z administratorem.\n");
                Console.ReadKey();
            }
            return data;
        }

        public static void addNewUser(User loggedUser) //Metoda z menu obsługującym dodawanie nowego użytkownika
        {
            bool cancel = false;
            while (!cancel) //Pętla od menu
            {
                Console.Clear();
                Console.WriteLine("1. Administrator");
                Console.WriteLine("2. Lekarz");
                Console.WriteLine("3. Pielęgniarka");
                Console.WriteLine("4. Anuluj");
                try
                {
                    int numer = UserDao.getIntNumber(); //pobranie numeru od uzytkownika

                    switch (numer)
                    {
                        //Numery przy "case" odpowiadają nuemrom z menu
                        case 1:
                            User administrator = new Admin(true); //Utworzenie nowego obiektu użytkownika oraz inicjalizacja danych przy uzyciu konstruktora
                            users.Add(administrator); //Dodanie uzytkownika do listy wszytskich userów
                            Console.Clear();
                            showUserList(loggedUser); //Wyświetlenie wszystkich użytkownikow
                            Console.WriteLine(Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true; //zmienna do zakonczenia pętli
                            break;

                        case 2:
                            User lekarz = new Doctor(true); //analogicznie z powyzszym kodem 
                            users.Add(lekarz);
                            Console.Clear();
                            showUserList(loggedUser);
                            Console.WriteLine(Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true;
                            break;

                        case 3:
                            User pielegniarka = new Nurse(true);
                            users.Add(pielegniarka);
                            Console.Clear();
                            showUserList(loggedUser);
                            Console.WriteLine(Key_to_back_to_menu);
                            Console.ReadKey();
                            Console.Clear();
                            cancel = true;
                            break;

                        case 4:
                            cancel = true;
                            break;

                        default: //jeśli numer wiekszy niz w menu
                            break;
                    }
                }
                catch (Exception) //Jeśli uzytkownik wpisal litery
                {
                    Console.Clear();
                    Console.WriteLine(Err_Select_Menu_Number);
                }
            }
        }

        public static void userEdit(User loggedUser) //menu edycji uzytkownika 
        {
            Console.Clear();
            showUserList(loggedUser); //wyswietla liste uzytkownikow 
            Console.WriteLine("\n\nWpisz id użytkownika, aby przejść do edycji jego danych:");
            long id = UserDao.getLongNumber(); //pobiera id od uzytkownika 
            User userToEdit = getUserById(id); //zwraca usera na podstawie podanego ID
            if (userToEdit != null) //jesli powyzsza zwrocila usera
            {
                Console.Clear();
                bool cancel = false;
                while (!cancel)
                {
                    userToEdit.printUser(1); //wypisuje wybranego uzytkownika 
                    Console.WriteLine("\nWybierz dane które chesz edytować:");
                    Console.WriteLine("1. Imię");
                    Console.WriteLine("2. Nazwisko");
                    Console.WriteLine("3. PESEL");
                    Console.WriteLine("4. Nazwa użytkownika");
                    Console.WriteLine("5. Hasło");
                    Console.WriteLine("6. ID");
                    if (!typeof(Admin).IsInstanceOfType(userToEdit)) //opcje tylko kiedy wybrany lekarz i pielegniarka
                    {
                        Console.WriteLine("7. Dyżury");
                        if (typeof(Doctor).IsInstanceOfType(userToEdit)) //opcje tylko kiedy wybrany lekarz 
                        {
                            Console.WriteLine("8. Numer PWZ");
                            Console.WriteLine("9. Specjalność");
                        }
                    }
                    Console.WriteLine("10. Anuluj");

                    int numer = UserDao.getIntNumber(); //Pobranie liczby menu

                    switch (numer)
                    {
                        case 1: //Poniżej wybranie liczby z menu oraz uzupełanianie odpowiednio nowych danych
                            userToEdit.setNewName();
                            cancel = true;
                            break;

                        case 2:
                            userToEdit.setNewSurname();
                            cancel = true;
                            break;

                        case 3:
                            userToEdit.setNewPesel();
                            cancel = true;
                            break;

                        case 4:
                            userToEdit.setNewUsername();
                            cancel = true;
                            break;

                        case 5:
                            userToEdit.setNewPassword();
                            cancel = true;
                            break;

                        case 6:
                            userToEdit.setNewID();
                            cancel = true;
                            break;

                        case 7:
                            if (!typeof(Admin).IsInstanceOfType(userToEdit)) //Jesli nie admin
                            {
                                DutyDao.dutyEditByUser(userToEdit);//Przejscie do edycji dyżurów
                                cancel = true;
                            }
                            else //Jeśli wybrany user nie jest lekarzem lub pielegniarka 
                            {
                                Console.Clear();
                                Console.WriteLine(Err_Select_Menu_Number);
                            }
                            break;

                        case 8:
                            if (typeof(Doctor).IsInstanceOfType(userToEdit)) //jesli lekarz
                            {
                                ((Doctor)userToEdit).setNewPwzNumber();
                                cancel = true;
                            }
                            else//Jeśli wybrany user nie jest lekarzem lub pielegniarka 
                            {
                                Console.Clear();
                                Console.WriteLine(Err_Select_Menu_Number);
                            }
                            break;

                        case 9:
                            if (typeof(Doctor).IsInstanceOfType(userToEdit))//jesli lekarz
                            {
                                ((Doctor)userToEdit).setNewSpeciality();
                                cancel = true;
                            }
                            else//Jeśli wybrany user nie jest lekarzem lub pielegniarka 
                            {
                                Console.Clear();
                                Console.WriteLine(Err_Select_Menu_Number);
                            }
                            break;

                        case 10:
                            Console.Clear();
                            cancel = true;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine(Err_Select_Menu_Number);
                            break;
                    }
                }
            }
            else // jesli podane błędne ID
            {
                Console.WriteLine("W systemie nie ma użytkownika o takim ID");
            }
        }

        public static void showUserList(User loggedUser) //Wyświetla wszytskich użytkowników
        {
            Console.WriteLine("Wszyscy dostępni użytkownicy:");
            int numer = 0;
            foreach (User user in users)
            {
                if (loggedUser.right.Equals(User.Right.HIGH) && typeof(Admin).IsInstanceOfType(user)) //Jeśli zalogowany ma wysokie uprawnienia to widzi też Adminów
                {
                    numer++;
                    user.printUser(numer); //metoda w klasie User wypisująca sformatowany tekst z danymi użytkownika
                }
                if (!typeof(Admin).IsInstanceOfType(user)) //Wyświetlanie wszytskich poza adminami
                {
                    numer++;
                    user.printUser(numer);
                }
            }
        }

        public static User getUserByUsernamePassword(String username, String password)
        {
            foreach (User user in users) // Sprawdza na całej liście użytkowników
            {
                if (user.username.Equals(username) && user.password.Equals(password)) //Jeśli znajdzie użytkownika o zadanym login i haśle
                    return user; // zwraca uzytkownika
            }
            return null;
        }

        public static User getUserById(long id) // Pobiera uzytkownika na podstawie zadanego ID
        {
            foreach (User user in users)
            {
                if (user.id.Equals(id)) //Zwraca pierwszego uzytkownika o zadanym ID
                    return user;
            }
            return null;
        }


    }

}
