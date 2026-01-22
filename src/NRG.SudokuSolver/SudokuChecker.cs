namespace NRG.SudokuSolver;

public static class SudokuChecker
{
    public static bool IsCorrect(Sudoku sudoku)
    {
        if (sudoku.Values.Any(e => e.Number is 0))
        {
            Console.WriteLine($"{nameof(SudokuChecker)}: There are empty fields (0 values). Check aborted.");
            return false;
        }

        var slicer = new SudokuSlicer(sudoku);
        for (var i = 0; i < 9; i++)
        {
            var b = slicer.GetBlock(i)
                .Where(e => e.Number > 0)
                .DistinctBy(e => e.Number)
                .Sum(e => e.Number);
            var h = slicer.GetHorizontalLine(i)
                .Where(e => e.Number > 0)
                .DistinctBy(e => e.Number)
                .Sum(e => e.Number);
            var v = slicer.GetVerticalLine(i)
                .Where(e => e.Number > 0)
                .DistinctBy(e => e.Number)
                .Sum(e => e.Number);

            var failed = new List<string>();

            if (b != 45)
            {
                failed.Add("block");
            }

            if (h != 45)
            {
                failed.Add("row");
            }

            if (v != 45)
            {
                failed.Add("column");
            }

            if (failed.Count > 0)
            {
                Console.WriteLine($"{nameof(SudokuChecker)}: Check failed at index {i} for {string.Join(", ", failed)}. Check Aborted.");
                return false;
            }
        }

        return true;
    }
}
