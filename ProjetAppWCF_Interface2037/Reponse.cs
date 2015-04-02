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
            if (String.IsNullOrEmpty(context.Request.Params.GetValues("question_fid").ToString()))
	        {
                throw new Exception(string.Format("{0} : Identifiant vide ou inexistant", "question_fid"));
	        }

            if (String.IsNullOrEmpty(context.Request.Params.GetValues("reponse").ToString()))
            {
                throw new Exception(string.Format("{0} : Identifiant vide ou inexistant", "reponse"));
            }


            //Gestion cache à faire

            EntiteReponse myEntity = new EntiteReponse();
            myEntity.question_fid =  Convert.ToInt16(context.Request.Params.GetValues("question_fid").ToString());
            myEntity.Contenu = context.Request.Params.GetValues("reponse_contenu").ToString();

            //Creer un

            SqlConnection maConnexion = new SqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=web_service");

            try
            {
                maConnexion.Open();
            }
            catch (SqlException e)
            {
                throw new Exception(string.Format("Impossible de se connecter à la base. \n Détails : Code erreur {0} : {1}", e.ErrorCode, e.Message));
            }
            

            SqlCommand maCommande = new SqlCommand();
            maCommande.Connection = maConnexion;
            maCommande.CommandText = "AjouterReponse";
            maCommande.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter monParam = new SqlParameter("@p_reponse_contenu", myEntity.Contenu);
            monParam.DbType = System.Data.DbType.AnsiString;

            maCommande.Parameters.Add(monParam);

            monParam = new SqlParameter("@p_question_fid", myEntity.question_fid);
            monParam.DbType = System.Data.DbType.Int16;

            maCommande.Parameters.Add(monParam);

            SqlDataReader monData;
            
            try 
	        {	        
		        monData = maCommande.ExecuteReader();
	        }
	        catch (SqlException e)
	        {
                throw new Exception(string.Format("Impossible d'exécuter la procédure stockée. \n Détails : Code erreur {0} : {1}", e.ErrorCode, e.Message));
	        }
                
            try 
	        {	        
		        monData.Close();
	        }
	        catch (SqlException e)
	        {
                throw new Exception(string.Format("Impossible de fermer la dataReader : voir monData.Close();. \n Détails : Code erreur {0} : {1}", e.ErrorCode, e.Message));
	        }

            try
	        {	        
		        maConnexion.Close();
	        }
	        catch (SqlException e)
	        {
                throw new Exception(string.Format("Impossible de fermer la la connexion : voir maConnexion.Close();. \n Détails : Code erreur {0} : {1}", e.ErrorCode, e.Message));
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

                            throw new Exception(messageException.ToString());
                        }
                    }
                    else
                    {
                        _maReponse = new EntiteReponse();

                        throw new Exception(messageException.ToString());
                    }
                }
            }
        }

        public override void Supprimer(System.Web.HttpContext context)
        {
            StringBuilder messageException = new StringBuilder();

            if (!context.Request.Params.AllKeys.Contains("reponse_contenu"))
            {
                messageException.AppendLine(string.Format("{0} : Vous n'avez pas saisi de contenu réponse", "reponse_contenu"));
            }

            int valIdReponse = 0;

            if (!int.TryParse(context.Request.Params.Get("reponse_contenu"), out valIdReponse))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir du texte", "reponse_contenu"));
            }

            using (bdd_service_web bdd = new bdd_service_web())
            {
                var uneReponse = bdd.reponses.Where(pp => pp.Id == valIdReponse).FirstOrDefault();

                if (uneReponse != null)
                {
                    _maReponse = uneReponse;
                    _maReponse.Contenu = context.Request.Params.Get("reponse_contenu").ToString();
                }
                else
                {
                    throw new Exception(messageException.ToString());
                }

                bdd.reponses.Remove(uneReponse);
                bdd.SaveChanges();
            }
        }

        public override void MiseAJour(System.Web.HttpContext context)
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

            string contenuReponse = "";

            if (!int.TryParse(context.Request.Params.Get("reponse_contenu"), out valIdReponse))
            {
                messageException.AppendLine(string.Format("{0} : Vous devez saisir du texte", "reponse_contenu"));
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
                            _maReponse.Contenu = contenuReponse;

                            bdd.SaveChanges();
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
                            _maReponse.Contenu = contenuReponse;

                            bdd.SaveChanges();
                        }
                        else
                        {
                            _maReponse = new EntiteReponse();

                            throw new Exception(messageException.ToString());
                        }
                    }
                    else
                    {
                        _maReponse = new EntiteReponse();

                        throw new Exception(messageException.ToString());
                    }
                }
            }
        }

        public override string GetString()
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();
            XmlSerializer xs = new XmlSerializer(typeof(Reponse));

            switch (_monContextHttp.Response.ContentType)
            {
                case "text/xml": // représentation XML
                    XmlWriter monWriterXml = XmlWriter.Create(chaineReponseBuilded);
                    xs.Serialize(monWriterXml, this);
                    break;
                case "text/html": // Représentation HTML
                    XmlWriter monWriterHtml = XmlWriter.Create(chaineReponseBuilded);
                    xs.Serialize(monWriterHtml, this);
                    break;
                case "text/plain": // Représentation text brut
                    break;
                default: // Représentation text brut
                    break;
            }

            return chaineReponseBuilded.ToString();
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
    }
}
