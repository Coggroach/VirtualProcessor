using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VProcessor.Hardware;

namespace VProcessor.Software.Assembly
{
    class AssemblyKeywords
    {
        private List<String> keywords;

        public AssemblyKeywords()
        {
            this.keywords = new List<String>();
        }

        public void KeywordRichTextBox(RichTextBox text)
        {
            foreach (var line in text.Lines)
            {
                foreach (var key in this.keywords.Where(key => line.Contains(key)))
                {
                    text.Select(line.IndexOf(key, StringComparison.Ordinal), key.Length);
                    text.SelectionColor = Color.Red;
                }
            }
        }
    }
}
