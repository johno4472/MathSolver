// See https://aka.ms/new-console-template for more information
using MathSolver.Models;

Console.WriteLine("Copy and paste the file path of the file you want to be solved:");

string equationFilePath = Console.ReadLine() ?? "";
if (String.IsNullOrEmpty(equationFilePath))
{
    ParsedLineResponse parsedLineResponse = new ParsedLineResponse();
    parsedLineResponse.ReadAndCalculate(equationFilePath);
}

Console.WriteLine("The answers have been added to your file");
