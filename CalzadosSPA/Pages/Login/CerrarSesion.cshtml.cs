using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfraestructuraTransversal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CalzadosSPA.Pages.Login
{
    public class CerrarSesionModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("MiCookieDeAutenticacion");
            Cache.Instance.LiberarCache();
            return RedirectToPage("/Index");
        }
    }
}
