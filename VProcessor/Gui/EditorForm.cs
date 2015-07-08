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
        private ICompiler compiler;
        private Processor processor;
        private SFile flashFile;
        private const String RegisterPrefix = "r";

        public EditorForm()
        {
            this.compiler = new Compiler();
            this.flashFile = new SFile(Settings.FlashMemoryLocation);
            
            this.processor = new Processor(
                    this.compiler.Compile64(new SFile(Settings.ControlMemoryLocation), Settings.ControlMemorySize), 
                    this.compiler.Compile32(this.flashFile, Settings.FlashMemorySize));
            
            this.InitializeComponent();
            this.SetupRegisterFile();
            this.SetupEditorBoxText();
            this.UpdateMemoryDisplays();
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
                this.RegisterFile[1, i].Value = registers[i];
            this.RegisterFile[1, length].Value = (Int32)this.processor.GetProgramCounter();
            this.RegisterFile[1, length+1].Value = this.processor.GetControlAddressRegister();
            this.RegisterFile[1, length+2].Value = this.processor.GetNzcv();
            this.UpdateMemoryDisplays();
        }

        public void SetupRegisterFile()
        {
            var registers = this.processor.GetRegisters();
            this.RegisterFile.ColumnCount = 2;
            for (var i = 0; i < registers.Length; i++)
            {
                Object[] row =
                {
                    String.Concat(RegisterPrefix, i), 
                    registers[i]
                };
                this.RegisterFile.Rows.Add(row);
            }

            this.RegisterFile.Rows.Add("pc", this.processor.GetProgramCounter());
            this.RegisterFile.Rows.Add("car", this.processor.GetControlAddressRegister());
            this.RegisterFile.Rows.Add("nzcv", this.processor.GetNzcv());
        }

        private void SetupEditorBoxText()
        {
            this.EditorBox.Text = this.flashFile.GetString();
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
            for(var i = 0; i < this.EditorBox.Lines.Length; i++)
            {
                var charIndex = this.EditorBox.GetFirstCharIndexFromLine(i);
                var current = this.EditorBox.Lines[i];
                this.EditorBox.Select(charIndex, current.Length);
                this.EditorBox.SelectionBackColor = (i != pc) ? this.EditorBox.BackColor : Color.LightYellow;
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
    }
}
