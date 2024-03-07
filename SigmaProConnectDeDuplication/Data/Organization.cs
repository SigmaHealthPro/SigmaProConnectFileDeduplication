using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigmaProConnectDeDuplication.Data
{
    public partial class Organization
    {
        public Guid Id { get; set; }

        public string OrganizationsId { get; set; } = null!;

        public string OrganizationName { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public bool Isdelete { get; set; }

        public Guid? JuridictionId { get; set; }

        public Guid? AddressId { get; set; }

        public virtual Address? Address { get; set; }

        public virtual Juridiction? Juridiction { get; set; }
    }
}
