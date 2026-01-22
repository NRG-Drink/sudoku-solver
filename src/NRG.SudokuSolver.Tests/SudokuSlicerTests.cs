namespace NRG.SudokuSolver.Tests;

public class SudokuSlicerTests
{
    private int[] _sudokuInput = [
        0, 8, 0,  0, 7, 0,  3, 0, 5,
        2, 5, 7,  0, 0, 8,  0, 9, 6,
        0, 0, 0,  0, 0, 5,  0, 7, 0,

        4, 0, 9,  3, 6, 0,  0, 0, 0,
        0, 6, 1,  0, 0, 0,  0, 4, 0,
        3, 7, 8,  0, 1, 0,  0, 2, 0,

        0, 3, 0,  0, 0, 1,  9, 0, 4,
        7, 0, 5,  0, 0, 0,  1, 8, 2,
        0, 0, 6,  4, 8, 2,  5, 0, 0,
    ];
    private Sudoku Sudoku => new(_sudokuInput);
    private SudokuSlicer Slicer => new(Sudoku);

    [Test, MatrixDataSource]
    public async Task BlockByCoordinate(
        [MatrixRange<int>(6, 8)] int x,
        [MatrixRange<int>(6, 8)] int y)
    {
        var solution = new int[] { 9, 0, 4, 1, 8, 2, 5, 0, 0 };
        var res = Slicer.GetBlock(x, y);

        await Assert.That(res).Count().IsEqualTo(9);
        var numbers = res.Select(e => e.Number);
        await Assert.That(numbers).IsEquivalentTo(solution);
    }

    [Test]
    public async Task BlockByNumber()
    {
        var solution = new int[] { 9, 0, 4, 1, 8, 2, 5, 0, 0 };
        var res = Slicer.GetBlock(8);

        await Assert.That(res).Count().IsEqualTo(9);
        var numbers = res.Select(e => e.Number);
        await Assert.That(numbers).IsEquivalentTo(solution);
    }

    [Test]
    public async Task VerticalLine()
    {
        var solution = new int[] { 5, 6, 0, 0, 0, 0, 4, 2, 0 };
        var res = Slicer.GetVerticalLine(8);

        await Assert.That(res).Count().IsEqualTo(9);
        var numbers = res.Select(e => e.Number);
        await Assert.That(numbers).IsEquivalentTo(solution);
    }

    [Test]
    public async Task HorizontalLine()
    {
        var solution = new int[] { 0, 0, 6, 4, 8, 2, 5, 0, 0 };
        var res = Slicer.GetHorizontalLine(8);

        await Assert.That(res).Count().IsEqualTo(9);
        var numbers = res.Select(e => e.Number);
        await Assert.That(numbers).IsEquivalentTo(solution);
    }
}
