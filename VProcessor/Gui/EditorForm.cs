using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;
using VProcessor.Tools;
using VProcessor.Common;
using VProcessor.Hardware.Memory;

namespace VProcessor.Gui
{
    public partial class EditorForm : Form
    {
        private IAssembler compiler;
        private Machine machine;
        private UserSettings settings;
        private VPFile flashFile;
        private const String RegisterPrefix = "r";

        public EditorForm()
        {
            this.compiler = new Assembler();
            this.settings = new UserSettings().Load();
            this.flashFile = new VPFile(this.settings.FlashFileLocation);

            this.machine = new Machine(this.compiler);
            
            this.InitializeComponent();
            this.Setup();
            this.Update();
        }

        public void Tick()
        {
            this.machine.Tick();
            this.Update();
        }

        #region Setup
        public void Setup()
        {
            this.SetupUserSettings();
            this.SetupRegisterFile();
            this.SetupEditorBoxText();
            this.SetupToolBar();         
        }

        public void SetupUserSettings()
        {
            this.tabsToolStripMenuItem.Checked = this.settings.IndentMode == 0;
            this.spacesToolStripMenuItem.Checked = this.settings.IndentMode == 1;
            this.size1toolStripMenuItem.Checked = this.settings.IndentSize == 1;
            this.size2toolStripMenuItem.Checked = this.settings.IndentSize == 2;
            this.size4toolStripMenuItem.Checked = this.settings.IndentSize == 4;
        }

        public void SetupRegisterFile()
        {
            var registers = this.machine.GetRegisters();
            this.RegisterFile.ColumnCount = 2;
            this.RegisterFile.RowCount = this.machine.GetRegisters().Length + 3;
        }

        private void SetupToolBar()
        {
            //this.ToolFontType.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.ToolFontSize.Items.AddRange(new Object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            this.ToolFontSize.SelectedItem = 12;
        }

        private void SetupEditorBoxText()
        {
            this.UpdateEditorBox();   
            this.FlashMemoryBox.Text = ConvertMemoryToString(this.machine.GetFlashMemory());
        }

        private String ConvertMemoryToString(MemoryUnit<UInt32> memory)
        {
            var chunk = memory.GetMemoryChunk();
            var s = "";
            const String template = "00000000";
            for(var i = 0; i < chunk.GetLength(); i++)
            {
                var convert = Convert.ToString(chunk.GetMemory(i), 16).ToUpper();
                s += template.Substring(0, template.Length - convert.Length) + convert +"\n";
            }
            return s;
        }
        #endregion Setup

        #region Update
        public new void Update()
        {
            this.UpdateRegisterFile();
            this.UpdateMemoryDisplays();
            this.UpdateToolBar();
            this.UpdateEditorBox();
        }

        private void UpdateEditorBox()
        {
            var parser = this.flashFile.GetString().Replace(VPFile.Delimiter, '\n');

            parser = Regex.Replace(parser, @"[ ]{2,}", " ");            
            parser = Regex.Replace(parser, @"[\t]{1,}", " ");            

            var indentMode = this.settings.IndentMode == UserSettings.IndentTab ? "\t" : " ";
            for (var i = 0; i < this.settings.IndentSize; i++)
                indentMode += indentMode;

            this.EditorBox.Text = parser.Replace(" ", indentMode);
        }

        public void UpdateRegisterFile()
        {
            var registers = this.machine.GetRegisters();
            var length = registers.Length;
            for (var i = 0; i < length; i++)
            {
                this.RegisterFile[0, i].Value = RegisterPrefix + i;
                this.RegisterFile[1, i].Value = registers[i];
            }
            this.RegisterFile[0, length].Value = "pc";
            this.RegisterFile[1, length].Value = (Int32)this.machine.GetProgramCounter();
            this.RegisterFile[0, length + 1].Value = "car";
            this.RegisterFile[1, length + 1].Value = this.machine.GetControlAddressRegister();
            this.RegisterFile[0, length + 2].Value = "nzcv";
            this.RegisterFile[1, length + 2].Value = this.machine.GetStatusRegister();
        }

        private void UpdateToolBar()
        {
            this.EditorBox.SelectAll();
            this.EditorBox.SelectionFont = new Font("Arial", Single.Parse(this.ToolFontSize.Text));
            this.EditorBox.DeselectAll();
        }

        private void UpdateMemoryDisplays()
        {
            var pc = this.machine.GetProgramCounter();
            var carM = Convert.ToString((Int64)this.machine.GetControlMemory().GetMemory(), 16);

            while (carM.Length < 16)
                carM = "0" + carM;
            for (var i = 1; i < 4; i++)
                carM = carM.Insert(5 * i - 1, " ");

            this.CurrentCommandTextBox.Text = carM.ToUpper();
            this.FlashMemoryBox.Text = ConvertMemoryToString(this.machine.GetFlashMemory());
            var colour = this.EditorBox.BackColor;
            for (var i = 0; i < this.EditorBox.Lines.Length; i++)
            {
                var charIndex = this.FlashMemoryBox.GetFirstCharIndexFromLine(i);
                var current = this.FlashMemoryBox.Lines[i];
                this.FlashMemoryBox.Select(charIndex, current.Length);
                this.FlashMemoryBox.SelectionBackColor = (i == pc) ? Color.Orange : colour;
            }
        }
        #endregion Update

        #region EventHandling
        private void EditorBox_SelectionChanged(Object sender, EventArgs e)
        {            
      
        }

        private void EditorBox_TextChanged(Object sender, EventArgs e)
        {

        }

        private void tickToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            this.Tick();
        }

        private void tickx25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
                this.machine.Tick(); 
            this.Update();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.machine.Reset();
            this.Update();
        } 

        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void UncheckToolStripMenuItems()
        {
            this.hexadecimalToolStripMenuItem.Checked = false;
            this.decimalToolStripMenuItem.Checked = false;
            this.assemblyToolStripMenuItem.Checked = false;
        }

        private ToolStripMenuItem GetToolStripMenuItemMode()
        {
            switch (this.flashFile.GetMode())
            {
                case VPFile.Assembly:
                    return this.assemblyToolStripMenuItem;
                case VPFile.Decimal:
                    return this.decimalToolStripMenuItem;
                case VPFile.Hexadecimal:
                    return this.hexadecimalToolStripMenuItem;
            }
            return null;
        }

        private void ModeToolStripMenuItem_Click(Int32 mode)
        {
            this.flashFile.SetMode(mode);
            this.UncheckToolStripMenuItems();
            this.GetToolStripMenuItemMode().Checked = true;  
        }

        private void hexadecimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ModeToolStripMenuItem_Click(VPFile.Hexadecimal);
        }

        private void decimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ModeToolStripMenuItem_Click(VPFile.Decimal);
        }

        private void assemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ModeToolStripMenuItem_Click(VPFile.Assembly);        
        }

        private void ToolFontSize_Click(object sender, EventArgs e)
        {
            this.Update();
        }

        private void TickButton_Click(object sender, EventArgs e)
        {
            var count = 1;
            try { count = Int32.Parse(this.TickCounter.Text); }
            catch (Exception ex) { Logger.Instance().Log("TickCounter (TextBox): " + ex.ToString()); }
            for (int i = 0; i < count; i++)
                this.machine.Tick();
            this.Update();
        }

        private void CommandBox_KeyPress(object sender, EventArgs e)
        {
    
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.settings.Save();
            Application.Exit();
        }


        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            
            dialog.Filter = "vps files (*.vps)|*.vps|vpo files (*.vpo)|*.vpo|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.flashFile.SetString(this.EditorBox.Text);
                this.flashFile.Save(dialog.FileName);
                this.UncheckToolStripMenuItems();
                this.GetToolStripMenuItemMode().Checked = true;
            }
        }

             

        private void assembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flashFile.SetString(this.EditorBox.Text);
            this.flashFile.Save();
            this.flashFile.Load();
            this.machine.Reset(this.compiler.Compile32(this.flashFile, Settings.FlashMemorySize));
            this.Update();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "vps files (*.vps)|*.vps|vpo files (*.vpo)|*.vpo|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {                
                this.flashFile.Load(dialog.FileName);
                this.SetupEditorBoxText();
                this.UncheckToolStripMenuItems();
                this.GetToolStripMenuItemMode().Checked = true;
            }
        }
                
        private void indentModeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }        

        private void tabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.indentModeToolStripMenuItem_Click(UserSettings.IndentTab);
        }

        private void spacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.indentModeToolStripMenuItem_Click(UserSettings.IndentSpace);
        }     
  
        private void indentModeToolStripMenuItem_Click(Int32 index)
        {
            this.tabsToolStripMenuItem.Checked = UserSettings.IndentTab == index;
            this.spacesToolStripMenuItem.Checked = UserSettings.IndentSpace == index;
            this.settings.IndentMode = index;
            this.Update();
        }

        private void size1toolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sizeXtoolStripMenuItem_Click(UserSettings.IndentSize1);
        }

        private void size2toolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sizeXtoolStripMenuItem_Click(UserSettings.IndentSize2);
        }

        private void size4toolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.sizeXtoolStripMenuItem_Click(UserSettings.IndentSize4);
        }

        private void sizeXtoolStripMenuItem_Click(Int32 number)
        {
            this.size1toolStripMenuItem.Checked = UserSettings.IndentSize1 == number;
            this.size2toolStripMenuItem.Checked = UserSettings.IndentSize2 == number;
            this.size4toolStripMenuItem.Checked = UserSettings.IndentSize4 == number;
            this.settings.IndentSize = number;
            this.Update();
        }


        #endregion EventHandling
    }
}
