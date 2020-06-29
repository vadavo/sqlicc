using System;
using System.IO;
using System.Threading.Tasks;

namespace Vadavo.Sqlicc
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 2)
                PrintError(1, "Input file name or output file name are not provided.");

            string inputFileName = args[0];
            string outputFileName = args[1];

            if (!File.Exists(outputFileName))
            {
                try
                {
                    var createdFile = File.Create(outputFileName);
                    createdFile.Dispose();
                    Console.WriteLine($"Created file {outputFileName}");
                }
                catch (PathTooLongException)
                {
                    PrintError(3, $"Cannot create {outputFileName}. Path name is too long.");
                }
                catch (DirectoryNotFoundException)
                {
                    PrintError(4, $"Cannot create {outputFileName}. Directory not found.");
                }
                catch (IOException)
                {
                    PrintError(2, $"Cannot create {outputFileName}. Check permissions or create it manually before executing utility.");
                }
            }

            Console.WriteLine($"Opening file {inputFileName}...");

            using var inputFileStream = new FileStream(inputFileName, FileMode.Open, FileAccess.Read);
            using var outputFileStream = new FileStream(outputFileName, FileMode.Open, FileAccess.Write);

            using var reader = new StreamReader(inputFileStream);
            using var writter = new StreamWriter(outputFileStream);

            string line;
            int count = 1;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.StartsWith("INSERT INTO", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine($"Skipped line {count} beacause contains INSERT INTO statement.");
                    count++;
                    continue;
                }

                await writter.WriteLineAsync(line);
                PrintEmphatized($"Copied line {count}.");
                count++;
            }

            Console.WriteLine("Completed.");
        }

        /// <summary>
        ///     Print error and die.
        /// </summary>
        /// <param name="code">Unique code about the error.</param>
        /// <param name="message">Error message that will be printed.</param>
        static void PrintError(int code, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error {code}: {message}");

            Environment.Exit(code);
        }

        static void PrintEmphatized(string message)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }
    }
}
