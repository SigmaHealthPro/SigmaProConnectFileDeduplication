using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaProConnectDeDuplication.Data
{
    public partial class County
    {
        public Guid Id { get; set; }

        public int CountyId { get; set; }

        public string? CountyName { get; set; }

        public string? CountyCode { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool? Isdelete { get; set; }

        public Guid? StateId { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

        public virtual ICollection<City> Cities { get; set; } = new List<City>();

        public virtual State? State { get; set; }
    }
}
