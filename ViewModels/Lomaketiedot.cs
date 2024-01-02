
//using TukiVerkko1.Models;

namespace TukiVerkko1.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class Lomaketiedot
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
        public Nullable<System.DateTime> Aika { get; set; }
        public Nullable<System.DateTime> Valmistumisaika { get; set; }
        public string Nimi { get; set; }
        //public string Status { get; set; }

    }
    
}