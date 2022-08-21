using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SyNotebook
{
    public class Note
    {
        public class FileUpdatedOutsideProgram : Exception
        {
    		Guid noteID;
    		public Guid NoteID { get { return noteID; } }

    		string noteName;
    		public string NoteName { get { return noteName; } }
    		
    		public FileUpdatedOutsideProgram(Guid noteID,string noteName)
    		{
    			this.noteID = noteID;
    			this.noteName = noteName;
    		}
    	}

        public string _lastTextAccessDate
        {
        	get { return lastTextAccessDate.ToString(); }
        }
        
        bool isWasChanged;
        
        public class InputStreamEndReached : Exception {}
        public class BadPassword : Exception
        {
            Note note;
            public Note Note { get { return note; } }
            public BadPassword(Note note) { this.note = note; }
        }
        
        Guid id;
        public Guid ID { get { return id; } }

        Guid parentID;
        public Guid ParentID
        {
            get { return parentID; }
            set
            {
            	if (parentID == value) return;
            	
            	parentID = value;
            	isWasChanged = true;
            }
        }

        int imageIndex = 0;
        public int ImageIndex
        {
            get { return imageIndex; }
            set
            {
            	if (imageIndex==value) return;
            	
            	imageIndex = value;
                isWasChanged = true;
            }
        }
        
        string name;
        public string Name
        {
            get
            {
                if (!isTopLevelCryptedItem) Unlock(password);
                return name;
            }
            set
            {
            	if (!isTopLevelCryptedItem) Unlock(password);
                
            	if (name!=value)
            	{
	            	name = value;
	                isWasChanged = true;
            	}
            }
        }

        string text;
        public string Text
        {
            get
            {
                EnshureLoad();
                
            	Unlock(password);
                
                lastTextAccessDate = DateTime.Now;
                return text;
            }
            set
            {
            	Unlock(password);

            	if (text!=value)
            	{
	                text = value;
	                isWasChanged = true;
            	}
            }
        }

        int position;
        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        enum Flags : byte
        {
            Crypted=1,
            TopLevelCryptedItem=2
        }
        Flags flags = Flags.TopLevelCryptedItem;
        
        /// <summary>
        /// Является ли данный элемент самым верхним в списке зашифрованных.
        /// Если да, и isCrypted=true, то name не зашифрован, а если нет, то зашифрован
        /// </summary>
        bool isTopLevelCryptedItem
        {
            get { return (flags & Flags.TopLevelCryptedItem) != 0; }
            set
            {
                if (value) flags |= Flags.TopLevelCryptedItem;
                else       flags &= ~Flags.TopLevelCryptedItem;
                isWasChanged = true;
            }
        }
        
        /// <summary>
        /// Содержит ли text зашифрованные данные или данные в открытом виде
        /// </summary>
        bool isCrypted
        {
            get { return (flags & Flags.Crypted) != 0; }
            set
            {
                if (value) flags |=  Flags.Crypted;
                else       flags &= ~Flags.Crypted;
				isWasChanged = true;                
            }
        }

        public bool IsCrypted { get { return isCrypted; } }

        
        /// <summary>
        /// Показывает - заблокирован ли объект в текущий момент времени. Имеет смысл только если isCrypted=true;
        /// если isLocked & isTopLevelCryptedItem - поле text защифровано;
        /// если isLocked & !isTopLevelCryptedItem - поля name и text защифрованы;
        /// если !isLocked - все поля содержать открытый текст.
        /// </summary>
        bool isLocked = true;
        public bool IsLocked
        {
            get { return isLocked; }
        }

        string password = "";
        public string Password
        {
            get { return password; }
            set
            {
                if (password==value) return;
                
                password = value;
                isWasChanged = true;
            }
        }

        /// <summary>
        /// Используется только если isCrypted=true 
        /// и содержит код для всех защифрованных частей
        /// </summary>
        UInt32 textCRC32;
        
        DateTime loadedFileDate;
        public string FileName
        { 
        	get 
        	{
        		return System.IO.Path.Combine(pathToDataFolder, id+".note");
        	}
       	}
        
        DateTime lastTextAccessDate;
        public DateTime LastTextAccessDate { get { return lastTextAccessDate; } }

        string pathToDataFolder;
        public string PathToDataFolder
        {
        	get { return pathToDataFolder; }
        }

        void readFSFlags(Stream finp)
        {
            int n = finp.ReadByte(); if (n == -1) throw new Note.InputStreamEndReached();
            byte flags = (byte)n;
        }

        void writeFSFlags(Stream fout)
        {
            fout.WriteByte(0);
        }

        public Note(Guid id, Guid parentID, string name, string text, int position,int imageIndex, string pathToDataFolder)
        {
            isWasChanged = true;
        	
        	this.id = id;
            this.parentID = parentID;
            this.name = name;
            this.text = text;
            this.position = position;
            this.imageIndex = imageIndex;
            
            this.pathToDataFolder = pathToDataFolder;
            this.lastTextAccessDate = DateTime.Now;
        }

        static void writeInt(Stream fout, int n)
        {
            fout.WriteByte((byte)(n & 0xFF));
            fout.WriteByte((byte)((n >> 8) & 0xFF));
            fout.WriteByte((byte)((n >> 16) & 0xFF));
            fout.WriteByte((byte)((n >> 24) & 0xFF));
        }

        static int readInt(Stream finp)
        {
            int a = finp.ReadByte();
            int b = finp.ReadByte();
            int c = finp.ReadByte();
            int d = finp.ReadByte();
            
            return a | (b << 8) | (c << 16) | (d << 24);
        }

        static void writeUInt(Stream fout, UInt32 n)
        {
            fout.WriteByte((byte)(n & 0xFF));
            fout.WriteByte((byte)((n >> 8) & 0xFF));
            fout.WriteByte((byte)((n >> 16) & 0xFF));
            fout.WriteByte((byte)((n >> 24) & 0xFF));
        }

        static UInt32 readUInt(Stream finp)
        {
            UInt32 a = (byte)finp.ReadByte();
            UInt32 b = (byte)finp.ReadByte();
            UInt32 c = (byte)finp.ReadByte();
            UInt32 d = (byte)finp.ReadByte();

            return a | (b << 8) | (c << 16) | (d << 24);
        }

        static void writeString(Stream fout, string s)
        {
            byte[] buf = System.Text.Encoding.Default.GetBytes(s);
            Int32 size = buf.Length;
            writeInt(fout, size);
            fout.Write(buf, 0, buf.Length);
        }

        static string readString(Stream finp)
        {
            Int32 size = readInt(finp); if (size==0) return "";
            byte[] buf = new byte[size];
            finp.Read(buf, 0, buf.Length);
            return System.Text.Encoding.Default.GetString(buf);
        }

        void readFlags(Stream finp)
        {
            int n = finp.ReadByte(); if (n == -1) throw new InputStreamEndReached();
            flags = (Flags)(byte)n;
        }

        void writeFlags(Stream fout)
        {
            fout.WriteByte((byte)flags);
        }

        public Note(string pathToDataFolder, string fileName)
        {
        	isWasChanged = false;
            this.pathToDataFolder = pathToDataFolder;
        	
            lastTextAccessDate = System.IO.File.GetLastWriteTime(fileName);

            FileStream finp = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            readFSFlags(finp);
            
            ICSharpCode.SharpZipLib.GZip.GZipInputStream zippedFINP = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(finp);

            readFlags(zippedFINP);

            byte[] id_bytes = new byte[16];
            zippedFINP.Read(id_bytes,0,16);
            id = new Guid(id_bytes);

            byte[] parentID_bytes = new byte[16];
            zippedFINP.Read(parentID_bytes, 0, 16);
            parentID = new Guid(parentID_bytes);

            if (isCrypted) textCRC32 = readUInt(zippedFINP);
                
            position = readInt(zippedFINP);
            imageIndex = readInt(zippedFINP);
            
            name = readString(zippedFINP);
            suspended_finp = finp;
            suspended_zippedFINP = zippedFINP;
            
            NotesAsyncLoader.Add(this);

            loadedFileDate = System.IO.File.GetLastWriteTime(fileName);
        }

        Stream suspended_zippedFINP;
        Stream suspended_finp;
        
        public void EnshureLoad()
        {
        	while (suspended_finp!=null) System.Threading.Thread.Sleep(20);
        }
        
        public void ContinueLoading()
        {
        	if (suspended_finp==null) return;
        	
        	text = readString(suspended_zippedFINP);

            suspended_zippedFINP.Close();
            suspended_finp.Close();
            suspended_finp.Dispose();
            
            suspended_finp = null;
            suspended_zippedFINP = null;
        }
        
        public void AbortLoading()
        {
        	if (suspended_finp==null) return;

        	suspended_zippedFINP.Close();
            suspended_finp.Close();
            suspended_finp.Dispose();
            
            suspended_finp = null;
            suspended_zippedFINP = null;
        }

        public void Write(string pathToDataFolder)
        {
        	if (pathToDataFolder!=this.pathToDataFolder) isWasChanged = true;
        	
        	if (!isWasChanged)
        	{
        		System.IO.File.SetLastWriteTime(FileName, lastTextAccessDate);
        		return;
        	}

        	string newFileName = System.IO.Path.Combine(pathToDataFolder,id+".note");
        	if (newFileName == FileName
                && System.IO.File.Exists(newFileName)
                && System.IO.File.GetLastWriteTime(newFileName) != loadedFileDate
            ) throw new FileUpdatedOutsideProgram(id,name);

            if (System.IO.File.Exists(FileName))
            {
				if (System.IO.File.Exists(FileName+".bak")) System.IO.File.Delete(FileName+".bak");
				System.IO.File.Move(FileName,FileName+".bak");
            }
            
            FileStream fout = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            writeFSFlags(fout);

            ICSharpCode.SharpZipLib.GZip.GZipOutputStream zippedFOUT = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(fout);

            writeFlags(zippedFOUT);
            
            zippedFOUT.Write(id.ToByteArray(), 0, 16);
            zippedFOUT.Write(parentID.ToByteArray(), 0, 16);
            
            if (isCrypted) Lock();

            if (isCrypted) writeUInt(zippedFOUT, textCRC32);

            writeInt(zippedFOUT, position);
            writeInt(zippedFOUT, imageIndex);
            
            writeString(zippedFOUT, name);
            writeString(zippedFOUT, text);

            zippedFOUT.Close();
            fout.Close();
            fout.Dispose();
            
            System.IO.File.SetLastWriteTime(FileName, lastTextAccessDate);

            if (System.IO.File.Exists(FileName+".bak")) System.IO.File.Delete(FileName+".bak");
            
            isWasChanged = false;
        }
        
        public static UInt32 GetTextCRC(string s)
        {
            UInt32 r = 0;

            byte[] buf = System.Text.Encoding.Default.GetBytes(s);
            for (int i = 0; i < buf.Length; i++)
            {
                unchecked { r += buf[i]; }
            }

            return r;
        }

        /// <summary>
        /// Переводит шифрованный объект во временно открытое состояние.
        /// </summary>
        public void Unlock(string password)
        {
            if (isCrypted && isLocked)
            {
                if (password==null || password.Length==0) throw new BadPassword(this);
                
                string potenName = "";
                if (!isTopLevelCryptedItem) potenName = Crypting.IdeaCFB.Decrypt(name, password);

                string potenText = Crypting.IdeaCFB.Decrypt(text, password);

                var crc = GetTextCRC(potenName + potenText);
                if (crc != textCRC32) throw new BadPassword(this);

                this.password = password;

                if (!isTopLevelCryptedItem) name = potenName;
                text = potenText;

                isLocked = false;
            }
        }

        /// <summary>
        /// Переводит шифрованный объект в защифрованное состояние.
        /// </summary>
        public void Lock()
        {
            if (isCrypted && !isLocked)
            {
                isCrypted = false;
                SetPassword(password, isTopLevelCryptedItem);
            }
        }

        /// <summary>
        /// Делает узел шифрованным. Узел не должен быть защищён паролем.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="isTopLevelCryptedItem"></param>
        public void SetPassword(string password, bool isTopLevelCryptedItem)
        {
            if (!isCrypted)
            {
                textCRC32 = GetTextCRC((!isTopLevelCryptedItem ? name : "") + text);
                
                if (!isTopLevelCryptedItem) name = Crypting.IdeaCFB.Encrypt(name, password);
                text = Crypting.IdeaCFB.Encrypt(text, password);

                isCrypted = true;
                isLocked = true;

                this.password = password;
                this.isTopLevelCryptedItem = isTopLevelCryptedItem;
            }
        }

        /// <summary>
        /// Убирает шифрацию узла вообще. Узел не должнен быть заблокирован.
        /// </summary>
        public void RemovePassword()
        {
            if (isCrypted && !isLocked)
            {
                isCrypted = false;
                this.password = "";
            }
        }
        
        public void Delete()
        {
        	if (System.IO.File.Exists(FileName)) System.IO.File.Delete(FileName);
        }
    }
}
