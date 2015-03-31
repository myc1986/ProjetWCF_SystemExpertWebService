using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public class RestServiceWebSystemExpert : IHttpHandler
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
            IRessource maRessource = RessourceFactory.Fabriquer(context);

            switch (context.Request.HttpMethod)
            {
                case "POST":
                    maRessource.Creer(context);
                    break;
                case "GET":
                    maRessource.Consulter(context);
                    break;
                case "DELETE":
                    maRessource.Supprimer(context);
                    break;
                case "PUT":
                    maRessource.MiseAJour(context);
                    break;
                default:
                    break;
            }

            context.Response.Write(maRessource.GetString());
        }
    }
}