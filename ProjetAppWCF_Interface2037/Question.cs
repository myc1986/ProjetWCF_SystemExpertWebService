using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ProjetAppWCF_Interface2037
{
    public class Question : Ressource
    {
        protected EntiteQuestion _maQuestion;
        protected Reponse _maReponse;

        public string Id { 
            get 
            {
                if (_maQuestion.Id.Equals(0))
                {
                    return "";
                }

                return _maQuestion.Id.ToString();
            } 

            set { _maQuestion.Id = int.Parse(value); } 
        }

        public string QuestionContenu { get { return _maQuestion.Contenu; } set { _maQuestion.Contenu = value; } }
        public Reponse ReponseString { get { return _maReponse; } set { _maReponse = value; } }

        protected Question()
        {
            _maQuestion = new EntiteQuestion();
            _nameChampId = "question_id";
        }

        public Question(HttpContext context)
        {
            _maQuestion = new EntiteQuestion();
            _maReponse = new Reponse(context);
            _monContextHttp = context;
            _nameChampId = "question_id";
        }

        public Question(int id, HttpContext context)
        {
            _maQuestion = new EntiteQuestion();

            using (bdd_service_web bdd = new bdd_service_web())
            {
                var uneQuestion = bdd.questions.Where(pp => pp.Id == id).FirstOrDefault();

                if (uneQuestion != null)
                {
                    _maQuestion = uneQuestion;
                }
                else
                {
                    throw new Exception(string.Format("{0} : cet identifiant n'existe pas. Détail :\n{1}", id));
                }
            }

            _nameChampId = "question_id";
        }

        public override void Creer(HttpContext context)
        {

            //HtmlString mm = new HtmlString(sContent);


            if (String.IsNullOrEmpty(context.Request["question_contenu"]))
            {
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi de contenu", "question_contenu"));
            }

            //Gestion cache à faire

            EntiteQuestion myEntity = new EntiteQuestion();
            myEntity.Contenu = context.Request.Params.Get("question_contenu").ToString();

            using (bdd_service_web bdd = new bdd_service_web())
            {
                bdd.questions.Add(myEntity);
                bdd.SaveChanges();
                _maQuestion = myEntity;

                ManagerHeader.AjouterCodeHeaderReponse(201, this);
            }
        }

        public override void Consulter(HttpContext context)
        {
            if (!context.Request.Params.AllKeys.Contains("question_id"))
            {
                //ManagerHeader.ModifierEntete("Content-type", HttpContext.Current.Request.ContentType);
                //ManagerHeader.ModifierEntete("CodeStatus", "400");
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi d'identifiant", "question_id"));
            }
            
            int val = 0;

            if (!int.TryParse(context.Request.Params.Get("question_id"), out val))
            {
                //ManagerHeader.ModifierEntete("Content-type", HttpContext.Current.Request.ContentType);
                //ManagerHeader.ModifierEntete("CodeStatus", "400");
                throw new HttpException(400, string.Format("{0} : Vous devez saisir un entier", "question_id"));
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                try
                {
                    var uneQuestion = bdd.questions.Where(pp => pp.Id == val).FirstOrDefault();
                    
                    if (uneQuestion != null)
                    {
                        _maQuestion = uneQuestion;

                        if (uneQuestion.reponses.Count > 0)
                        {
                            _maReponse = new Reponse(_monContextHttp);
                            _maReponse.Consulter(_monContextHttp);
                        }
                        else
                        {
                            _maReponse = new Reponse(_monContextHttp);
                        }
                    }
                    else
                    {
                        //ManagerHeader.ModifierEntete("Content-type", HttpContext.Current.Request.ContentType);
                        //ManagerHeader.ModifierEntete("CodeStatus", "400");

                        throw new HttpException(404, string.Format("La question N°{0} n'existe pas.", val));
                    } 
                }
                catch (Exception e)
                {
                    //ManagerHeader.ModifierEntete("Content-Type", HttpContext.Current.Request.ContentType);
                    //ManagerHeader.ModifierEntete("status", "400");
                    
                    throw new HttpException(e.GetHashCode(), string.Format("<h2>Question N°{0}</h2><p>Détail: {1}</p>", val, e.Message));
                }               
            }
        }

        public override void Supprimer(HttpContext context)
        {
            if (String.IsNullOrEmpty(context.Request.Params.Get("question_id").ToString()))
            {
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi d'identifiant", "question_id"));
            }

            int val = 0;

            if (!int.TryParse(context.Request.Params.Get("question_id").ToString(), out val))
            {
                throw new HttpException(400, string.Format("{0} : Vous devez saisir un entier", "question_id"));
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                try
                {
                    var uneQuestion = bdd.questions.Where(pp => pp.Id == val).FirstOrDefault();

                    if (uneQuestion != null)
                    {
                        _maQuestion = uneQuestion;
                        _maQuestion.Contenu = context.Request.Params.Get("question_contenu").ToString();
                    }
                    else
                    {
                        throw new HttpException(404, string.Format("La question n°{0} n'existe pas.", val));
                    }

                    bdd.questions.Remove(_maQuestion);
                    bdd.SaveChanges();

                    HttpContext.Current.Response.StatusCode = 204;
                }
                catch (Exception e)
                {
                    //ManagerHeader.ModifierEntete("Content-Type", HttpContext.Current.Request.ContentType);
                    //ManagerHeader.ModifierEntete("status", "400");

                    throw new HttpException(e.GetHashCode(), string.Format("<h2>Question N°{0}</h2><p>Détail: {1}</p>", val, e.Message));
                } 
            }
        }

        public override void MiseAJour(HttpContext context)
        {
            if (!context.Request.Form.AllKeys.Contains("question_id"))
            {
                throw new HttpException(400, string.Format("{0} : Dans votre formulaire, vous n'avez pas de champ dont l'identifiant est '{0}'.", "question_id"));
            }

            if (String.IsNullOrEmpty(context.Request["question_id"]))
            {
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi d'identifiant", "question_id"));
            }

            if (!context.Request.Form.AllKeys.Contains("question_contenu"))
            {
                throw new HttpException(400, string.Format("{0} : Dans votre formulaire, vous n'avez pas de champ dont l'identifiant est '{0}'.", "question_contenu"));
            }

            if (String.IsNullOrEmpty(context.Request["question_contenu"]))
            {
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi de contenu", "question_contenu"));
            }

            int val = 0;

            if (!int.TryParse(context.Request["question_id"], out val))
            {
                throw new HttpException(400, string.Format("{0} : Vous devez saisir un entier", "question_id"));
            }

            string contenuQuestion = context.Request["question_contenu"];
            string contenuReponse = "";

            if (context.Request.Form.AllKeys.Contains("reponse_contenu"))
            {
                if (String.IsNullOrEmpty(context.Request["reponse_contenu"]))
                {
                    throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi de contenu", "reponse_contenu"));
                }

                contenuReponse = context.Request["reponse_contenu"];
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                try
                {
                    if (val > 0)
                    {
                        var uneQuestion = bdd.questions.Where(pp => pp.Id == val).FirstOrDefault();

                        if (uneQuestion != null)
                        {
                            _maQuestion = uneQuestion;
                            _maQuestion.Contenu = contenuQuestion;

                            if (_maQuestion.reponses.Count > 0)
                            {
                                _maQuestion.reponses.First().Contenu = contenuReponse;
                                bdd.SaveChanges();
                            }
                            else
                            {
                                if (contenuReponse.Length > 0)
                                {
                                    EntiteReponse uneReponse = new EntiteReponse();
                                    uneReponse.Contenu = contenuReponse;
                                    uneReponse.question_fid = val;
                                    bdd.reponses.Add(uneReponse);
                                    bdd.SaveChanges();

                                    _maReponse.Consulter(context);
                                }
                            }
                        }
                        else
                        {
                            throw new HttpException(404, string.Format("{0} : La question n°{0} n'existe pas.", val));
                        }
                    }
                    else
                    {
                        throw new HttpException(404, string.Format("{0} : La question n°{1} n'existe pas.", "question_id", val));
                    }
                }
                catch (Exception e)
                {
                    throw new HttpException(e.GetHashCode(), string.Format("<h2>Question N°{0}</h2><p>Détail: {1}</p>", val, e.Message));
                }
            }
        }

        public override string GetString()
        {
            return RepresentationFactory.GetRepresentation(_monContextHttp, this);
        }

        public override string GetString(string formatRepresentationRessource)
        {
            throw new NotImplementedException();
        }

        public override string GetNameClass()
        {
            return this.GetType().Name;
        }

        public override string GetContenu()
        {
            return _maQuestion.Contenu;
        }

        public bool AUneReponse()
        {
            bool rep = false;

            if(_maQuestion.reponses.Count > 0)
            {
                var uneReponse = _maQuestion.reponses.FirstOrDefault();

                if (uneReponse != null)
                {
                    if (uneReponse.Id.ToString().Equals(0))
                    {
                        rep = false;
                    }
                    else
                    {
                        rep = true;
                    }
                }
                else
                {
                    rep = false;
                }
            }

            return rep;
        }

        public override string GetId()
        {
            return Id;
        }
    }
}