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
    
    public partial class Tiketit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tiketit()
        {
            this.Asiakkaat = new HashSet<Asiakkaat>();
        }
    
        public int TikettiID { get; set; }
        public int KategoriaID { get; set; }
        public string Otsikko { get; set; }
        public string Kuvaus { get; set; }
        public System.DateTime Aika { get; set; }
        public Nullable<System.DateTime> Valmistumisaika { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asiakkaat> Asiakkaat { get; set; }
        public virtual Kategoriat Kategoriat { get; set; }
    }
}