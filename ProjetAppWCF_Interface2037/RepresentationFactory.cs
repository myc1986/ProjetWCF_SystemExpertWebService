using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ProjetAppWCF_Interface2037
{
    public static class RepresentationFactory
    {
        public static string Test(Ressource uneRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();

            chaineReponseBuilded.AppendLine("<html>");
            chaineReponseBuilded.AppendLine("<head>");
            chaineReponseBuilded.AppendLine(string.Format("<title>{0} : {0}</title>", uneRessource.GetNameClass(), uneRessource.GetContenu()));
            chaineReponseBuilded.AppendLine("</head>");
            chaineReponseBuilded.AppendLine("<body>");
            chaineReponseBuilded.AppendLine(string.Format("<h2>{0} : {1}</h2>", uneRessource.GetNameClass(), uneRessource.GetContenu()));

            if (uneRessource.GetNameClass() == "Question")
	        {
		        Question uneQuestion = (Question) uneRessource;

                if (uneQuestion.AUneReponse())
	            {
		            chaineReponseBuilded.AppendLine(string.Format("<p>Réponse : {0}</p>" , uneQuestion.ReponseString));
	            }
	        }

            chaineReponseBuilded.AppendLine("</body>");
            chaineReponseBuilded.AppendLine("</html>");

            HtmlString monHtml = new HtmlString(chaineReponseBuilded.ToString());

            return monHtml.ToHtmlString();
        }

        public static string GetRepresentation(string contenTypeRepresentation, IRessource laRessource)
        {
            StringBuilder chaineReponseBuilded = new StringBuilder();
            XmlSerializer xs;

            switch (contenTypeRepresentation)
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

            chaineReponseBuilded.AppendLine("<html>");
            chaineReponseBuilded.AppendLine("<head>");
            chaineReponseBuilded.AppendLine(string.Format("<title>{0} : {0}</title>", laRessource.GetNameClass(), laRessource.GetContenu()));
            chaineReponseBuilded.AppendLine("</head>");
            chaineReponseBuilded.AppendLine("<body>");
            chaineReponseBuilded.AppendLine(string.Format("<h2>{0} : {1}</h2>", laRessource.GetNameClass(), laRessource.GetContenu()));

            if (laRessource.GetNameClass() == "Question")
	        {
		        Question uneQuestion = (Question) laRessource;

                if (uneQuestion.AUneReponse())
	            {
		            chaineReponseBuilded.AppendLine(string.Format("<p>Réponse : {0}</p>" , uneQuestion.ReponseString.GetContenu()));
	            }
	        }

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

            

            switch (leContext.Request.ContentType)
            {
                case "text/html": // Représentation HTML
                    chaineReponseBuilded.Append(GetHtmlSerialiserRessource(laRessource));
                    break;
                case "text/plain": // Représentation text brut
                case "text/xml": // représentation XML
                    xs = GetXmlSerialiserRessource(laRessource);
                    SerialiserRessourceXml(laRessource, ref chaineReponseBuilded, xs);
                    break;
                case "text/json": // représentation XML
                    chaineReponseBuilded.Append(SerialiserRessourceJson(laRessource));
                    break;
                default: // Représentation text brut
                    break;
            }

            return chaineReponseBuilded.ToString();
        }
    }
}