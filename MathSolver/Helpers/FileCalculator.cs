using System.Text;
using MathSolver.Models;

namespace MathSolver.Helpers
{
    public static class FileCalculator
    {
        public string ReadAndCalculate(string file)
        {
            if (File.Exists(file))
            {
                StringBuilder sb = new StringBuilder();
                File.Open(file, FileMode.Open);
                StreamReader sr = new StreamReader(file);
                string? line = sr.ReadLine();

                while (line != null)
                {
                    ParsedLineResponse response = new ParsedLineResponse();
                    response.equation = line;

                    response.ParseAndMath();
                    sb.AppendLine(response.Message);
                }
            }
        }
    }
}
