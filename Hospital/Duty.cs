using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class Duty
    {
        //Klasa dyżurów, poniżej pola dyżuru i poźniej konstruktory
        public DateTime day;
        public long userID;

        public Duty() { }

        public Duty(DateTime day, long userId) {
            this.day = day;
            this.userID = userId;
        }

    }
}
