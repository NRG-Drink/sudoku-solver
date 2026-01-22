namespace NRG.SudokuSolver;

public class SudokuSolver
{
    private SudokuSlicerInt slicer = null!;
    private SudokuSlicer _slicer2 = null!;

    public int[] SolveSudoku(int[] sudoku)
    {
        var solved = SolveSudoku(new Sudoku(sudoku));
        return solved.AsNumbers;
    }

    public Sudoku SolveSudoku(Sudoku sudoku)
    {
        slicer = new(sudoku);
        _slicer2 = new(sudoku);
        var step = 0;
        var previousAllHash = 0;
        while (sudoku.Checksum != previousAllHash)
        {
            previousAllHash = sudoku.Checksum;
            step++;
            for (var i = 0; i < sudoku.Values.Length; i++)
            {
                var y = (int)i / 9;
                var x = i % 9;
                var e = sudoku.Values[i];
                if (e.Number != 0)
                {
                    continue;
                }

                var b = slicer.GetBlock(x, y);
                e.Possibles.RemoveAll(e => b.Contains(e));
                var h = slicer.GetHorizontalLine(y);
                e.Possibles.RemoveAll(e => h.Contains(e));
                var v = slicer.GetVerticalLine(x);
                e.Possibles.RemoveAll(e => v.Contains(e));

                if (e.SinglePossible is not 0)
                {
                    e.Number = e.SinglePossible;
                    continue;
                }

                var b2 = _slicer2.GetBlock(x, y);
                FindOnlyPossible(b2);
                var bp2 = b2.Except([e]).SelectMany(e => e.Possibles).Distinct().ToArray();

                var h2 = _slicer2.GetHorizontalLine(y);
                FindOnlyPossible(h2);
                var hp2 = h2.Except([e]).SelectMany(e => e.Possibles).Distinct().ToArray();

                var v2 = _slicer2.GetVerticalLine(x);
                FindOnlyPossible(v2);
                var vp2 = v2.Except([e]).SelectMany(e => e.Possibles).Distinct().ToArray();

                foreach (var p in e.Possibles)
                {
                    var hasBlockMonopoly = !bp2.Contains(p);
                    var hasHorizontalMonopoly = !hp2.Contains(p);
                    var hasVerticalMonopoly = !vp2.Contains(p);

                    var hasMonopoly = hasBlockMonopoly || hasHorizontalMonopoly || hasVerticalMonopoly;
                    if (hasMonopoly)
                    {
                        e.Number = p;
                        break;
                    }
                }
            }

            //System.Console.WriteLine($"Step {step}:");
            if (!sudoku.Values.Any(e => e.Number is 0))
            {
                System.Console.WriteLine($"Sudoku Finished In {step} Steps!");
                return sudoku;
                //break;
            }
        }

        Console.WriteLine($"Failed to solve the Sudoku completely :( (tried {step} steps)");
        return sudoku;
    }

    private void FindOnlyPossible(SudokuNumber[] wip)
    {
        for (var n = 1; n < 10; n++)
        {
            if (wip.Any(e => e.Number == n))
            {
                continue;
            }

            var c = wip.Where(e => e.Possibles.Any(p => p == n));
            if (c.Count() == 1)
            {
                var w = c.First();
                w.Number = n;
            }
        }
    }
}
