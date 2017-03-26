namespace Gingerstock2.Store
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ClosedQuantity = c.Int(nullable: false),
                        BrokerEmail = c.String(maxLength: 2147483647),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        SellLotTime = c.DateTime(nullable: false),
                        BuyLotTime = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        SellBorkerEmail = c.String(maxLength: 2147483647),
                        BuyBorkerEmail = c.String(maxLength: 2147483647),
                        SellLotId = c.Int(),
                        BuyLotId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lots", t => t.BuyLotId)
                .ForeignKey("dbo.Lots", t => t.SellLotId)
                .Index(t => t.SellLotId)
                .Index(t => t.BuyLotId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "SellLotId", "dbo.Lots");
            DropForeignKey("dbo.Transactions", "BuyLotId", "dbo.Lots");
            DropIndex("dbo.Transactions", new[] { "BuyLotId" });
            DropIndex("dbo.Transactions", new[] { "SellLotId" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Lots");
        }
    }
}
