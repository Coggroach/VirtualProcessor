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

namespace VProcessor.Gui
{
    public partial class EditorForm : Form
    {
        private Processor processor;
        private SFile file;
        private const String RegisterPrefix = "r";

        public EditorForm()
        {
            this.processor = new Processor();
            this.file = new SFile(this.processor.GetUserMemory().GetPath());
            this.InitializeComponent();
            this.SetupRegisterFile();
            this.SetupEditorBoxText();
        }

        private void MenuLabel_Click(Object sender, EventArgs e)
        {

        }

        public void Tick()
        {
            this.processor.Tick();
            this.UpdateRegisterFile();
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
            this.LabelControlCode.Text = ConvertRegisterContent();
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

            this.LabelControlCode.Text = ConvertRegisterContent();
            this.RegisterFile.Rows.Add("pc", this.processor.GetProgramCounter());
            this.RegisterFile.Rows.Add("car", this.processor.GetControlAddressRegister());
            this.RegisterFile.Rows.Add("nzcv", this.processor.GetNzcv());
        }

        private String ConvertRegisterContent()
        {
            var i = (UInt32)this.processor.GetControlMemory().GetMemory() & 0xFFFFF;
            var j = (UInt32)this.processor.GetUserMemory().GetMemory() & 0xFFFF;
            var k = (UInt32)this.processor.GetInstructionRegister() & 0xFFFF;

            String s = Convert.ToString(i, 16);
            String t = Convert.ToString(j, 16);
            String r = Convert.ToString(k, 16);

            while (r.Length < 4)
            {
                r = 0 + r;
            }
            while(t.Length < 4)
            {
                t = 0 + t;
            }
            while(s.Length < 4)
            {
                s = 0 + s;
            }                       

            return "Car: " + s + "\nPc : " + t + "\nIr : " + r;
        }

        private void SetupEditorBoxText()
        {
            this.EditorBox.Text = this.file.GetString();
        }

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
                this.processor.Tick();
            this.UpdateRegisterFile();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.processor.Restart();
            this.UpdateRegisterFile();
        }
    }
}
