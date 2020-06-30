using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Switcheroo",
    Author = "David Hayden",
    Website = "https://www.davidhayden.me",
    Version = "1.0.0-rc2",
    Description = "Dynamically select active theme for current session using query parameter. e.g. ?theme=TheAgencyTheme",
    Category = "Development",
    Dependencies = new[]
    {
        "OrchardCore.Themes"
    }
)]