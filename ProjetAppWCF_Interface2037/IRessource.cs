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

        /// <summary>
        /// Retourne une représentation de la ressource celon le context http (content-type)
        /// </summary>
        /// <returns></returns>
        string GetString();

        /// <summary>
        /// Retourne une représentation de la ressource fixé par le paramètre d'entrée format de representation
        /// </summary>
        /// <param name="formatRepresentationRessource"></param>
        /// <returns></returns>
        string GetString(string formatRepresentationRessource);

        /// <summary>
        /// Retourne le nom de la classe de la ressource
        /// </summary>
        /// <returns></returns>
        string GetNameClass();

        /// <summary>
        /// retourne le contenu de la ressource. N'inclus pas les sous ressources
        /// </summary>
        /// <returns></returns>
        string GetContenu();

        /// <summary>
        /// retourne l'identifiant de la ressource
        /// </summary>
        /// <returns></returns>
        string GetId();

        /// <summary>
        /// Retourne le nom du champ utilisé comme identifiant
        /// </summary>
        string NameChampId { get; }

        /// <summary>
        /// Retourne le chemin d'accès à la ressource
        /// </summary>
        string LienRessource { get; }
    }
}
