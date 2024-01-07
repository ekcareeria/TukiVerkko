using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TukiVerkko1.Models
{
    public class LoginRoolit1 : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Tarkistetaan käyttäjän rooli
            string käyttäjänRooli = HttpContext.Current.Session["Rooli"] as string;

            if (käyttäjänRooli != null && (käyttäjänRooli.Equals("Ylläpitäjä", StringComparison.OrdinalIgnoreCase) || käyttäjänRooli.Equals("Opiskelija", StringComparison.OrdinalIgnoreCase)))
            {
                return true; // Palauta true, jos käyttäjällä on oikea rooli
            }

            return false; // Palauta false,jos käyttäjällä ei ole oikeaa roolia
        }
    }
}