namespace NRG.SudokuSolver;

public class SudokuSlicerInt(Sudoku sudoku)
{
    private int Size => Sudoku.Size;
    private SudokuNumber[] Values => sudoku.Values;

    public int GetAt(int x, int y)
        => Values[y * Size + x];

    public int[] GetBlock(int x, int y)
    {
        var blockNumber = (x, y) switch
        {
            ( < 3, < 3) => 0,
            ( < 6, < 3) => 1,
            (_, < 3) => 2,
            ( < 3, < 6) => 3,
            ( < 6, < 6) => 4,
            (_, < 6) => 5,
            ( < 3, _) => 6,
            ( < 6, _) => 7,
            (_, _) => 8,
        };

        return GetBlock(blockNumber);
    }

    public int[] GetBlock(int blockNumber)
    {
        var hStart = blockNumber switch
        {
            0 or 1 or 2 => 0,
            3 or 4 or 5 => 3,
            6 or 7 or 8 => 6,
            _ => throw new NotImplementedException()
        };
        var vStart = blockNumber switch
        {
            0 or 3 or 6 => 0,
            1 or 4 or 7 => 3,
            2 or 5 or 8 => 6,
            _ => throw new NotImplementedException(),
        };

        var block = new int[9];
        for (var i = 0; i < 3; i++)
        {
            block[i * 3 + 0] = GetAt(vStart + 0, (hStart + i));
            block[i * 3 + 1] = GetAt(vStart + 1, (hStart + i));
            block[i * 3 + 2] = GetAt(vStart + 2, (hStart + i));
        }

        return block;
    }

    public int[] GetHorizontalLine(int lineNumber)
    {
        var lines = Values.Chunk(9);
        return [.. lines.ElementAt(lineNumber).Select(e => e.Number)];
    }

    public int[] GetVerticalLine(int lineNumber)
    {
        var lines = new int[9];
        for (var i = 0; i < 9; i++)
        {
            lines[i] = Values[lineNumber + Size * i];
        }

        return lines;
    }

}
