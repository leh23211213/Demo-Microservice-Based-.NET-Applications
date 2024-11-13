// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Pages.Login;

public class InputModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
    public string? Password { get; set; }
    public bool RememberLogin { get; set; }
    public string? ReturnUrl { get; set; }
    public string? Button { get; set; }

    [Required(ErrorMessage = "Security code is required.")]
    [StringLength(4, ErrorMessage = "The code must be exactly 4 digits.", MinimumLength = 4)]
    public string? EnteredCode { get; set; }

    // The code that will be generated and compared during validation.
    [HiddenInput]
    public string? GeneratedCode { get; set; }
}