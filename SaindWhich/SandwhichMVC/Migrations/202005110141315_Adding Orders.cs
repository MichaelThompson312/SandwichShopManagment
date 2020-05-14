namespace SandwhichMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderFirstName = c.String(),
                        OrderLastName = c.String(),
                        OrderEmail = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.StandardItems",
                c => new
                    {
                        StandardItemID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Order_OrderID = c.Int(),
                    })
                .PrimaryKey(t => t.StandardItemID)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .Index(t => t.Order_OrderID);
            
            CreateTable(
                "dbo.AddOns",
                c => new
                    {
                        IngredientID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        StandardItem_StandardItemID = c.Int(),
                    })
                .PrimaryKey(t => t.IngredientID)
                .ForeignKey("dbo.StandardItems", t => t.StandardItem_StandardItemID)
                .Index(t => t.StandardItem_StandardItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StandardItems", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.AddOns", "StandardItem_StandardItemID", "dbo.StandardItems");
            DropIndex("dbo.AddOns", new[] { "StandardItem_StandardItemID" });
            DropIndex("dbo.StandardItems", new[] { "Order_OrderID" });
            DropTable("dbo.AddOns");
            DropTable("dbo.StandardItems");
            DropTable("dbo.Orders");
        }
    }
}
