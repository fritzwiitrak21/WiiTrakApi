using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Enums;

namespace WiiTrakApi.Models
{
    [Table(name: "CountyCodes")]
    public class CountyCodeModel : EntityModel
    {
        public string CountyName { get; set; } = string.Empty;
        public string CountyCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }

    }
}
