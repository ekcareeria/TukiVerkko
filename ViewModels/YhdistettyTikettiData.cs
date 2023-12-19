namespace TukiVerkko1.ViewModels
{
    using System;                               //ViewModel kansioon luotu class YhdistettyTikettiData. Mallissa turhat using-lauseet poistettiin ja tarvittavat siirrettiin tähän eli ei enää tuolla ylimmäisenä.
    using System.ComponentModel.DataAnnotations;

    public class YhdistettyTikettiData
    {
        public int AsiakasID { get; set; }      //Tarvittavat rivit tietokannasta, eri tauluista.
        public int TikettiID { get; set; }
        public string Etunimi { get; set; }
        public string Sukunimi { get; set; }
        public string Puhelinnumero { get; set; }
        public string Sähköposti { get; set; }
        public int KategoriaID { get; set; }
        public string Otsikko { get; set; }
        public string Kuvaus { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> Aika { get; set; }
        public Nullable<System.DateTime> Valmistumisaika { get; set; }
        public string Nimi { get; set; }
        public string Status { get; set; }

    }

}