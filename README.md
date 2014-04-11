AssemblyCompressor
==================

Use GZip to compress your .NET assemblies for loading with AssemblyResolve.

Instructions:

1. Drag your assembly (typically a .dll file) over Compressor.exe, it will now create a file with the same name of your assembly without the extension in the directory of the original assembly.

2. Open your project's Resources file, and drag the new file (the one without the extension) over the window. It should add the new file to the project's resources.

3. Now you'll want to load your assembly when it's called, so we'll create the following code to your Program.cs file for decompression:

Code:
private static byte[] DecompressAssembly(byte[] assembly)
      {
      Stream stream = new MemoryStream(assembly);
      using (var decompress = new GZipStream(stream, CompressionMode.Decompress, false))
      {
            try
            {
            const int size = 1024;
            byte[] buffer = new byte[size];
            using (MemoryStream memory = new MemoryStream())
            {
            int count = 0;
            do
            {
            count = decompress.Read(buffer, 0, size);
            if (count > 0)
            {
            memory.Write(buffer, 0, count);
            }
            }
            while (count > 0);
            return memory.ToArray();
            }  
            }
            catch (Exception Ex)
            {
            Console.WriteLine(Ex.ToString());
            return null;
            }
      }
      }

4. Now we can add our assembly resolving code. Add the following as the first line under the Main function in Program.cs:

Code:
AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(pHandleAssemblyResolver);

5. Add the following function to Program.cs:

Code:
private static System.Reflection.Assembly pHandleAssemblyResolver(object sender, ResolveEventArgs args)
      {
      }

6. Inside of the pHandleAssemblyResolver function, you will have to add the name of your assembly. For instance, let's say my assembly that I want to load is called "MyAssembly.dll" and the default namespace is called MyAssembly. It will then look like this:

Code:
private static System.Reflection.Assembly pHandleAssemblyResolver(object sender, ResolveEventArgs args)
      {
             switch(true)
             {
            case args.Name.Contains("MyAssembly"):
             return Assembly.Load(DecompressAssembly(Properties.Resources.MyAssembly));
            default:
            //not included in the statement?
            return null;
             }
      }
