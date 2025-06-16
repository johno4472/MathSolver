using System.Text;
using System.Text.RegularExpressions;

namespace MathSolver.Models
{
    public class EquationSolverInfo
    {
        public string Equation { get; set; } = "";

        public string Message { get; set; } = "";

        public string Result { get; set; } = "";

        public bool IsValidEquation { get; set; }

        //at this point the equation is stripped of all whitespace
        public bool ValidParen()
        {
            int parenCounter = 0;
            for (int i = 0; i < Equation.Length; i++)
            {
                if (Equation[i] == '(')
                {
                    parenCounter++;
                }
                else if (Equation[i] == ')')
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

        public EquationSolverInfo RemoveWhiteSpace()
        {
            //remove the spaces
            Equation = Regex.Replace(Equation, @"\s+", "");
            return this;
        }

        public EquationSolverInfo ValidateEquation()
        {
            //make sure every character is a number or operator
            //needs to be numberoperatornumber
            if (ValidParen())
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

        public static int FindCloseParen(string equation)
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
                string leftEquation = SolveMultiplyDivide(equation[1..FindCloseParen(equation)]).ToString();
                string rightEquation = equation[(closeParenIndex + 1)..];
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
                    return SolveAddSubtract(equation[..i]) / SolveMultiplyDivide(SolveIfInParen(equation[(i + 1)..]));
                }
                else if (equation[i] == '*')
                {
                    return SolveAddSubtract(equation[..i]) * SolveMultiplyDivide(SolveIfInParen(equation[(i + 1)..]));
                }
                else if (equation[i] == '(')
                {
                    equation = equation[..i] + SolveIfInParen(equation[i..]).ToString();
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
                    return Convert.ToInt32(equation[..i]) + SolveAddSubtract(SolveIfInParen(equation[(i + 1)..]));
                }
                else if (equation[i] == '-')
                {
                    return Convert.ToInt32(equation[..i]) - SolveAddSubtract(SolveIfInParen(equation[(i + 1)..]));
                }
            }
            return Convert.ToInt32(equation);
        }

        
        public EquationSolverInfo SolveEquationRecursively()
        {
            if (IsValidEquation)
            {
                Result = SolveMultiplyDivide(Equation).ToString();
            }
            Message = $"{Equation} = {Result}";
            return this;
        }
    }
}

