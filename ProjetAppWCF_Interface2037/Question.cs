using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public class Question : Ressource
    {
        protected EntiteQuestion _maQuestion;
        protected Reponse _maReponse;

        public int Id { get { return _maQuestion.id; } set { _maQuestion.id = value; } }
        public string QuestionContenu { get { return _maQuestion.question; } set { _maQuestion.question = value; } }
        public Reponse ReponseString { get { return _maReponse; } set { _maReponse = value; } }

        public Question()
        {
            _maQuestion = new EntiteQuestion();
            _maReponse = new Reponse();
        }

        public Question(int id)
        {
            _maQuestion = new EntiteQuestion();
        }

        public override void Creer(HttpContext context)
        {
            throw new NotImplementedException();
        } 

        public override void Consulter(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void Supprimer(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public override void MiseAJour(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}