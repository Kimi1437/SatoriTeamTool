using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoriTeamTool.ViewModel
{
    public class NeedCheckViewModel
    {
        public int ID { get; set; }
        public string ModelDate { get; set; }
        public string Ontology { get; set; }
        public int ModelID { get; set; }
        public int VersionID { get; set; }
        public string ModelName { get; set; }
        public string ModelOwner { get; set; }
        public string ModelAlertType { get; set; }
        public string IssueSymptom { get; set; }
        public string Action { get; set; }
        public string CauseAnalysis { get; set; }

    }
}
