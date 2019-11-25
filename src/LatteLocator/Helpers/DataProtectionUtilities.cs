using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;

namespace LatteLocator.Helpers
{
    public static class DataProtectionUtilities
    {
        public static async Task<IBuffer> ProtectAsync(string strMsg)
        {
            var buffMsg = CryptographicBuffer.ConvertStringToBinary(strMsg, BinaryStringEncoding.Utf8);
            var buffProtected = await new DataProtectionProvider().ProtectAsync(buffMsg);

            return buffProtected;
        }

        public static async Task<string> UnprotectData(IBuffer buffProtected)
        {
            var buffUnprotected = await new DataProtectionProvider().UnprotectAsync(buffProtected);
            var strClearText = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnprotected);
            return strClearText;
        }
    }
}
