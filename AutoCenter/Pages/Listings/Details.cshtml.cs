using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Models;
using AutoCenter.Web.Infrastructure.Data;

namespace AutoCenter.Web.Pages.Listings
{
    public class DetailsModel : PageModel
    {
        private readonly AutoCenterDbContext _context;

        public DetailsModel(AutoCenterDbContext context)
        {
            _context = context;
        }

        
    }
}
