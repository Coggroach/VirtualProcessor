using System;
using System.Windows.Forms;
using VProcessor.Gui;
using VProcessor.Tools;

namespace VProcessor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new EditorForm());
            }
            catch(Exception ex)
            {
                Logger.Instance().Log(ex.ToString());
            }            
        }
    }
}
