using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer
{
    public class JWTKey
    {
        public static byte[] CreateSecretKey()
        {
            byte[] secretKey = new byte[32]; // 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretKey);
            }

            return secretKey;

        }
    }
}