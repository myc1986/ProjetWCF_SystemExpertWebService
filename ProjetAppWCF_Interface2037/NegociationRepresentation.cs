using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjetAppWCF_Interface2037
{
    public class NegociationRepresentation
    {
        private static List<string> _mesMimes;

        public static void InitMimes()
        {
            _mesMimes = new List<string>();
            _mesMimes.Add("text/html");
            _mesMimes.Add("text/xml");
            _mesMimes.Add("text/plain");
        }

        public static List<string> MimesSupportes
        {
            get
            {
                if (_mesMimes.Count() <= 0)
                {
                    _mesMimes = new List<string>();
                }

                return _mesMimes;
            }
        }

        public static void AddMimeSupporte(string mime)
        {
            _mesMimes.Add(mime.ToLower());
        }

        public static string NegocierRepresentation(string[] lesMimesAcceptes)
        {
            string reponseRepresentation = "text/html";

            int iMime = 0;
            bool accordNegociation = false;

            while (lesMimesAcceptes.Count() > 0 && iMime < lesMimesAcceptes.Count() && !accordNegociation)
            {
                if (_mesMimes.Contains(lesMimesAcceptes[iMime].ToLower()))
                {
                    reponseRepresentation = lesMimesAcceptes[iMime].ToLower();
                    accordNegociation = true;
                }

                iMime++;
            }

            if (!accordNegociation)
            {
                throw new HttpException(415, string.Format("Format de représentation demandé non supporté."));
            }

            return reponseRepresentation;
        }
    }
}
