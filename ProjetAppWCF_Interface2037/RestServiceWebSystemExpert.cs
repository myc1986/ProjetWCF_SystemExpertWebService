using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public class RestServiceWebSystemExpert : IHttpHandler
    {
        public RestServiceWebSystemExpert()
        {
            NegociationRepresentation.InitMimes();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
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
            catch (Exception e)
            {
                context.Response.Write(string.Format("<!DOCTYPE html><html><head><title>Erreur {0} : HashCode {1}</title></head><body><div id='messageErreur'><h1>Erreur {0} : HashCode {1}</h1><p id='corpsMessage'>{2}</p></div></body></html>", HttpContext.Current.Response.Status, e.GetHashCode(), e.Message));
            }
            finally
            {
                HttpContext.Current.Response.Cache.SetVaryByCustom(string.Format("AllParams=[{0}];AllFormParams=[{1}]", HttpContext.Current.Request.Params, HttpContext.Current.Request.Form));
                HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
                HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow);
                HttpContext.Current.Response.Cache.SetLastModified(DateTime.UtcNow);
                HttpContext.Current.Response.Cache.SetETagFromFileDependencies();
            }
        }
    }
}