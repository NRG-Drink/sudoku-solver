namespace NRG.SudokuSolver.Tests;

public class SolveTests
{
    public IEnumerable<Func<(string, int[])>> GetSudokus()
    {
        yield return () => ("ISE AG (Roger)", SudokuRiddles.Sudoku7);
        yield return () => ("ISE AG", SudokuRiddles.Sudoku700);
        yield return () => ("ISE AG (presolved)", SudokuRiddles.Sudoku701);
        yield return () => ("Easy 1", SudokuRiddles.SudokuEasy1);
        yield return () => ("Expert 1", SudokuRiddles.SudokuExpert1);
        yield return () => ("Extreme 1", SudokuRiddles.SudokuExtreme1);
    }

    [Test]
    [MethodDataSource(nameof(GetSudokus))]
    [DisplayName("$name")]
    public async Task Solve(string name, int[] input)
    {
        var sudoku = new Sudoku(input);
        var solver = new SudokuSolver();
        var solved = solver.SolveSudoku(sudoku);

        var isCorrect = SudokuChecker.IsCorrect(solved);
        Console.WriteLine($"Sudo was solved correct? - {isCorrect}");

        var printer = new SudokuConsolePrinter();
        printer.Print(solved.AsNumbers);

        await Assert.That(isCorrect).IsTrue();
    }

    [Test]
    [MethodDataSource(nameof(GetSudokus))]
    [DisplayName("$name")]
    public async Task SolveRoger(string name, int[] input)
    {
        var sudoku = new Sudoku(input);
        var solver = new SudokuSolverRB();
        var solved = solver.SolveSudoku(sudoku);
        // Check clone worked.
        await Assert.That(sudoku).IsNotEqualTo(solved);
        await Assert.That(sudoku.Values.Select(e => e.Number)).IsNotEqualTo(solved.Values.Select(e => e.Number));

        var isCorrect = SudokuChecker.IsCorrect(solved);
        Console.WriteLine($"Sudo was solved correct? - {isCorrect}");

        var printer = new SudokuConsolePrinter();
        printer.Print(solved.AsNumbers);

        await Assert.That(isCorrect).IsTrue();
    }
}
