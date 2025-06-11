using System.Security.Cryptography.X509Certificates;

namespace MathSolver.Helpers
{
    public static class RecursiveMathSolver
    {

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
        public static string SolveIfInParen(string equation)
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

        public static int SolveMultiplyDivide(string equation)
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
            }
            return SolveAddSubtract(equation);
        }

        public static int SolveAddSubtract(string equation)
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

        public static int SolveEquationRecursively(string equation)
        {
            return SolveMultiplyDivide(equation);
        }
    }
}
