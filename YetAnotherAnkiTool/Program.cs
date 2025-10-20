namespace YetAnotherAnkiTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string mutexName = "YetAnotherAnkiTool_SingleInstance";

            using var mutex = new Mutex(true, mutexName, out bool isNewInstance);
            if (!isNewInstance)
            {
                MessageBox.Show("Error: YetAnotherAnkiTool is already running. Only one instance can be open at a time.", "Already Open", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // To customize application configurationcsuch as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}