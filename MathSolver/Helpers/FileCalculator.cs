using System.Text;
using MathSolver.Models;
using Microsoft.AspNetCore.Identity;

namespace MathSolver.Helpers
{
    public static class FileCalculator
    {
        public static string ReadAndCalculate(string file)
        {
            Console.WriteLine(file);
            if (!File.Exists(file))
                throw new Exception("File does not exist");
            StringBuilder sb = new StringBuilder();
            ParsedLineResponse mathResponse = new ParsedLineResponse();
            mathResponse.Messages = [];
            using (StreamReader sr = new StreamReader(file))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    mathResponse.equation = line;

                    int result = mathResponse.ParseAndMath(line);
                    mathResponse.Messages.Add($"{mathResponse.equation} = {result}");
                    line = sr.ReadLine();
                }
            }
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                for (int i = 0; i < mathResponse.Messages.Count; i++)
                {
                    outputFile.WriteLine(mathResponse.Messages[i]);
                }
            }
            return mathResponse.Messages[0];
        }
    }
}
