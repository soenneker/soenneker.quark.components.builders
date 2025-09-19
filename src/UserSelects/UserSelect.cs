using Soenneker.Quark.Enums.GlobalKeywords;

namespace Soenneker.Quark.Components.Builders.UserSelects;

public static class UserSelect
{
    public static UserSelectBuilder None => new(Enums.UserSelects.UserSelect.NoneValue);
    public static UserSelectBuilder Auto => new(Enums.UserSelects.UserSelect.AutoValue);
    public static UserSelectBuilder All => new(Enums.UserSelects.UserSelect.AllValue);

    public static UserSelectBuilder Inherit => new(GlobalKeyword.InheritValue);
    public static UserSelectBuilder Initial => new(GlobalKeyword.InitialValue);
    public static UserSelectBuilder Revert => new(GlobalKeyword.RevertValue);
    public static UserSelectBuilder RevertLayer => new(GlobalKeyword.RevertLayerValue);
    public static UserSelectBuilder Unset => new(GlobalKeyword.UnsetValue);
}
