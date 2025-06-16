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
        public List<EquationSolverInfo> EquationInfoList { get; set; } = [];

        public void ReadFile(string file)
        {
            Console.WriteLine(file);
            if (!File.Exists(file))
                throw new Exception("File does not exist");

            using StreamReader sr = new(file);
            string? line = sr.ReadLine();

            while (line != null)
            {
                EquationSolverInfo response = new()
                {
                    Equation = line
                };
                EquationInfoList.Add(response);
                line = sr.ReadLine();
            }
            return;
        }

        public void WriteResponseToFile(string file)
        {
            using StreamWriter outputFile = new(file);
            for (int i = 0; i < EquationInfoList
                .Count; i++)
            {
                outputFile.WriteLine(EquationInfoList
                    [i].Message);
            }
        }
    }
}
