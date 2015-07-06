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
            this.RegisterFile = new System.Windows.Forms.DataGridView();
            this.CommandBox = new System.Windows.Forms.TextBox();
            this.EditorBox = new System.Windows.Forms.RichTextBox();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LabelControlCode = new System.Windows.Forms.Label();
            this.tickx25ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.RegisterFile)).BeginInit();
            this.MenuStrip.SuspendLayout();
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
            this.RegisterFile.GridColor = System.Drawing.SystemColors.Control;
            this.RegisterFile.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.RegisterFile.Location = new System.Drawing.Point(12, 27);
            this.RegisterFile.Name = "RegisterFile";
            this.RegisterFile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RegisterFile.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.RegisterFile.Size = new System.Drawing.Size(231, 523);
            this.RegisterFile.TabIndex = 0;
            // 
            // CommandBox
            // 
            this.CommandBox.Location = new System.Drawing.Point(249, 530);
            this.CommandBox.Name = "CommandBox";
            this.CommandBox.Size = new System.Drawing.Size(523, 20);
            this.CommandBox.TabIndex = 1;
            // 
            // EditorBox
            // 
            this.EditorBox.AcceptsTab = true;
            this.EditorBox.Location = new System.Drawing.Point(249, 27);
            this.EditorBox.Name = "EditorBox";
            this.EditorBox.Size = new System.Drawing.Size(523, 458);
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
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tickToolStripMenuItem,
            this.tickx25ToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // tickToolStripMenuItem
            // 
            this.tickToolStripMenuItem.Name = "tickToolStripMenuItem";
            this.tickToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.tickToolStripMenuItem.Text = "Tick";
            this.tickToolStripMenuItem.Click += new System.EventHandler(this.tickToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
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
            // LabelControlCode
            // 
            this.LabelControlCode.AutoSize = true;
            this.LabelControlCode.Location = new System.Drawing.Point(249, 488);
            this.LabelControlCode.Name = "LabelControlCode";
            this.LabelControlCode.Size = new System.Drawing.Size(32, 39);
            this.LabelControlCode.TabIndex = 4;
            this.LabelControlCode.Text = "Code\r\n0000\r\n\r\n";
            // 
            // tickx25ToolStripMenuItem
            // 
            this.tickx25ToolStripMenuItem.Name = "tickx25ToolStripMenuItem";
            this.tickx25ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tickx25ToolStripMenuItem.Text = "Tick x25";
            this.tickx25ToolStripMenuItem.Click += new System.EventHandler(this.tickx25ToolStripMenuItem_Click);
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.LabelControlCode);
            this.Controls.Add(this.EditorBox);
            this.Controls.Add(this.CommandBox);
            this.Controls.Add(this.RegisterFile);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "EditorForm";
            this.Text = "Virtual Processor";
            ((System.ComponentModel.ISupportInitialize)(this.RegisterFile)).EndInit();
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView RegisterFile;
        private System.Windows.Forms.TextBox CommandBox;
        private System.Windows.Forms.RichTextBox EditorBox;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tickToolStripMenuItem;
        private System.Windows.Forms.Label LabelControlCode;
        private System.Windows.Forms.ToolStripMenuItem tickx25ToolStripMenuItem;
    }
}