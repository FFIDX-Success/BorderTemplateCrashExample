using static System.Net.Mime.MediaTypeNames;

namespace BorderTemplateCrash;
public class NovaButton : ContentView
{
    /// <summary>
    /// BindableProperty for <see cref="Text" />
    /// </summary>
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text),
                                typeof(string),
                                typeof(NovaButton),
                                propertyChanged: OnIsTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnIsTextChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is not NovaButton novaButton)
        {
            return;
        }
        novaButton.InvalidateMeasure();
    }

    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source),
                                typeof(string),
                                typeof(NovaButton));

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }
}
