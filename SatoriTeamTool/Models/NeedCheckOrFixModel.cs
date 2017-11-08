 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoriTeamTool.Models
{
    public class NeedCheckOrFixModel
    {
        public int ID { get; set; }
        public DateTime ModelDate { get; set; }
        public string Ontology { get; set; }
        public int ModelID { get; set; }
        public int VersionID { get; set; }
        public string ModelName { get; set; }
        public string ModelOwner { get; set; } 
        public AlertType ModelAlertType { get; set; }
        public string IssueSymptom { get; set; }
        public string Action { get; set; }
        public string CauseAnalysis { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set;}

    }

    public enum AlertType{
        Extraction,
        Regression,
        Warning,
        Verification
    }
}
