using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Compressor
{
    class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (string str in args)
                {
                    FileInfo info = new FileInfo(str);
                    long length = info.Length, read = 0L;
                    using (FileStream stream = File.OpenRead(str))
                    {
                        using (FileStream stream2 = File.Create(Path.GetDirectoryName(str) + @"\" + Path.GetFileNameWithoutExtension(str)))
                        {
                            using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Compress, false))
                            {
                                byte[] buffer = new byte[0x400];
                                int count = stream.Read(buffer, 0, buffer.Length);
                                int num4 = 0;
                                while (count > 0)
                                {
                                    read += count;
                                    if (num4 >= 50)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("{0} out of {1} bytes compressed.", read, length);
                                        num4 = 0;
                                    }
                                    else
                                    {
                                        num4++;
                                    }
                                    stream3.Write(buffer, 0, count);
                                    count = stream.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }
            }
        }


    }
}
