using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;
using VProcessor.Tools;
using VProcessor.Common;
using VProcessor.Hardware.Memory;
using System.Collections;
using System.Threading;

namespace VProcessor.Gui
{
    public partial class EditorForm : Form
    {
        private IAssembler compiler;
        private volatile Machine machine;
        private UserSettings settings;
        private VPFile flashFile;
        private const String RegisterPrefix = "r";
        private VProcessor.Hardware.Peripherals.Timer timer;

        public EditorForm()
        {
            this.compiler = new Assembler();
            this.settings = new UserSettings().Load();
            this.flashFile = new VPFile(this.settings.FlashFileLocation);
            
            this.machine = new Machine(this.compiler);
            this.timer = new Hardware.Peripherals.Timer();
            this.machine.RegisterPeripheral(this.timer);
            this.SetupThread();
            this.InitializeComponent();
            this.Setup();
            this.Update();
            this.UpdateEditorBox();
        }

        public void Tick()
        {
            this.machine.Tick();
            this.Update();
        }

        #region Threading

        private volatile Boolean IsRunning;
        private Thread RunThread;

        public void SetupThread()
        {
            this.RunThread = new Thread(this.Run);
            this.IsRunning = false;
        }

        public void Run(Object context)
        {
            this.IsRunning = true;
            //var timeSpan = new TimeSpan(Settings.ClockTime);
            this.RunningStatus.BackColor = Color.Green;

            while (!this.machine.HasTerminated() && this.IsRunning)
            {
                this.machine.Tick();
                //Thread.Sleep(timeSpan);
            }
            this.Stop();
        }

        public void Stop()
        {
            this.IsRunning = false;
            Thread.Sleep(100);
            this.RunningStatus.BackColor = Color.Red;
            this.Update();
        }
        #endregion

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
            this.RegisterFile.Font = new Font("Arial", 12);
        }

        private void SetupToolBar()
        {
            //this.ToolFontType.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.ToolFontSize.Items.AddRange(new Object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            this.ToolFontSize.SelectedItem = 14;
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
            for(var i = 0; i < chunk.Length; i++)
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
            if(this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateHelper));
                return;
            }
            this.UpdateHelper();
        }

        public void UpdateHelper()
        {
            this.UpdateRegisterFile();
            this.UpdateMemoryDisplays();
            this.UpdateToolBar();                
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
            this.HighlightEditorBox();
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

            this.FlashMemoryBox.SelectAll();
            this.FlashMemoryBox.SelectionFont = new Font("Arial", 12);
            this.FlashMemoryBox.DeselectAll();

            for (var i = 0; i < this.FlashMemoryBox.Lines.Length; i++)
            {
                var charIndex = this.FlashMemoryBox.GetFirstCharIndexFromLine(i);
                var current = this.FlashMemoryBox.Lines[i];
                this.FlashMemoryBox.Select(charIndex, current.Length);
                this.FlashMemoryBox.SelectionBackColor = (i == pc) ? Color.Orange : Color.WhiteSmoke;                
            }
        }
        #endregion Update

        #region Highlight
        private void HighlightDefaultEditorBox()
        {
            this.EditorBox.SelectAll();
            this.EditorBox.SelectionColor = Color.Black;
            this.EditorBox.DeselectAll();
        }

        private void HighlightEditorBox(String mode = "All")
        {
            if (this.EditorBox.Text.Length <= 0)
                return;

            var save = this.EditorBox.SelectionStart;
            var charIndex = 0;
            var line = 0;

            if (!this.settings.Highlight)
            {
                this.HighlightDefaultEditorBox();
                return;
            }

            if (mode == "Line")
            {
                line = this.EditorBox.GetLineFromCharIndex(save);
                charIndex = this.EditorBox.GetFirstCharIndexFromLine(line);
                var current = this.EditorBox.Lines[line];
                this.EditorBox.Select(charIndex, current.Length);
                this.EditorBox.SelectionColor = Color.Black;
                this.HighlightEditorBoxLine(line, charIndex);                
            }

            if(mode == "All")
            {
                this.HighlightDefaultEditorBox();
                for (line = 0; line < this.EditorBox.Lines.Length; line++)
                {
                    charIndex = this.EditorBox.GetFirstCharIndexFromLine(line);

                    this.HighlightEditorBoxLine(line, charIndex);
                }
            }               
            this.EditorBox.DeselectAll();
            this.EditorBox.SelectionStart = save;
        }

        private void HighlightEditorBoxLine(Int32 line, Int32 charIndex)
        {
            try
            {
                this.HighlightOpcodes(charIndex);
                this.HighlightRegisters(charIndex);
                this.HighlightNumbers(line, charIndex);
                this.HighlightKeywords(line, charIndex);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentOutOfRangeException)
                    return;

                if (!(ex is IndexOutOfRangeException))
                    throw;
            }
        }

        private void HighlightOpcodes(int index)
        {
            this.EditorBox.Select(index, 3);

            var selection = this.EditorBox.SelectedText.Trim().ToUpper();
            if (OpcodeRegistry.Instance.IsValidCode(selection))
                this.EditorBox.SelectionColor = Color.DarkCyan;
        }

        private void HighlightKeywords(int index, int charIndex)
        {
            var current = this.EditorBox.Lines[index];
            var entries = ((Assembler)this.compiler).KeywordLookup;

            foreach (DictionaryEntry entry in entries)
            {
                if (current.Contains((String)entry.Key))
                {
                    var numIndex = current.IndexOf((String)entry.Key);
                    this.EditorBox.Select(charIndex + numIndex, current.Length - numIndex);
                    this.EditorBox.SelectionColor = Color.MediumPurple;
                }
            }
        }

        private void HighlightRegisters(int index)
        {

        }

        private void HighlightNumbers(int index, int charIndex)
        {
            var current = this.EditorBox.Lines[index];
            var numIndicatorArray = new String[] { "#", "=" };

            for (var i = 0; i < numIndicatorArray.Length; i++)
                if (current.Contains(numIndicatorArray[i]))
                {
                    var numIndex = current.IndexOf(numIndicatorArray[i]);
                    this.EditorBox.Select(charIndex + numIndex, current.Length - numIndex);
                    this.EditorBox.SelectionColor = Color.PaleVioletRed;
                }
        }
        #endregion

        #region EventHandling
        private void EditorBox_SelectionChanged(Object sender, EventArgs e)
        {            
            
        }

        private void EditorBox_TextChanged(Object sender, EventArgs e)
        {
            this.HighlightEditorBox("Line");
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
            this.UpdateEditorBox();
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
            this.machine.Reset(this.compiler.Compile32(this.flashFile, VPConsts.FlashMemorySize));
            this.Update();
            this.UpdateEditorBox();
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
            this.UpdateEditorBox();
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
            this.UpdateEditorBox();
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {

        }

        private void highlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.highlightToolStripMenuItem.Checked = !this.highlightToolStripMenuItem.Checked;
            this.settings.Highlight = this.highlightToolStripMenuItem.Checked;
            this.HighlightEditorBox("All");
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.IsRunning)
                this.Stop();
            else
            {
                ThreadPool.QueueUserWorkItem(this.Run);                
            }
        }
        #endregion EventHandling
    }
}
