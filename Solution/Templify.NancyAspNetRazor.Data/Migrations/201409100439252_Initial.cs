namespace Templify.NancyAspNetRazor.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        UserName = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.RoleClaims",
                c => new
                    {
                        Role_Name = c.String(nullable: false, maxLength: 128),
                        Claim_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Role_Name, t.Claim_Name })
                .ForeignKey("dbo.Roles", t => t.Role_Name, cascadeDelete: true)
                .ForeignKey("dbo.Claims", t => t.Claim_Name, cascadeDelete: true)
                .Index(t => t.Role_Name)
                .Index(t => t.Claim_Name);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        User_UserId = c.Guid(nullable: false),
                        Claim_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Claim_Name })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Claims", t => t.Claim_Name, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Claim_Name);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_UserId = c.Guid(nullable: false),
                        Role_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Role_Name })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Name, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Role_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "Role_Name", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "Claim_Name", "dbo.Claims");
            DropForeignKey("dbo.UserClaims", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.RoleClaims", "Claim_Name", "dbo.Claims");
            DropForeignKey("dbo.RoleClaims", "Role_Name", "dbo.Roles");
            DropIndex("dbo.UserRoles", new[] { "Role_Name" });
            DropIndex("dbo.UserRoles", new[] { "User_UserId" });
            DropIndex("dbo.UserClaims", new[] { "Claim_Name" });
            DropIndex("dbo.UserClaims", new[] { "User_UserId" });
            DropIndex("dbo.RoleClaims", new[] { "Claim_Name" });
            DropIndex("dbo.RoleClaims", new[] { "Role_Name" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserClaims");
            DropTable("dbo.RoleClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Claims");
        }
    }
}
