//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TukiVerkko1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Logins
    {
        public int LoginID { get; set; }
        [Required(ErrorMessage = "Anna käyttäjätunnus!")]
        public string Käyttäjätunnus { get; set; }
        [Required(ErrorMessage = "Anna salasana!")]
        [DataType(DataType.Password)]
        public string Salasana { get; set; }
        public string Rooli { get; set; }
        public string KirjautumisVirheIlmoitus { get; set; }
    }
}
