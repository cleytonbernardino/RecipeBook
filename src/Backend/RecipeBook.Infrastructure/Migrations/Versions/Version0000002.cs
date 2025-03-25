using FluentMigrator;
using System.Data;

namespace RecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_RECIPES, "Create table to save  the recipes' information")]
    public class Version0000002 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Recipes")
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("Fk_Recipe_User_Id", "Users", "ID")
                .WithColumn("LastUpdate").AsDateTime().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("CookingTime").AsInt32().Nullable()
                .WithColumn("Difficulty").AsInt32().Nullable()
            ;

            CreateTable("Ingredients")
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("Fk_Ingredient_Recipe_Id", "Recipes", "ID").OnDelete(Rule.Cascade)
            ;

            CreateTable("Intructions")
                .WithColumn("Step").AsInt32().NotNullable()
                .WithColumn("Text").AsString(2000).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("Fk_Intruction_Recipe_Id", "Recipes", "ID").OnDelete(Rule.Cascade)
            ;

            CreateTable("DishTypes")
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("Fk_DishType_Recipe_Id", "Recipes", "ID").OnDelete(Rule.Cascade)
            ;
        }
    }
}
