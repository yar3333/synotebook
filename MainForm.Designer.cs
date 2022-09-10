namespace SyNotebook
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolTip toolTip;
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Заметки");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btSetTextFontMonospace = new System.Windows.Forms.Button();
            this.btSetTextFontProportional = new System.Windows.Forms.Button();
            this.btFontSizeInc = new System.Windows.Forms.Button();
            this.btFontSizeDec = new System.Windows.Forms.Button();
            this.btDeleteBookmark = new System.Windows.Forms.Button();
            this.btChangeParam = new System.Windows.Forms.Button();
            this.btRemovePassword = new System.Windows.Forms.Button();
            this.btSetPassword = new System.Windows.Forms.Button();
            this.btAddBookmark = new System.Windows.Forms.Button();
            this.btSetTextUnderline = new System.Windows.Forms.Button();
            this.btSetTextItalic = new System.Windows.Forms.Button();
            this.btSetTextBold = new System.Windows.Forms.Button();
            this.btUndentDec = new System.Windows.Forms.Button();
            this.btIndentInc = new System.Windows.Forms.Button();
            this.tree = new System.Windows.Forms.TreeView();
            this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miSetPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemovePassword = new System.Windows.Forms.ToolStripMenuItem();
            this.miLock = new System.Windows.Forms.ToolStripMenuItem();
            this.miSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbNoteText = new System.Windows.Forms.RichTextBox();
            this.textMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miCut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.miPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btSetTextColor8 = new System.Windows.Forms.Button();
            this.btSetTextColor9 = new System.Windows.Forms.Button();
            this.btSetTextColor14 = new System.Windows.Forms.Button();
            this.btSetTextColor0 = new System.Windows.Forms.Button();
            this.btSetTextColor3 = new System.Windows.Forms.Button();
            this.btSetTextColor4 = new System.Windows.Forms.Button();
            this.baseImageList = new System.Windows.Forms.ImageList(this.components);
            toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.treeMenu.SuspendLayout();
            this.textMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSetTextFontMonospace
            // 
            this.btSetTextFontMonospace.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btSetTextFontMonospace.Location = new System.Drawing.Point(241, 14);
            this.btSetTextFontMonospace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextFontMonospace.Name = "btSetTextFontMonospace";
            this.btSetTextFontMonospace.Size = new System.Drawing.Size(56, 27);
            this.btSetTextFontMonospace.TabIndex = 2;
            this.btSetTextFontMonospace.Text = "моно";
            toolTip.SetToolTip(this.btSetTextFontMonospace, "Моноширинный шрифт");
            this.btSetTextFontMonospace.UseVisualStyleBackColor = true;
            this.btSetTextFontMonospace.Click += new System.EventHandler(this.btSetTextFontMonospace_Click);
            // 
            // btSetTextFontProportional
            // 
            this.btSetTextFontProportional.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btSetTextFontProportional.Location = new System.Drawing.Point(189, 14);
            this.btSetTextFontProportional.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextFontProportional.Name = "btSetTextFontProportional";
            this.btSetTextFontProportional.Size = new System.Drawing.Size(51, 27);
            this.btSetTextFontProportional.TabIndex = 2;
            this.btSetTextFontProportional.Text = "проп";
            toolTip.SetToolTip(this.btSetTextFontProportional, "Пропорциональный шрифт");
            this.btSetTextFontProportional.UseVisualStyleBackColor = true;
            this.btSetTextFontProportional.Click += new System.EventHandler(this.btSetTextFontProportional_Click);
            // 
            // btFontSizeInc
            // 
            this.btFontSizeInc.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btFontSizeInc.Location = new System.Drawing.Point(115, 14);
            this.btFontSizeInc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btFontSizeInc.Name = "btFontSizeInc";
            this.btFontSizeInc.Size = new System.Drawing.Size(30, 27);
            this.btFontSizeInc.TabIndex = 12;
            this.btFontSizeInc.Text = "+";
            toolTip.SetToolTip(this.btFontSizeInc, "Увеличить шрифт");
            this.btFontSizeInc.UseVisualStyleBackColor = true;
            this.btFontSizeInc.Click += new System.EventHandler(this.btFontSizeInc_Click);
            // 
            // btFontSizeDec
            // 
            this.btFontSizeDec.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btFontSizeDec.Location = new System.Drawing.Point(147, 14);
            this.btFontSizeDec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btFontSizeDec.Name = "btFontSizeDec";
            this.btFontSizeDec.Size = new System.Drawing.Size(30, 27);
            this.btFontSizeDec.TabIndex = 12;
            this.btFontSizeDec.Text = "-";
            toolTip.SetToolTip(this.btFontSizeDec, "Уменьщить шрифт");
            this.btFontSizeDec.UseVisualStyleBackColor = true;
            this.btFontSizeDec.Click += new System.EventHandler(this.btFontSizeDec_Click);
            // 
            // btDeleteBookmark
            // 
            this.btDeleteBookmark.Image = global::SyNotebook.Properties.Resources.imgDeleteItem;
            this.btDeleteBookmark.Location = new System.Drawing.Point(46, 12);
            this.btDeleteBookmark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btDeleteBookmark.Name = "btDeleteBookmark";
            this.btDeleteBookmark.Size = new System.Drawing.Size(35, 29);
            this.btDeleteBookmark.TabIndex = 3;
            toolTip.SetToolTip(this.btDeleteBookmark, "Удалить элемент");
            this.btDeleteBookmark.UseVisualStyleBackColor = true;
            this.btDeleteBookmark.Click += new System.EventHandler(this.btDeleteBookmark_Click);
            // 
            // btChangeParam
            // 
            this.btChangeParam.Image = global::SyNotebook.Properties.Resources.imgChangeParam;
            this.btChangeParam.Location = new System.Drawing.Point(168, 12);
            this.btChangeParam.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btChangeParam.Name = "btChangeParam";
            this.btChangeParam.Size = new System.Drawing.Size(31, 29);
            this.btChangeParam.TabIndex = 2;
            toolTip.SetToolTip(this.btChangeParam, "Настройка");
            this.btChangeParam.UseVisualStyleBackColor = true;
            this.btChangeParam.Click += new System.EventHandler(this.btChangeParam_Click);
            // 
            // btRemovePassword
            // 
            this.btRemovePassword.Image = global::SyNotebook.Properties.Resources.imgRemovePassword;
            this.btRemovePassword.Location = new System.Drawing.Point(126, 12);
            this.btRemovePassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btRemovePassword.Name = "btRemovePassword";
            this.btRemovePassword.Size = new System.Drawing.Size(31, 29);
            this.btRemovePassword.TabIndex = 2;
            toolTip.SetToolTip(this.btRemovePassword, "Выключить шифрование раздела");
            this.btRemovePassword.UseVisualStyleBackColor = true;
            this.btRemovePassword.Click += new System.EventHandler(this.btRemovePassword_Click);
            // 
            // btSetPassword
            // 
            this.btSetPassword.Image = global::SyNotebook.Properties.Resources.imgSetPassword;
            this.btSetPassword.Location = new System.Drawing.Point(91, 12);
            this.btSetPassword.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetPassword.Name = "btSetPassword";
            this.btSetPassword.Size = new System.Drawing.Size(31, 29);
            this.btSetPassword.TabIndex = 2;
            toolTip.SetToolTip(this.btSetPassword, "Включить шифрование раздела");
            this.btSetPassword.UseVisualStyleBackColor = true;
            this.btSetPassword.Click += new System.EventHandler(this.btSetPassword_Click);
            // 
            // btAddBookmark
            // 
            this.btAddBookmark.Image = global::SyNotebook.Properties.Resources.imgNewItem;
            this.btAddBookmark.Location = new System.Drawing.Point(10, 12);
            this.btAddBookmark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btAddBookmark.Name = "btAddBookmark";
            this.btAddBookmark.Size = new System.Drawing.Size(31, 29);
            this.btAddBookmark.TabIndex = 2;
            toolTip.SetToolTip(this.btAddBookmark, "Создать элемент");
            this.btAddBookmark.UseVisualStyleBackColor = true;
            this.btAddBookmark.Click += new System.EventHandler(this.btAddNote_Click);
            // 
            // btSetTextUnderline
            // 
            this.btSetTextUnderline.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.btSetTextUnderline.Image = global::SyNotebook.Properties.Resources.imgUnderline;
            this.btSetTextUnderline.Location = new System.Drawing.Point(72, 14);
            this.btSetTextUnderline.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextUnderline.Name = "btSetTextUnderline";
            this.btSetTextUnderline.Size = new System.Drawing.Size(30, 27);
            this.btSetTextUnderline.TabIndex = 2;
            toolTip.SetToolTip(this.btSetTextUnderline, "Подчёркнутый");
            this.btSetTextUnderline.UseVisualStyleBackColor = true;
            this.btSetTextUnderline.Click += new System.EventHandler(this.btSetTextUnderline_Click);
            // 
            // btSetTextItalic
            // 
            this.btSetTextItalic.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.btSetTextItalic.Image = global::SyNotebook.Properties.Resources.imgItalic;
            this.btSetTextItalic.Location = new System.Drawing.Point(41, 14);
            this.btSetTextItalic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextItalic.Name = "btSetTextItalic";
            this.btSetTextItalic.Size = new System.Drawing.Size(30, 27);
            this.btSetTextItalic.TabIndex = 2;
            toolTip.SetToolTip(this.btSetTextItalic, "Курсив");
            this.btSetTextItalic.UseVisualStyleBackColor = true;
            this.btSetTextItalic.Click += new System.EventHandler(this.btSetTextItalic_Click);
            // 
            // btSetTextBold
            // 
            this.btSetTextBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btSetTextBold.Image = global::SyNotebook.Properties.Resources.imgBold;
            this.btSetTextBold.Location = new System.Drawing.Point(9, 14);
            this.btSetTextBold.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextBold.Name = "btSetTextBold";
            this.btSetTextBold.Size = new System.Drawing.Size(30, 27);
            this.btSetTextBold.TabIndex = 2;
            toolTip.SetToolTip(this.btSetTextBold, "Жирный");
            this.btSetTextBold.UseVisualStyleBackColor = true;
            this.btSetTextBold.Click += new System.EventHandler(this.btSetTextBold_Click);
            // 
            // btUndentDec
            // 
            this.btUndentDec.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btUndentDec.Image = global::SyNotebook.Properties.Resources.imgIndentDec;
            this.btUndentDec.Location = new System.Drawing.Point(308, 14);
            this.btUndentDec.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btUndentDec.Name = "btUndentDec";
            this.btUndentDec.Size = new System.Drawing.Size(30, 27);
            this.btUndentDec.TabIndex = 12;
            toolTip.SetToolTip(this.btUndentDec, "Уменьшить отступ");
            this.btUndentDec.UseVisualStyleBackColor = true;
            this.btUndentDec.Click += new System.EventHandler(this.btUndentDec_Click);
            // 
            // btIndentInc
            // 
            this.btIndentInc.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btIndentInc.Image = global::SyNotebook.Properties.Resources.imgIndentInc;
            this.btIndentInc.Location = new System.Drawing.Point(340, 14);
            this.btIndentInc.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btIndentInc.Name = "btIndentInc";
            this.btIndentInc.Size = new System.Drawing.Size(30, 27);
            this.btIndentInc.TabIndex = 12;
            toolTip.SetToolTip(this.btIndentInc, "Увеличить отступ");
            this.btIndentInc.UseVisualStyleBackColor = true;
            this.btIndentInc.Click += new System.EventHandler(this.btIndentInc_Click);
            // 
            // tree
            // 
            this.tree.AllowDrop = true;
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.ContextMenuStrip = this.treeMenu;
            this.tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tree.HideSelection = false;
            this.tree.LabelEdit = true;
            this.tree.Location = new System.Drawing.Point(0, 47);
            this.tree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tree.Name = "tree";
            treeNode1.Name = "Root";
            treeNode1.Text = "Заметки";
            treeNode1.ToolTipText = "Корневой раздел";
            this.tree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tree.ShowNodeToolTips = true;
            this.tree.ShowRootLines = false;
            this.tree.Size = new System.Drawing.Size(199, 366);
            this.tree.TabIndex = 0;
            this.tree.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_BeforeLabelEdit);
            this.tree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tree_AfterLabelEdit);
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tree_ItemDrag);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            this.tree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tree_DragDrop);
            this.tree.DragEnter += new System.Windows.Forms.DragEventHandler(this.tree_DragEnter);
            this.tree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDoubleClick);
            this.tree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDown);
            // 
            // treeMenu
            // 
            this.treeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddItem,
            this.miDeleteItem,
            this.miSeparator1,
            this.miSetPassword,
            this.miRemovePassword,
            this.miLock,
            this.miSeparator2,
            this.miProperties});
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(215, 148);
            this.treeMenu.Opening += new System.ComponentModel.CancelEventHandler(this.treeMenu_Opening);
            // 
            // miAddItem
            // 
            this.miAddItem.Image = global::SyNotebook.Properties.Resources.imgNewItem;
            this.miAddItem.Name = "miAddItem";
            this.miAddItem.Size = new System.Drawing.Size(214, 22);
            this.miAddItem.Text = "Добавить элемент";
            this.miAddItem.Click += new System.EventHandler(this.miAddItem_Click);
            // 
            // miDeleteItem
            // 
            this.miDeleteItem.Image = global::SyNotebook.Properties.Resources.imgDeleteItem;
            this.miDeleteItem.Name = "miDeleteItem";
            this.miDeleteItem.Size = new System.Drawing.Size(214, 22);
            this.miDeleteItem.Text = "Удалить элемент";
            this.miDeleteItem.Click += new System.EventHandler(this.miDeleteItem_Click);
            // 
            // miSeparator1
            // 
            this.miSeparator1.Name = "miSeparator1";
            this.miSeparator1.Size = new System.Drawing.Size(211, 6);
            // 
            // miSetPassword
            // 
            this.miSetPassword.Image = global::SyNotebook.Properties.Resources.imgSetPassword;
            this.miSetPassword.Name = "miSetPassword";
            this.miSetPassword.Size = new System.Drawing.Size(214, 22);
            this.miSetPassword.Text = "Включить шифрование";
            this.miSetPassword.Click += new System.EventHandler(this.miSetPassword_Click);
            // 
            // miRemovePassword
            // 
            this.miRemovePassword.Image = global::SyNotebook.Properties.Resources.imgRemovePassword;
            this.miRemovePassword.Name = "miRemovePassword";
            this.miRemovePassword.Size = new System.Drawing.Size(214, 22);
            this.miRemovePassword.Text = "Выключить шифрование";
            this.miRemovePassword.Click += new System.EventHandler(this.miRemovePassword_Click);
            // 
            // miLock
            // 
            this.miLock.Name = "miLock";
            this.miLock.Size = new System.Drawing.Size(214, 22);
            this.miLock.Text = "Зашифровать обратно";
            this.miLock.ToolTipText = "После того, как вы ввели пароль к разделу, он временно расшифровывается. Нажмите " +
    "сюда, чтобы снова зашифровать старым паролем.";
            this.miLock.Click += new System.EventHandler(this.miLock_Click);
            // 
            // miSeparator2
            // 
            this.miSeparator2.Name = "miSeparator2";
            this.miSeparator2.Size = new System.Drawing.Size(211, 6);
            // 
            // miProperties
            // 
            this.miProperties.Name = "miProperties";
            this.miProperties.Size = new System.Drawing.Size(214, 22);
            this.miProperties.Text = "Свойства";
            this.miProperties.Click += new System.EventHandler(this.miProperties_Click);
            // 
            // rtbNoteText
            // 
            this.rtbNoteText.AcceptsTab = true;
            this.rtbNoteText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbNoteText.ContextMenuStrip = this.textMenu;
            this.rtbNoteText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbNoteText.HideSelection = false;
            this.rtbNoteText.Location = new System.Drawing.Point(1, 47);
            this.rtbNoteText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtbNoteText.Name = "rtbNoteText";
            this.rtbNoteText.Size = new System.Drawing.Size(632, 366);
            this.rtbNoteText.TabIndex = 1;
            this.rtbNoteText.Text = "";
            this.rtbNoteText.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.rtbNoteText_LinkClicked);
            this.rtbNoteText.TextChanged += new System.EventHandler(this.rtbNoteText_TextChanged);
            this.rtbNoteText.MouseEnter += new System.EventHandler(this.rtbNoteText_MouseEnter);
            this.rtbNoteText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtbNoteText_MouseUp);
            // 
            // textMenu
            // 
            this.textMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miCut,
            this.miCopy,
            this.miPaste});
            this.textMenu.Name = "textMenu";
            this.textMenu.Size = new System.Drawing.Size(140, 70);
            // 
            // miCut
            // 
            this.miCut.Image = global::SyNotebook.Properties.Resources.imgCut;
            this.miCut.Name = "miCut";
            this.miCut.Size = new System.Drawing.Size(139, 22);
            this.miCut.Text = "Вырезать";
            this.miCut.Click += new System.EventHandler(this.miCut_Click);
            // 
            // miCopy
            // 
            this.miCopy.Image = global::SyNotebook.Properties.Resources.imgCopy;
            this.miCopy.Name = "miCopy";
            this.miCopy.Size = new System.Drawing.Size(139, 22);
            this.miCopy.Text = "Копировать";
            this.miCopy.Click += new System.EventHandler(this.miCopy_Click);
            // 
            // miPaste
            // 
            this.miPaste.Image = global::SyNotebook.Properties.Resources.imgPaste;
            this.miPaste.Name = "miPaste";
            this.miPaste.Size = new System.Drawing.Size(139, 22);
            this.miPaste.Text = "Вставить";
            this.miPaste.Click += new System.EventHandler(this.miPaste_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btDeleteBookmark);
            this.splitContainer1.Panel1.Controls.Add(this.btChangeParam);
            this.splitContainer1.Panel1.Controls.Add(this.btRemovePassword);
            this.splitContainer1.Panel1.Controls.Add(this.btSetPassword);
            this.splitContainer1.Panel1.Controls.Add(this.btAddBookmark);
            this.splitContainer1.Panel1.Controls.Add(this.tree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btIndentInc);
            this.splitContainer1.Panel2.Controls.Add(this.btUndentDec);
            this.splitContainer1.Panel2.Controls.Add(this.btFontSizeDec);
            this.splitContainer1.Panel2.Controls.Add(this.btFontSizeInc);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor8);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor9);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor14);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor0);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor3);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextColor4);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextFontMonospace);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextFontProportional);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextUnderline);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextItalic);
            this.splitContainer1.Panel2.Controls.Add(this.btSetTextBold);
            this.splitContainer1.Panel2.Controls.Add(this.rtbNoteText);
            this.splitContainer1.Size = new System.Drawing.Size(825, 413);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 5;
            // 
            // btSetTextColor8
            // 
            this.btSetTextColor8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor8.BackColor = System.Drawing.Color.Gray;
            this.btSetTextColor8.ForeColor = System.Drawing.Color.Gray;
            this.btSetTextColor8.Location = new System.Drawing.Point(531, 14);
            this.btSetTextColor8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor8.Name = "btSetTextColor8";
            this.btSetTextColor8.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor8.TabIndex = 9;
            this.btSetTextColor8.UseVisualStyleBackColor = false;
            this.btSetTextColor8.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // btSetTextColor9
            // 
            this.btSetTextColor9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btSetTextColor9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btSetTextColor9.Location = new System.Drawing.Point(468, 14);
            this.btSetTextColor9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor9.Name = "btSetTextColor9";
            this.btSetTextColor9.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor9.TabIndex = 4;
            this.btSetTextColor9.UseVisualStyleBackColor = false;
            this.btSetTextColor9.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // btSetTextColor14
            // 
            this.btSetTextColor14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btSetTextColor14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btSetTextColor14.Location = new System.Drawing.Point(500, 14);
            this.btSetTextColor14.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor14.Name = "btSetTextColor14";
            this.btSetTextColor14.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor14.TabIndex = 6;
            this.btSetTextColor14.UseVisualStyleBackColor = false;
            this.btSetTextColor14.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // btSetTextColor0
            // 
            this.btSetTextColor0.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor0.BackColor = System.Drawing.Color.Black;
            this.btSetTextColor0.Location = new System.Drawing.Point(437, 14);
            this.btSetTextColor0.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor0.Name = "btSetTextColor0";
            this.btSetTextColor0.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor0.TabIndex = 3;
            this.btSetTextColor0.UseVisualStyleBackColor = false;
            this.btSetTextColor0.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // btSetTextColor3
            // 
            this.btSetTextColor3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor3.BackColor = System.Drawing.Color.Olive;
            this.btSetTextColor3.ForeColor = System.Drawing.Color.Olive;
            this.btSetTextColor3.Location = new System.Drawing.Point(563, 14);
            this.btSetTextColor3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor3.Name = "btSetTextColor3";
            this.btSetTextColor3.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor3.TabIndex = 3;
            this.btSetTextColor3.UseVisualStyleBackColor = false;
            this.btSetTextColor3.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // btSetTextColor4
            // 
            this.btSetTextColor4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSetTextColor4.BackColor = System.Drawing.Color.Green;
            this.btSetTextColor4.ForeColor = System.Drawing.Color.Green;
            this.btSetTextColor4.Location = new System.Drawing.Point(594, 14);
            this.btSetTextColor4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btSetTextColor4.Name = "btSetTextColor4";
            this.btSetTextColor4.Size = new System.Drawing.Size(33, 27);
            this.btSetTextColor4.TabIndex = 3;
            this.btSetTextColor4.UseVisualStyleBackColor = false;
            this.btSetTextColor4.Click += new System.EventHandler(this.btSetTextColor_Click);
            // 
            // baseImageList
            // 
            this.baseImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.baseImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("baseImageList.ImageStream")));
            this.baseImageList.TransparentColor = System.Drawing.Color.White;
            this.baseImageList.Images.SetKeyName(0, "imgPic0.png");
            this.baseImageList.Images.SetKeyName(1, "imgPic1.png");
            this.baseImageList.Images.SetKeyName(2, "imgPic2.png");
            this.baseImageList.Images.SetKeyName(3, "imgPic3.png");
            this.baseImageList.Images.SetKeyName(4, "imgPic4.png");
            this.baseImageList.Images.SetKeyName(5, "imgPic5.png");
            this.baseImageList.Images.SetKeyName(6, "imgPic6.png");
            this.baseImageList.Images.SetKeyName(7, "imgPic7.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(825, 413);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.Text = "SyNotebook";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Move += new System.EventHandler(this.MainForm_Move);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.treeMenu.ResumeLayout(false);
            this.textMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.RichTextBox rtbNoteText;
        private System.Windows.Forms.Button btAddBookmark;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btDeleteBookmark;
        private System.Windows.Forms.Button btSetTextColor0;
        private System.Windows.Forms.Button btSetTextColor3;
		private System.Windows.Forms.Button btSetTextColor4;
        private System.Windows.Forms.Button btSetTextItalic;
		private System.Windows.Forms.Button btSetTextBold;
        private System.Windows.Forms.Button btSetTextColor8;
        private System.Windows.Forms.Button btSetTextColor9;
        private System.Windows.Forms.Button btSetTextColor14;
        private System.Windows.Forms.Button btSetTextFontMonospace;
        private System.Windows.Forms.Button btSetTextFontProportional;
        private System.Windows.Forms.Button btFontSizeDec;
        private System.Windows.Forms.Button btFontSizeInc;
        private System.Windows.Forms.ContextMenuStrip treeMenu;
        private System.Windows.Forms.ToolStripMenuItem miAddItem;
        private System.Windows.Forms.ToolStripMenuItem miDeleteItem;
        private System.Windows.Forms.ToolStripMenuItem miSetPassword;
        private System.Windows.Forms.ToolStripMenuItem miRemovePassword;
        private System.Windows.Forms.Button btSetPassword;
		private System.Windows.Forms.Button btRemovePassword;
		private System.Windows.Forms.Button btChangeParam;
		private System.Windows.Forms.ContextMenuStrip textMenu;
		private System.Windows.Forms.ToolStripMenuItem miCut;
		private System.Windows.Forms.ToolStripMenuItem miCopy;
		private System.Windows.Forms.ToolStripMenuItem miPaste;
		private System.Windows.Forms.ImageList baseImageList;
		private System.Windows.Forms.Button btSetTextUnderline;
		private System.Windows.Forms.Button btUndentDec;
		private System.Windows.Forms.Button btIndentInc;
        private System.Windows.Forms.ToolStripSeparator miSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miLock;
        private System.Windows.Forms.ToolStripSeparator miSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miProperties;
    }
}

