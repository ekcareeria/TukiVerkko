//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

namespace TukiVerkko1.ViewModels
{
    public class Lomaketiedot
    {
        namespace TukiVerkko1.ViewModels
    {
        using System;
        using TukiVerkko1.Models;
        public class Lomake
        {
            public int AsiakasID { get; set; }
            public int TikettiID { get; set; }
            public string Etunimi { get; set; }
            public string Sukunimi { get; set; }
            public string Puhelinnumero { get; set; }
            public string Sähköposti { get; set; }
            public int KategoriaID { get; set; }
            public string Otsikko { get; set; }
            public string Kuvaus { get; set; }
            public DateTime Aika { get; set; }
            public Nullable<System.DateTime> Valmistumisaika { get; set; }
            public string Nimi { get; set; }    // tämä on kategorian nimi

        }
    }
}
}