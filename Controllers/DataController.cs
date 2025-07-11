using HelpDeskSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Controllers
{
    public class DataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetTicketSubCategories(int Id)
        {
            try
            {
                 var data = await _context
                    .TicketSubCategories
                    .Where(x =>x.CategoryId ==Id)
                    .OrderBy(c => c.Name)
                    .Select(i => new { Id = i.Id, Name = i.Name })
                    .Distinct()
                    .ToListAsync();

                return Json(data);
            }catch (Exception ex) 
            {
                return Json(new { });
            }
    }
    }
}
