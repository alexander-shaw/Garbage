using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
class Program
{
    static void Main(string[] args)
    {
        using (FileStream file = new FileStream("log.txt", FileMode.Create))
        using (StreamWriter sw = new StreamWriter(file))
        {
            sw.WriteLine("Hello from I disposable ");
        }

        using (FileStream file = new FileStream("log.txt", FileMode.Open))
        using (StreamReader sr = new StreamReader(file))
        {
            Console.WriteLine(sr.ReadToEnd());
        }
        
    }


    class SafeHandle : IDisposable
    {
        private SafeFileHandle handle;

        public SafeHandle(string path)
        {
            handle = CreateFile(path, 0x80000000, 1, IntPtr.Zero, 3, 0, IntPtr.Zero);
        }

        public void Dispose()
        {
            handle?.Dispose();
        }
        [DllImport("kernel32.dll",SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpfilename, uint desiredAccess, uint shareMode, IntPtr securityAttributes, uint creationDisposition, uint flagAttributes, IntPtr TemplateFile);
     
    }

}