using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathSolver.Models;

namespace MathSolverConsole
{
    public class EquationFileParser
    {
        public List<ParsedLineResponse> ParsedLineResponses { get; set; } = [];

        public void ReadFile(string file)
        {
            Console.WriteLine(file);
            if (!File.Exists(file))
                throw new Exception("File does not exist");
            List<string> equationList = [];
            using (StreamReader sr = new StreamReader(file))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    ParsedLineResponse response = new ParsedLineResponse();
                    response.Equation = line;
                    ParsedLineResponses.Add(response);
                    line = sr.ReadLine();
                }
            }
            return;
        }

        public void WriteResponseToFile(string file)
        {
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                for (int i = 0; i < ParsedLineResponses.Count; i++)
                {
                    outputFile.WriteLine(ParsedLineResponses[i].Message);
                }
            }
        }
    }
}
