using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public static class RessourceFactory
    {
        public static Ressource Fabriquer(HttpContext context)
        {
            Ressource laRessource;

            switch (context.Request.Url.AbsolutePath.Split('/').Last())
            {
                case "Question":
                    laRessource = new Question();
                    break;
                case "Reponse":
                    laRessource = new Reponse();
                    break;
                default:
                    laRessource = null;
                    break;
            }

            return laRessource;
        }
    }
}