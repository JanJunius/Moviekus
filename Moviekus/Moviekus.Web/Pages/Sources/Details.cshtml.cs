﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moviekus.EntityFramework;
using Moviekus.Models;
using Moviekus.Services;

namespace Moviekus.Web.Pages.Sources
{
    public class DetailsModel : PageModel
    {
        private readonly Moviekus.EntityFramework.MoviekusDbContext _context;

        private SourceService SourceService = new SourceService();

        public DetailsModel(Moviekus.EntityFramework.MoviekusDbContext context)
        {
            _context = context;
        }

        public Source Source { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            Source = await SourceService.GetAsync(id);

            if (Source == null)
                return NotFound();

            return Page();
        }
    }
}