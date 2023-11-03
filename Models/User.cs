using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Ch11Lab.Models;

public class User 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required]
    [RegularExpression("\\d{3}\\-\\d{3}\\-\\d{4}", ErrorMessage = "Not correct phone number format, ###-###-####")]
    public string? Phone { get; set; }
    [Required]
    [DisplayName("Unique E-Mail Address")] 
    [Remote("CheckEmail", "Home")]
    public string? Email { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? RegionCode { get; set; }
    public string? PostalCode { get; set; }
}