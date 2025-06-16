using System.Text;
using System.Text.RegularExpressions;

namespace MathSolver.Models
{
    public class ParsedLineResponse
    {
        public string Equation { get; set; } = "";

        public string Message { get; set; } = "";

        public string Result { get; set; } = "";

        public bool IsValidEquation { get; set; }

        //at this point the equation is stripped of all whitespace
        public bool ValidParen(string line)
        {
            int parenCounter = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '(')
                {
                    parenCounter++;
                }
                else if (line[i] == ')')
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

        public ParsedLineResponse RemoveWhiteSpace()
        {
            //remove the spaces
            Equation = Regex.Replace(Equation, @"\s+", "");
            return this;
        }

        public ParsedLineResponse ValidateEquation()
        {
            //make sure every character is a number or operator
            //needs to be numberoperatornumber
            if (ValidParen(Equation))
            {
                string checkWoParen = Equation.Replace("(", "").Replace(")", "");
                string pattern = @"^((\d+[+\-*/])+\d+)+$";
                if (Regex.Match(checkWoParen, pattern).Success)
                {
                    IsValidEquation = true;
                    return this;
                }
            }
            Result = "Not a valid equation";
            IsValidEquation = false;
            return this;
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

        
        public ParsedLineResponse SolveEquationRecursively()
        {
            if (IsValidEquation)
            {
                Result = SolveMultiplyDivide(Equation).ToString();
            }
            Message = $"{Equation} = {Result}";
            return this;
        }

        public List<string> ReadFile(string file)
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
                    equationList.Add(line);
                }
            }
            return equationList;
        }
    }
}

