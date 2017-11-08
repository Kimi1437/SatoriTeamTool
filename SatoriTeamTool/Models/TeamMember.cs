using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatoriTeamTool.Models
{
    public class TeamMember
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Mail { get; set; }
        public int Tasks { get; set; }

        public virtual ICollection<NeedCheckOrFixModel> NeedCheckOrFixModels { get; set; }
    }
    public class SatoriDBContext : DbContext
    {
        public DbSet<TeamMember> TeamMembers { get; set; }

        public DbSet<NeedCheckOrFixModel> NeedCheckOrFixModels { get; set; }
    }
}
