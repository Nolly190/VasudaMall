namespace VasudaMall.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFullName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
