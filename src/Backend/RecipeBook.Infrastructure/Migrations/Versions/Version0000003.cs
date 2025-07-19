using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.IMAGES_FOR_RECIPES, "Add collumn on recipe table to save images")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        Alter.Table("recipes")
            .AddColumn("ImageIndentifier").AsString().Nullable();
    }
}
