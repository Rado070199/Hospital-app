using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLibrary
{
    public class Admin : User
    {
        //Poniżej konstruktory klasy -base to odwołanie do konstruktora klasy nadrzędnej Pusty jest wymagany do serializacji
        public Admin() { }

        public Admin(string name, string surname, long pesel, string username, string password) : base(name, surname, pesel, username, password)
        { }

        public Admin(bool newUser) : base(newUser) 
        { }

        public override string getName()//Metoda przeciązona po metodzie abstrakcyjnej z klasu User
        {
            return "Administrator";// Dopisuje nazwe posady do printUser
        }

        public override void setRights()//Tak jak wyzej 
        {
            this.right = Right.HIGH;//Ustawia zawsze taką wartość uprawnien podczas tworzenianowego usera
        }
    }
}
