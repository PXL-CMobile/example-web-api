﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ExampleWebApi.Domain;

public class User : IdentityUser<Guid>
{
    [Required]
    public string NickName { get; set; }
}