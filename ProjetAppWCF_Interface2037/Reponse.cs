using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ProjetAppWCF_Interface2037
{

    public class Reponse : Ressource
    {
        protected EntiteReponse _maReponse;

        public string Id { 
            get {
                if (_maReponse.Id.Equals(0))
                {
                    return "";
                }

                return _maReponse.Id.ToString(); 
            } 
            set { _maReponse.Id = int.Parse(value); } 
        }

        public string ReponseContenu { get { return _maReponse.Contenu; } set { _maReponse.Contenu = value; } }

        protected Reponse()
        {
            _maReponse = new EntiteReponse();
            _nameChampId = "reponse_id";
        }

        public Reponse(HttpContext context)
        {
            _maReponse = new EntiteReponse();
            _monContextHttp = context;
            _nameChampId = "reponse_id";
        }

        public Reponse(string id, HttpContext context)
        {
            _maReponse = new EntiteReponse();
            _monContextHttp = context;
            _nameChampId = "reponse_id";
        }

        public override void Creer(System.Web.HttpContext context)
        {
            if (!context.Request.Form.AllKeys.Contains("question_fid"))
            {
                HttpContext.Current.Response.StatusCode = 400;
                throw new HttpException(400, string.Format("{0} : Dans votre formlaire, vous n'avez de champ dont l'identifiant est '{0}'.", "question_fid"));
            }

            if (!context.Request.Form.AllKeys.Contains("reponse_contenu"))
            {
                HttpContext.Current.Response.StatusCode = 400;
                throw new HttpException(400, string.Format("{0} : Dans votre formlaire, vous n'avez de champ dont l'identifiant est '{0}'.", "reponse_contenu"));
            }

            if (String.IsNullOrEmpty(context.Request["question_fid"]))
	        {
                HttpContext.Current.Response.StatusCode = 400;
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi d'identifiant.", "question_fid"));
	        }

            int val_question_fid = 0;

            if (!int.TryParse(context.Request["question_fid"], out val_question_fid))
            {
                HttpContext.Current.Response.StatusCode = 400;
                throw new HttpException(400, string.Format("{0} : Vous devez saisir un entier.", "question_fid"));
            }

            if (String.IsNullOrEmpty(context.Request["reponse_contenu"]))
            {
                throw new HttpException(400, string.Format("{0} : Vous n'avez pas saisi de contenu pour votre réponse à la quesion n°{1}", "reponse_contenu", val_question_fid));
            }

            EntiteReponse myEntity = new EntiteReponse();
            myEntity.question_fid =  val_question_fid;
            myEntity.Contenu = context.Request["reponse_contenu"];

            using (bdd_service_web bdd = new bdd_service_web())
            {
                try
                {
                    var uneQuestion = bdd.questions.Where(pp => pp.Id.Equals(val_question_fid)).FirstOrDefault();

                    if (uneQuestion != null)
                    {
                        if (uneQuestion.reponses.Count > 0)
                        {
                            HttpContext.Current.Response.StatusCode = 400;
                            throw new HttpException(400, string.Format("{0} : Une réponse existe déjà pour cette question n°{1}. Réponse présente : {2}.\nSi vous souhaitez mettre à jour la réponse à la question, merci de consulter la documentation pour une mise à jour de la réponse.", "question_fid", val_question_fid, uneQuestion.reponses.First().Contenu));
                        }
                    }
                    else
                    {
                        HttpContext.Current.Response.StatusCode = 404;
                        throw new HttpException(404, string.Format("{0} : La quesion n°{1} n'existe pas.", "question_fid", val_question_fid));
                    }

                    bdd.reponses.Add(myEntity);
                    bdd.SaveChanges();

                    _maReponse = myEntity;

                    ManagerHeader.AjouterCodeHeaderReponse(201, this);
                }
                catch (Exception e)
                {
                    throw new HttpException(e.GetHashCode(), string.Format("<h2>Question N°{0}</h2><p>Détail: {1}</p>", val_question_fid, e.Message));
                }
            }
        }

        public override void Consulter(System.Web.HttpContext context)
        {
            StringBuilder messageException = new StringBuilder();

            if (!context.Request.Params.AllKeys.Contains("question_id"))
            {
                messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de d'identifiant", "question_id"));
            }

            int val = 0;

            if (!int.TryParse(context.Request.Params.Get("question_id"), out val))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "question_id"));
            }

            if (!context.Request.Params.AllKeys.Contains("reponse_id"))
            {
                messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de d'identifiant", "reponse_id"));
            }

            int valIdReponse = 0;

            if (!int.TryParse(context.Request.Params.Get("reponse_id"), out valIdReponse))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "reponse_id"));
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                if (val > 0)
                {
                    var uneQuestion = bdd.questions.Where(pp => pp.Id == val).FirstOrDefault();

                    if (uneQuestion != null)
                    {
                        if (uneQuestion.reponses.Count > 0)
                        {
                            _maReponse = uneQuestion.reponses.First();
                        }
                        else
                        {
                            _maReponse = new EntiteReponse();
                        }
                    }
                }
                else
                {
                    if (valIdReponse > 0)
                    {
                        var uneReponse = bdd.reponses.Where(pp => pp.Id == valIdReponse).FirstOrDefault();

                        if (uneReponse != null)
                        {
                            _maReponse = uneReponse;
                        }
                        else
                        {
                            _maReponse = new EntiteReponse();
                            HttpContext.Current.Response.StatusCode = 404;
                            throw new Exception(messageException.ToString());
                        }
                    }
                    else
                    {
                        _maReponse = new EntiteReponse();

                        HttpContext.Current.Response.StatusCode = 400;
                        throw new Exception(messageException.ToString());
                    }
                }
            }
        }

        public override void Supprimer(System.Web.HttpContext context)
        {
            StringBuilder messageException = new StringBuilder();

            if (!context.Request.Form.AllKeys.Contains("reponse_id"))
            {
                messageException.AppendLine(string.Format("{0} : Dans votre formulaire, vous n'avez pas de champ dont l'identifiant est '{0}'", "reponse_id"));
            }

            int valIdReponse = 0;

            if (!int.TryParse(context.Request["reponse_id"], out valIdReponse))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "reponse_id"));
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                var uneReponse = bdd.reponses.Where(pp => pp.Id == valIdReponse).FirstOrDefault();

                if (uneReponse == null)
                {
                    HttpContext.Current.Response.StatusCode = 400;
                    throw new HttpException(messageException.ToString());
                }

                bdd.reponses.Remove(uneReponse);
                bdd.SaveChanges();

                _maReponse = new EntiteReponse();
            }
        }

        public override void MiseAJour(System.Web.HttpContext context)
        {
            int valIdException = 0;

            StringBuilder messageException = new StringBuilder();

            if (!context.Request.Form.AllKeys.Contains("question_id"))
            {
                messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de d'identifiant", "question_id"));
            }

            int val = 0;

            if (!int.TryParse(context.Request["question_id"], out val))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "question_id"));
            }

            if (val <= 0)
            {
                if (!context.Request.Form.AllKeys.Contains("question_fid"))
                {
                    messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de d'identifiant", "question_fid"));
                }

                if (!int.TryParse(context.Request["question_fid"], out val))
                {
                    messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "question_fid"));
                }
            }

            valIdException = val;

            int valIdReponse = 0;

            if (val <= 0)
            {
                if (!context.Request.Form.AllKeys.Contains("reponse_id"))
                {
                    messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de d'identifiant", "reponse_id"));
                }

                if (!int.TryParse(context.Request["reponse_id"], out valIdReponse))
                {
                    messageException.AppendLine(string.Format("{0} : Vous devez saisir un entier", "reponse_id"));
                }

                valIdException = valIdReponse;
            }

            string contenuReponse = context.Request["reponse_contenu"];

            if (string.IsNullOrEmpty(context.Request["reponse_contenu"]))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir du texte", "reponse_contenu"));
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
                            if (uneQuestion.reponses.Count > 0)
                            {
                                _maReponse = uneQuestion.reponses.First();
                                _maReponse.Contenu = contenuReponse;

                                bdd.SaveChanges();

                                valIdException = _maReponse.Id;

                                ManagerHeader.AjouterCodeHeaderReponse(204, this);
                            }
                            else
                            {
                                HttpContext.Current.Response.StatusCode = 304;
                                throw new HttpException(304, string.Format("{0} : La ressource doit être créer avant d'être mise à jour. Consulter la documentation pour créer la ressource.", "reponse_id", val));
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.StatusCode = 404;
                            throw new HttpException(404, string.Format("{0} : La question n°{1} associé à cette ressource n'existe pas.", "reponse_id", val));
                        }
                    }
                    else
                    {
                        if (valIdReponse > 0)
                        {
                            var uneReponse = bdd.reponses.Where(pp => pp.Id == valIdReponse).FirstOrDefault();

                            if (uneReponse != null)
                            {
                                _maReponse = uneReponse;
                                _maReponse.Contenu = contenuReponse;

                                bdd.SaveChanges();

                                ManagerHeader.AjouterCodeHeaderReponse(204, this);
                            }
                            else
                            {
                                HttpContext.Current.Response.StatusCode = 404;
                                throw new HttpException(404, string.Format("{0} : L'identifiant réponse n°{1} n'existe pas.", "reponse_id", valIdReponse));
                            }
                        }
                        else
                        {
                            HttpContext.Current.Response.StatusCode = 404;
                            throw new HttpException(404, string.Format("{0} : Dans votre formulaire, vous devez avoir un champ dont l'identifiant est '{0}' et la valeur doit être un entier. Valeur saisie : '{1}'", "reponse_id", valIdReponse));
                        }
                    }
                }
                catch (Exception e)
                {
                    if (val <= 0)
                    {
                        throw new HttpException(e.GetHashCode(), string.Format("<h2>Question N°{0}</h2><p>Détail: {1}</p>", valIdException, e.Message));
                    }
                    else
                    {
                        throw new HttpException(e.GetHashCode(), string.Format("<h2>Réponse N°{0}</h2><p>Détail: {1}</p>", valIdException, e.Message));
                    }
                }
            }
        }

        public override string GetString()
        {
            return RepresentationFactory.GetRepresentation(_monContextHttp, this);
        }

        public override string GetString(string formatRepresentationRessource)
        {
            return RepresentationFactory.GetRepresentation(_monContextHttp, this);
        }

        public override string GetNameClass()
        {
            return this.GetType().Name;
        }

        public override string GetContenu()
        {
            return _maReponse.Contenu;
        }

        public override string GetId()
        {
            return Id;
        }

        public string LienRessource
        {
            get
            {
                if (_maReponse.Id == 0)
                {
                    return "";
                }

                return string.Format("http://{3}:{4}/{5}/{0}?{1}={2}", this.GetNameClass(), this.NameChampId, this.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath.Remove(0, 1));
            }

            set
            {
                _lienRessource = string.Format("http://{3}:{4}/{5}/{0}?{1}={2}", this.GetNameClass(), this.NameChampId, this.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath.Remove(0, 1));
            }
        }
    }
}
