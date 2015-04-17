using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ProjetAppWCF_Interface2037
{
    public class RepresentationFactory
    {
        public static string GetRepresentation(string contenTypeRepresentation, IRessource laRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();
            XmlSerializer xs;

            if (string.IsNullOrEmpty(laRessource.GetId()))
            {
                return string.Format("Ressource '{0}' supprimé", laRessource.GetNameClass());
            }

            List<string> lesAcceptesTypes = new List<string>();
            lesAcceptesTypes.Add(contenTypeRepresentation);

            switch (NegociationRepresentation.NegocierRepresentation(lesAcceptesTypes.ToArray()))
            {
                case "text/html": // Représentation HTML
                    chaineReponseBuilded.Append(GetHtmlSerialiserRessource(laRessource));
                    break;
                case "text/xml": // représentation XML
                case "text/plain": // Représentation text brut
                    xs = GetXmlSerialiserRessource(laRessource);
                    SerialiserRessourceXml(laRessource, ref chaineReponseBuilded, xs);
                    break;
                default: // Représentation text brut
                    throw new Exception(string.Format("{0} : ce content-type n'existe pas ou n'est pas implémenté !", contenTypeRepresentation));
            }

            return chaineReponseBuilded.ToString();
        }

        private static void SerialiserRessourceXml(IRessource laRessource, ref StringBuilder chaineReponseBuilded, XmlSerializer xs)
        {
            XmlWriter monWriterHtml = XmlWriter.Create(chaineReponseBuilded);
            xs.Serialize(monWriterHtml, laRessource);
        }

        private static XmlSerializer GetXmlSerialiserRessource(IRessource laRessource)
        {
            XmlSerializer xs;

            switch (laRessource.GetNameClass())
            {
                case "Question":
                    xs = new XmlSerializer(typeof(Question));
                    break;
                case "Reponse":
                    xs = new XmlSerializer(typeof(Reponse));
                    break;
                default:
                    throw new Exception("Impossible de générer une réprésentation d'une ressource qui n'a pas de nom de class ou n'est pas implémenté.");
            }

            return xs;
        }

        private static HtmlString GetHtmlSerialiserRessource(IRessource laRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();

            chaineReponseBuilded.AppendLine("<!DOCTYPE html>");
            chaineReponseBuilded.AppendLine("<html>");
            chaineReponseBuilded.AppendLine("<head>");
            chaineReponseBuilded.AppendLine(string.Format("<title>{0} : {0}</title>", laRessource.GetNameClass(), laRessource.GetContenu()));
            chaineReponseBuilded.AppendLine("</head>");
            chaineReponseBuilded.AppendLine("<body>");
            chaineReponseBuilded.AppendLine(string.Format("<div id='{0}_{1}'>", laRessource.GetNameClass(), laRessource.GetId()));
            chaineReponseBuilded.AppendLine(string.Format("<h2>{0} N°{2}: {1}</h2>", laRessource.GetNameClass(), laRessource.GetContenu(), laRessource.GetId()));

            if (laRessource.GetNameClass() == "Question")
	        {
		        Question uneQuestion = (Question) laRessource;

                if (uneQuestion.AUneReponse())
	            {
		            chaineReponseBuilded.AppendLine(string.Format("<p id='{0}_{1}'>Réponse N°{1}: {2}</p>" , uneQuestion.ReponseString.GetNameClass(), uneQuestion.ReponseString.GetId(), uneQuestion.ReponseString.GetContenu()));
	            }
                else
                {
                    chaineReponseBuilded.AppendLine(string.Format("<p id=''>Réponse : Aucune réponse n'est disponible</p>", uneQuestion.ReponseString.GetNameClass(), uneQuestion.ReponseString.GetId(), uneQuestion.ReponseString.GetContenu()));
                }
	        }

            string chaineLienConsultation = string.Format("http://{3}:{4}/{0}?{1}={2}", laRessource.GetNameClass(), laRessource.NameChampId, laRessource.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port);

            chaineReponseBuilded.AppendLine(string.Format("<p id='pLienConsultation'>Lien de consultation : <a id='lienConsultation' href='/{0}?{1}={2}'>http://{3}:{4}/{0}?{1}={2}</a></p>", laRessource.GetNameClass(), laRessource.NameChampId, laRessource.GetId(), HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port));

            chaineReponseBuilded.AppendLine("</div>");
            chaineReponseBuilded.AppendLine("</body>");
            chaineReponseBuilded.AppendLine("</html>");

            HtmlString monHtml = new HtmlString(chaineReponseBuilded.ToString());


            return monHtml;
        }

        private static string SerialiserRessourceJson(IRessource laRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();

            

            return chaineReponseBuilded.ToString();
        }

        public static string GetRepresentation(HttpContext leContext, IRessource laRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();
            XmlSerializer xs;

            //if (string.IsNullOrEmpty(laRessource.GetId()))
            //{
            //    return string.Format("1", laRessource.GetNameClass());
            //}

            switch(NegociationRepresentation.NegocierRepresentation(leContext.Request.AcceptTypes))
            {
                case "text/html": // Représentation HTML
                    chaineReponseBuilded.Append(GetHtmlSerialiserRessource(laRessource));
                    break;
                case "text/plain": // Représentation text brut
                    chaineReponseBuilded.Append(SerialiserRessourceToTextPlain(laRessource));
                    break;
                case "text/xml": // représentation XML
                    xs = GetXmlSerialiserRessource(laRessource);
                    SerialiserRessourceXml(laRessource, ref chaineReponseBuilded, xs);
                    break;
                case "text/json": // représentation XML
                    chaineReponseBuilded.Append(SerialiserRessourceToJson(laRessource));
                    break;
                case "text/csv":
                    chaineReponseBuilded.Append(SerialiserRessourceToTextCsv(laRessource, "\"","|"));
                    break;
                default: // Représentation text brut
                    chaineReponseBuilded.Append(GetHtmlSerialiserRessource(laRessource));
                    break;
            }

            return chaineReponseBuilded.ToString();
        }

        public static string SerialiserRessourceToTextPlain(IRessource laRessource)
        {
            StringBuilder monTextePlain = new StringBuilder();

            monTextePlain.Append(string.Format("{0} {1} {2} {3}", laRessource.GetId(), laRessource.GetNameClass(), laRessource.GetContenu(),laRessource.Lien));

            if (laRessource.GetNameClass() == "Question")
            {
                Question uneQuestion = (Question)laRessource;

                monTextePlain.Append(string.Format("{0} {1} {2} {3}", uneQuestion.ReponseString.GetId(), uneQuestion.ReponseString.GetNameClass(), uneQuestion.ReponseString.GetContenu(), uneQuestion.ReponseString.Lien));
            }

            return monTextePlain.ToString();
        }

        public static string SerialiserRessourceToTextCsv(IRessource laRessource, string delimiteur, string separateur)
        {
            StringBuilder monTextePlain = new StringBuilder();

            string mesEntetes = string.Format("{4}{0}{4}{5}{4}{1}{4}{5}{4}{2}{4}{5}{4}{3}{4}", "Identifiant ressource", "Nom de la ressource", "Contenu de la ressource", "Lien de la ressource", delimiteur, separateur);
            string mesDonnees = string.Format("{4}{0}{4}{5}{4}{1}{4}{5}{4}{2}{4}{5}{4}{3}{4}", laRessource.GetId(), laRessource.GetNameClass(), laRessource.GetContenu(), laRessource.Lien, delimiteur, separateur);

            if (laRessource.GetNameClass() == "Question")
            {
                Question uneQuestion = (Question)laRessource;

                mesEntetes = mesEntetes + string.Format("{5}{4}{0}{4}{5}{4}{1}{4}{5}{4}{2}{4}{5}{4}{3}{4}", "Identifiant sous-ressource", "Nom de la sous-ressource", "Contenu de la sous-ressource", "Lien de la sous-ressource", delimiteur, separateur);
                mesDonnees = mesDonnees + string.Format("{5}{4}{0}{4}{5}{4}{1}{4}{5}{4}{2}{4}{5}{4}{3}{4}", uneQuestion.ReponseString.GetId(), uneQuestion.ReponseString.GetNameClass(), uneQuestion.ReponseString.GetContenu(), uneQuestion.ReponseString.Lien, delimiteur, separateur);
            }

            monTextePlain.AppendLine(mesEntetes);
            monTextePlain.AppendLine(mesDonnees);

            return monTextePlain.ToString();
        }

        public static string SerialiserRessourceToJson(IRessource laRessource)
        {
            StringBuilder monTextePlain = new StringBuilder();

            monTextePlain.AppendLine(JsonConvert.SerializeObject(laRessource));

            return monTextePlain.ToString();
        }
    }
}