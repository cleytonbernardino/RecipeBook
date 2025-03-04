using FluentMigrator;

namespace RecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USER, "Create table to save the user's information")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable().Unique()
                .WithColumn("Password").AsString(2000).NotNullable()
                .WithColumn("UserIdentifier").AsGuid().NotNullable();
        }
    }
}
