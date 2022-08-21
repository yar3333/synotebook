using System;

namespace SYCrypt
{
	/// <summary>
	/// ���������� ��������� �������� ������ Idea.
	/// </summary>
	internal class IdeaBase
	{
		const int IDEA_ROUNDS = 8;
		const int IDEA_KEY_SIZE = IDEA_ROUNDS*6 + 4;	// ���������� ������ ����� � ������

//		typedef UInt16 KeyIDEA[IDEA_KEY_SIZE];

		/// <summary>
		/// �������� ��� ����� �� ������ 2^16 + 1 (������ ������� �������� ��������� ������������� ����, ��� �� ����� 2^16).
		/// </summary>
		static UInt16 mul(UInt16 x, UInt16 y)
		{
			unchecked
			{
				if (x!=0)
				{
					if(y!=0)
					{
						UInt32 t = (UInt32)x*y;
						y = (UInt16)t;
						x = (UInt16)(t>>16);
						return (UInt16)(y-x + (y<x ? 1u : 0u));
					}
					else
					{
						return (UInt16)(1-x);
					}
				}
				else
				{
					return (UInt16)(1-y);
				}
			}
		}

		/// <summary>
		/// ����������� �����.
		/// </summary>
		static UInt16 inv(UInt16 x)
		{
			UInt16 t0, t1;
			UInt16 q, y;

			if (x <= 1) return x;
			t1 = (UInt16)(0x10001L/x);
			y = (UInt16)(0x10001L%x);
			
			if (y == 1) return (UInt16)(1-t1);
			
			t0 = 1;
			do
			{
				q = (UInt16)(x / y);
				x = (UInt16)(x % y);
				t0 += (UInt16)(q * t1);
				if(x == 1) return t0;
				q = (UInt16)(y / x);
				y = (UInt16)(y % x);
				t1 += (UInt16)(q * t0);
			} while(y != 1);
			
			return (UInt16)(1-t1);
		}

		/// <summary>
		/// ����������� ���� ���������� � ���� �������������.
		/// </summary>
		/// <param name="dec_key">����������� ���� ��� ������������� (IDEA_KEY_SIZE ���������)</param>
		/// <param name="enc_key">�������� ���� ��� ���������� (IDEA_KEY_SIZE ���������)</param>
		static void invertKey(UInt16[] dec_key, UInt16[] enc_key)
		{
			unchecked
			{
				int i = 0;
				int j = 48;
				dec_key[i++] = inv(enc_key[j++]);
				dec_key[i++] = (UInt16)(-enc_key[j++]);
				dec_key[i++] = (UInt16)(-enc_key[j++]);
				dec_key[i++] = inv(enc_key[j++]);
				j = 42;
				while(i<IDEA_KEY_SIZE)
				{
					dec_key[i++] = enc_key[j+4];
					dec_key[i++] = enc_key[j+5];
					dec_key[i++] = inv(enc_key[j]);
					dec_key[i++] = (UInt16)(-enc_key[j+2]);
					dec_key[i++] = (UInt16)(-enc_key[j+1]);
					dec_key[i++] = inv(enc_key[j+3]);
					j -= 6;
				}
				UInt16 t = dec_key[i-3];
				dec_key[i-3] = dec_key[i-2];
				dec_key[i-2] = t;
			}
		}

		/// <summary>
		/// ������ ���������� ����, ������������ ��� ����������.
		/// </summary>
		/// <param name="key_out">����������� ���� (52 ��������)</param>
		/// <param name="key_phraze">�������� ���� (8 ���������)</param>
		static public void makeEncryptKey(UInt16[] key_out, UInt16[] key_phraze)
		{
			int i,j, k = 0;
			
			for (j=0;j<8;j++) key_out[j] = key_phraze[j];
			for (i=0;j<IDEA_KEY_SIZE;j++) 
			{
				i++;
				key_out[i+7 + k] = (UInt16)(key_out[i&7 + k] << 9 | key_out[i + 1&7 + k] >> 7);
				k += i & 8;
				i &= 7;
			}
		}

	  /// <summary>
	  /// ������ ���������� ���� ��� �������������.
	  /// </summary>
	  /// <param name="key_out">����������� ���� (52 ��������)</param>
	  /// <param name="key_phraze">�������� ���� (8 ���������)</param>
		static public void makeDecryptKey(UInt16[] key_out, UInt16[] key_phraze)
		{
			UInt16[] temp_key = new UInt16[IDEA_KEY_SIZE];
			makeEncryptKey(temp_key, key_phraze);
			invertKey(key_out, temp_key);
		}
   
		/// <summary>
		/// ������� ���� ������ (4 ����� * 2 ����� = 8 ����).
		/// </summary>
		/// <param name="BufIn">��������� ���� ������ (4 ����� = 8 ����)</param>
		/// <param name="BufOut">������������� ���� ������ (4 ����� = 8 ����)</param>
		/// <param name="key">���������� ���� ����������</param>
		static public void encryptBlock(UInt16[] BufIn, UInt16[] BufOut, UInt16[] key)
		{
			UInt16 A, B, C, D, E, F;
			
			int i = 0;
			
			unchecked
			{
				A = BufIn[0];
				B = BufIn[1];
				C = BufIn[2];
				D = BufIn[3];
				
				for(int j=0;j<8;j++)
				{
					A = mul(A, key[i++]);
					B += key[i++];
					C += key[i++];
					D = mul(D,key[i++]);
					F = (UInt16)(A^C);
					F = mul(F,key[i++]);
					E = (UInt16)(F+(B^D));
					E = mul(E,key[i++]);
					F = (UInt16)(E+F);
					A ^= E;
					D ^= F;
					F ^= B;
					B = (UInt16)(C^E);
					C = F;
				}

				BufOut[0] = mul(A, key[i++]);
				BufOut[1] = (UInt16)(C + key[i++]);
				BufOut[2] = (UInt16)(B + key[i++]);
				BufOut[3] = mul(D, key[i]);
			}
		}
		
		static public UInt64 encryptBlock(UInt64 p, UInt16[] key)
		{
			UInt16[] buf = new UInt16[4];
			buf[0] = (UInt16)(p & 0xFFFF);
			buf[1] = (UInt16)((p>>16) & 0xFFFF);
			buf[2] = (UInt16)((p>>32) & 0xFFFF);
			buf[3] = (UInt16)((p>>48) & 0xFFFF);
			
			UInt16[] r = new UInt16[4];
			encryptBlock(buf,r,key);
			
			return ((UInt64)r[3]<<48) | ((UInt64)r[2]<<32) | ((UInt64)r[1]<<16) | ((UInt64)r[0]);
		}
	}

	/// <summary>
	/// ��������� ���������������� ��������� ���������� Idea � ������ �������� ����� �� �����.
	/// </summary>
	public class IdeaCFB
	{
		byte[] encIV;
		byte[] decIV;

		UInt16[] encKey = new UInt16[52];
		
		/// <summary>
		/// �������������� ����� ��� ����������.
		/// </summary>
		/// <param name="IV">������ ������������� (8 ���������)</param>
		/// <param name="key">���� (8 ���������)</param>
		public IdeaCFB(byte[] IV, UInt16[] key)
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
        
        /// <summary>
		/// ������� ���� ������.
		/// </summary>
		/// <param name="p">�������� ���� ������</param>
		public byte Encrypt(byte p)
		{
			UInt16[] iv16 = new UInt16[4];
			iv16[0] = (UInt16)((encIV[1]<<8) | encIV[0]);
			iv16[1] = (UInt16)((encIV[3]<<8) | encIV[2]);
			iv16[2] = (UInt16)((encIV[5]<<8) | encIV[4]);
			iv16[3] = (UInt16)((encIV[7]<<8) | encIV[6]);
			
			UInt16[] tbuf = new UInt16[4]; IdeaBase.encryptBlock(iv16,tbuf,encKey);
			byte k = (byte)tbuf[0];
			byte r = (byte)(p ^ k);
			
			for (int i=encIV.Length-1;i>0;i--) encIV[i] = encIV[i-1];
			encIV[0] = r;
			
			return r;
		}

		/// <summary>
		/// �������������� ���� ������.
		/// </summary>
		/// <param name="c">������������� ���� ������</param>
		public byte Decrypt(byte c)
		{
			UInt16[] iv16 = new UInt16[4];
			iv16[0] = (UInt16)((decIV[1]<<8) | decIV[0]);
			iv16[1] = (UInt16)((decIV[3]<<8) | decIV[2]);
			iv16[2] = (UInt16)((decIV[5]<<8) | decIV[4]);
			iv16[3] = (UInt16)((decIV[7]<<8) | decIV[6]);
			
			UInt16[] tbuf = new UInt16[4]; IdeaBase.encryptBlock(iv16,tbuf,encKey);
			byte k = (byte)tbuf[0];
			byte r = (byte)(c ^ k);
			
			for (int i=decIV.Length-1;i>0;i--) decIV[i] = decIV[i-1];
			decIV[0] = c;
			
			return r;
		}
		
		public void Encrypt(byte[] buf)
		{
			for (int i=0;i<buf.Length;i++) buf[i] = Encrypt(buf[i]);
		}

		public void Decrypt(byte[] buf)
		{
			for (int i=0;i<buf.Length;i++) buf[i] = Decrypt(buf[i]);
		}

        public string Encrypt(string s)
        {
            byte[] buf = System.Text.Encoding.Default.GetBytes(s);
            Encrypt(buf);
            return System.Text.Encoding.Default.GetString(buf);
        }

        public string Decrypt(string s)
        {
            byte[] buf = System.Text.Encoding.Default.GetBytes(s);
            Decrypt(buf);
            return System.Text.Encoding.Default.GetString(buf);
        }

        /// <summary>
        /// ����������� ������ �� ������������������ ����� � ������ 8-�� ���������.
        /// </summary>
        /// <param name="password">������������� ������ hex ������</param>
        /// <returns></returns>
        static UInt16[] ConvertPassword(string password)
        {
            byte[] pas = System.Text.Encoding.Default.GetBytes(password);
            UInt16[] buf = new UInt16[8];
            for (int i = 0; i < pas.Length; i+=2)
            {
                buf[i % 8] = (UInt16)(((UInt16)pas[i % pas.Length]) | (((UInt16)pas[(i + 1) % pas.Length]) << 8));
            }
            return buf;
        }

        static public byte[] DefaultCryptIV = new byte[8] { 234, 125, 3, 70, 201, 91, 211, 29 };
    }
}
