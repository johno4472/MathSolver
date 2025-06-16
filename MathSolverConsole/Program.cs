// See https://aka.ms/new-console-template for more information
using MathSolver.Models;
using MathSolverConsole;

Console.WriteLine("Copy and paste the file path of the file you want to be solved:");

string equationFilePath = Console.ReadLine() ?? "";
if (!String.IsNullOrEmpty(equationFilePath))
{
    //create a new equationfileresponse instance
    EquationFileParser equationFileParser = new EquationFileParser();
    ParsedLineResponse parsedLineResponse = new ParsedLineResponse();
    //read file and make list of equation strings
    equationFileParser.ReadFile(equationFilePath);
    //for each equation string
    for (int i = 0; i < equationFileParser.ParsedLineResponses.Count; i++)
    {
        //strip of spaces
        equationFileParser.ParsedLineResponses[i].RemoveWhiteSpace()
            //verify if valid equation
            .ValidateEquation()
            //solve equation
            .SolveEquationRecursively();
    }
    //write response message to file
    equationFileParser.WriteResponseToFile(equationFilePath);
}

Console.WriteLine("The answers have been added to your file");
