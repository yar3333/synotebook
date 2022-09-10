using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SyNotebook;

public partial class MainForm : Form
{
    private static readonly string pathToDataFolder = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\Notes";
    private static readonly string pathToIniFile = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\SyNotebook.ini";

    private readonly Notebook notebook;

    private TreeNode draggedNode;

    private Font proportionalFont = new Font(FontFamily.GenericSansSerif, 10);
    private Font monospaceFont = new Font(FontFamily.GenericMonospace, 10);

    private readonly NotifyIcon trayIcon = new NotifyIcon();
    private Rectangle rcSaveWinPos;
    private FormWindowState lastWinState;

    private readonly ImageList imageList = new ImageList();
        
    public MainForm()
    {
        InitializeComponent();
            
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        trayIcon.Icon = new Icon(assembly.GetManifestResourceStream("SyNotebook.Images.App.ico"));
        trayIcon.Click += trayIcon_Click;

        // обработка imageList - создание элементов с подкрашенными уголками

        for (var i = 0; i < baseImageList.Images.Count; i++)
        {
            imageList.Images.Add(baseImageList.Images[i]);
        }

        for (var i = 0; i < baseImageList.Images.Count; i++)
        {
            var img = baseImageList.Images[i];
            var imgLocked = new Bitmap(img);
            for (var y=0;y<6;y++)
            for (var x = img.Width - (5 - y); x < img.Width; x++)
            {
                imgLocked.SetPixel(x, y, Color.Red);
            }
            imageList.Images.Add(imgLocked);
        }

        for (var i = 0; i < baseImageList.Images.Count; i++)
        {
            var img = baseImageList.Images[i];
            var imgUnLocked = new Bitmap(img);
            for (var y = 0; y < 6; y++)
            for (var x = img.Width - (5 - y); x < img.Width; x++)
            {
                imgUnLocked.SetPixel(x, y, Color.Green);
            }
            imageList.Images.Add(imgUnLocked);
        }

        tree.ImageList = imageList;
            
        notebook = new Notebook(pathToDataFolder, baseImageList.Images.Count);
        notebook.FillTreeView(tree,null);

        rtbNoteText.Font = proportionalFont;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        LoadGlobalParam();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        SaveGlobalParam();
    }

    private void AutoFillNotePositions(TreeNode root)
    {
        var note = (Note)root.Tag;
        if (note!=null) note.Position = root.Index;
        foreach (TreeNode node in root.Nodes) AutoFillNotePositions(node);
    }
        
    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        AutoFillNotePositions(tree.Nodes[0]);
            
        notebook.Save(pathToDataFolder);
    }

    private void trayIcon_Click(object sender, EventArgs e)
    {
        ShowInTaskbar = true;
        trayIcon.Visible = false;
        Bounds = rcSaveWinPos;
        WindowState = lastWinState;
    }

    private void btAddNote_Click(object sender, EventArgs e)
    {
        if (tree.SelectedNode==null) return;
            
        var form = new NoteForm(baseImageList);
        if (form.ShowDialog(this)==DialogResult.OK)
        {
            var dropNode = tree.SelectedNode;
            var dropNote = (Note)dropNode.Tag;
				
            if (!dropNote.IsCrypted)
            {
                tree.SelectedNode = notebook.AddNote(dropNode, form.tbBookmarkName.Text, form.ImageIndex);
            }
            else
            {
                var dropNodeTopCrypted = getTopCryptedParentNode(dropNode);
                var dropNoteTopCrypted = (Note)dropNodeTopCrypted.Tag;
                    
                if (unlockSubTree(dropNodeTopCrypted,"Введите пароль"))
                {
                    var password = dropNoteTopCrypted.Password;
                    notebook.RemovePassword(dropNodeTopCrypted, password);
                    tree.SelectedNode = notebook.AddNote(dropNode, form.tbBookmarkName.Text, form.ImageIndex);
                    notebook.SetPassword(dropNodeTopCrypted,password);
                    notebook.UnlockNote(dropNodeTopCrypted, password);
                }
            }
        }
    }

    private void tree_ItemDrag(object sender, ItemDragEventArgs e)
    {
        tree.SelectedNode = draggedNode;
        draggedNode = (TreeNode)e.Item;
        var strItem = e.Item.ToString();
        DoDragDrop(strItem, DragDropEffects.Copy | DragDropEffects.Move);
    }

    private void tree_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.Text))
            e.Effect = DragDropEffects.Move;
        else
            e.Effect = DragDropEffects.None;
    }

    private bool isNodeHasChild(TreeNode baseNode, TreeNode childNode)
    {
        foreach (TreeNode node in baseNode.Nodes)
        {
            if (node == childNode) return true;
            if (isNodeHasChild(node, childNode)) return true;
        }
        return false;
    }

    private TreeNode getTopCryptedParentNode(TreeNode node)
    {
        while (true)
        {
            if (node.Parent == null) return null;
            var note = (Note)node.Parent.Tag;
            if (note == null) return node;
            if (!note.IsCrypted || note.Id == Guid.Empty) return node;
            node = node.Parent;
        }
    }

    private void tree_DragDrop(object sender, DragEventArgs e)
    {
        var dropNode = tree.GetNodeAt(tree.PointToClient(new Point(e.X, e.Y)));
        if (dropNode == null || dropNode == draggedNode || isNodeHasChild(draggedNode, dropNode))
        {
            draggedNode = null;
            return;
        }

        var draggedNote = (Note)draggedNode.Tag;
        var dropNote = (Note)dropNode.Tag;
            
        // ситуации, когда возможно просто переподкрепить узел
        if (
            // оба не зашифрованы
            (!draggedNote.IsCrypted && !dropNote.IsCrypted)
            // оба шифрованы, но перенос - в пределах одного поддерева (а значит, и пароля)
            || (
                draggedNote.IsCrypted && dropNote.IsCrypted
                                      &&
                                      getTopCryptedParentNode(draggedNode) == getTopCryptedParentNode(dropNode)
            )
        ) {
            notebook.MoveNote(draggedNode, dropNode);
        }
        else // сложные ситуации
        {
            if (!draggedNote.IsCrypted && dropNote.IsCrypted)
            {
                var dropNodeTopCrypted = getTopCryptedParentNode(dropNode);
                var dropNoteTopCrypted = (Note)dropNodeTopCrypted.Tag;
                    
                if (unlockSubTree(dropNodeTopCrypted,"Введите пароль"))
                {
                    var password = dropNoteTopCrypted.Password;
                    notebook.RemovePassword(dropNodeTopCrypted, password);
                    notebook.MoveNote(draggedNode, dropNode);
                    notebook.SetPassword(dropNodeTopCrypted,password);
                    notebook.UnlockNote(dropNodeTopCrypted, password);
                }
            }
            else
            if (draggedNote.IsCrypted && !dropNote.IsCrypted)
            {
                if (getTopCryptedParentNode(draggedNode) == draggedNode)
                {
                    notebook.MoveNote(draggedNode, dropNode);
                }
                else
                {
                    if (draggedNote.IsLocked)
                        MessageBox.Show("Сначала разблокируйте элемент \"" + draggedNote.Name + "\"", "Отмена переноса");
                    else
                    {
                        notebook.RemovePassword(draggedNode, draggedNote.Password);
                        notebook.MoveNote(draggedNode, dropNode);
                    }
                }
            }
            else
            if (dropNote.IsCrypted && draggedNote.IsCrypted)
            {
                // убеждаемся, что цель разблокирована
                var dropNodeTopCrypted = getTopCryptedParentNode(dropNode);
                var dropNoteTopCrypted = (Note)dropNodeTopCrypted.Tag;
                if (unlockSubTree(dropNodeTopCrypted, "Введите пароль"))
                {
                    if (unlockSubTree(draggedNode, "Введите пароль"))
                    {
                        notebook.RemovePassword(draggedNode, draggedNote.Password);
							
                        var password = dropNoteTopCrypted.Password;
                        notebook.RemovePassword(dropNodeTopCrypted, password);
                        notebook.MoveNote(draggedNode, dropNode);
                        notebook.SetPassword(dropNodeTopCrypted,password);
                        notebook.UnlockNote(dropNodeTopCrypted, password);
                    }
                }
            }
        }

        tree.SelectedNode = draggedNode;
        draggedNode = null;
    }

    private void btDeleteBookmark_Click(object sender, EventArgs e)
    {
        if (tree.SelectedNode == null || tree.SelectedNode==tree.Nodes[0]) return;

        if (
            MessageBox.Show(
                "Вы уверены, что хотиту удалить элемент '" + tree.SelectedNode.Text + "' и все его подэлементы?",
                "Удаление элемента",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation
            ) == DialogResult.Yes
        ) notebook.DeleteNote(tree.SelectedNode);
    }

    private void tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        if (e.Label!=null && e.Label.Trim().Length > 0) 
            ((Note)e.Node.Tag).Name = e.Label;
        else 
            e.CancelEdit = true;
    }

    private void tree_AfterSelect(object sender, TreeViewEventArgs e)
    {
        var node = tree.SelectedNode;
        if (node==null) return;

        if (node == tree.Nodes[0])
        {
            rtbNoteText.Text = "";
            rtbNoteText.Enabled = false;
        }
        else
        {
            var note = (Note)node.Tag;
                
            if (!note.IsCrypted || (note.IsCrypted && unlockSubTree(node,"Введите пароль доступа")))
            {
                rtbNoteText.Enabled = false;	// оптимизация
                rtbNoteText.Rtf = note.Text;
                rtbNoteText.Enabled = true;
                if (note.Text.Length==0)
                {
                    rtbNoteText.Font = proportionalFont;
                }
            }
            else
            {
                rtbNoteText.Enabled = false;
                rtbNoteText.Rtf = "";
            }
        }
    }

    private bool unlockSubTree(TreeNode root, string preText)
    {
        var password = "";
        for (; ; )
        {
            var note = (Note)root.Tag;
            try
            {
                notebook.UnlockNote(root, password);
                return true;
            }
            catch (Note.BadPassword)
            {
                notebook.LockNote(root);
                var form = new PasswordForm();
                form.labText.Text = preText + " для узла \"" + note.Name + "\".";
                if (form.ShowDialog(this) == DialogResult.Cancel) return false;
                password = form.tbPassword.Text;
            }
        }
    }
        
    private void rtbNoteText_TextChanged(object sender, EventArgs e)
    {
        if (tree.SelectedNode != tree.Nodes[0] && rtbNoteText.Enabled)
        {
            ((Note)tree.SelectedNode.Tag).Text = rtbNoteText.Rtf;
        }
    }

    private Font getSelectionFont()
    {
        var r = rtbNoteText.SelectionFont;
        if (r == null)
        {
            var saveSelLen = rtbNoteText.SelectionLength;
            rtbNoteText.SelectionLength = 1;
            r = rtbNoteText.SelectionFont;
            rtbNoteText.SelectionLength = saveSelLen;
            if (r == null) r = proportionalFont;
        }
        return r;
    }

    private delegate void TextBlockFindedHandler();

    private void enumFontBlocks(bool ignoreFontSize, TextBlockFindedHandler fontBlockFinded)
    {
        var saveSelStart = rtbNoteText.SelectionStart;
        var saveSelLen = rtbNoteText.SelectionLength;

        var saveSelEnd = saveSelStart + saveSelLen;

        while (rtbNoteText.SelectionStart < saveSelEnd)
        {
            rtbNoteText.SelectionLength = 1;
            var saveFontSize = rtbNoteText.SelectionFont.Size;
            while (rtbNoteText.SelectionFont != null
                   && rtbNoteText.SelectionStart + rtbNoteText.SelectionLength < saveSelEnd
                   && (ignoreFontSize || rtbNoteText.SelectionFont.Size == saveFontSize)
                  ) rtbNoteText.SelectionLength++;
            if (rtbNoteText.SelectionFont == null 
                || (!ignoreFontSize && rtbNoteText.SelectionFont.Size != saveFontSize)
               ) rtbNoteText.SelectionLength--;
            if (rtbNoteText.SelectionLength > 0) fontBlockFinded();
            else break;
            rtbNoteText.SelectionStart += rtbNoteText.SelectionLength;
        }

        rtbNoteText.SelectionStart = saveSelStart;
        rtbNoteText.SelectionLength = saveSelLen;
    }
        
    private void btSetTextBold_Click(object sender, EventArgs e)
    {
        var baseFont = getSelectionFont();
        rtbNoteText.SelectionFont = new Font(
            baseFont,
            (baseFont.Style & FontStyle.Bold) != 0 ? 
                baseFont.Style ^ FontStyle.Bold : 
                baseFont.Style | FontStyle.Bold
        );
    }

    private void btSetTextItalic_Click(object sender, EventArgs e)
    {
        var baseFont = getSelectionFont();
        rtbNoteText.SelectionFont = new Font(
            baseFont,
            (baseFont.Style & FontStyle.Italic) != 0 ?
                baseFont.Style ^ FontStyle.Italic :
                baseFont.Style | FontStyle.Italic
        );
    }

    private void btSetTextColor_Click(object sender, EventArgs e)
    {
        rtbNoteText.SelectionColor = ((Button)sender).BackColor;
    }

    private void setFontProportional()
    {
        var font = new Font(proportionalFont, rtbNoteText.SelectionFont.Style);
        rtbNoteText.SelectionFont = font;
    }
    private void btSetTextFontProportional_Click(object sender, EventArgs e)
    {
        enumFontBlocks(true, setFontProportional);
    }

    private void setFontMonospace()
    {
        var font = new Font(monospaceFont, rtbNoteText.SelectionFont.Style);
        rtbNoteText.SelectionFont = font;
    }
    private void btSetTextFontMonospace_Click(object sender, EventArgs e)
    {
        enumFontBlocks(true, setFontMonospace);
    }

    private void setFontSizeInc()
    {
        rtbNoteText.SelectionFont = new Font(
            rtbNoteText.SelectionFont.FontFamily,
            rtbNoteText.SelectionFont.Size+1,
            rtbNoteText.SelectionFont.Style
        );
    }
    private void btFontSizeInc_Click(object sender, EventArgs e)
    {
        enumFontBlocks(false, setFontSizeInc);
    }

    private void setFontSizeDec()
    {
        rtbNoteText.SelectionFont = new Font(
            rtbNoteText.SelectionFont.FontFamily,
            Math.Max(rtbNoteText.SelectionFont.Size-1, 4),
            rtbNoteText.SelectionFont.Style
        );
    }
    private void btFontSizeDec_Click(object sender, EventArgs e)
    {
        enumFontBlocks(false, setFontSizeDec);
    }

    private void MainForm_Move(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Normal)
        {
            if (Left != -32000 || Top != -32000) rcSaveWinPos = Bounds;
            lastWinState = FormWindowState.Normal;
        }
        else
        if (WindowState == FormWindowState.Maximized)
        {
            lastWinState = FormWindowState.Maximized;
        }
        else
        if (WindowState == FormWindowState.Minimized)
        {
            ShowInTaskbar = false;
            trayIcon.Visible = true;
            trayIcon.Text = Text;
        }
    }

    private void MainForm_Resize(object sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Normal)
        {
            if (Left != -32000 || Top != -32000) rcSaveWinPos = Bounds;
        }
    }

    private void SaveGlobalParam()
    {
        var w = new System.Xml.XmlTextWriter(pathToIniFile, null);
        w.Formatting = System.Xml.Formatting.Indented;
        w.Indentation = 1;
        w.IndentChar = '\t';
        w.WriteStartDocument();
        w.WriteStartElement("SyNotebook");
        w.WriteStartElement("Fonts");
        w.WriteStartElement("Proportional");
        w.WriteAttributeString("FamilyName",proportionalFont.FontFamily.Name);
        w.WriteAttributeString("Size",proportionalFont.Size.ToString(CultureInfo.InvariantCulture));
        w.WriteEndElement();
        w.WriteStartElement("Monospace");
        w.WriteAttributeString("FamilyName",monospaceFont.FontFamily.Name);
        w.WriteAttributeString("Size",monospaceFont.Size.ToString(CultureInfo.InvariantCulture));
        w.WriteEndElement();
        w.WriteEndElement();
        w.WriteStartElement("WindowPos");
        w.WriteAttributeString("Left",rcSaveWinPos.Left.ToString());
        w.WriteAttributeString("Top",rcSaveWinPos.Top.ToString());
        w.WriteAttributeString("Width",rcSaveWinPos.Width.ToString());
        w.WriteAttributeString("Height",rcSaveWinPos.Height.ToString());
        w.WriteAttributeString("WindowState", ((int)WindowState).ToString());
        w.WriteAttributeString("SplitterPos", splitContainer1.SplitterDistance.ToString());
        w.WriteEndElement();
        w.WriteEndElement();
        w.WriteEndDocument();
        w.Close();
    }

    private void LoadGlobalParam()
    {
        if (!System.IO.File.Exists(pathToIniFile)) return;

        var xml = new System.Xml.XmlDocument();
        xml.Load(pathToIniFile);
			
        try
        {
            proportionalFont = new Font(
                xml["SyNotebook"]["Fonts"]["Proportional"].Attributes["FamilyName"].Value,
                float.Parse(xml["SyNotebook"]["Fonts"]["Proportional"].Attributes["Size"].Value)
            );
            monospaceFont = new Font(
                xml["SyNotebook"]["Fonts"]["Monospace"].Attributes["FamilyName"].Value,
                float.Parse(xml["SyNotebook"]["Fonts"]["Monospace"].Attributes["Size"].Value)
            );
        }
        catch {}
				
        try
        {
            System.Xml.XmlNode windowPos = xml["SyNotebook"]["WindowPos"];
            Left = rcSaveWinPos.X = int.Parse(windowPos.Attributes["Left"].Value);
            Top = rcSaveWinPos.Y = int.Parse(windowPos.Attributes["Top"].Value);
            Width = rcSaveWinPos.Width = int.Parse(windowPos.Attributes["Width"].Value);
            Height = rcSaveWinPos.Height = int.Parse(windowPos.Attributes["Height"].Value);
            WindowState = (FormWindowState)(int.Parse(windowPos.Attributes["WindowState"].Value));
            splitContainer1.SplitterDistance = int.Parse(windowPos.Attributes["SplitterPos"].Value);
        }
        catch {}
    }

    private void tree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
    {
        e.CancelEdit = e.Node == tree.Nodes[0];
    }

    private void miAddItem_Click(object sender, EventArgs e)
    {
        btAddNote_Click(sender, e);
    }

    private void miDeleteItem_Click(object sender, EventArgs e)
    {
        btDeleteBookmark_Click(sender, e);
    }

    private void miSetPassword_Click(object sender, EventArgs e)
    {
        btSetPassword_Click(sender, e);
    }

    private void miRemovePassword_Click(object sender, EventArgs e)
    {
        btRemovePassword_Click(sender, e);
    }

    private bool isNodeHasCryptedParents(TreeNode node)
    {
        while (true)
        {
            if (node?.Parent == null) return false;
            var note = (Note)node.Parent.Tag;
            if (note == null) return false;
            if (note.IsCrypted) return true;
            node = node.Parent;
        }
    }

    private void btSetPassword_Click(object sender, EventArgs e)
    {
        if (tree.SelectedNode != null && tree.SelectedNode != tree.Nodes[0])
        {
            var node = getTopCryptedParentNode(tree.SelectedNode);
            if (node == null) return;
                
            if (node != tree.SelectedNode)
            {
                if (
                    MessageBox.Show(
                        "Внимание! Пароль будет установлен для всего шифрованного поддерева - начиная с узла \""+node.Name+"\".",
                        "Установка пароля",
                        MessageBoxButtons.OKCancel
                    )==DialogResult.Cancel
                ) return;
            }

            if (unlockSubTree(tree.SelectedNode,"Введите старый пароль"))
            {
                var note = (Note)node.Tag;
                notebook.RemovePassword(node, note.Password);
                var form2 = new PasswordNewForm();
                form2.labText.Text = "Введите новый пароль для узла \"" + note.Name + "\"";
                if (form2.ShowDialog(this) == DialogResult.Cancel) return;
                notebook.SetPassword(node, form2.tbPassword.Text);
                notebook.UnlockNote(node, form2.tbPassword.Text);
            }
        }
    }

    private void btRemovePassword_Click(object sender, EventArgs e)
    {
        if (tree.SelectedNode != null)
        {
            var node = getTopCryptedParentNode(tree.SelectedNode);
            if (node == null) return;
                
            if (node!=tree.SelectedNode)
            {
                if (
                    MessageBox.Show(
                        "Внимание! Пароль будет убран для всего шифрованного поддерева - начиная с узла \""+node.Text+"\".",
                        "Удаление пароля",
                        MessageBoxButtons.OKCancel
                    )==DialogResult.Cancel
                ) return;
            }
                
            if (unlockSubTree(tree.SelectedNode,"Введите пароль"))
            {
                notebook.RemovePassword(node, ((Note)tree.SelectedNode.Tag).Password);
            }
        }
    }

    private void tree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        var note = (Note)e.Node.Tag;
        if (note == null) return;

        if (note.IsCrypted)
        {
            if (!unlockSubTree(e.Node, "Введите пароль"))
            {
                e.Cancel = true;
            }
        }
    }

    private void tree_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            tree.SelectedNode = tree.GetNodeAt(e.X ,e.Y );
        }
    }

    private void treeMenu_Opening(object sender, CancelEventArgs e)
    {
        if (tree.SelectedNode == null)
        {
            e.Cancel = true;
            return;
        }

        var isRoot = tree.SelectedNode == tree.Nodes[0];
            
        miSetPassword.Visible = !isRoot;
        miRemovePassword.Visible = !isRoot;
        miLock.Visible = !isRoot;
        miProperties.Visible = !isRoot;
        miSeparator1.Visible = !isRoot;
        miSeparator2.Visible = !isRoot;

        if (isRoot) return;
            
        var note = (Note)tree.SelectedNode.Tag;

        miAddItem.Enabled = !note.IsCrypted || !note.IsLocked;
        miSetPassword.Enabled = !note.IsCrypted || !note.IsLocked;
        miRemovePassword.Enabled = note.IsCrypted && !note.IsLocked;
        miLock.Enabled = note.IsCrypted && !note.IsLocked;
    }

    private void tree_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (tree.SelectedNode == null) return;

        var note = (Note)tree.SelectedNode.Tag;
            
        if (note.Id == Guid.Empty)
        {
            tree.SelectedNode.Expand();
            return;
        }
            
        var form = new NoteForm(baseImageList);
        form.tbBookmarkName.Text = note.Name;
        form.ImageIndex = note.ImageIndex;
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            notebook.EditNote(tree.SelectedNode, form.tbBookmarkName.Text, form.ImageIndex);
        }
    }

    private void btChangeParam_Click(object sender, EventArgs e)
    {
        var form = new ParamForm();
        form.propFontDialog.Font = proportionalFont;
        form.monoFontDialog.Font = monospaceFont;
			
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            proportionalFont = form.propFontDialog.Font;
            monospaceFont = form.monoFontDialog.Font;
        }
    }

    private void miCut_Click(object sender, EventArgs e)
    {
        rtbNoteText.Cut();
    }

    private void miCopy_Click(object sender, EventArgs e)
    {
        if (rtbNoteText.SelectedText == "")
        {
            var start = rtbNoteText.GetCharIndexFromPosition(rtbMouseOnConectMenu.Location);
            var n = start;
            while (n >= 0)
            {
                var s = rtbNoteText.Text.Substring(n);
                if (s.StartsWith("http://") || s.StartsWith("https://") || s.StartsWith("ftp://"))
                {
                    var m = Regex.Match(s, @"^[^ \t(),!\r\n""']+");
                    if (m.Success && n + m.Length >= start)  Clipboard.SetText(m.Value);
                    else break;
                }
                n--;
            }
        }
        else
        {
            rtbNoteText.Copy();
        }
    }

    private void miPaste_Click(object sender, EventArgs e)
    {
        rtbNoteText.Paste();
    }

    private void rtbNoteText_MouseEnter(object sender, EventArgs e)
    {
        rtbNoteText.Focus();
    }

    private void btSetTextUnderline_Click(object sender, EventArgs e)
    {
        var baseFont = getSelectionFont();
        rtbNoteText.SelectionFont = new Font(
            baseFont,
            (baseFont.Style & FontStyle.Underline) != 0 ? 
                baseFont.Style ^ FontStyle.Underline : 
                baseFont.Style | FontStyle.Underline
        );
    }

    private void btUndentDec_Click(object sender, EventArgs e)
    {
        rtbNoteText.SelectionIndent = Math.Max(0,rtbNoteText.SelectionIndent-20);
    }

    private void btIndentInc_Click(object sender, EventArgs e)
    {
        rtbNoteText.SelectionIndent += 20;
    }

    private void rtbNoteText_LinkClicked(object sender, LinkClickedEventArgs e)
    {
        try { System.Diagnostics.Process.Start(e.LinkText); }
        catch {}
    }

    private void miLock_Click(object sender, EventArgs e)
    {
        if (tree.SelectedNode != null && tree.SelectedNode != tree.Nodes[0])
        {
            var selectedNote = (Note)tree.SelectedNode.Tag;
            if (!selectedNote.IsCrypted || selectedNote.IsLocked) return;
                
            var node = getTopCryptedParentNode(tree.SelectedNode);
            if (node == null) return;

            if (node != tree.SelectedNode)
            {
                if (
                    MessageBox.Show(
                        "Внимание! Блокировка возможна только с самого верхнего шифрованного узла \"" + node.Text + "\". Заблокировать, начиная с него?",
                        "Блокирование",
                        MessageBoxButtons.OKCancel
                    ) == DialogResult.Cancel
                ) return;
            }

            notebook.LockNote(node);
                
            node.Collapse();
            rtbNoteText.Enabled = false;
            rtbNoteText.Rtf = "";
        }
    }

    private void miProperties_Click(object sender, EventArgs e)
    {
        tree_MouseDoubleClick(null, null);
    }

    private MouseEventArgs rtbMouseOnConectMenu;

    private void rtbNoteText_MouseUp(object sender, MouseEventArgs e)
    {
        rtbMouseOnConectMenu = e;
    }
}