using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;

namespace GridPanelDemo;

public class ItemViewModel
{
    private readonly AppViewModel appViewModel;

    public ItemViewModel(AppViewModel appViewModel, Color color, double minWidth, double minHeight)
    {
        this.appViewModel = appViewModel;
        this.Color = color;
        this.MinHeight = minHeight;
        this.MinWidth = minWidth;
        this.RemoveCommand = new RelayCommand(() => appViewModel.Items.Remove(this));
    }

    public IRelayCommand RemoveCommand { get; }

    public Color Color { get; }
    public double MinWidth { get; }
    public double MinHeight { get; }
}