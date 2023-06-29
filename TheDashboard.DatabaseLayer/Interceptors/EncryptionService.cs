using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheDashboard.DatabaseLayer.Interfaces;

namespace TheDashboard.DatabaseLayer.Interceptors;

public class EncryptionService : IEncryptionService
{
  public string DecryptString(string key, string cipherText)
  {
    return AesOperation.DecryptString(key, cipherText);
  }

  public string EncryptString(string key, string plainText)
  {
    return AesOperation.EncryptString(key, plainText);
  }
}
