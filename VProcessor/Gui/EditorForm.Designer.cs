namespace VProcessor.Gui
{
    partial class EditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            this.RegisterFile = new System.Windows.Forms.DataGridView();
            this.EditorBox = new System.Windows.Forms.RichTextBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assembleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexadecimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickx25ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indentSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.size1toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.size2toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.size4toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentCommandTextBox = new System.Windows.Forms.TextBox();
            this.FlashMemoryBox = new System.Windows.Forms.RichTextBox();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.TickButton = new System.Windows.Forms.ToolStripButton();
            this.ToolFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.TickCounter = new System.Windows.Forms.ToolStripTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterFile)).BeginInit();
            this.MenuStrip.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // RegisterFile
            // 
            this.RegisterFile.AllowUserToAddRows = false;
            this.RegisterFile.AllowUserToDeleteRows = false;
            this.RegisterFile.AllowUserToResizeColumns = false;
            this.RegisterFile.AllowUserToResizeRows = false;
            this.RegisterFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RegisterFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.RegisterFile.ColumnHeadersVisible = false;
            this.RegisterFile.GridColor = System.Drawing.SystemColors.Control;
            this.RegisterFile.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.RegisterFile.Location = new System.Drawing.Point(12, 55);
            this.RegisterFile.Name = "RegisterFile";
            this.RegisterFile.RowHeadersVisible = false;
            this.RegisterFile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RegisterFile.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.RegisterFile.Size = new System.Drawing.Size(201, 495);
            this.RegisterFile.TabIndex = 0;
            // 
            // EditorBox
            // 
            this.EditorBox.AcceptsTab = true;
            this.EditorBox.Location = new System.Drawing.Point(219, 55);
            this.EditorBox.Name = "EditorBox";
            this.EditorBox.Size = new System.Drawing.Size(420, 495);
            this.EditorBox.TabIndex = 2;
            this.EditorBox.Text = "";
            this.EditorBox.SelectionChanged += new System.EventHandler(this.EditorBox_SelectionChanged);
            this.EditorBox.TextChanged += new System.EventHandler(this.EditorBox_TextChanged);
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(784, 24);
            this.MenuStrip.TabIndex = 3;
            this.MenuStrip.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem1,
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem1
            // 
            this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            this.saveToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
            this.saveToolStripMenuItem1.Text = "Save";
            this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.assembleToolStripMenuItem,
            this.modeToolStripMenuItem,
            this.tickToolStripMenuItem,
            this.tickx25ToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // assembleToolStripMenuItem
            // 
            this.assembleToolStripMenuItem.Name = "assembleToolStripMenuItem";
            this.assembleToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.assembleToolStripMenuItem.Text = "Assemble";
            this.assembleToolStripMenuItem.Click += new System.EventHandler(this.assembleToolStripMenuItem_Click);
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexadecimalToolStripMenuItem,
            this.decimalToolStripMenuItem,
            this.assemblyToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.modeToolStripMenuItem.Text = "Mode";
            this.modeToolStripMenuItem.Click += new System.EventHandler(this.modeToolStripMenuItem_Click);
            // 
            // hexadecimalToolStripMenuItem
            // 
            this.hexadecimalToolStripMenuItem.Checked = true;
            this.hexadecimalToolStripMenuItem.CheckOnClick = true;
            this.hexadecimalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hexadecimalToolStripMenuItem.Name = "hexadecimalToolStripMenuItem";
            this.hexadecimalToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.hexadecimalToolStripMenuItem.Text = "Hexadecimal";
            this.hexadecimalToolStripMenuItem.Click += new System.EventHandler(this.hexadecimalToolStripMenuItem_Click);
            // 
            // decimalToolStripMenuItem
            // 
            this.decimalToolStripMenuItem.CheckOnClick = true;
            this.decimalToolStripMenuItem.Name = "decimalToolStripMenuItem";
            this.decimalToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.decimalToolStripMenuItem.Text = "Decimal";
            this.decimalToolStripMenuItem.Click += new System.EventHandler(this.decimalToolStripMenuItem_Click);
            // 
            // assemblyToolStripMenuItem
            // 
            this.assemblyToolStripMenuItem.CheckOnClick = true;
            this.assemblyToolStripMenuItem.Name = "assemblyToolStripMenuItem";
            this.assemblyToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.assemblyToolStripMenuItem.Text = "Assembly";
            this.assemblyToolStripMenuItem.Click += new System.EventHandler(this.assemblyToolStripMenuItem_Click);
            // 
            // tickToolStripMenuItem
            // 
            this.tickToolStripMenuItem.Name = "tickToolStripMenuItem";
            this.tickToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.tickToolStripMenuItem.Text = "Tick";
            this.tickToolStripMenuItem.Click += new System.EventHandler(this.tickToolStripMenuItem_Click);
            // 
            // tickx25ToolStripMenuItem
            // 
            this.tickx25ToolStripMenuItem.Name = "tickx25ToolStripMenuItem";
            this.tickx25ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.tickx25ToolStripMenuItem.Text = "Tick x25";
            this.tickx25ToolStripMenuItem.Click += new System.EventHandler(this.tickx25ToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indentModeToolStripMenuItem,
            this.indentSizeToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // indentModeToolStripMenuItem
            // 
            this.indentModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.spacesToolStripMenuItem,
            this.tabsToolStripMenuItem});
            this.indentModeToolStripMenuItem.Name = "indentModeToolStripMenuItem";
            this.indentModeToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.indentModeToolStripMenuItem.Text = "Indent Mode";
            // 
            // spacesToolStripMenuItem
            // 
            this.spacesToolStripMenuItem.CheckOnClick = true;
            this.spacesToolStripMenuItem.Name = "spacesToolStripMenuItem";
            this.spacesToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.spacesToolStripMenuItem.Text = "Spaces";
            this.spacesToolStripMenuItem.Click += new System.EventHandler(this.spacesToolStripMenuItem_Click);
            // 
            // tabsToolStripMenuItem
            // 
            this.tabsToolStripMenuItem.Checked = true;
            this.tabsToolStripMenuItem.CheckOnClick = true;
            this.tabsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tabsToolStripMenuItem.Name = "tabsToolStripMenuItem";
            this.tabsToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.tabsToolStripMenuItem.Text = "Tabs";
            this.tabsToolStripMenuItem.Click += new System.EventHandler(this.tabsToolStripMenuItem_Click);
            // 
            // indentSizeToolStripMenuItem
            // 
            this.indentSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.size1toolStripMenuItem,
            this.size2toolStripMenuItem,
            this.size4toolStripMenuItem});
            this.indentSizeToolStripMenuItem.Name = "indentSizeToolStripMenuItem";
            this.indentSizeToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.indentSizeToolStripMenuItem.Text = "Indent Size";
            // 
            // size1toolStripMenuItem
            // 
            this.size1toolStripMenuItem.CheckOnClick = true;
            this.size1toolStripMenuItem.Name = "size1toolStripMenuItem";
            this.size1toolStripMenuItem.Size = new System.Drawing.Size(80, 22);
            this.size1toolStripMenuItem.Text = "1";
            this.size1toolStripMenuItem.Click += new System.EventHandler(this.size1toolStripMenuItem_Click);
            // 
            // size2toolStripMenuItem
            // 
            this.size2toolStripMenuItem.Checked = true;
            this.size2toolStripMenuItem.CheckOnClick = true;
            this.size2toolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.size2toolStripMenuItem.Name = "size2toolStripMenuItem";
            this.size2toolStripMenuItem.Size = new System.Drawing.Size(80, 22);
            this.size2toolStripMenuItem.Text = "2";
            this.size2toolStripMenuItem.Click += new System.EventHandler(this.size2toolStripMenuItem_Click);
            // 
            // size4toolStripMenuItem
            // 
            this.size4toolStripMenuItem.CheckOnClick = true;
            this.size4toolStripMenuItem.Name = "size4toolStripMenuItem";
            this.size4toolStripMenuItem.Size = new System.Drawing.Size(80, 22);
            this.size4toolStripMenuItem.Text = "4";
            this.size4toolStripMenuItem.Click += new System.EventHandler(this.size4toolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // CurrentCommandTextBox
            // 
            this.CurrentCommandTextBox.Location = new System.Drawing.Point(645, 55);
            this.CurrentCommandTextBox.Name = "CurrentCommandTextBox";
            this.CurrentCommandTextBox.ReadOnly = true;
            this.CurrentCommandTextBox.Size = new System.Drawing.Size(126, 20);
            this.CurrentCommandTextBox.TabIndex = 4;
            // 
            // FlashMemoryBox
            // 
            this.FlashMemoryBox.Location = new System.Drawing.Point(645, 81);
            this.FlashMemoryBox.Name = "FlashMemoryBox";
            this.FlashMemoryBox.ReadOnly = true;
            this.FlashMemoryBox.Size = new System.Drawing.Size(127, 469);
            this.FlashMemoryBox.TabIndex = 5;
            this.FlashMemoryBox.Text = "";
            // 
            // ToolBar
            // 
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TickButton,
            this.ToolFontSize,
            this.TickCounter});
            this.ToolBar.Location = new System.Drawing.Point(0, 24);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(784, 25);
            this.ToolBar.TabIndex = 6;
            this.ToolBar.Text = "toolStrip1";
            // 
            // TickButton
            // 
            this.TickButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TickButton.Image = ((System.Drawing.Image)(resources.GetObject("TickButton.Image")));
            this.TickButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TickButton.Name = "TickButton";
            this.TickButton.Size = new System.Drawing.Size(23, 22);
            this.TickButton.Text = "Tick";
            this.TickButton.Click += new System.EventHandler(this.TickButton_Click);
            // 
            // ToolFontSize
            // 
            this.ToolFontSize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ToolFontSize.Name = "ToolFontSize";
            this.ToolFontSize.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ToolFontSize.Size = new System.Drawing.Size(75, 25);
            this.ToolFontSize.Text = "12";
            this.ToolFontSize.Click += new System.EventHandler(this.ToolFontSize_Click);
            // 
            // TickCounter
            // 
            this.TickCounter.Name = "TickCounter";
            this.TickCounter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TickCounter.Size = new System.Drawing.Size(50, 25);
            this.TickCounter.Text = "1";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.ToolBar);
            this.Controls.Add(this.FlashMemoryBox);
            this.Controls.Add(this.CurrentCommandTextBox);
            this.Controls.Add(this.EditorBox);
            this.Controls.Add(this.RegisterFile);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "EditorForm";
            this.Text = "Virtual Processor";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RegisterFile)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView RegisterFile;
        private System.Windows.Forms.RichTextBox EditorBox;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tickToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tickx25ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.TextBox CurrentCommandTextBox;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexadecimalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblyToolStripMenuItem;
        private System.Windows.Forms.RichTextBox FlashMemoryBox;
        private System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripComboBox ToolFontSize;
        private System.Windows.Forms.ToolStripButton TickButton;
        private System.Windows.Forms.ToolStripTextBox TickCounter;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem assembleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indentModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spacesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tabsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indentSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem size1toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem size2toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem size4toolStripMenuItem;
    }
}