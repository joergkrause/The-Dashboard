using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class EncryptionService : IEncryptionService
{
  public string DecryptString(string key, string cipherText)
  {
    if (String.IsNullOrEmpty(key))
    {
      throw new ArgumentNullException(nameof(key));
    }
    if (key.Length != 16)
    {
      throw new ArgumentOutOfRangeException(nameof(key));
    }

    try
    {
      byte[] iv = new byte[16];
      byte[] buffer = Convert.FromBase64String(cipherText);

      using Aes aes = Aes.Create();
      aes.Key = Encoding.UTF8.GetBytes(key);
      aes.IV = iv;
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

      using var memoryStream = new MemoryStream(buffer);
      using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
      using var streamReader = new StreamReader(cryptoStream);
      return streamReader.ReadToEnd();
    }
    catch (Exception)
    {
      return cipherText;
    }
  }

  public string EncryptString(string key, string plainText)
  {
    if (String.IsNullOrEmpty(key))
    {
      throw new ArgumentNullException(nameof(key));
    }
    if (key.Length != 16)
    {
      throw new ArgumentOutOfRangeException(nameof(key));
    }

    byte[] iv = new byte[16];
    byte[] array;

    using var aes = Aes.Create();
    aes.Key = Encoding.UTF8.GetBytes(key);
    aes.IV = iv;

    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

    using var memoryStream = new MemoryStream();
    using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    using var streamWriter = new StreamWriter(cryptoStream);
    streamWriter.Write(plainText);
    array = memoryStream.ToArray();

    return Convert.ToBase64String(array);
  }
}
