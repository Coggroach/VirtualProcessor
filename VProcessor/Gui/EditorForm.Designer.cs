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
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tickx25ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CurrentCommandTextBox = new System.Windows.Forms.TextBox();
            this.modeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hexadecimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FlashMemoryBox = new System.Windows.Forms.RichTextBox();
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
            this.EditorBox.Location = new System.Drawing.Point(249, 54);
            this.EditorBox.Name = "EditorBox";
            this.EditorBox.Size = new System.Drawing.Size(444, 470);
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
            this.restartToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tickToolStripMenuItem,
            this.tickx25ToolStripMenuItem,
            this.modeToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // tickToolStripMenuItem
            // 
            this.tickToolStripMenuItem.Name = "tickToolStripMenuItem";
            this.tickToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tickToolStripMenuItem.Text = "Tick";
            this.tickToolStripMenuItem.Click += new System.EventHandler(this.tickToolStripMenuItem_Click);
            // 
            // tickx25ToolStripMenuItem
            // 
            this.tickx25ToolStripMenuItem.Name = "tickx25ToolStripMenuItem";
            this.tickx25ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.tickx25ToolStripMenuItem.Text = "Tick x25";
            this.tickx25ToolStripMenuItem.Click += new System.EventHandler(this.tickx25ToolStripMenuItem_Click);
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
            // CurrentCommandTextBox
            // 
            this.CurrentCommandTextBox.Location = new System.Drawing.Point(250, 28);
            this.CurrentCommandTextBox.Name = "CurrentCommandTextBox";
            this.CurrentCommandTextBox.ReadOnly = true;
            this.CurrentCommandTextBox.Size = new System.Drawing.Size(522, 20);
            this.CurrentCommandTextBox.TabIndex = 4;
            // 
            // modeToolStripMenuItem
            // 
            this.modeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexadecimalToolStripMenuItem,
            this.decimalToolStripMenuItem,
            this.assemblyToolStripMenuItem});
            this.modeToolStripMenuItem.Name = "modeToolStripMenuItem";
            this.modeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.modeToolStripMenuItem.Text = "Mode";
            this.modeToolStripMenuItem.Click += new System.EventHandler(this.modeToolStripMenuItem_Click);
            // 
            // hexadecimalToolStripMenuItem
            // 
            this.hexadecimalToolStripMenuItem.Name = "hexadecimalToolStripMenuItem";
            this.hexadecimalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hexadecimalToolStripMenuItem.Text = "Hexadecimal";
            this.hexadecimalToolStripMenuItem.Click += new System.EventHandler(this.hexadecimalToolStripMenuItem_Click);
            // 
            // decimalToolStripMenuItem
            // 
            this.decimalToolStripMenuItem.Name = "decimalToolStripMenuItem";
            this.decimalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.decimalToolStripMenuItem.Text = "Decimal";
            this.decimalToolStripMenuItem.Click += new System.EventHandler(this.decimalToolStripMenuItem_Click);
            // 
            // assemblyToolStripMenuItem
            // 
            this.assemblyToolStripMenuItem.Name = "assemblyToolStripMenuItem";
            this.assemblyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.assemblyToolStripMenuItem.Text = "Assembly";
            this.assemblyToolStripMenuItem.Click += new System.EventHandler(this.assemblyToolStripMenuItem_Click);
            // 
            // FlashMemoryBox
            // 
            this.FlashMemoryBox.Location = new System.Drawing.Point(699, 55);
            this.FlashMemoryBox.Name = "FlashMemoryBox";
            this.FlashMemoryBox.ReadOnly = true;
            this.FlashMemoryBox.Size = new System.Drawing.Size(73, 469);
            this.FlashMemoryBox.TabIndex = 5;
            this.FlashMemoryBox.Text = "";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.FlashMemoryBox);
            this.Controls.Add(this.CurrentCommandTextBox);
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
        private System.Windows.Forms.ToolStripMenuItem tickx25ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.TextBox CurrentCommandTextBox;
        private System.Windows.Forms.ToolStripMenuItem modeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hexadecimalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblyToolStripMenuItem;
        private System.Windows.Forms.RichTextBox FlashMemoryBox;
    }
}