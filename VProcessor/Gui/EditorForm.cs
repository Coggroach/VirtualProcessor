using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VProcessor.Hardware;
using VProcessor.Software.Assembly;
using VProcessor.Tools;

namespace VProcessor.Gui
{
    public partial class EditorForm : Form
    {
        private IAssembler compiler;
        private Processor processor;
        private SFile flashFile;
        private const String RegisterPrefix = "r";

        public EditorForm()
        {
            this.compiler = new Assembler();
            this.flashFile = new SFile(Settings.FlashMemoryLocation);
            
            this.processor = new Processor(
                    this.compiler.Compile64(new SFile(Settings.ControlMemoryLocation), Settings.ControlMemorySize), 
                    this.compiler.Compile32(this.flashFile, Settings.FlashMemorySize));
            
            this.InitializeComponent();
            this.SetupRegisterFile();
            this.SetupEditorBoxText();
            this.SetupToolBar();
            this.Update();
        }

        public void Tick()
        {
            this.processor.Tick();
            this.Update();
        }

        public void UpdateRegisterFile()
        {
            var registers = this.processor.GetRegisters();
            var length = registers.Length;
            for (var i = 0; i < length; i++)
            {
                this.RegisterFile[0, i].Value = RegisterPrefix + i;
                this.RegisterFile[1, i].Value = registers[i];
            }
            this.RegisterFile[0, length].Value = "pc";
            this.RegisterFile[1, length].Value = (Int32)this.processor.GetProgramCounter();
            this.RegisterFile[0, length+1].Value = "car";
            this.RegisterFile[1, length+1].Value = this.processor.GetControlAddressRegister();
            this.RegisterFile[0, length+2].Value = "nzcv";
            this.RegisterFile[1, length+2].Value = this.processor.GetStatusRegister();           
        }

        public void SetupRegisterFile()
        {
            var registers = this.processor.GetRegisters();
            this.RegisterFile.ColumnCount = 2;
            this.RegisterFile.RowCount = this.processor.GetRegisters().Length + 3;
        }

        private void SetupToolBar()
        {
            //this.ToolFontType.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.ToolFontSize.Items.AddRange(new Object[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
            this.ToolFontSize.SelectedItem = 12;
        }

        private void UpdateToolBar()
        {
            this.EditorBox.SelectAll();
            this.EditorBox.SelectionFont = new Font("Arial", Single.Parse(this.ToolFontSize.Text)); 
            this.EditorBox.DeselectAll();
        }

        private void SetupEditorBoxText()
        {
            this.EditorBox.Text = this.flashFile.GetString().Replace(SFile.Delimiter, '\n');
            this.FlashMemoryBox.Text = ConvertMemoryToString(this.processor.GetUserMemory());
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

        private void EditorBox_SelectionChanged(Object sender, EventArgs e)
        {            
      
        }

        private void UpdateMemoryDisplays()
        {
            var pc = this.processor.GetProgramCounter();
            var carM = Convert.ToString((Int64) this.processor.GetControlMemory().GetMemory(), 16);

            while (carM.Length < 16)
                carM = "0" + carM;
            for (var i = 1; i < 4; i++ )
                carM = carM.Insert(5*i - 1, " ");

            this.CurrentCommandTextBox.Text = carM.ToUpper();
            this.FlashMemoryBox.Text = ConvertMemoryToString(this.processor.GetUserMemory());
            var colour = this.EditorBox.BackColor;
            for(var i = 0; i < this.EditorBox.Lines.Length; i++)
            {
                var charIndex = this.FlashMemoryBox.GetFirstCharIndexFromLine(i);
                var current = this.FlashMemoryBox.Lines[i];
                this.FlashMemoryBox.Select(charIndex, current.Length);
                this.FlashMemoryBox.SelectionBackColor = (i == pc) ? Color.Orange : colour;
            }            
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
                this.processor.Tick(); 
            this.Update();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.processor.Reset();
            this.Update();
        }

        public new void Update()
        {
            this.UpdateRegisterFile();
            this.UpdateMemoryDisplays();
            this.UpdateToolBar();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flashFile.SetString(this.EditorBox.Text);
            this.flashFile.Save();
            this.flashFile.Load();
            this.processor.Reset(this.compiler.Compile32(this.flashFile, Settings.FlashMemorySize));
            this.Update();
        }

        private void modeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void hexadecimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flashFile.SetMode(SFile.Hexadecimal);
        }

        private void decimalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flashFile.SetMode(SFile.Decimal);
        }

        private void assemblyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.flashFile.SetMode(SFile.Assembly);
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
                this.processor.Tick();
            this.Update();
        }

        private void CommandBox_KeyPress(object sender, EventArgs e)
        {
            var k = (KeyEventArgs)e;
            if(k.KeyCode == Keys.Enter)
            {
                UInt32 i = 0;
                try
                {
                    i = UInt32.Parse(this.CommandBox.Text);
                }
                catch (Exception ex) { Logger.Instance().Log("CommandBox (TextBox): " + ex.ToString()); }
                
                if(this.processor.HasTerminated())
                {
                    this.processor.SetInstructionRegister(i);
                    this.processor.Tick();
                }
            }         
        }
    }
}
