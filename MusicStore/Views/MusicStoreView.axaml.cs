using Avalonia.Controls;
using System;

namespace MusicStore.Views;

public partial class MusicStoreView : Window
{
    private readonly Control _searchBox;

    public MusicStoreView()
    {
        InitializeComponent();
        _searchBox = this.FindControl<TextBox>("SearchBox") ?? throw new Exception("Cannot find SearchBox by name");
    }

    private void WindowOpened(object? sender, EventArgs e)
    {
        _searchBox.Focus();
    }
}