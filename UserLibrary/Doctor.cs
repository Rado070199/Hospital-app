using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLibrary
{
    public class Doctor : User
    {
        public Speciality speciality; //Pola tylko dla lekarza
        public long PWZnumber;

        //Poniżej konstruktory klasy -base to odwołanie do konstruktora klasy nadrzędnej Pusty jest wymagany do serializacji
        public Doctor() { }

        public Doctor(string name, string surname, long pesel, string username, string password, long PWZnumber, Speciality speciality) : base(name, surname, pesel, username, password)
        {
            this.speciality = speciality;
            this.PWZnumber = PWZnumber;
        }

        public Doctor(bool newUser) : base(newUser)
        {
            setNewPwzNumber();
            setNewSpeciality();
        }

        public void setNewPwzNumber() //ustawia numer PWZ
        {
            Console.WriteLine("\nPodaj numer PWZ: ");
            long newPWZnumber = UserDao.getLongNumber(); //Pobierai sprawdza poprawność liczbową
            this.PWZnumber = newPWZnumber;
        }

        public override void setRights() 
        {
            this.right = Right.LOW;//Ustawia zawsze taką wartość uprawnien podczas tworzenianowego usera
        }

        public void setNewSpeciality() //Ułatwia ustawianie specjalizacji podczas dodawnia nowego lekarza
        {
            int numer = 0;
            while (true)
            {
                Console.WriteLine("\nWybierz specjalizację: ");
                Console.WriteLine("1. " + Speciality.KARDIOLOG.ToString());
                Console.WriteLine("2. " + Speciality.UROLOG.ToString());
                Console.WriteLine("3. " + Speciality.NEUROLOG.ToString());
                Console.WriteLine("4. " + Speciality.LARYNGOLOG.ToString());
                numer = UserDao.getIntNumber(); //Pobiera liczbe od uzytkownika 
                if (numer <= 4 && numer >= 1) //sprawdza czy miesci sie w menu
                {
                    numer--; //zmienjsza liczbę o jeden ponieważ enum Speciality zaczyna się od 0
                    break;
                }
                else
                    Console.WriteLine("Błąd! Wpisz poprawną liczbę!");
            }
            this.speciality = (Speciality)numer;
        }

        public enum Speciality //analogicznie do usera //enum - kolejne wpisy mają koljne numery (HIGH = 0 , LOW = 1) tylko dla usystematyzowania i wyeliminowania pomyłek w kodzie
        {
            KARDIOLOG,
            UROLOG,
            NEUROLOG,
            LARYNGOLOG
        }

        public override string getName() //Metoda przeciązona po metodzie abstrakcyjnej z klasu User
        {
            return "Lekarz - " + speciality.ToString(); // Dopisuje nazwe posady i specjalnosci do printUser
        }

    }
}
