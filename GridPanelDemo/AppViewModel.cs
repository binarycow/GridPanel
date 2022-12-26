using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GridPanelDemo;

public class AppViewModel : ObservableValidator
{
    public const double MinimumValue = 25d;
    public const double MaximumValue = 500d;
    
    public AppViewModel()
    {
        this.AddRandomItemCommand = new RelayCommand(() => this.Items.Add(CreateRandomItem()));
        this.AddSpecificItemCommand = new RelayCommand(() => this.Items.Add(CreateSpecificItem()));
    }

    public ObservableCollection<ItemViewModel> Items { get; } = new(Enumerable.Range(0, 5).Select(_ => CreateRandomItem()));

    public IRelayCommand AddRandomItemCommand { get; }
    public RelayCommand AddSpecificItemCommand { get; }
    
    private static Color GetRandomColor()
    {
        Span<byte> bytes = stackalloc byte[3];
        Random.Shared.NextBytes(bytes);
        return Color.FromRgb(bytes[0], bytes[1], bytes[2]);
    }


    private double nextMinWidth = 100d;
    [Range(MinimumValue, MaximumValue)]
    public double NextMinWidth
    {
        get => this.nextMinWidth;
        set => this.SetProperty(ref this.nextMinWidth, value, true);
    }
    private double nextMinHeight = 100d;
    [Range(MinimumValue, MaximumValue)]
    public double NextMinHeight
    {
        get => this.nextMinHeight;
        set => this.SetProperty(ref this.nextMinHeight, value, true);
    }
    
    private static double GetRandomSize() => Random.Shared.Next(50, 250);
    private ItemViewModel CreateSpecificItem() => new (
        GetRandomColor(),
        this.NextMinWidth,
        this.NextMinHeight
    );
    private static ItemViewModel CreateRandomItem() => new (
        GetRandomColor(),
        GetRandomSize(),
        GetRandomSize()
    );
}