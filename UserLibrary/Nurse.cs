using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserLibrary
{
    public class Nurse : User
    {
        //Poniżej konstruktory klasy -base to odwołanie do konstruktora klasy nadrzędnej Pusty jest wymagany do serializacji
        public Nurse() { } 

        public Nurse(string name, string surname, long pesel, string username, string password) : base(name, surname, pesel, username, password)
        { }

        public Nurse(bool newUser) : base(newUser)
        { }

        public override string getName() //Metoda przeciązona po metodzie abstrakcyjnej z klasu User
        {
            return "Pielegniarka"; // Dopisuje nazwe posady do printUser
        }

        public override void setRights() //Tak jak wyzej 
        {
            this.right = Right.LOW; //Ustawia zawsze taką wartość uprawnien podczas tworzenianowego usera
        }
    }
}
