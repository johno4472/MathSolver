using FluentAssertions;
using MathSolver.Models;
using MathSolverConsole;

namespace MathSolverTests
{
    public class UnitTest1
    {
        [InlineData("4+7", "11")]
        [InlineData("9-2", "7")]
        [InlineData("6*5", "30")]
        [InlineData("12/3", "4")]
        [InlineData("8/(2+2)", "2")]
        [Theory]
        public void SolvesBasicMathProblems(string equation, string expected)
        {
            //Arrange
            EquationSolverInfo solverInfo = new() { Equation = equation, IsValidEquation = true };

            //Act
            solverInfo.SolveEquationRecursively();

            //Assert
            solverInfo.Result.Should().BeEquivalentTo(expected);
        }
    }
}