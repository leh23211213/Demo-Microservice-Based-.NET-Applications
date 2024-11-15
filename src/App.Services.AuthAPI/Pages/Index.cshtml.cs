// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Reflection;
using App.Services.AuthAPI.Pages.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.AuthAPI.Pages.Home;

[AllowAnonymous]
public class Index : PageModel
{
    public InputModel Input { get; set; } = default!;

    public async Task<IActionResult> OnGet()
    {
        Input = new InputModel { };
        return Page();
    }

    public string Version
    {
        get => typeof(Duende.IdentityServer.Hosting.IdentityServerMiddleware).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion.Split('+').First()
            ?? "unavailable";
    }
}
