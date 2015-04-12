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

        public static void AjouterCodeHeaderReponse(int code, IRessource laRessource)
        {
            switch (code)
            {
                case 201: // Création d'une ressource
                    string chaineLienConsultation = string.Format("http://{3}:{4}/{0}?{1}={2}", laRessource.GetNameClass(), laRessource.NameChampId, laRessource.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);

                    HttpContext.Current.Response.AppendHeader("Location", chaineLienConsultation);
                    HttpContext.Current.Response.StatusCode = code;
                    break;
                default:
                    HttpContext.Current.Response.StatusCode = code;
                    break;
            }
        }
    }
}