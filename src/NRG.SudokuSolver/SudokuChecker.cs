namespace NRG.SudokuSolver;

public static class SudokuChecker
{
    public static bool IsCorrect(Sudoku sudoku)
    {
        var slicer = new SudokuSlicerInt(sudoku);
        for (var i = 0; i < 9; i++)
        {
            var b = slicer.GetBlock(i);
            var h = slicer.GetHorizontalLine(i);
            var v = slicer.GetVerticalLine(i);

            var checksum = b.Distinct().Count() + h.Distinct().Count() + v.Distinct().Count();
            if (checksum != 27)
            {
                System.Console.WriteLine($"Sudoku Check Failed On {i}");
                return false;
            }
        }

        return true;
    }
}
