namespace SatoriTeamTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNeedCheckOrFixModelClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NeedCheckOrFixModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ModelDate = c.DateTime(nullable: false),
                        Ontology = c.String(),
                        ModelID = c.Int(nullable: false),
                        VersionID = c.Int(nullable: false),
                        ModelName = c.String(),
                        ModelOwner = c.String(),
                        ModelAlertType = c.Int(nullable: false),
                        IssueSymptom = c.String(),
                        Action = c.String(),
                        CauseAnalysis = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.NeedCheckOrFixModelTeamMembers",
                c => new
                    {
                        NeedCheckOrFixModel_ID = c.Int(nullable: false),
                        TeamMember_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NeedCheckOrFixModel_ID, t.TeamMember_ID })
                .ForeignKey("dbo.NeedCheckOrFixModels", t => t.NeedCheckOrFixModel_ID, cascadeDelete: true)
                .ForeignKey("dbo.TeamMembers", t => t.TeamMember_ID, cascadeDelete: true)
                .Index(t => t.NeedCheckOrFixModel_ID)
                .Index(t => t.TeamMember_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NeedCheckOrFixModelTeamMembers", "TeamMember_ID", "dbo.TeamMembers");
            DropForeignKey("dbo.NeedCheckOrFixModelTeamMembers", "NeedCheckOrFixModel_ID", "dbo.NeedCheckOrFixModels");
            DropIndex("dbo.NeedCheckOrFixModelTeamMembers", new[] { "TeamMember_ID" });
            DropIndex("dbo.NeedCheckOrFixModelTeamMembers", new[] { "NeedCheckOrFixModel_ID" });
            DropTable("dbo.NeedCheckOrFixModelTeamMembers");
            DropTable("dbo.NeedCheckOrFixModels");
        }
    }
}
