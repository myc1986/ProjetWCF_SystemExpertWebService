using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetAppWCF_Interface2037
{

    public class Reponse : Ressource
    {
        protected EntiteReponse _maReponse;
        public int Id { get { return _maReponse.id; } set { _maReponse.id = value; } }
        public string ReponseContenu { get { return _maReponse.reponse; } set { _maReponse.reponse = value; } }

        public override void Creer(System.Web.HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void Consulter(System.Web.HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void Supprimer(System.Web.HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void MiseAJour(System.Web.HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}
