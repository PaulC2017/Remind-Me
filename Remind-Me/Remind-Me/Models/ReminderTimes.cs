using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace RemindMe.Models
{
    public class ReminderTimes
    {
        public int ID { get; set; }
        public string ReminderTimesName { get; set; }

        public IList<RecurringReminders> Reminders { get; set; }
        /*
        the naming convention says the private variable should start with lower case letters
         and the public variable should start with the upper case letters.
        I did the reverse - so I wrote this note to document the naming convention
         for future projects

        private string SkipThisOne = "Skip This One";
        public string skipThisOne { get { return SkipThisOne; } set { SkipThisOne = value; } }

        private string OneAm = "01:00 AM";
        public string oneAm { get { return OneAm; } set { OneAm = value; } }

        private string TwoAm = "02:00 AM";
        public string twoAm { get { return TwoAm; } set { TwoAm = value; } }

        private string ThreeAm = "03:00 AM";
        public string threeAm { get { return ThreeAm; } set { ThreeAm = value; } }

        private string FourAm = "04:00 AM";
        public string fourAm { get { return FourAm; } set { FourAm = value; } }

        private string FiveAm = "05:00 AM";
        public string fiveAm { get { return FiveAm; } set { FiveAm = value; } }

        private string SixAm = "06:00 AM";
        public string sixAm { get { return SixAm; } set { SixAm = value; } }

        private string SevenAm = "07:00 AM";
        public string sevenAm { get { return SevenAm; } set { SevenAm = value; } }

        private string EightAm = "08:00 AM";
        public string eightAm { get { return EightAm; } set { EightAm = value; } }

        private string NineAm = "09:00 AM";
        public string nineAm { get { return NineAm; } set { NineAm = value; } }

        private string TenAm = "10:00 AM";
        public string tenAm { get { return TenAm; } set { TenAm = value; } }

        private string ElevenAm = "11:00 AM";
        public string elevenAm { get { return ElevenAm; } set { ElevenAm = value; } }

        private string Noon = "12:00 PM";
        public string noon { get { return Noon; } set { Noon = value; } }

        private string OnePm = "01:00 PM";
        public string onePm { get { return OnePm; } set { OnePm = value; } }

        private string TwoPm = "02:00 PM";
        public string twoPm { get { return TwoPm; } set { TwoPm = value; } }

        private string ThreePm = "03:00 PM";
        public string threePm { get { return ThreePm; } set { ThreePm = value; } }

        private string FourPm = "04:00 PM";
        public string fourPm { get { return FourPm; } set { FourPm = value; } }

        private string FivePm = "05:00 PM";
        public string fivePm { get { return FivePm; } set { FivePm = value; } }

        private string SixPm = "06:00 PM";
        public string sixPm { get { return SixPm; } set { SixPm = value; } }

        private string SevenPm = "07:00 PM";
        public string sevenPm { get { return SevenPm; } set { SevenPm = value; } }

        private string EightPm = "08:00 PM";
        public string eightPm { get { return EightPm; } set { EightPm = value; } }

        private string NinePm = "09:00 PM";
        public string ninePm { get { return NinePm; } set { NinePm = value; } }

        private string TenPm = "10:00 PM";
        public string tenPm { get { return TenPm; } set { TenPm = value; } }

        private string ElevenPm = "11:00 PM";
        public string elevenPm { get { return ElevenPm; } set { ElevenPm = value; } }

        private string Midnight = "12:00 AM";
        public string midnight { get { return Midnight; } set { Midnight = value; } }

        public IList<RecurringReminders> Reminders { get; set; }
        */
        //Default Constructor
        public ReminderTimes()
        {
        }

    }
}
