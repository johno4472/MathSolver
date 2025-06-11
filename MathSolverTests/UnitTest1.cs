using FluentAssertions;
using MathSolver.Helpers;

namespace MathSolverTests
{
    public class UnitTest1
    {
        [InlineData("4+7", 11)]
        [InlineData("9-2", 7)]
        [InlineData("6*5", 30)]
        [InlineData("12/3", 4)]
        [InlineData("8/(2+2)", 4)]
        [Theory]
        public void SolvesBasicMathProblems(string equation, int expected)
        {
            //Arrange
            int result;

            //Act
            result = RecursiveMathSolver.SolveEquationRecursively(equation);

            //Assert
            result.Should().Be(expected);
        }
    }
}