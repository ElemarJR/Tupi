using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Tupi.Indexing;

namespace LoadingBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + @"\t8.shakespeare.txt";
            var text1 = File.ReadAllText(filename);
            var text2 = File.ReadAllText(filename);
            var text3 = File.ReadAllText(filename);

            var beforeStartingGen0 = GC.CollectionCount(0);
            var beforeStartingGen1 = GC.CollectionCount(1);
            var beforeStartingGen2 = GC.CollectionCount(2);

            Console.WriteLine($"Press any key to start indexing.");
            Console.ReadLine();

            var sw = new Stopwatch();
            sw.Start();
            var index = new StringIndexer(new DefaultAnalyzer())
                .CreateIndex(text1, text2, text3);
            sw.Stop();

            Console.WriteLine($"Time in ms: {sw.Elapsed}.");
            Console.WriteLine($"Number of terms: {index.NumberOfTerms}");

            Console.WriteLine($"# Gen 0: {GC.CollectionCount(0) - beforeStartingGen0}");
            Console.WriteLine($"# Gen 1: {GC.CollectionCount(1) - beforeStartingGen1}");
            Console.WriteLine($"# Gen 2: {GC.CollectionCount(2) - beforeStartingGen2}");
        }
    }
}
