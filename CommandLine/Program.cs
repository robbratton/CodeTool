using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommandLine
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                var files = GetFiles(arg);
                foreach (var file in files)
                {
                    Console.WriteLine("File: " + file);
                    DumpClassMethods(file);
                    Console.WriteLine("");
                }
            }
        }

        private static IEnumerable<string> GetFiles(string input)
        {
            var output = new List<string>();

            if (input.Contains("?") || input.Contains("*"))
            {
                var directory = Path.GetDirectoryName(input);
                var pattern = Path.GetFileName(input);
                output.AddRange(Directory.GetFiles(directory, pattern));
            }
            else
            {
                output.Add(input);
            }

            return output;
        }
        private static void DumpClassMethods(string path)
        {
            var wrapper = new SyntaxWrapper(path);

            var classes = wrapper.GetClassesFromTree();
            foreach (var @class in classes)
            {
                Console.WriteLine("  Class:  " + @class.Identifier);
                if (@class.Modifiers.Any())
                {
                    Console.WriteLine("    Modifiers: " + string.Join(", ", @class.Modifiers.Select(x => x.Text)));
                }
                if (@class.BaseList != null)
                {  
                    Console.WriteLine("    Bases: " + string.Join(", ",
                                        @class.BaseList.Types));
                }

                var methods = wrapper.GetMethodsFromClass(@class);
                foreach (var method in methods)
                {
                    Console.WriteLine("    Method: " + method.Identifier + " " + string.Join(", ", method.Modifiers.Select(x => x.Text)));
                }
            }
        }
    }
}

