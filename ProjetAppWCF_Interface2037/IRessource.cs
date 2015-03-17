using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public interface IRessource
    {
        /// <summary>
        /// Crée la ressource et retourne la réponse à un POST.
        /// </summary>
        /// <param name="context">Context http</param>
        void Creer(HttpContext context);
        /// <summary>
        /// Retourne la réponse suite à une requête GET de la ressource
        /// </summary>
        /// <param name="context">Context http</param>
        void Consulter(HttpContext context);
        /// <summary>
        /// Supprimer la ressource et retourne la réponse suite à un DELETE
        /// </summary>
        /// <param name="context"></param>
        void Supprimer(HttpContext context);
        /// <summary>
        /// Mise à jour de la ressource et retourne la réponse suite à un PUT
        /// </summary>
        /// <param name="context"></param>
        void MiseAJour(HttpContext context);
    }
}
