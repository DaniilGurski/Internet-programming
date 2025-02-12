using System;


internal class Program
{
    private static void Main(string[] args)
    {
        try {
            for (int i = 0; i < 25; i++) {
                var processInfo = new System.Diagnostics.ProcessStartInfo("https://youtu.be/dQw4w9WgXcQ") { UseShellExecute = true };
                System.Diagnostics.Process.Start(processInfo);
            }
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);

        }
    }
}