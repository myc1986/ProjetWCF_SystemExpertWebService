using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public abstract class Ressource : IRessource
    {
        public abstract void Creer(HttpContext context);

        public abstract void Consulter(HttpContext context);

        public abstract void Supprimer(HttpContext context);

        public abstract void MiseAJour(HttpContext context);
    }
}