using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.

    public class RestFullSystemExpert : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            //switch (context.Request.HttpMethod)
            //{
            //    case "POST":
            //        Creer(context);
            //        break;
            //    case "GET":
            //        Consulter(context);
            //        break;
            //    case "DELETE":
            //        Supprimer(context);
            //        break;
            //    case "PUT":
            //        MiseAJour(context);
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}
