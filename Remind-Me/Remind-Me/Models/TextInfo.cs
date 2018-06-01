using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindMe.Models
{
    public class TextInfo
    {
        public int ID { get; set; }
        public string TextUserId { get; set; }  // this is the Text User ID
        public string TextToken { get; set; } // this is the Text Token
        public string TextSecret { get; set; } //this is the Text Secret
        public string TextFrom { get; set; } // this is the Text From Number

        //default constructor
        public TextInfo()
        {

            TextUserId = "";
            TextToken = "";
            TextSecret = "";
            TextFrom = "";
        }
    }
}
