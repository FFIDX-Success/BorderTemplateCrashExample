namespace BorderTemplateCrash;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        for (int i = 0; i < (int)CountSlider.Value; i++)
        {
            var control = new NovaButton();
            control.Text = $"Button {i} 0x{control.GetHashCode():x8}";

            if (ReferenceEquals(sender, NoBorderButton))
            {
                control.Style = Resources["NoBorderControlTemplate"] as Style;
            }
            else
            {
                control.Style = Resources["BorderControlTemplate"] as Style;
            }

            const int width = 192;
            const int height = 80;
            var x = Random.Shared.Next(0, (int)TheLayout.Width - width);
            var y = Random.Shared.Next(0, (int)TheLayout.Height - height);

            View innermost = control;
            for (int j = 0; j < (int)DepthSlider.Value; j++)
            {
                var stackLayout = new HorizontalStackLayout();
                stackLayout.BackgroundColor = Color.FromHsv((float)j / (float)DepthSlider.Value, 1, 1);
                stackLayout.Margin = new Thickness(1);
                stackLayout.Add(innermost);
                innermost = stackLayout;
            }

            AbsoluteLayout.SetLayoutBounds(innermost, new Rect(x, y, 192, 40));
            TheLayout.Add(innermost);
        }
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        TheLayout.Clear();
    }

    private NovaButton? FindNovaButton(View view)
    {
        if (view is NovaButton button)
        {
            return button;
        }

        if (view is Layout layout)
        {
            foreach (var child in layout.Children)
            {
                var found = FindNovaButton((View)child);
                if (found != null)
                {
                    return found;
                }
            }
        }
        return null;
    }

    private void OnInvalidateClicked(object sender, EventArgs e)
    {
        foreach (var child in TheLayout.Children)
        {
            var button = FindNovaButton((View)child);
            if (button != null)
            {
                button.Text += "1";
            }

            var bounds = AbsoluteLayout.GetLayoutBounds((View)child);
            bounds.Width++;
            bounds.Height++;
            bounds.X++;
            bounds.Y++;
            AbsoluteLayout.SetLayoutBounds((View)child, bounds);
            child.InvalidateMeasure();
            child.InvalidateArrange();
        }
        TheLayout.InvalidateMeasure();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}

