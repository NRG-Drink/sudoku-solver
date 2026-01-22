using System.Text;

namespace NRG.SudokuSolver;

public class SudokuConsolePrinter
{
    public void Print(int[] sudoku)
    {
        var sb = new StringBuilder();
        var lines = sudoku.Chunk(Sudoku.Size);
        var stringLines = lines.Select(e => string.Join(" | ", e.Chunk(3).Select(e => string.Join(", ", e))));
        var blocks = stringLines.Chunk(3).Select(e => string.Join("\n", e));
        var diff = new string([.. Enumerable.Range(0, stringLines.First().Length).Select(e => '-')]);
        var res = string.Join($"\n{diff}\n", blocks);
        sb.Append(res);

        Console.WriteLine(sb);
    }
}
