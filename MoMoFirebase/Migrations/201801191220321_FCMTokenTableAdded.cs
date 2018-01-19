namespace MoMoFirebase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FCMTokenTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FCMTokens",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeviceType = c.String(maxLength: 50),
                        FcmTokenValue = c.String(maxLength: 255),
                        Active = c.Boolean(nullable: false),
                        CreatedUtc = c.DateTime(nullable: false),
                        ModifiedUtc = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FCMTokens");
        }
    }
}
