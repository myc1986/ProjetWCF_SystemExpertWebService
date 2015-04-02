using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public class ManagerHeader
    {
        protected IDictionary<string, string> _mesEntetes;

        public static void DefinirRessourceCible(string chemin)
        {

        }

        public static void SupprimerEntete(string nomEntete)
        {
            HttpContext.Current.Response.Headers.Remove(nomEntete);
        }

        public static void AjouterEntete(string nomEntete, string valeurEntete)
        {
            HttpContext.Current.Response.Headers.Add(nomEntete, valeurEntete);
        }

        public static void ModifierEntete(string nomEntete, string valeurEntete)
        {
            HttpContext.Current.Response.Headers.Set(nomEntete, valeurEntete);
        }
    }
}