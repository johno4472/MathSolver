using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

namespace MathSolver.Models
{
    public class ParsedLineResponse
    {
        public string equation {  get; set; }
        
        public bool isValid { get; set; }

        public string Message { get; set; }

        public int Result { get; set; }

        //at this point the equation is stripped of all whitespace
        public string SolveEquation(string equation)
        {
            return new DataTable().Compute(equation, null).ToString();
        }

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

        public int ParseAndMath(string line)
        {
            //remove the spaces
            string NoSpaceLine = Regex.Replace(line, @"\s+", "");
            //make sure every character is a number or operator
            //needs to be numberoperatornumber
            if (ValidParen(NoSpaceLine))
            {
                string checkWoParen = NoSpaceLine.Replace("(", "").Replace(")", "");
                string pattern = @"^((\d+[+\-*/])+\d+)+$";
                if (Regex.Match(checkWoParen, pattern).Success)
                {
                    return Int32.Parse(SolveEquation(NoSpaceLine));
                }
            }
            return -1;
        }

        
    }
}
