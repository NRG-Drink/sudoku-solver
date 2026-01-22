namespace NRG.SudokuSolver;

public class Sudoku(SudokuNumber[] values)
{
    public Sudoku(int[] numbers) : this([.. numbers.Select(e => new SudokuNumber(e))]) { }

    public const int Size = 9;
    public SudokuNumber[] Values => values;
    public int[] AsNumbers => [.. values.Select(e => e.Number)];
    public int NumbersChecksum => values.Sum(e => e.Number);
    public int Checksum => values.Sum(e => e.Checksum);

    public Sudoku Clone() => new([.. values.Select(e => e.Clone())]);
}

public class SudokuNumber(int number, List<int> possibleValues)
{
    public SudokuNumber(int number) : this(number, number is 0 ? [1, 2, 3, 4, 5, 6, 7, 8, 9] : []) { }

    public int Number
    {
        get => number;
        set { number = value; Possibles.Clear(); }
    }
    public List<int> Possibles => possibleValues;
    public int SinglePossible => Possibles.Count is 1 ? Possibles[0] : 0;
    public int Checksum => Possibles.Sum() + number;

    public static explicit operator SudokuNumber(int Number) => new(Number);
    public static implicit operator int(SudokuNumber sn) => sn.Number;

    public SudokuNumber RemovePossibles(params SudokuNumber[] sns)
    {
        var numbers = sns.DistinctBy(e => e.Number);
        foreach (var n in numbers)
        {
            Possibles.Remove(n.Number);
        }

        return this;
    }

    public SudokuNumber Clone() => new(number, [..possibleValues.Select(e => e)]);
}
