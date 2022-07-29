/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WiiTrakApi.Cores;
namespace WiiTrakApi.Models
{
    [Table(name: "RepairIssues")]
    public class RepairIssueModel: EntityModel
    {
        [MaxLength(Numbers.TwoFifty)]
        [Required(ErrorMessage = "{0} is required.")]
        public string? Issue { get; set; } = string.Empty;
        [MaxLength(Numbers.Thousand)]
        public string? Description { get; set; } = string.Empty;
    }
}
