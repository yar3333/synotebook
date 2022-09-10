using System;

namespace SyNotebook.Crypting
{
	/// <summary>
	/// Реализует функционирование алгоритма шифрования Idea в режиме обратной связи по шифру.
	/// </summary>
	public class IdeaCFB
	{
        private byte[] encIV;
        private byte[] decIV;

        private ushort[] encKey = new ushort[52];
		
		/// <summary>
		/// Инициализирует поток для шифрования.
		/// </summary>
		/// <param name="IV">вектор инициализации (8 элементов)</param>
		/// <param name="key">ключ (8 элементов)</param>
        private IdeaCFB(byte[] IV, ushort[] key)
		{
			encIV = (byte[])IV.Clone();
			decIV = (byte[])IV.Clone();

			IdeaBase.makeEncryptKey(encKey,key);
		}

        public IdeaCFB(string password)
        {
            encIV = (byte[])DefaultCryptIV.Clone();
            decIV = (byte[])DefaultCryptIV.Clone();

            IdeaBase.makeEncryptKey(encKey, ConvertPassword(password));
        }

        public string Encrypt(string s)
        {
            var buf = System.Text.Encoding.Default.GetBytes(s);
            Encrypt(buf);
            return System.Text.Encoding.Default.GetString(buf);
        }

        public string Decrypt(string s)
        {
            var buf = System.Text.Encoding.Default.GetBytes(s);
            Decrypt(buf);
            return System.Text.Encoding.Default.GetString(buf);
        }

        /// <summary>
        /// Шифрует байт данных.
        /// </summary>
        /// <param name="p">исходный байт данных</param>
        private byte Encrypt(byte p)
		{
			var iv16 = new ushort[4];
			iv16[0] = (ushort)((encIV[1]<<8) | encIV[0]);
			iv16[1] = (ushort)((encIV[3]<<8) | encIV[2]);
			iv16[2] = (ushort)((encIV[5]<<8) | encIV[4]);
			iv16[3] = (ushort)((encIV[7]<<8) | encIV[6]);
			
			var tbuf = new ushort[4]; IdeaBase.encryptBlock(iv16,tbuf,encKey);
			var k = (byte)tbuf[0];
			var r = (byte)(p ^ k);
			
			for (var i=encIV.Length-1;i>0;i--) encIV[i] = encIV[i-1];
			encIV[0] = r;
			
			return r;
		}

		/// <summary>
		/// Расшифровывает байт данных.
		/// </summary>
		/// <param name="c">зашифрованный байт данных</param>
        private byte Decrypt(byte c)
		{
			var iv16 = new ushort[4];
			iv16[0] = (ushort)((decIV[1]<<8) | decIV[0]);
			iv16[1] = (ushort)((decIV[3]<<8) | decIV[2]);
			iv16[2] = (ushort)((decIV[5]<<8) | decIV[4]);
			iv16[3] = (ushort)((decIV[7]<<8) | decIV[6]);
			
			var tbuf = new ushort[4]; IdeaBase.encryptBlock(iv16,tbuf,encKey);
			var k = (byte)tbuf[0];
			var r = (byte)(c ^ k);
			
			for (var i=decIV.Length-1;i>0;i--) decIV[i] = decIV[i-1];
			decIV[0] = c;
			
			return r;
		}

        private void Encrypt(byte[] buf)
		{
			for (var i=0;i<buf.Length;i++) buf[i] = Encrypt(buf[i]);
		}

        private void Decrypt(byte[] buf)
		{
			for (var i=0;i<buf.Length;i++) buf[i] = Decrypt(buf[i]);
		}

        /// <summary>
        /// Преобразует пароль из шестнадцатиричного числа в массив 8-ми элементов.
        /// </summary>
        /// <param name="password">представление пароля hex числом</param>
        /// <returns></returns>
        private static ushort[] ConvertPassword(string password)
        {
            var pas = System.Text.Encoding.Default.GetBytes(password);
            var buf = new ushort[8];
            for (var i = 0; i < pas.Length; i+=2)
            {
                buf[i % 8] = (ushort)(((ushort)pas[i % pas.Length]) | (((ushort)pas[(i + 1) % pas.Length]) << 8));
            }
            return buf;
        }

        static public byte[] DefaultCryptIV = new byte[8] { 234, 125, 3, 70, 201, 91, 211, 29 };
    }

    /// <summary>
    /// Реализация алгоритма шифрации данных Idea.
    /// </summary>
    internal class IdeaBase
    {
        private const int IDEA_ROUNDS = 8;
        private const int IDEA_KEY_SIZE = IDEA_ROUNDS * 6 + 4;  // внутренний размер ключа в байтах

        //		typedef UInt16 KeyIDEA[IDEA_KEY_SIZE];

        /// <summary>
        /// Умножает два числа по модулю 2^16 + 1 (причём нулевое значение аргумента соответствует тому, что он равен 2^16).
        /// </summary>
        private static ushort mul(ushort x, ushort y)
        {
            unchecked
            {
                if (x != 0)
                {
                    if (y != 0)
                    {
                        var t = (uint)x * y;
                        y = (ushort)t;
                        x = (ushort)(t >> 16);
                        return (ushort)(y - x + (y < x ? 1u : 0u));
                    }
                    else
                    {
                        return (ushort)(1 - x);
                    }
                }
                else
                {
                    return (ushort)(1 - y);
                }
            }
        }

        /// <summary>
        /// Инвертирует число.
        /// </summary>
        private static ushort inv(ushort x)
        {
            ushort t0, t1;
            ushort q, y;

            if (x <= 1) return x;
            t1 = (ushort)(0x10001L / x);
            y = (ushort)(0x10001L % x);

            if (y == 1) return (ushort)(1 - t1);

            t0 = 1;
            do
            {
                q = (ushort)(x / y);
                x = (ushort)(x % y);
                t0 += (ushort)(q * t1);
                if (x == 1) return t0;
                q = (ushort)(y / x);
                y = (ushort)(y % x);
                t1 += (ushort)(q * t0);
            } while (y != 1);

            return (ushort)(1 - t1);
        }

        /// <summary>
        /// Преобразует ключ шифрования в ключ расшифрования.
        /// </summary>
        /// <param name="dec_key">вычисляемый ключ для расшифрования (IDEA_KEY_SIZE элементов)</param>
        /// <param name="enc_key">исходный ключ для шифрования (IDEA_KEY_SIZE элементов)</param>
        private static void invertKey(ushort[] dec_key, ushort[] enc_key)
        {
            unchecked
            {
                var i = 0;
                var j = 48;
                dec_key[i++] = inv(enc_key[j++]);
                dec_key[i++] = (ushort)(-enc_key[j++]);
                dec_key[i++] = (ushort)(-enc_key[j++]);
                dec_key[i++] = inv(enc_key[j++]);
                j = 42;
                while (i < IDEA_KEY_SIZE)
                {
                    dec_key[i++] = enc_key[j + 4];
                    dec_key[i++] = enc_key[j + 5];
                    dec_key[i++] = inv(enc_key[j]);
                    dec_key[i++] = (ushort)(-enc_key[j + 2]);
                    dec_key[i++] = (ushort)(-enc_key[j + 1]);
                    dec_key[i++] = inv(enc_key[j + 3]);
                    j -= 6;
                }
                var t = dec_key[i - 3];
                dec_key[i - 3] = dec_key[i - 2];
                dec_key[i - 2] = t;
            }
        }

        /// <summary>
        /// Создаёт внутренний ключ, используемый для шифрования.
        /// </summary>
        /// <param name="key_out">создаваемый ключ (52 элемента)</param>
        /// <param name="key_phraze">исходный ключ (8 элементов)</param>
        static public void makeEncryptKey(ushort[] key_out, ushort[] key_phraze)
        {
            int i, j, k = 0;

            for (j = 0; j < 8; j++) key_out[j] = key_phraze[j];
            for (i = 0; j < IDEA_KEY_SIZE; j++)
            {
                i++;
                key_out[i + 7 + k] = (ushort)(key_out[i & 7 + k] << 9 | key_out[i + 1 & 7 + k] >> 7);
                k += i & 8;
                i &= 7;
            }
        }

        /// <summary>
        /// Создаёт внутренний ключ для расшифрования.
        /// </summary>
        /// <param name="key_out">создаваемый ключ (52 элемента)</param>
        /// <param name="key_phraze">исходный ключ (8 элементов)</param>
        static public void makeDecryptKey(ushort[] key_out, ushort[] key_phraze)
        {
            var temp_key = new ushort[IDEA_KEY_SIZE];
            makeEncryptKey(temp_key, key_phraze);
            invertKey(key_out, temp_key);
        }

        /// <summary>
        /// Шифрует блок данных (4 слова * 2 байта = 8 байт).
        /// </summary>
        /// <param name="BufIn">шифруемый блок данных (4 слова = 8 байт)</param>
        /// <param name="BufOut">зашифрованный блок данных (4 слова = 8 байт)</param>
        /// <param name="key">внутренний ключ шифрования</param>
        static public void encryptBlock(ushort[] BufIn, ushort[] BufOut, ushort[] key)
        {
            ushort A, B, C, D, E, F;

            var i = 0;

            unchecked
            {
                A = BufIn[0];
                B = BufIn[1];
                C = BufIn[2];
                D = BufIn[3];

                for (var j = 0; j < 8; j++)
                {
                    A = mul(A, key[i++]);
                    B += key[i++];
                    C += key[i++];
                    D = mul(D, key[i++]);
                    F = (ushort)(A ^ C);
                    F = mul(F, key[i++]);
                    E = (ushort)(F + (B ^ D));
                    E = mul(E, key[i++]);
                    F = (ushort)(E + F);
                    A ^= E;
                    D ^= F;
                    F ^= B;
                    B = (ushort)(C ^ E);
                    C = F;
                }

                BufOut[0] = mul(A, key[i++]);
                BufOut[1] = (ushort)(C + key[i++]);
                BufOut[2] = (ushort)(B + key[i++]);
                BufOut[3] = mul(D, key[i]);
            }
        }

        static public ulong encryptBlock(ulong p, ushort[] key)
        {
            var buf = new ushort[4];
            buf[0] = (ushort)(p & 0xFFFF);
            buf[1] = (ushort)((p >> 16) & 0xFFFF);
            buf[2] = (ushort)((p >> 32) & 0xFFFF);
            buf[3] = (ushort)((p >> 48) & 0xFFFF);

            var r = new ushort[4];
            encryptBlock(buf, r, key);

            return ((ulong)r[3] << 48) | ((ulong)r[2] << 32) | ((ulong)r[1] << 16) | ((ulong)r[0]);
        }
    }
}
