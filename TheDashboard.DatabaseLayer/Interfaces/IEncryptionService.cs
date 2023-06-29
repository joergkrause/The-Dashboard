namespace TheDashboard.DatabaseLayer.Interfaces;

public interface IEncryptionService
{
    string EncryptString(string key, string plainText);

    string DecryptString(string key, string cipherText);
}