namespace SandwhichMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEmplyeeID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "EmployeeID");
        }
    }
}
