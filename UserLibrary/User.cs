using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLibrary
{
    [Serializable()]
    [System.Xml.Serialization.XmlInclude(typeof(Admin))]
    [System.Xml.Serialization.XmlInclude(typeof(Nurse))]
    [System.Xml.Serialization.XmlInclude(typeof(Doctor))]
    public abstract class User
    {
        public long id;
        public string name;
        public string surname;
        public long pesel;
        public string username;
        public string password;
        public Right right;

        public User() { } //Konstruktor domyślny - pusty

        //Wszytskie konstruktory oczywiście są rozszerzone w klasach dziedziczących - np lekarz
        protected User(string name, string surname, long pesel, string username, string password) //Konstruktor nie używany podczas wykonywania aplikacji - użyty tyko podczas generowania przykłądowych użytkowników kiedy nie było danych
        {
            this.name = name;
            this.surname = surname;
            this.pesel = pesel;
            this.username = username;
            this.password = password;
            setRights();
        }

        public User(bool newUser) //konstruktor do inicjalizacji przy dodawaniu nowego użytkownika z poziomu aplikacji
        {
            //dodawanie poszczególneych dancyh
            setNewName();
            setNewSurname();
            setNewPesel();
            setNewUsername();
            setNewPassword();
            setNewID();
            setRights();
        }

        public enum Right //enum - kolejne wpisy mają koljne numery (HIGH = 0 , LOW = 1) tylko dla usystematyzowania i wyeliminowania pomyłek w kodzie
        {
            HIGH,
            LOW
        }

        public abstract string getName(); //Metody abstrakcyjne nie mogą posiadać ciała(kodu) i muszą być nadpisane w klasie dziedziczącej np lekarz 

        public abstract void setRights();

        public virtual void printUser(int number) //Metoda wirtualna - może posaidać ciało (kod), ale wykonana może być tylko na obiekcie dziedziczącym czyli np Lekarz
        {
            string sNumber = number + "."; //zamiana numeru na tekst i dodanie kropki
            //Poniżej formatowanie tekstu numery w nawiasach klamrowych odpowiadają kolejno zmiennym po przecinku
            //Metoda PadRight ustawia długość wyrazu, aby w konsoli wyrównać tekst
            Console.WriteLine(String.Format("{0} ID[{1}] - {2} {3} [{4}]", sNumber.PadRight(3), this.id, this.name, this.surname, this.getName())); 
        }

        public void setNewName() //Dodawanie imienia itd poniżej
        {
            Console.WriteLine("\nPodaj imię: ");
            this.name = Console.ReadLine();
        }

        public void setNewSurname()
        {
            Console.WriteLine("\nPodaj nazwisko: ");
            this.surname = Console.ReadLine();
        }

        public void setNewPesel()
        {
            Console.WriteLine("\nPodaj PESEL: ");
            long newPesel = UserDao.getLongNumber(); //Tu jest odrobinę inaczej - ponieważ należy sprawdzić czy na pewno użytkownik wpisał liczbę
            this.pesel = newPesel;
        }

        public void setNewUsername()
        {
            Console.WriteLine("\nPodaj nazwę użytkownika: ");
            this.username = Console.ReadLine();
        }

        public void setNewPassword()
        {
            Console.WriteLine("\nPodaj hasło: ");
            this.password = Console.ReadLine();
        }
        
        public void setNewID()
        {
            Console.WriteLine("\nPodaj ID: "); 
            long newId = UserDao.getLongNumber(); //Tu jest odrobinę inaczej - ponieważ należy sprawdzić czy na pewno użytkownik wpisał liczbę
            this.id = newId;
        }

    }
}
