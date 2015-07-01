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
        private AssemblyFile file;
        private AssemblyKeywords keywords;
        private const String RegisterPrefix = "r";

        public EditorForm()
        {
            this.processor = new Processor();
            this.file = new AssemblyFile("Software\\Assembly.txt");
            this.keywords = new AssemblyKeywords();
            this.InitializeComponent();
            this.SetupRegisterFile();
            this.SetupEditorBoxText();
        }

        private void MenuLabel_Click(Object sender, EventArgs e)
        {

        }


        public void UpdateRegisterFile()
        {
            for (var i = 0; i < this.processor.GetRegisters().Length; i++)
                this.RegisterFile.UpdateCellValue(1, i);
            this.RegisterFile.UpdateCellValue(1, (Int32)this.processor.GetProgramCounter());
            this.RegisterFile.UpdateCellValue(1, this.processor.GetNzcv());
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
            this.RegisterFile.Rows.Add("nzcv", this.processor.GetNzcv());
        }

        private void SetupEditorBoxText()
        {
            this.EditorBox.Text = this.file.GetString();
        }

        private void EditorBox_SelectionChanged(Object sender, EventArgs e)
        {
            this.keywords.KeywordRichTextBox(this.EditorBox);
        }

        private void EditorBox_TextChanged(Object sender, EventArgs e)
        {

        }
    }
}
