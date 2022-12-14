using System;
using System.Text;
using System.IO;

namespace SyNotebook;

public class Note
{
    public class FileUpdatedOutsideProgram : Exception
    {
        private Guid NoteId { get; }
        public string NoteName { get; }

        public FileUpdatedOutsideProgram(Guid noteId,string noteName)
        {
            NoteId = noteId;
            NoteName = noteName;
        }
    }

    private bool isWasChanged;

    private class InputStreamEndReached : Exception {}
        
    public class BadPassword : Exception
    {
    }

    private Guid id;
    public Guid Id => id;

    private Guid parentId;
    public Guid ParentId
    {
        get => parentId;
        set
        {
            if (parentId == value) return;
            	
            parentId = value;
            isWasChanged = true;
        }
    }

    private int imageIndex;
    public int ImageIndex
    {
        get => imageIndex;
        set
        {
            if (imageIndex == value) return;
            	
            imageIndex = value;
            isWasChanged = true;
        }
    }

    private string name;
    public string Name
    {
        get
        {
            if (!isTopLevelCryptedItem) Unlock(Password);
            return name;
        }
        set
        {
            if (!isTopLevelCryptedItem) Unlock(Password);
                
            if (name != value)
            {
                name = value;
                isWasChanged = true;
            }
        }
    }

    private string text;
    public string Text
    {
        get
        {
            EnshureLoad();
                
            Unlock(Password);
                
            LastTextAccessDate = DateTime.Now;
            return text;
        }
        set
        {
            Unlock(Password);

            if (text != value)
            {
                text = value;
                isWasChanged = true;
            }
        }
    }

    public int Position { get; set; }

    [Flags]
    private enum Flags : byte
    {
        Crypted = 1,
        TopLevelCryptedItem = 2
    }

    private Flags flags = Flags.TopLevelCryptedItem;
        
    /// <summary>
    /// ???????????????? ???? ???????????? ?????????????? ?????????? ?????????????? ?? ???????????? ??????????????????????????.
    /// ???????? ????, ?? isCrypted=true, ???? name ???? ????????????????????, ?? ???????? ??????, ???? ????????????????????
    /// </summary>
    private bool isTopLevelCryptedItem
    {
        get => (flags & Flags.TopLevelCryptedItem) != 0;
        set
        {
            if (value) flags |= Flags.TopLevelCryptedItem;
            else       flags &= ~Flags.TopLevelCryptedItem;
            isWasChanged = true;
        }
    }
        
    /// <summary>
    /// ???????????????? ???? text ?????????????????????????? ???????????? ?????? ???????????? ?? ???????????????? ????????
    /// </summary>
    private bool isCrypted
    {
        get => (flags & Flags.Crypted) != 0;
        set
        {
            if (value) flags |=  Flags.Crypted;
            else       flags &= ~Flags.Crypted;
            isWasChanged = true;                
        }
    }

    public bool IsCrypted => isCrypted;


    /// <summary>
    /// ???????????????????? - ???????????????????????? ???? ???????????? ?? ?????????????? ???????????? ??????????????. ?????????? ?????????? ???????????? ???????? isCrypted=true;
    /// ???????? isLocked & isTopLevelCryptedItem - ???????? text ??????????????????????;
    /// ???????? isLocked & !isTopLevelCryptedItem - ???????? name ?? text ??????????????????????;
    /// ???????? !isLocked - ?????? ???????? ?????????????????? ???????????????? ??????????.
    /// </summary>
    public bool IsLocked { get; private set; } = true;

    public string Password { get; private set; } = "";

    /// <summary>
    /// ???????????????????????? ???????????? ???????? isCrypted=true 
    /// ?? ???????????????? ?????? ?????? ???????? ?????????????????????????? ????????????
    /// </summary>
    private uint textCrc32;

    private readonly DateTime loadedFileDate;
    public string FileName => Path.Combine(PathToDataFolder, id + ".note");

    private DateTime LastTextAccessDate { get; set; }

    private string PathToDataFolder { get; }

    private static void readFsFlags(Stream finp)
    {
        var n = finp.ReadByte(); 
        if (n == -1) throw new InputStreamEndReached();
    }

    private void writeFSFlags(Stream fout)
    {
        fout.WriteByte(0);
    }

    public Note(Guid id, Guid parentId, string name, string text, int position,int imageIndex, string pathToDataFolder)
    {
        isWasChanged = true;
        	
        this.id = id;
        this.parentId = parentId;
        this.name = name;
        this.text = text;
        this.Position = position;
        this.imageIndex = imageIndex;
            
        this.PathToDataFolder = pathToDataFolder;
        LastTextAccessDate = DateTime.Now;
    }

    private static void writeInt(Stream fout, int n)
    {
        fout.WriteByte((byte)(n & 0xFF));
        fout.WriteByte((byte)((n >> 8) & 0xFF));
        fout.WriteByte((byte)((n >> 16) & 0xFF));
        fout.WriteByte((byte)((n >> 24) & 0xFF));
    }

    private static int readInt(Stream finp)
    {
        var a = finp.ReadByte();
        var b = finp.ReadByte();
        var c = finp.ReadByte();
        var d = finp.ReadByte();
            
        return a | (b << 8) | (c << 16) | (d << 24);
    }

    private static void writeUInt(Stream fout, uint n)
    {
        fout.WriteByte((byte)(n & 0xFF));
        fout.WriteByte((byte)((n >> 8) & 0xFF));
        fout.WriteByte((byte)((n >> 16) & 0xFF));
        fout.WriteByte((byte)((n >> 24) & 0xFF));
    }

    private static uint readUInt(Stream finp)
    {
        uint a = (byte)finp.ReadByte();
        uint b = (byte)finp.ReadByte();
        uint c = (byte)finp.ReadByte();
        uint d = (byte)finp.ReadByte();

        return a | (b << 8) | (c << 16) | (d << 24);
    }

    private static void writeString(Stream fout, string s)
    {
        var buf = Encoding.UTF8.GetBytes(s);
        writeInt(fout, buf.Length + 1);
        fout.WriteByte(0);
        fout.Write(buf, 0, buf.Length);
    }

    private static string readString(Stream finp)
    {
        var size = readInt(finp); if (size == 0) return "";
        var buf = new byte[size];
        // ReSharper disable once MustUseReturnValue
        finp.Read(buf, 0, buf.Length);
        return buf[0] == '\0' ? Encoding.UTF8.GetString(buf, 1, buf.Length - 1) 
                              : Encoding.GetEncoding(1251).GetString(buf);
    }

    private void readFlags(Stream finp)
    {
        var n = finp.ReadByte(); if (n == -1) throw new InputStreamEndReached();
        flags = (Flags)(byte)n;
    }

    private void writeFlags(Stream fout)
    {
        fout.WriteByte((byte)flags);
    }

    public Note(string pathToDataFolder, string fileName)
    {
        isWasChanged = false;
        PathToDataFolder = pathToDataFolder;
        	
        LastTextAccessDate = File.GetLastWriteTime(fileName);

        var finp = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        readFsFlags(finp);
            
        var zippedFinp = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(finp);

        readFlags(zippedFinp);

        var idBytes = new byte[16];
        // ReSharper disable once MustUseReturnValue
        zippedFinp.Read(idBytes,0,16);
        id = new Guid(idBytes);

        var parentIdBytes = new byte[16];
        // ReSharper disable once MustUseReturnValue
        zippedFinp.Read(parentIdBytes, 0, 16);
        parentId = new Guid(parentIdBytes);

        if (isCrypted) textCrc32 = readUInt(zippedFinp);
                
        Position = readInt(zippedFinp);
        imageIndex = readInt(zippedFinp);
            
        name = readString(zippedFinp);
        suspendedFinp = finp;
        suspendedZippedFinp = zippedFinp;
            
        NotesAsyncLoader.Add(this);

        loadedFileDate = File.GetLastWriteTime(fileName);
    }

    private Stream suspendedZippedFinp;
    private Stream suspendedFinp;

    private void EnshureLoad()
    {
        while (suspendedFinp!=null) System.Threading.Thread.Sleep(20);
    }
        
    public void ContinueLoading()
    {
        if (suspendedFinp==null) return;
        	
        text = readString(suspendedZippedFinp);

        suspendedZippedFinp.Close();
        suspendedFinp.Close();
        suspendedFinp.Dispose();
            
        suspendedFinp = null;
        suspendedZippedFinp = null;
    }
        
    public void AbortLoading()
    {
        if (suspendedFinp == null) return;

        suspendedZippedFinp.Close();
        suspendedFinp.Close();
        suspendedFinp.Dispose();
            
        suspendedFinp = null;
        suspendedZippedFinp = null;
    }

    public void Write(string pathToDataFolder)
    {
        if (pathToDataFolder != PathToDataFolder) isWasChanged = true;
        	
        if (!isWasChanged)
        {
            File.SetLastWriteTime(FileName, LastTextAccessDate);
            return;
        }

        var newFileName = Path.Combine(pathToDataFolder,id+".note");
        if (newFileName == FileName
            && File.Exists(newFileName)
            && File.GetLastWriteTime(newFileName) != loadedFileDate
           ) throw new FileUpdatedOutsideProgram(id,name);

        if (File.Exists(FileName))
        {
            if (File.Exists(FileName+".bak")) File.Delete(FileName+".bak");
            File.Move(FileName,FileName+".bak");
        }
            
        var fout = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
        writeFSFlags(fout);

        var zippedFout = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(fout);

        writeFlags(zippedFout);
            
        zippedFout.Write(id.ToByteArray(), 0, 16);
        zippedFout.Write(parentId.ToByteArray(), 0, 16);
            
        if (isCrypted) Lock();

        if (isCrypted) writeUInt(zippedFout, textCrc32);

        writeInt(zippedFout, Position);
        writeInt(zippedFout, imageIndex);
            
        writeString(zippedFout, name);
        writeString(zippedFout, text);

        zippedFout.Close();
        fout.Close();
        fout.Dispose();
            
        File.SetLastWriteTime(FileName, LastTextAccessDate);

        if (File.Exists(FileName + ".bak")) File.Delete(FileName + ".bak");
            
        isWasChanged = false;
    }

    private static uint CalcCrc32(string s)
    {
        uint r = 0;
        foreach (var b in Encoding.UTF8.GetBytes(s)) unchecked { r += b; }
        return r;
    }    
    
    private static bool IsCrc32Correct(string s, uint mustBe)
    {
        uint r = 0;
        foreach (var b in Encoding.UTF8.GetBytes(s)) unchecked { r += b; }
        if (r == mustBe) return true;

        r = 0;
        foreach (var b in Encoding.GetEncoding(1251).GetBytes(s)) unchecked { r += b; }
        return r == mustBe;
    }

    /// <summary>
    /// ?????????????????? ?????????????????????? ???????????? ???? ???????????????? ???????????????? ??????????????????.
    /// </summary>
    public void Unlock(string password)
    {
        if (isCrypted && IsLocked)
        {
            if (string.IsNullOrEmpty(password)) throw new BadPassword();
                
            string potenName = null;
            string potenText = null;
                
            try
            {
                potenName = !isTopLevelCryptedItem ? Crypting.Aes.Decrypt(name, password) : "";
                potenText = Crypting.Aes.Decrypt(text, password);
            }
            catch {}

            if (potenName == null || potenText == null || !IsCrc32Correct(potenName + potenText, textCrc32))
            {
                var idea = new Crypting.IdeaCFB(password);
                potenName = !isTopLevelCryptedItem ? idea.Decrypt(name) : "";
                potenText = idea.Decrypt(text);
                if (!IsCrc32Correct(potenName + potenText, textCrc32)) throw new BadPassword(); 
            }

            Password = password;

            if (!isTopLevelCryptedItem) name = potenName;
            text = potenText;

            IsLocked = false;
        }
    }

    /// <summary>
    /// ?????????????????? ?????????????????????? ???????????? ?? ?????????????????????????? ??????????????????.
    /// </summary>
    public void Lock()
    {
        if (isCrypted && !IsLocked)
        {
            isCrypted = false;
            SetPassword(Password, isTopLevelCryptedItem);
        }
    }

    /// <summary>
    /// ???????????? ???????? ??????????????????????. ???????? ???? ???????????? ???????? ?????????????? ??????????????.
    /// </summary>
    public void SetPassword(string password, bool isTopLevelCryptedItem)
    {
        if (!isCrypted)
        {
            textCrc32 = CalcCrc32((!isTopLevelCryptedItem ? name : "") + text);
                
            if (!isTopLevelCryptedItem) name = Crypting.Aes.Encrypt(name, password);
            text = Crypting.Aes.Encrypt(text, password);

            isCrypted = true;
            IsLocked = true;

            this.Password = password;
            this.isTopLevelCryptedItem = isTopLevelCryptedItem;
        }
    }

    /// <summary>
    /// ?????????????? ???????????????? ???????? ????????????. ???????? ???? ?????????????? ???????? ????????????????????????.
    /// </summary>
    public void RemovePassword()
    {
        if (isCrypted && !IsLocked)
        {
            isCrypted = false;
            Password = "";
        }
    }
        
    public void Delete()
    {
        if (File.Exists(FileName)) File.Delete(FileName);
    }
}