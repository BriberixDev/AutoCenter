using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;

namespace AutoCenter.Web.Pages.Listings
{
    public class EditModel : PageModel
    {
        private readonly AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext _context;

        public EditModel(AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext context)
        {
            _context = context;
        }

    }
}
