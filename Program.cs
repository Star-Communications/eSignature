using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace eSignature
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader file = File.OpenText(@"./canonical.txt");
            string canonical = file.ReadToEnd();

            ContentInfo content = new ContentInfo(Encoding.UTF8.GetBytes(canonical));
            SignedCms signedCms = new SignedCms(content, false);
            CmsSigner signer = new CmsSigner(SubjectIdentifierType.IssuerAndSerialNumber, GetCertificateFromStore("EEI-RootCA-PREPROD"));
            signedCms.ComputeSignature(signer, false);
        }

        private static X509Certificate2 GetCertificateFromStore(string certName)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certCollection = store.Certificates;
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindByIssuerName, certName, false);
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}