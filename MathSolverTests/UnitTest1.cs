using FluentAssertions;
using MathSolver.Helpers;
using MathSolver.Models;

namespace MathSolverTests
{
    public class UnitTest1
    {
        [InlineData("4+7", 11)]
        [InlineData("9-2", 7)]
        [InlineData("6*5", 30)]
        [InlineData("12/3", 4)]
        [InlineData("8/(2+2)", 2)]
        [InlineData("8+(12/(3*2))", 10)]
        [InlineData("(8+4)/(9-3)", 2)]
        [Theory]
        public void SolvesBasicMathProblems(string equation, int expected)
        {
            //Arrange
            ParsedLineResponse parsedLineResponse = new ParsedLineResponse();
            int result;

            //Act
            result = parsedLineResponse.SolveEquationRecursively(equation);

            //Assert
            result.Should().Be(expected);
        }
    }
}