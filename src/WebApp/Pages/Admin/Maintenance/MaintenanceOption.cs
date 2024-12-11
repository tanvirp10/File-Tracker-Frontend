namespace MyApp.WebApp.Pages.Admin.Maintenance;

public class MaintenanceOption
{
    public string SingularName { get; private init; } = string.Empty;
    public string PluralName { get; private init; } = string.Empty;
    public bool StartsWithVowelSound { get; private init; }

    private MaintenanceOption() { }

    public static MaintenanceOption EntryType { get; } =
        new() { SingularName = "Work Entry Type", PluralName = "Work Entry Types", StartsWithVowelSound = false };

    public static MaintenanceOption Office { get; } =
        new() { SingularName = "Office", PluralName = "Offices", StartsWithVowelSound = true };
}
