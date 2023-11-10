namespace C8.eServices.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Prop : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Priorities", "MinTime", c => c.Int(nullable: false));
            AlterColumn("dbo.Priorities", "MaxTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Priorities", "MaxTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Priorities", "MinTime", c => c.DateTime(nullable: false));
        }
    }
}
