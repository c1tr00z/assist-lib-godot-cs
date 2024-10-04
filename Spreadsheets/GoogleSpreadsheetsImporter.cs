using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using Godot;

namespace projectwitch.addons.AssistLib.Spreadsheets;

public class GoogleSpreadsheetsImporter {
    
    #region Accessors

    private static RemoteCertificateValidationCallback allowCertificate = (sender, cert, chain, sslPolicyErrors) => true;

    #endregion

    #region Class Implementation

    public static bool Import(string documentId, string pageId, out string documentString) {
        ServicePointManager.ServerCertificateValidationCallback += allowCertificate;
        string downloadUrl = $"http://spreadsheets.google.com/feeds/download/spreadsheets/Export?key={documentId}&gid={pageId}&exportFormat=csv";
        documentString = null;
        
        HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(downloadUrl);
        HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

        WebHeaderCollection header = aResponse.Headers;

        var encoding = ASCIIEncoding.UTF8;

        using (var reader = new StreamReader(aResponse.GetResponseStream(), encoding)) {
            documentString = reader.ReadToEnd();
        }

        ServicePointManager.ServerCertificateValidationCallback -= allowCertificate;

        return true;
    }

    #endregion
}