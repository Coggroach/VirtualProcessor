using System;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using VProcessor.Common;
using VProcessor.Hardware;
using VProcessor.Hardware.Memory;
using VProcessor.Hardware.Peripherals;
using VProcessor.Software;
using VProcessor.Tools;
using Timer = VProcessor.Hardware.Peripherals.Timer;

namespace VProcessor.Gui
{
    public partial class EditorForm : Form
    {
        private readonly IAssembler _compiler;
        private volatile Machine _machine;
        private readonly UserSettings _settings;
        private readonly VpFile _flashFile;
        private const string RegisterPrefix = "r";
        private readonly Timer _timer;
        private readonly LedBoard _leds;

        public EditorForm()
        {
            _compiler = new Assembler();
            _settings = new UserSettings().Load();
            _flashFile = new VpFile(_settings.FlashFileLocation);
            
            _machine = new Machine(_compiler);
            _timer = new Timer();
            _machine.RegisterPeripheral(_timer);
            _leds = new LedBoard();
            _machine.RegisterPeripheral(_leds);

            SetupThread();
            InitializeComponent();
            Setup();
            Update();
            UpdateEditorBox();
        }

        public void Tick()
        {
            _machine.Tick();
            Update();
        }

        #region Threading

        private volatile bool _isRunning;
        private Thread _runThread;

        public void SetupThread()
        {
            _runThread = new Thread(Run);
            _isRunning = false;
        }

        public void Run(object context)
        {
            _isRunning = true;
            //var timeSpan = new TimeSpan(Settings.ClockTime);
            RunningStatus.BackColor = Color.Green;

            while (!_machine.HasTerminated() && _isRunning)
            {
                _machine.Tick();
                //Thread.Sleep(timeSpan);
            }
            Stop();
        }

        public void Stop()
        {
            _isRunning = false;
            Thread.Sleep(100);
            RunningStatus.BackColor = Color.Red;
            Update();
        }
        #endregion

        #region Setup
        public void Setup()
        {
            SetupUserSettings();
            SetupRegisterFile();
            SetupEditorBoxText();
            SetupToolBar();            
        }

        public void SetupUserSettings()
        {
            tabsToolStripMenuItem.Checked = _settings.IndentMode == 0;
            spacesToolStripMenuItem.Checked = _settings.IndentMode == 1;
            size1toolStripMenuItem.Checked = _settings.IndentSize == 1;
            size2toolStripMenuItem.Checked = _settings.IndentSize == 2;
            size4toolStripMenuItem.Checked = _settings.IndentSize == 4;
        }

        public void SetupRegisterFile()
        {
            var registers = _machine.GetRegisters();
            RegisterFile.ColumnCount = 2;
            RegisterFile.RowCount = _machine.GetRegisters().Length + 3;
            RegisterFile.Font = new Font("Arial", 12);
        }

        private void SetupToolBar()
        {
            //this.ToolFontType.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            ToolFontSize.Items.AddRange(new object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            ToolFontSize.SelectedItem = 14;
        }

        private void SetupEditorBoxText()
        {
            UpdateEditorBox();   
            FlashMemoryBox.Text = ConvertMemoryToString(_machine.GetFlashMemory());
        }

        private string ConvertMemoryToString(MemoryUnit<uint> memory)
        {
            var chunk = memory.GetMemoryChunk();
            var s = "";
            const string template = "00000000";
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
            if(InvokeRequired)
            {
                Invoke(new Action(UpdateHelper));
                return;
            }
            UpdateHelper();
        }

        public void UpdateHelper()
        {
            UpdateRegisterFile();
            UpdateMemoryDisplays();
            UpdateToolBar();                
        }

        private void UpdateEditorBox()
        {
            var parser = _flashFile.GetString().Replace(VpFile.Delimiter, '\n');

            parser = Regex.Replace(parser, @"[ ]{2,}", " ");            
            parser = Regex.Replace(parser, @"[\t]{1,}", " ");            

            var indentMode = _settings.IndentMode == UserSettings.IndentTab ? "\t" : " ";
            for (var i = 0; i < _settings.IndentSize; i++)
                indentMode += indentMode;

            EditorBox.Text = parser.Replace(" ", indentMode);
            HighlightEditorBox();
        }

       
        public void UpdateRegisterFile()
        {
            var registers = _machine.GetRegisters();
            var length = registers.Length;
            for (var i = 0; i < length; i++)
            {
                RegisterFile[0, i].Value = RegisterPrefix + i;
                RegisterFile[1, i].Value = registers[i];
            }
            RegisterFile[0, length].Value = "pc";
            RegisterFile[1, length].Value = (int)_machine.GetProgramCounter();
            RegisterFile[0, length + 1].Value = "car";
            RegisterFile[1, length + 1].Value = _machine.GetControlAddressRegister();
            RegisterFile[0, length + 2].Value = "nzcv";
            RegisterFile[1, length + 2].Value = _machine.GetStatusRegister();
        }

        private void UpdateToolBar()
        {
            EditorBox.SelectAll();
            EditorBox.SelectionFont = new Font("Arial", float.Parse(ToolFontSize.Text));
            EditorBox.DeselectAll();
        }

        private void UpdateMemoryDisplays()
        {
            var pc = _machine.GetProgramCounter();
            var carM = Convert.ToString((long)_machine.GetControlMemory().GetMemory(), 16);

            while (carM.Length < 16)
                carM = "0" + carM;
            for (var i = 1; i < 4; i++)
                carM = carM.Insert(5 * i - 1, " ");

            CurrentCommandTextBox.Text = carM.ToUpper();
            FlashMemoryBox.Text = ConvertMemoryToString(_machine.GetFlashMemory());

            FlashMemoryBox.SelectAll();
            FlashMemoryBox.SelectionFont = new Font("Arial", 12);
            FlashMemoryBox.DeselectAll();

            for (var i = 0; i < FlashMemoryBox.Lines.Length; i++)
            {
                var charIndex = FlashMemoryBox.GetFirstCharIndexFromLine(i);
                var current = FlashMemoryBox.Lines[i];
                FlashMemoryBox.Select(charIndex, current.Length);
                FlashMemoryBox.SelectionBackColor = (i == pc) ? Color.Orange : Color.WhiteSmoke;                
            }
        }
        #endregion Update

        #region Highlight
        private void HighlightDefaultEditorBox()
        {
            EditorBox.SelectAll();
            EditorBox.SelectionColor = Color.Black;
            EditorBox.DeselectAll();
        }

        private void HighlightEditorBox(string mode = "All")
        {
            if (EditorBox.Text.Length <= 0)
                return;

            var save = EditorBox.SelectionStart;
            var charIndex = 0;
            var line = 0;

            if (!_settings.Highlight)
            {
                HighlightDefaultEditorBox();
                return;
            }

            if (mode == "Line")
            {
                line = EditorBox.GetLineFromCharIndex(save);
                charIndex = EditorBox.GetFirstCharIndexFromLine(line);
                var current = EditorBox.Lines[line];
                EditorBox.Select(charIndex, current.Length);
                EditorBox.SelectionColor = Color.Black;
                HighlightEditorBoxLine(line, charIndex);                
            }

            if(mode == "All")
            {
                HighlightDefaultEditorBox();
                for (line = 0; line < EditorBox.Lines.Length; line++)
                {
                    charIndex = EditorBox.GetFirstCharIndexFromLine(line);

                    HighlightEditorBoxLine(line, charIndex);
                }
            }               
            EditorBox.DeselectAll();
            EditorBox.SelectionStart = save;
        }

        private void HighlightEditorBoxLine(int line, int charIndex)
        {
            try
            {
                HighlightOpcodes(charIndex);
                HighlightRegisters(charIndex);
                HighlightNumbers(line, charIndex);
                HighlightKeywords(line, charIndex);
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
            EditorBox.Select(index, 3);

            var selection = EditorBox.SelectedText.Trim().ToUpper();
            if (OpcodeRegistry.Instance.IsValidCode(selection))
                EditorBox.SelectionColor = Color.DarkCyan;
        }

        private void HighlightKeywords(int index, int charIndex)
        {
            var current = EditorBox.Lines[index];
            var entries = ((Assembler)_compiler).KeywordLookup;

            foreach (DictionaryEntry entry in entries)
            {
                if (current.Contains((string)entry.Key))
                {
                    var numIndex = current.IndexOf((string)entry.Key);
                    EditorBox.Select(charIndex + numIndex, current.Length - numIndex);
                    EditorBox.SelectionColor = Color.MediumPurple;
                }
            }
        }

        private void HighlightRegisters(int index)
        {

        }

        private void HighlightNumbers(int index, int charIndex)
        {
            var current = EditorBox.Lines[index];
            var numIndicatorArray = new[] { "#", "=" };

            for (var i = 0; i < numIndicatorArray.Length; i++)
                if (current.Contains(numIndicatorArray[i]))
                {
                    var numIndex = current.IndexOf(numIndicatorArray[i]);
                    EditorBox.Select(charIndex + numIndex, current.Length - numIndex);
                    EditorBox.SelectionColor = Color.PaleVioletRed;
                }
        }
        #endregion

        #region EventHandling
        private void EditorBox_SelectionChanged(object sender, EventArgs e)
        {            
            
        }

        private void EditorBox_TextChanged(object sender, EventArgs e)
        {
            HighlightEditorBox("Line");
        }

        private void tickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tick();
        }

        private void tickx25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 25; i++)
                _machine.Tick(); 
            Update();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.Reset();
            Update();
        } 

        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void UncheckToolStripMenuItems()
        {
            hexadecimalToolStripMenuItem.Checked = false;
            decimalToolStripMenuItem.Checked = false;
            assemblyToolStripMenuItem.Checked = false;
        }

        private ToolStripMenuItem GetToolStripMenuItemMode()
        {
            switch (_flashFile.GetMode())
            {
                case VpFile.Assembly:
                    return assemblyToolStripMenuItem;
                case VpFile.Decimal:
                    return decimalToolStripMenuItem;
                case VpFile.Hexadecimal:
                    return hexadecimalToolStripMenuItem;
            }
            return null;
        }

        private void ModeToolStripMenuItem_Click(int mode)
        {
            _flashFile.SetMode(mode);
            UncheckToolStripMenuItems();
            GetToolStripMenuItemMode().Checked = true;  
        }

        private void hexadecimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModeToolStripMenuItem_Click(VpFile.Hexadecimal);
        }

        private void decimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModeToolStripMenuItem_Click(VpFile.Decimal);
        }

        private void assemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModeToolStripMenuItem_Click(VpFile.Assembly);        
        }

        private void ToolFontSize_Click(object sender, EventArgs e)
        {
            UpdateEditorBox();
        }

        private void TickButton_Click(object sender, EventArgs e)
        {
            var count = 1;
            try { count = int.Parse(TickCounter.Text); }
            catch (Exception ex) { Logger.Instance().Log("TickCounter (TextBox): " + ex); }
            for (int i = 0; i < count; i++)
                _machine.Tick();
            Update();
        }

        private void CommandBox_KeyPress(object sender, EventArgs e)
        {
    
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _settings.Save();
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
                _flashFile.SetString(EditorBox.Text);
                _flashFile.Save(dialog.FileName);
                UncheckToolStripMenuItems();
                GetToolStripMenuItemMode().Checked = true;
            }
        }

             

        private void assembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _flashFile.SetString(EditorBox.Text);
            _flashFile.Save();
            _flashFile.Load();
            _machine.Reset(_compiler.Compile32(_flashFile, VpConsts.FlashMemorySize));
            Update();
            UpdateEditorBox();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "vps files (*.vps)|*.vps|vpo files (*.vpo)|*.vpo|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {                
                _flashFile.Load(dialog.FileName);
                SetupEditorBoxText();
                UncheckToolStripMenuItems();
                GetToolStripMenuItemMode().Checked = true;
            }
        }
                
        private void indentModeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }        

        private void tabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indentModeToolStripMenuItem_Click(UserSettings.IndentTab);
        }

        private void spacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indentModeToolStripMenuItem_Click(UserSettings.IndentSpace);
        }     
  
        private void indentModeToolStripMenuItem_Click(int index)
        {
            tabsToolStripMenuItem.Checked = UserSettings.IndentTab == index;
            spacesToolStripMenuItem.Checked = UserSettings.IndentSpace == index;
            _settings.IndentMode = index;
            UpdateEditorBox();
        }

        private void size1toolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeXtoolStripMenuItem_Click(UserSettings.IndentSize1);
        }

        private void size2toolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeXtoolStripMenuItem_Click(UserSettings.IndentSize2);
        }

        private void size4toolStripMenuItem_Click(object sender, EventArgs e)
        {
            sizeXtoolStripMenuItem_Click(UserSettings.IndentSize4);
        }

        private void sizeXtoolStripMenuItem_Click(int number)
        {
            size1toolStripMenuItem.Checked = UserSettings.IndentSize1 == number;
            size2toolStripMenuItem.Checked = UserSettings.IndentSize2 == number;
            size4toolStripMenuItem.Checked = UserSettings.IndentSize4 == number;
            _settings.IndentSize = number;
            UpdateEditorBox();
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {

        }

        private void highlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            highlightToolStripMenuItem.Checked = !highlightToolStripMenuItem.Checked;
            _settings.Highlight = highlightToolStripMenuItem.Checked;
            HighlightEditorBox("All");
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isRunning)
                Stop();
            else
            {
                ThreadPool.QueueUserWorkItem(Run);                
            }
        }
        #endregion EventHandling
    }
}
