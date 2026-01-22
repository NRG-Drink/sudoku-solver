using System.Diagnostics.CodeAnalysis;

namespace NRG.SudokuSolver;

public class SudokuSolverRoger
{
    //private SudokuSlicerInt _slicer = null!;
    private readonly SudokuHelper _helper = new();

    public int[] SolveSudoku(int[] sudoku)
    {
        var solved = SolveSudoku(new Sudoku(sudoku));
        return solved.AsNumbers;
    }

    public Sudoku SolveSudoku(Sudoku input)
    {
        var isSuccesful = false;
        if (TrySolveSudoku(input, out var restult, out var step))
        {
            //Console.WriteLine($"Sudoku Finished In {step} Steps!");
            isSuccesful = true;
        }
        else
        {
            Console.WriteLine($"Failed to find a solution the easy way ({step} steps). Start branching.");
            if (TryBranches(restult, step, out var tryRestult, out var branchSteps))
            {
                restult = tryRestult;
                step = branchSteps;
                isSuccesful = true;
            }

            //Console.WriteLine($"Failed to solve the Sudoku completely :( (tried {step} steps)");
        }

        if (isSuccesful)
        {
            Console.WriteLine($"Sudoku Finished In {step} Steps!");
        }
        else
        {
            Console.WriteLine($"Failed to solve the Sudoku completely :( (tried {step} steps)");
        }

        return restult;
    }

    private bool TryBranches(Sudoku restult, int inputSteps, out Sudoku tryResult, out int outputSteps, string ident = "")
    {
        //var ident = new string([.. Enumerable.Repeat('_', recursionCount)]);
        ident += "  ";
        tryResult = restult;
        outputSteps = inputSteps;
        var minPoss = restult.Values.Where(e => e.Possibles.Count != 0).MinBy(e => e.Possibles.Count);
        if (minPoss is null)
        {
            Console.WriteLine($"{ident}Branch run out of possibilities.");
            return false;
        }

        var num = minPoss!.Possibles.Count;
        var poss = restult.Values.First(e => e.Possibles.Count == minPoss!.Possibles.Count);
        var index = restult.Values.IndexOf(poss);
        foreach (var tryNumber in poss.Possibles.Shuffle())
        {
            Console.WriteLine($"{ident}Open branch for index {index} and number {tryNumber}  [{string.Join(", ", poss.Possibles)}]");
            var clone = restult.Clone();
            clone.Values[index].Number = tryNumber;
            if (TrySolveSudoku(clone, out tryResult, out var tryStep))
            {
                outputSteps += tryStep;
                Console.WriteLine($"{ident}Branch was successful after {tryStep} steps. (total: {outputSteps} steps)");
                return true;
            }
            else
            {
                Console.WriteLine($"{ident}Branch failed after {tryStep} steps. (total: {outputSteps + tryStep} steps)");
                if (TryBranches(tryResult, outputSteps + tryStep, out var tryClone, out var tryOutputSteps, ident))
                {
                    tryResult = tryClone;
                    outputSteps = tryOutputSteps;
                    return true;
                }
            }
        }

        return false;
    }

    public bool TrySolveSudoku(Sudoku input, out Sudoku result, out int step)
    {
        var sudoku = input.Clone();
        result = sudoku;
        step = 0;
        var previousAllHash = 0;
        while (sudoku.Checksum != previousAllHash)
        {
            previousAllHash = sudoku.Checksum;
            step++;
            SolveStep(sudoku);

            if (!sudoku.Values.Any(e => e.Number is 0))
            {
                return true;
            }
        }

        return false;

    }

    private void SolveStep(Sudoku sudoku)
    {
        for (var i = 0; i < sudoku.Values.Length; i++)
        {
            var e = sudoku.Values[i];
            if (e.Number != 0)
            {
                continue;
            }

            var y = (int)(i / 9);
            var x = i % 9;
            var b = _helper.GetBlock(sudoku, x, y);
            var h = _helper.GetHorizontalLine(sudoku, y);
            var v = _helper.GetVerticalLine(sudoku, x);

            // Remove non possible numbers.
            e.RemovePossibles([.. b, .. h, .. v]);

            if (e.SinglePossible is not 0)
            {
                e.Number = e.SinglePossible;
                continue;
            }

            // See if one number is only possible in one place and fill it.
            FindOnlyPossible(b);
            FindOnlyPossible(h);
            FindOnlyPossible(v);
        }
    }

    private static void FindOnlyPossible(SudokuNumber[] wip)
    {
        var freeNumbers = Enumerable.Range(1, 9).Except(wip.Select(e => e.Number));
        foreach (var n in freeNumbers)
        {
            var c = wip.Where(e => e.Possibles.Any(p => p == n));
            if (c.Count() == 1)
            {
                var w = c.First();
                w.Number = n;
            }
        }
    }
}
