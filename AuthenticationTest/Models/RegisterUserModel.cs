﻿using System.ComponentModel.DataAnnotations;

public class RegisterUserModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public string Role { get; set; }

    public int? CompanyId { get; set; }
}
