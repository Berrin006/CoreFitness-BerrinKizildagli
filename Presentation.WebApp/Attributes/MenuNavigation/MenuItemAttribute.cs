namespace Presentation.WebApp.Attributes.MenuNavigation;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MenuItemAttribute : Attribute
{
    public string Title { get; set; } = string.Empty;
    public int Order { get; set; } = 1000;

    public MenuItemAttribute()
    {
    }

    public MenuItemAttribute(string title, int order = 1000)
    {
        Title = title;
        Order = order;
    }
}