using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            

            if (String.IsNullOrEmpty(context.Request.Params.GetValues("fid_question").ToString()))
	        {
		        throw new Exception(string.Format("{0} : Identifiant vide ou inexistant", "fid_question"));
	        }

            if (String.IsNullOrEmpty(context.Request.Params.GetValues("reponse").ToString()))
            {
                throw new Exception(string.Format("{0} : Identifiant vide ou inexistant", "reponse"));
            }


            //Gestion cache à faire

            EntiteReponse myEntity = new EntiteReponse();
            myEntity.fid_question =  Convert.ToInt16(context.Request.Params.GetValues("fid_question").ToString());
            myEntity.reponse = context.Request.Params.GetValues("reponse").ToString();

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

            SqlParameter monParam = new SqlParameter("@p_reponse", myEntity.reponse);
            monParam.DbType = System.Data.DbType.AnsiString;

            maCommande.Parameters.Add(monParam);

            monParam = new SqlParameter("@p_fid_question", myEntity.fid_question);
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
