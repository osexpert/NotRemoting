using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NotRemoting;

public static class CertificatesExporter
{
	/// <summary>
	/// Export root and CA certs from user and machine stores.
	/// </summary>
	/// <returns></returns>
	public static string ExportCertificates()
	{
		var thumbprints = new HashSet<string>();
		var sb = new StringBuilder();

		foreach (var location in new[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
		{
			using var store = new X509Store(StoreName.Root, location);
			store.Open(OpenFlags.ReadOnly);

			foreach (var cert in store.Certificates)
			{
				if (thumbprints.Add(cert.Thumbprint))
				{
					sb.AppendLine("-----BEGIN CERTIFICATE-----");
					sb.AppendLine(Convert.ToBase64String(cert.RawData, Base64FormattingOptions.InsertLineBreaks));
					sb.AppendLine("-----END CERTIFICATE-----");
				}
			}
		}

		foreach (var location in new[] { StoreLocation.CurrentUser, StoreLocation.LocalMachine })
		{
			using var store = new X509Store(StoreName.CertificateAuthority, location);
			store.Open(OpenFlags.ReadOnly);

			foreach (var cert in store.Certificates)
			{
				if (thumbprints.Add(cert.Thumbprint))
				{
					sb.AppendLine("-----BEGIN CERTIFICATE-----");
					sb.AppendLine(Convert.ToBase64String(cert.RawData, Base64FormattingOptions.InsertLineBreaks));
					sb.AppendLine("-----END CERTIFICATE-----");
				}
			}
		}

		return sb.ToString();
	}
}

