using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace SyNotebook
{
    class Notebook
    {
        enum Images
        {
            Uncrypted,
            Locked,
            UnLocked
        }

        Dictionary<Guid, Note> notes = new Dictionary<Guid, Note>(10);

        string pathToDataFolder;

        List<Note> getNotesByParentID(Guid parentID)
        {
            List<Note> r = new List<Note>();
            foreach (Guid noteID in notes.Keys)
            {
                if (notes[noteID].ParentID == parentID) r.Add(notes[noteID]);
            }

            r.Sort(new NotePositionComparer());

            return r;
        }

        class NotePositionComparer : IComparer<Note>
        {
            public int Compare(Note x, Note y)
            {
                if (x.Position < y.Position) return -1;
                if (x.Position > y.Position) return +1;
                return 0;
            }
        }
        
        class NoteLastTextAccessDateComparer : IComparer<Note>
        {
            public int Compare(Note x, Note y)
            {
                if (x.LastTextAccessDate < y.LastTextAccessDate) return -1;
                if (x.LastTextAccessDate > y.LastTextAccessDate) return +1;
                return 0;
            }
        }

        int imageListLength;
        
        class NoteFile : IComparable<NoteFile>
        {
        	string fileName;
        	public string FileName { get { return fileName; } }
        	DateTime date;
        	public DateTime Date { get { return date; } }
        	
        	public NoteFile(string fileName,DateTime date)
        	{
        		this.fileName = fileName;
        		this.date = date;
        	}
        	
			public int CompareTo(NoteFile other)
			{
				return date.CompareTo(other.date);
			}
        }
        
        public Notebook(string pathToDataFolder, int imageListLength)
        {
            this.pathToDataFolder = pathToDataFolder;
            this.imageListLength = imageListLength;
            
            if (!System.IO.Directory.Exists(pathToDataFolder)) System.IO.Directory.CreateDirectory(pathToDataFolder);
            
            List<NoteFile> noteFiles = new List<NoteFile>();
            foreach (string s in System.IO.Directory.GetFiles(pathToDataFolder,"*.note"))
            {
            	noteFiles.Add(new NoteFile(s, System.IO.File.GetLastWriteTime(s)));
            }
            noteFiles.Sort();
            
            for (int i=noteFiles.Count-1;i>=0;i--)
            {
            	Note note = new Note(pathToDataFolder, noteFiles[i].FileName);
                notes.Add(note.ID, note);
            }
            
            NotesAsyncLoader.Start();
        }

        public void Save(string pathToDataFolder)
        {
            NotesAsyncLoader.Abort();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        	
        	foreach (Guid noteID in notes.Keys)
            {
	            Note note = notes[noteID];
            	
            	try
	            {
	                note.Write(pathToDataFolder);
	            }
	            catch (Note.FileUpdatedOutsideProgram e)
	            {
	                MessageBox.Show(
	                    "Файл заметки '" + e.NoteName + "' был изменён с момента последнего считывания. После нажатия на 'ОК' файл будет перезаписан данными из памяти. При необходимости, вручную сохраните этот файл под другим именем.",
	                    "Предупреждение",
	                    MessageBoxButtons.OK,
	                    MessageBoxIcon.Warning
	                );
	
	                System.IO.File.Delete(note.FileName);
	                note.Write(pathToDataFolder);
	            }
            }
        }

        public void FillTreeView(TreeView tree, TreeNode root)
        {
            if (root==null)
            {
                tree.Nodes.Clear();
                root = new TreeNode();
                root.Text = "Заметки";
                root.Tag = new Note(Guid.Empty,Guid.Empty,"Заметки",null, 0, 0, pathToDataFolder);
                tree.Nodes.Add(root);
                root.Expand();
            }

            List<Note> childNotes = getNotesByParentID(((Note)root.Tag).ID);
            foreach (Note note in childNotes)
            {
                TreeNode node = new TreeNode();
                try { node.Text = note.Name; }
                catch (Note.BadPassword) {}
                node.Tag = note;
                root.Nodes.Add(node);
                updateNodeImage(node);
                node.Collapse();

                FillTreeView(tree, node);
            }
        }

        void updateNodeImage(TreeNode node)
        {
            Note note = (Note)node.Tag;
            if (!note.IsCrypted) node.ImageIndex = (int)Images.Uncrypted * imageListLength + note.ImageIndex;
            else node.ImageIndex = (note.IsLocked ? (int)Images.Locked : (int)Images.UnLocked) * imageListLength + note.ImageIndex;
            node.SelectedImageIndex = node.ImageIndex;
        }
        
        public TreeNode AddNote(TreeNode root, string name, int imageIndex)
        {
            Note note = new Note(Guid.NewGuid(), ((Note)root.Tag).ID, name, "", root.Nodes.Count, imageIndex, pathToDataFolder);
            notes.Add(note.ID, note);

            TreeNode node = new TreeNode(note.Name);
            node.Tag = note;
            root.Nodes.Add(node);

            updateNodeImage(node);

            return node;
        }

        public void EditNote(TreeNode root, string name, int imageIndex)
        {
            Note note = (Note)root.Tag;
            note.Name = name;
            root.Text = name;
            note.ImageIndex = imageIndex;
            updateNodeImage(root);
        }

        public void MoveNote(TreeNode node, TreeNode newParentNode)
        {
            ((Note)node.Tag).ParentID = ((Note)newParentNode.Tag).ID;
            ((Note)node.Tag).Position = newParentNode.Nodes.Count;

            node.Parent.Nodes.Remove(node);
            newParentNode.Nodes.Add(node);
        }

        void DeleteNote(Note note)
        {
            List<Note> childs = getNotesByParentID(note.ID);
            foreach (Note child in childs) DeleteNote(child);
            notes.Remove(note.ID);
            note.Delete();
        }

        public void DeleteNote(TreeNode node)
        {
            node.Parent.Nodes.Remove(node);

            Note note = (Note)node.Tag;
            DeleteNote(note);
        }

        public void UnlockNote(TreeNode node, string password)
        {
            Note note = (Note)node.Tag;
            note.Unlock(password);
            
            node.Text = note.Name;
            updateNodeImage(node);

            foreach (TreeNode child in node.Nodes) UnlockNote(child, password);
        }

        /*public void LockNote(TreeNode node)
        {
            Note note = (Note)node.Tag;
            note.Lock();

            updateNodeImage(node);

            foreach (TreeNode child in node.Nodes) LockNote(child);
        }*/

        public void LockNote(TreeNode node)
        {
            Note note = (Note)node.Tag;
            note.Lock();
            
            node.Text = "";
            try { node.Text = note.Name; }
            catch (Note.BadPassword) { }
            updateNodeImage(node);

            foreach (TreeNode child in node.Nodes) LockNote(child);
        }

        public void SetPassword(TreeNode node, string password)
        {
            setPassword(node,password,true);
        }

        void setPassword(TreeNode node, string password, bool isTopLevelCryptedItem)
        {
            Note note = (Note)node.Tag;
            note.SetPassword(password, isTopLevelCryptedItem);
            updateNodeImage(node);

            foreach (TreeNode child in node.Nodes) setPassword(child, password, false);
        }

        public void RemovePassword(TreeNode node, string password)
        {
            UnlockNote(node, password);
            
            Note note = (Note)node.Tag;
            note.RemovePassword();
            updateNodeImage(node);

            foreach (TreeNode child in node.Nodes) RemovePassword(child, password);
        }
    }
}
