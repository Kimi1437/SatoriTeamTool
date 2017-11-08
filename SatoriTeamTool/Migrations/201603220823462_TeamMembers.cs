namespace SatoriTeamTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamMembers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.NeedCheckOrFixModelTeamMembers", newName: "TeamMemberNeedCheckOrFixModels");
            DropPrimaryKey("dbo.TeamMemberNeedCheckOrFixModels");
            AddPrimaryKey("dbo.TeamMemberNeedCheckOrFixModels", new[] { "TeamMember_ID", "NeedCheckOrFixModel_ID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TeamMemberNeedCheckOrFixModels");
            AddPrimaryKey("dbo.TeamMemberNeedCheckOrFixModels", new[] { "NeedCheckOrFixModel_ID", "TeamMember_ID" });
            RenameTable(name: "dbo.TeamMemberNeedCheckOrFixModels", newName: "NeedCheckOrFixModelTeamMembers");
        }
    }
}
