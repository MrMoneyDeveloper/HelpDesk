namespace C8.eServices.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class account_update : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.AccountInformations", "CreatedBySystemUserId", "dbo.SystemUsers");
            //DropForeignKey("dbo.AccountInformations", "ModifiedBySystemUserId", "dbo.SystemUsers");
            //DropForeignKey("dbo.AccountTypes", "CreatedBySystemUserId", "dbo.SystemUsers");
            //DropForeignKey("dbo.AccountTypes", "ModifiedBySystemUserId", "dbo.SystemUsers");
            //DropIndex("dbo.AccountInformations", new[] { "CreatedBySystemUserId" });
            //DropIndex("dbo.AccountInformations", new[] { "ModifiedBySystemUserId" });
            //DropIndex("dbo.AccountTypes", new[] { "CreatedBySystemUserId" });
            //DropIndex("dbo.AccountTypes", new[] { "ModifiedBySystemUserId" });
            //DropTable("dbo.AccountInformations");
            //DropTable("dbo.AccountTypes");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.AccountTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100),
            //            Description = c.String(maxLength: 500),
            //            Key = c.String(maxLength: 100),
            //            IsActive = c.Boolean(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            IsLocked = c.Boolean(),
            //            CreatedBySystemUserId = c.Int(),
            //            CreatedDateTime = c.DateTime(),
            //            ModifiedBySystemUserId = c.Int(),
            //            ModifiedDateTime = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.AccountInformations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            AccountNumber = c.String(maxLength: 32),
            //            DisconnectionRequireAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Ownership = c.String(maxLength: 1),
            //            DisconnectionStatus = c.String(maxLength: 32),
            //            DisconnectionDate = c.DateTime(nullable: false),
            //            IndigentStatus = c.String(maxLength: 32),
            //            TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            AdvanceAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            TotalBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            DateUpdated = c.DateTime(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            IsDeleted = c.Boolean(nullable: false),
            //            IsLocked = c.Boolean(),
            //            CreatedBySystemUserId = c.Int(),
            //            CreatedDateTime = c.DateTime(),
            //            ModifiedBySystemUserId = c.Int(),
            //            ModifiedDateTime = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.AccountTypes", "ModifiedBySystemUserId");
            //CreateIndex("dbo.AccountTypes", "CreatedBySystemUserId");
            //CreateIndex("dbo.AccountInformations", "ModifiedBySystemUserId");
            //CreateIndex("dbo.AccountInformations", "CreatedBySystemUserId");
            //AddForeignKey("dbo.AccountTypes", "ModifiedBySystemUserId", "dbo.SystemUsers", "Id");
            //AddForeignKey("dbo.AccountTypes", "CreatedBySystemUserId", "dbo.SystemUsers", "Id");
            //AddForeignKey("dbo.AccountInformations", "ModifiedBySystemUserId", "dbo.SystemUsers", "Id");
            //AddForeignKey("dbo.AccountInformations", "CreatedBySystemUserId", "dbo.SystemUsers", "Id");
        }
    }
}
