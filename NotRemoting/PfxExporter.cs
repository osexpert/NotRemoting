using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace NotRemoting;

#if NETSTANDARD2_1_OR_GREATER

public static class PfxExporter
{
	public static (string certificatePem, string privateKeyPem) ExportPfx(string pfxFile, string password)
	{
		var cert = new X509Certificate2(
			pfxFile,
			password,
			X509KeyStorageFlags.Exportable |
			X509KeyStorageFlags.EphemeralKeySet);

		string certPem = ExportCertificatePem(cert);
		string keyPem = ExportPrivateKeyPem(cert);

		return (certPem, keyPem);
	}

	private static string ExportCertificatePem(X509Certificate2 cert)
	{
		var base64 = Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks);
		return $"-----BEGIN CERTIFICATE-----\n{base64}\n-----END CERTIFICATE-----\n";
	}

	private static string ExportPrivateKeyPem(X509Certificate2 cert)
	{
		if (!cert.HasPrivateKey)
			throw new InvalidOperationException("Certificate has no private key.");

		using RSA? rsa = cert.GetRSAPrivateKey();
		using ECDsa? ecdsa = cert.GetECDsaPrivateKey();

		byte[] keyBytes;

		if (rsa != null)
		{
			try
			{
				keyBytes = rsa.ExportPkcs8PrivateKey();
			}
			catch
			{
				// ExportPkcs8PrivateKey() may throw exception "The requested operation is not supported."
				// This seems to work: https://github.com/dotnet/runtime/issues/77590#issuecomment-1295100267
				using RSA tmp = RSA.Create();
				const string pwd = "dummyPassword";
				PbeParameters pbeParameters = new PbeParameters(PbeEncryptionAlgorithm.Aes128Cbc, HashAlgorithmName.SHA256, 1);
				tmp.ImportEncryptedPkcs8PrivateKey(pwd, rsa.ExportEncryptedPkcs8PrivateKey(pwd, pbeParameters), out var bytesRead);
				rsa.ImportParameters(tmp.ExportParameters(true));

				keyBytes = rsa.ExportPkcs8PrivateKey();
			}
		}
		else if (ecdsa != null)
		{
			keyBytes = ecdsa.ExportPkcs8PrivateKey();
		}
		else
		{
			throw new NotSupportedException("Certificate has no RSA or ECDSA private key.");
		}

		var base64 = Convert.ToBase64String(keyBytes, Base64FormattingOptions.InsertLineBreaks);
		return $"-----BEGIN PRIVATE KEY-----\n{base64}\n-----END PRIVATE KEY-----\n";
	}
}
#endif
