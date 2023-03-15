using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Board.Data;
using Board.Models;
using System.Diagnostics;

namespace Board.Controllers
{
    public class HomeController : Controller
    {
        private readonly BoardContext _context;
        public HomeController(BoardContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.Notice != null ?
                         View(await _context.Notice.ToListAsync()) :
                         Problem("Entity set 'BoardContext.Notice'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}