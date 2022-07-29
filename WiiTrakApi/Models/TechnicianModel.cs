/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Cores;
namespace WiiTrakApi.Models
{
    [Table(name: "Technicians")]
    public class TechnicianModel : EntityModel
    {
        [MaxLength(Numbers.OneTwoEight)]
        [Required(ErrorMessage = "{0} is required.")]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(Numbers.OneTwoEight)]
        [Required(ErrorMessage = "{0} is required.")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Phone number is invalid.")]
        public string Phone { get; set; } = string.Empty;
        [Url(ErrorMessage = "{0} is invalid.")]
        public string ProfilePic { get; set; } = string.Empty;
        [ForeignKey(nameof(SystemOwnerModel))]
        public Guid? SystemOwnerId { get; set; }
        public SystemOwnerModel? SystemOwner { get; set; }
        [ForeignKey(nameof(CompanyModel))]
        public Guid? CompanyId { get; set; }
        public CompanyModel? Company { get; set; }
    }
}
