using System.Windows.Media;

namespace GridPanel;

public class ItemViewModel
{
    public ItemViewModel(Color color, double minWidth, double minHeight)
    {
        this.Color = color;
        this.MinHeight = minHeight;
        this.MinWidth = minWidth;
    }

    public Color Color { get; }
    public double MinWidth { get; }
    public double MinHeight { get; }
}