using System;
using LaserPointer.WebApi.Domain.Entities;

namespace LaserPointer.WebApi.Domain
{
    public static class HashExtension
    {
        // https://stackoverflow.com/a/9995303
        public static Hash FromHexString(this Hash hash, string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var binArr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                binArr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            hash.Value = binArr;
            return hash;
        }

        private static int GetHexVal(char hex) {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
       
        // Weird but fast
        // https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa/14333437#14333437
        public static string AsHexString(this Hash hash)
        {
            var bytes = hash.Value;
            var c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++) {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b-10)>>31)&-7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b-10)>>31)&-7));
            }
            return new string(c);
        }
    }
}
