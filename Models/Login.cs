using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User   
{
    [Key]
    public int id {get;set;}
    
    [Required]
    [MinLength(2)]
    public string FirstName {get;set;}

    [Required]
    [MinLength(2)]
    public string LastName {get;set;}

    [Required]
    [EmailAddress]
    public string Email {get;set;}

    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
    public string Password {get;set;}

    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string Confirm {get;set;}
}

public class LoginUser 
{
    public string Email {get;set;}
    public string Password {get;set;}
}
