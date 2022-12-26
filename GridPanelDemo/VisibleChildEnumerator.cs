using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace GridPanelDemo;

internal struct VisibleChildEnumerator : IEnumerator<UIElement>, IEnumerable<UIElement>
{
    private const int StateNotStarted = 0;
    private const int StateInProgress = 1;
    private const int StateFinished = 2;
    private int state;
    private readonly UIElementCollection? children;
    private readonly int startIndex;
    private int currentIndex;
    public VisibleChildEnumerator(UIElementCollection children, int startIndex)
    {
        this.children = children;
        this.startIndex = startIndex;
        this.currentIndex = default;
        this.Current = default!;
        this.state = StateNotStarted;
    }

    public void Reset()
    {
        this.currentIndex = default;
        this.Current = default!;
        this.state = StateNotStarted;
    }
    
    public bool MoveNext()
    {
        switch (this.state)
        {
            case StateNotStarted when this.TryGetNextItem(this.startIndex, out var foundIndex, out var item):
                this.Current = item;
                this.currentIndex = foundIndex;
                this.state = StateInProgress;
                return true;
            case StateInProgress when this.TryGetNextItem(this.currentIndex + 1, out var foundIndex, out var item):
                this.Current = item;
                this.currentIndex = foundIndex;
                return true;
            default:
                this.state = StateFinished;
                return false;
        }
    }

    private bool TryGetNextItem(int index, out int foundIndex, [NotNullWhen(true)] out UIElement? result)
    {
        (foundIndex, result) = (default, default);
        if (this.children is null)
            return false;
        for (foundIndex = index; foundIndex < this.children.Count; ++foundIndex)
        {
            result = this.children[foundIndex];
            if (result.Visibility is Visibility.Visible)
                return true;
        }
        return false;
    }


    public UIElement Current { get; private set; }

    object IEnumerator.Current => this.Current;

    void IDisposable.Dispose()
    {
    }

    public VisibleChildEnumerator GetEnumerator() => this;
    
    IEnumerator<UIElement> IEnumerable<UIElement>.GetEnumerator() => this.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}