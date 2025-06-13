using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

namespace MathSolver.Models
{
    public class ParsedLineResponse
    {
        public string Equation {  get; set; }
        
        public bool IsValid { get; set; }

        public List<string> Messages { get; set; }

        public int Result { get; set; }

        //at this point the equation is stripped of all whitespace
        public bool ValidParen(string line)
        {
            int parenCounter = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    parenCounter++;
                } else if (line[i] == ')')
                {
                    parenCounter -= 1;
                }
                if (parenCounter < 0)
                {
                    return false;
                }                    
            }
            if (parenCounter == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ParseAndMath(string line)
        {
            //remove the spaces
            string noSpaceLine = Regex.Replace(line, @"\s+", "");
            //make sure every character is a number or operator
            //needs to be numberoperatornumber
            if (ValidParen(noSpaceLine))
            {
                string checkWoParen = noSpaceLine.Replace("(", "").Replace(")", "");
                string pattern = @"^((\d+[+\-*/])+\d+)+$";
                if (Regex.Match(checkWoParen, pattern).Success)
                {
                    return SolveEquationRecursively(noSpaceLine).ToString();
                }
            }
            return "Not a valid equation";
        }

        public int FindCloseParen(string equation)
        {
            int parenCounter = 0;
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '(') { parenCounter++; }
                else if (equation[i] == ')')
                {
                    parenCounter--;
                    if (parenCounter == 0) { return i; }
                }
            }
            throw new Exception("Missing Closing Paren");
        }

        //This assumes the string passed in has no spaces, and has valid equation syntax
        public string SolveIfInParen(string equation)
        {
            if (equation[0] == '(')
            {
                int closeParenIndex = FindCloseParen(equation);
                string leftEquation = SolveMultiplyDivide(equation.Substring(1, FindCloseParen(equation) - 1)).ToString();
                string rightEquation = equation.Substring(closeParenIndex + 1);
                return leftEquation + rightEquation;
            }
            return equation;
        }

        public int SolveMultiplyDivide(string equation)
        {
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '/')
                {
                    return SolveAddSubtract(equation.Substring(0, i)) / SolveMultiplyDivide(SolveIfInParen(equation.Substring(i + 1)));
                }
                else if (equation[i] == '*')
                {
                    return SolveAddSubtract(equation.Substring(0, i)) * SolveMultiplyDivide(SolveIfInParen(equation.Substring(i + 1)));
                }
                else if (equation[i] == '(')
                {
                    equation = equation.Substring(0, i) + SolveIfInParen(equation.Substring(i)).ToString();
                    i--;
                }
            }
            return SolveAddSubtract(equation);
        }

        public int SolveAddSubtract(string equation)
        {
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '+')
                {
                    return Convert.ToInt32(equation.Substring(0, i)) + SolveAddSubtract(SolveIfInParen(equation.Substring(i + 1)));
                }
                else if (equation[i] == '-')
                {
                    return Convert.ToInt32(equation.Substring(0, i)) - SolveAddSubtract(SolveIfInParen(equation.Substring(i + 1)));
                }
            }
            return Convert.ToInt32(equation);
        }

        public int SolveEquationRecursively(string equation)
        {
            return SolveMultiplyDivide(equation);
        }

        public string ReadAndCalculate(string file)
        {
            Console.WriteLine(file);
            if (!File.Exists(file))
                throw new Exception("File does not exist");
            StringBuilder sb = new StringBuilder();
            Messages = [];
            using (StreamReader sr = new StreamReader(file))
            {
                string? line = sr.ReadLine();

                while (line != null)
                {
                    Equation = line;

                    string result = ParseAndMath(line);
                    Messages.Add($"{Equation} = {result}");
                    line = sr.ReadLine();
                }
            }
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                for (int i = 0; i < Messages.Count; i++)
                {
                    outputFile.WriteLine(Messages[i]);
                }
            }
            return Messages[0];
        }
    }
}
