using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SyNotebook;

internal class Notebook
{
    private enum Images
    {
        Uncrypted,
        Locked,
        UnLocked
    }

    private readonly Dictionary<Guid, Note> notes = new(10);

    private readonly string pathToDataFolder;

    private List<Note> GetNotesByParentId(Guid parentId)
    {
        var r = new List<Note>();
        foreach (var noteId in notes.Keys)
        {
            if (notes[noteId].ParentId == parentId) r.Add(notes[noteId]);
        }

        r.Sort(new NotePositionComparer());

        return r;
    }

    private class NotePositionComparer : IComparer<Note>
    {
        public int Compare(Note x, Note y)
        {
            if (x.Position < y.Position) return -1;
            if (x.Position > y.Position) return +1;
            return 0;
        }
    }

    private readonly int imageListLength;

    private class NoteFile : IComparable<NoteFile>
    {
        public readonly string FileName;
        private readonly DateTime date;

        public NoteFile(string fileName,DateTime date)
        {
        	this.FileName = fileName;
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
        
        if (!Directory.Exists(pathToDataFolder)) Directory.CreateDirectory(pathToDataFolder);
        
        var noteFiles = new List<NoteFile>();
        foreach (var s in Directory.GetFiles(pathToDataFolder,"*.note"))
        {
            noteFiles.Add(new NoteFile(s, File.GetLastWriteTime(s)));
        }
        noteFiles.Sort();
        
        for (var i = noteFiles.Count - 1; i >= 0; i--)
        {
            var note = new Note(pathToDataFolder, noteFiles[i].FileName);
            notes.Add(note.Id, note);
        }
        
        NotesAsyncLoader.Start();
    }

    // ReSharper disable once ParameterHidesMember
    public void Save(string pathToDataFolder)
    {
        NotesAsyncLoader.Abort();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        
        foreach (var noteId in notes.Keys)
        {
	        var note = notes[noteId];
            
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

	            File.Delete(note.FileName);
	            note.Write(pathToDataFolder);
	        }
        }
    }

    public void FillTreeView(TreeView tree, TreeNode root)
    {
        if (root==null)
        {
            tree.Nodes.Clear();
            root = new TreeNode
            {
                Text = "Заметки",
                Tag = new Note(Guid.Empty,Guid.Empty,"Заметки",null, 0, 0, pathToDataFolder)
            };
            tree.Nodes.Add(root);
            root.Expand();
        }

        var childNotes = GetNotesByParentId(((Note)root.Tag).Id);
        foreach (var note in childNotes)
        {
            var node = new TreeNode();
            try { node.Text = note.Name; }
            catch (Note.BadPassword) {}
            node.Tag = note;
            root.Nodes.Add(node);
            updateNodeImage(node);
            node.Collapse();

            FillTreeView(tree, node);
        }
    }

    private void updateNodeImage(TreeNode node)
    {
        var note = (Note)node.Tag;
        // ReSharper disable once UselessBinaryOperation
        if (!note.IsCrypted) node.ImageIndex = (int)Images.Uncrypted * imageListLength + note.ImageIndex;
        else node.ImageIndex = (note.IsLocked ? (int)Images.Locked : (int)Images.UnLocked) * imageListLength + note.ImageIndex;
        node.SelectedImageIndex = node.ImageIndex;
    }
    
    public TreeNode AddNote(TreeNode root, string name, int imageIndex)
    {
        var note = new Note(Guid.NewGuid(), ((Note)root.Tag).Id, name, "", root.Nodes.Count, imageIndex, pathToDataFolder);
        notes.Add(note.Id, note);

        var node = new TreeNode(note.Name)
        {
            Tag = note
        };
        root.Nodes.Add(node);

        updateNodeImage(node);

        return node;
    }

    public void EditNote(TreeNode root, string name, int imageIndex)
    {
        var note = (Note)root.Tag;
        note.Name = name;
        root.Text = name;
        note.ImageIndex = imageIndex;
        updateNodeImage(root);
    }

    public void MoveNote(TreeNode node, TreeNode newParentNode)
    {
        ((Note)node.Tag).ParentId = ((Note)newParentNode.Tag).Id;
        ((Note)node.Tag).Position = newParentNode.Nodes.Count;

        node.Parent.Nodes.Remove(node);
        newParentNode.Nodes.Add(node);
    }

    private void DeleteNote(Note note)
    {
        var childs = GetNotesByParentId(note.Id);
        foreach (var child in childs) DeleteNote(child);
        notes.Remove(note.Id);
        note.Delete();
    }

    public void DeleteNote(TreeNode node)
    {
        node.Parent.Nodes.Remove(node);

        var note = (Note)node.Tag;
        DeleteNote(note);
    }

    public void UnlockNote(TreeNode node, string password)
    {
        var note = (Note)node.Tag;
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
        var note = (Note)node.Tag;
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

    private void setPassword(TreeNode node, string password, bool isTopLevelCryptedItem)
    {
        var note = (Note)node.Tag;
        note.SetPassword(password, isTopLevelCryptedItem);
        updateNodeImage(node);

        foreach (TreeNode child in node.Nodes) setPassword(child, password, false);
    }

    public void RemovePassword(TreeNode node, string password)
    {
        UnlockNote(node, password);
        
        var note = (Note)node.Tag;
        note.RemovePassword();
        updateNodeImage(node);

        foreach (TreeNode child in node.Nodes) RemovePassword(child, password);
    }
}
