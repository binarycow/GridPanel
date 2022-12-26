using System;
using System.Windows;
using System.Windows.Controls;

namespace GridPanelDemo;

public class GridPanel : Panel
{
    private Size cellSize;
    private int columns;
    private int rows;
    
    private VisibleChildEnumerator GetVisibleChildren() => new (this.InternalChildren, 0);
    
    private Size MeasureAllChildren(Size availableSize, out int visibleChildCount)
    {
        var currentCellSize = Size.Empty;
        visibleChildCount = default;
        foreach (var child in this.GetVisibleChildren())
        {
            ++visibleChildCount;
            child.Measure(availableSize);
            currentCellSize = Max(
                currentCellSize: currentCellSize, 
                childDesired: child.DesiredSize
            );
        }
        return new (
            double.IsNegativeInfinity(currentCellSize.Width) ? 0d : currentCellSize.Width,
            double.IsNegativeInfinity(currentCellSize.Height) ? 0d : currentCellSize.Height
        );
    }

    private static Size Max(Size currentCellSize, Size childDesired) => new (
        Math.Max(currentCellSize.Width, childDesired.Width),
        Math.Max(currentCellSize.Height, childDesired.Height)
    );

    
    protected override Size MeasureOverride(Size availableSize)
    {
        this.cellSize = MeasureAllChildren(availableSize, out var visibleChildCount);
        var maxCellsDouble = availableSize.Width / this.cellSize.Width;
        var maxCellsPerRow = double.IsInfinity(maxCellsDouble) ? 1 : (int)Math.Floor(maxCellsDouble);
        this.columns = Math.Max(1, Math.Min(maxCellsPerRow, visibleChildCount));
        this.rows = Math.DivRem(visibleChildCount, this.columns) switch
        {
            (Quotient: var quotient, Remainder: 0) => quotient,
            (Quotient: var quotient, Remainder: _) => quotient + 1,
        };
        var result = new Size(
            this.columns * this.cellSize.Width, 
            this.rows * this.cellSize.Height
        );
        return result;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var index = 0;
        foreach (var child in this.GetVisibleChildren())
        {
            var (row, column) = GetRowAndColumnFromIndex(index);
            ++index;
            var rect = new Rect(
                new (
                    column * this.cellSize.Width,
                    row * this.cellSize.Height
                ), 
                this.cellSize
            );
            child.Arrange(rect);
        }
        return finalSize;
    }
    
    private (int Row, int Col) GetRowAndColumnFromIndex(int index) 
        => this.columns is 0 
            ? (0, 0) 
            : Math.DivRem(index, this.columns);
}