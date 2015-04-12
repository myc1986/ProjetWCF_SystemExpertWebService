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

        public abstract string GetString();

        public abstract string GetString(string formatRepresentationRessource);

        protected HttpContext _monContextHttp;

        public abstract string GetNameClass();

        public abstract string GetContenu();

        public abstract string GetId();

        protected string _nameChampId;

        public string NameChampId
        {
            get { return _nameChampId; }
        }


        public string LienRessource
        {
            get 
            { 
                return string.Format("http://{3}:{4}/{0}?{1}={2}", this.GetNameClass(), this.NameChampId, this.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);
            }
        }
    }
}