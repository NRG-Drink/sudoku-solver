namespace NRG.SudokuSolver.Tests;

public class SolveTests
{
    // Leicht (solved)
    //private int[] _sudokuInput = [
    //    0, 8, 0,  0, 7, 0,  3, 0, 5,
    //    2, 5, 7,  0, 0, 8,  0, 9, 6,
    //    0, 0, 0,  0, 0, 5,  0, 7, 0,

    //    4, 0, 9,  3, 6, 0,  0, 0, 0,
    //    0, 6, 1,  0, 0, 0,  0, 4, 0,
    //    3, 7, 8,  0, 1, 0,  0, 2, 0,

    //    0, 3, 0,  0, 0, 1,  9, 0, 4,
    //    7, 0, 5,  0, 0, 0,  1, 8, 2,
    //    0, 0, 6,  4, 8, 2,  5, 0, 0,
    //];
    // Experte (solved)
    //    int[] _sudokuInput = [
    //    0, 0, 0,  0, 0, 0,  0, 7, 0,
    //    2, 7, 5,  0, 0, 0,  3, 1, 4,
    //    0, 0, 0,  0, 2, 7,  0, 5, 0,

    //    9, 8, 0,  0, 0, 0,  0, 3, 1,
    //    0, 3, 1,  8, 0, 4,  0, 0, 0,
    //    0, 0, 0,  1, 0, 0,  8, 0, 5,

    //    7, 0, 6,  2, 0, 0,  1, 8, 0,
    //    0, 9, 0,  7, 0, 0,  0, 0, 0,
    //    4, 1, 0,  0, 0, 5,  0, 0, 7,
    //];
    // Extrem (not solved)
    //int[] _sudokuInput = [
    //     0, 0, 1,  0, 0, 0,  0, 0, 6,
    //     0, 0, 3,  2, 1, 0,  5, 0, 0,
    //     0, 0, 2,  0, 7, 0,  0, 0, 8,

    //     0, 9, 0,  0, 5, 0,  0, 0, 4,
    //     0, 7, 0,  0, 0, 4,  6, 0, 0,
    //     0, 0, 0,  6, 0, 1,  0, 0, 0,

    //     0, 0, 7,  0, 4, 0,  0, 3, 0,
    //     0, 0, 0,  0, 0, 0,  0, 0, 0,
    //     0, 8, 0,  0, 2, 0,  0, 0, 5,
    // ];
    // Zeitung (as it is)
    //int[] _sudokuInput = [
    //     0, 0, 0,  6, 0, 0,  0, 0, 4,
    //     2, 9, 0,  0, 4, 8,  0, 3, 7,
    //     0, 0, 0,  7, 0, 0,  2, 9, 0,

    //     0, 1, 0,  0, 0, 0,  0, 8, 5,
    //     0, 0, 4,  0, 0, 0,  0, 0, 0,
    //     8, 6, 0,  0, 0, 0,  0, 0, 0,

    //     0, 2, 5,  0, 0, 7,  0, 0, 0,
    //     7, 8, 0,  3, 1, 0,  0, 6, 2,
    //     4, 0, 0,  0, 0, 6,  0, 0, 0,
    // ];
    // Zeitung (can finish from this state)
    int[] _sudokuInput = [
         1, 7, 3,  6, 0, 0,  8, 5, 4,
         2, 9, 6,  5, 4, 8,  1, 3, 7,
         5, 4, 8,  7, 3, 1,  2, 9, 6,

         0, 1, 0,  0, 0, 4,  0, 8, 5,
         0, 5, 4,  8, 0, 3,  0, 2, 0,
         8, 6, 0,  1, 5, 0,  0, 4, 0,

         6, 2, 5,  4, 0, 7,  3, 1, 0,
         7, 8, 9,  3, 1, 5,  4, 6, 2,
         4, 3, 1,  9, 0, 6,  5, 7, 0,
     ];

    private Sudoku Sudoku => new(_sudokuInput);

    public IEnumerable<Func<(string, int[])>> GetSudokus()
    {
        yield return () => ("ISE AG (Roger)", SudokuRiddles.Sudoku7);
        yield return () => ("ISE AG", SudokuRiddles.Sudoku700);
        yield return () => ("ISE AG (presolved)", SudokuRiddles.Sudoku701);
        yield return () => ("Easy 1", SudokuRiddles.SudokuEasy1);
        yield return () => ("Expert 1", SudokuRiddles.SudokuExpert1);
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
        var solver = new SudokuSolverRoger();
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
