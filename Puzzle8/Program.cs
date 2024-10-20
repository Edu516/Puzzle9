namespace Puzzle8
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(GlobalExceptionHandler);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        private static void GlobalExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Ocorreu um erro inesperado: " + e.Exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}