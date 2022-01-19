using System.ComponentModel.DataAnnotations.Schema;

namespace WiiTrakApi.Models
{
    [Table(name: "CompanyCorporates")]
    public class CompanyCorporateModel
    {
        public Guid CompanyId { get; set; }

        public Guid CorporateId { get; set; }

        public CompanyModel? Company { get; set; }

        public CorporateModel? Corporate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
