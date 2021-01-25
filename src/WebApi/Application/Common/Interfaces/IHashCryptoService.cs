namespace LaserPointer.WebApi.Application.Common.Interfaces
{
    public interface IHashCryptoService
    {
        string DecryptPlainTextValue(byte[] plain);
        byte[] EncryptPlainTextValue(string plain);
    }
}
