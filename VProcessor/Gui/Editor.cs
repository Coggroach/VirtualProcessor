using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VProcessor.Hardware;

namespace VProcessor.Gui
{
    public class Editor : Form
    {
        private DataGridView RegisterBox;
        private RichTextBox EditorBox;
        private TextBox CommandBox;
        private const String RegisterPrefix = "r";
        private Processor processor;

        public Editor()
        {
            this.processor = new Processor();
            this.Initialize();
        }

        private void Initialize()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(800, 600);
            this.Name = "Editor";
            this.ResumeLayout(false);
            this.Padding = this.DefaultMargin;

            this.RegisterBox = new DataGridView
            {
                Name = "RegisterGroupBox",
                Size = new Size(Scale(this.ClientSize.Width, 0.3), Scale(this.ClientSize.Height, 1)),
                Text = "Registers",
                Location = new Point(this.Padding.Size.Width, this.Padding.Size.Height),
                Padding = Padding.Add(this.Padding, this.Padding),
                ColumnCount = 2
            };
            this.Controls.Add(this.RegisterBox);
            var registers = this.processor.GetRegisters();
            for (var i = 0; i < registers.Length; i++)
            {
                Object[] row =
                {
                    String.Concat(RegisterPrefix, i), 
                    registers[i]
                };
                this.RegisterBox.Rows.Add(row);
            }

            this.RegisterBox.Rows.Add("pc", this.processor.GetProgramCounter());
            this.RegisterBox.Rows.Add("nzcv", this.processor.GetNzcv());
        }

        public void UpdateRegisters()
        {
            for(var i = 0; i < this.processor.GetRegisters().Length; i++)
                this.RegisterBox.UpdateCellValue(1, i);
            this.RegisterBox.UpdateCellValue(1, (Int32) this.processor.GetProgramCounter());
            this.RegisterBox.UpdateCellValue(1, this.processor.GetNzcv());
        }

        private static Int32 Scale(Int32 orginal, Double percentage)
        {
            return (Int32)(orginal * percentage);
        }
    }
}
