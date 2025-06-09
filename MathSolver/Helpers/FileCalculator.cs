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
            using (StreamReader sr = new StreamReader(file))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    mathResponse.equation = line;

                    int result = mathResponse.ParseAndMath(line);
                    mathResponse.Message = $"{mathResponse.equation} = {result}";
                    sb.AppendLine(mathResponse.Message);
                    line = sr.ReadLine();
                }
                var test = sb.ToString();
            }
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.WriteLine(mathResponse.Message);
            }
            return mathResponse.Message;
        }
    }
}
