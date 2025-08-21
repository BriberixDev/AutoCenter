using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AutoCenter.Web.Infrastructure.Data;
using AutoCenter.Web.Models;

namespace AutoCenter.Web.Pages.Listings
{
    public class DeleteModel : PageModel
    {
        private readonly AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext _context;

        public DeleteModel(AutoCenter.Web.Infrastructure.Data.AutoCenterDbContext context)
        {
            _context = context;
        }

        
    }
}
