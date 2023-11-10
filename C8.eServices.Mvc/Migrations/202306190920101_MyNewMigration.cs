namespace C8.eServices.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyNewMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AccountInformations", "CreatedBySystemUserId", "dbo.SystemUsers");
            DropForeignKey("dbo.AccountInformations", "ModifiedBySystemUserId", "dbo.SystemUsers");
            DropForeignKey("dbo.AccountTypes", "CreatedBySystemUserId", "dbo.SystemUsers");
            DropForeignKey("dbo.AccountTypes", "ModifiedBySystemUserId", "dbo.SystemUsers");
            DropIndex("dbo.AccountInformations", new[] { "CreatedBySystemUserId" });
            DropIndex("dbo.AccountInformations", new[] { "ModifiedBySystemUserId" });
            DropIndex("dbo.AccountTypes", new[] { "CreatedBySystemUserId" });
            DropIndex("dbo.AccountTypes", new[] { "ModifiedBySystemUserId" });
            DropTable("dbo.AccountInformations");
            DropTable("dbo.AccountTypes");
        }

        public override void Down()
        {
        }
    }
}
