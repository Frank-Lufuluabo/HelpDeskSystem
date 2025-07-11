using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using HelpDeskSystem.ClaimsManagement;
using HelpDeskSystem.Data;
using HelpDeskSystem.Data.Migrations;
using HelpDeskSystem.Interfaces;
using HelpDeskSystem.Models;
using HelpDeskSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Controllers
{
    public class ExcelExportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;

        public ExcelExportsController(ApplicationDbContext context, IExportService exportService, IConfiguration configuration,
            IMapper imapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = imapper;
            _exportService = exportService; 
        }
      
        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportTicketsList(TicketViewModel vm)
        {

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Title))
                {
                    alltickets = alltickets.Where(x => x.Title == vm.Title);
                }

                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    alltickets = alltickets.Where(x => x.CreatedById == vm.CreatedById);
                }

                if (vm.StatusId > 0)
                {
                    alltickets = alltickets.Where(x => x.StatusId == vm.StatusId);
                }

                if (vm.PriorityId > 0)
                {
                    alltickets = alltickets.Where(x => x.PriorityId == vm.PriorityId);
                }

                if (vm.CategoryId > 0)
                {
                    alltickets = alltickets.Where(x => x.SubCategory.CategoryId == vm.CategoryId);
                }
            }

           var data = await alltickets.Select(x => new 
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               CreatedOn = x.CreatedOn,
               UserName = x.CreatedBy.FullName,
               Status = x.Status.Description,
               Priority = x.Priority.Description,
               Category = x.SubCategory.Category.Name,
               SubCategory = x.SubCategory.Name,
               Comments = x.TicketComments.Count
           }).ToListAsync();

            return _exportService.ExportToExcel(data, "RecentTicketsList");

        }


        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportAssignedTicketsList(TicketViewModel vm)
        {
            var assignedStatus = await _context
            .SystemCodeDetails
            .Include(x => x.SystemCode)
            .Where(x => x.SystemCode.Code == "ResolutionStatus" && x.Code == "Assigned")
            .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .Where(x => x.StatusId == assignedStatus.Id)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Title))
                {
                    alltickets = alltickets.Where(x => x.Title == vm.Title);
                }

                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    alltickets = alltickets.Where(x => x.CreatedById == vm.CreatedById);
                }

                if (vm.StatusId > 0)
                {
                    alltickets = alltickets.Where(x => x.StatusId == vm.StatusId);
                }

                if (vm.PriorityId > 0)
                {
                    alltickets = alltickets.Where(x => x.PriorityId == vm.PriorityId);
                }

                if (vm.CategoryId > 0)
                {
                    alltickets = alltickets.Where(x => x.SubCategory.CategoryId == vm.CategoryId);
                }
            }

            var data = await alltickets.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedOn = x.CreatedOn,
                UserName = x.CreatedBy.FullName,
                Status = x.Status.Description,
                Priority = x.Priority.Description,
                Category = x.SubCategory.Category.Name,
                SubCategory = x.SubCategory.Name,
                Comments = x.TicketComments.Count
            }).ToListAsync();

            return _exportService.ExportToExcel(data, "AssignedTicketsList");

        }


        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportClosedTicketsList(TicketViewModel vm)
        {
            var closedStatus = await _context
            .SystemCodeDetails
            .Include(x => x.SystemCode)
            .Where(x => x.SystemCode.Code == "ResolutionStatus" && x.Code == "Closed")
            .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .Where(x => x.StatusId == closedStatus.Id)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Title))
                {
                    alltickets = alltickets.Where(x => x.Title == vm.Title);
                }

                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    alltickets = alltickets.Where(x => x.CreatedById == vm.CreatedById);
                }

                if (vm.StatusId > 0)
                {
                    alltickets = alltickets.Where(x => x.StatusId == vm.StatusId);
                }

                if (vm.PriorityId > 0)
                {
                    alltickets = alltickets.Where(x => x.PriorityId == vm.PriorityId);
                }

                if (vm.CategoryId > 0)
                {
                    alltickets = alltickets.Where(x => x.SubCategory.CategoryId == vm.CategoryId);
                }
            }

            var data = await alltickets.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedOn = x.CreatedOn,
                UserName = x.CreatedBy.FullName,
                Status = x.Status.Description,
                Priority = x.Priority.Description,
                Category = x.SubCategory.Category.Name,
                SubCategory = x.SubCategory.Name,
                Comments = x.TicketComments.Count
            }).ToListAsync();

            return _exportService.ExportToExcel(data, "ClosedTicketsList");

        }



        [Permission("TICKETS:VIEW")]
        public async Task<IActionResult> ExportResolvedTicketsList(TicketViewModel vm)
        {
            var resolvedStatus = await _context
            .SystemCodeDetails
            .Include(x => x.SystemCode)
            .Where(x => x.SystemCode.Code == "ResolutionStatus" && x.Code == "Resolved")
            .FirstOrDefaultAsync();

            var alltickets = _context.Tickets
                .Include(t => t.CreatedBy)
                .Include(t => t.SubCategory)
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.TicketComments)
                .OrderBy(x => x.CreatedOn)
                .Where(x => x.StatusId == resolvedStatus.Id)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Title))
                {
                    alltickets = alltickets.Where(x => x.Title == vm.Title);
                }

                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    alltickets = alltickets.Where(x => x.CreatedById == vm.CreatedById);
                }

                if (vm.StatusId > 0)
                {
                    alltickets = alltickets.Where(x => x.StatusId == vm.StatusId);
                }

                if (vm.PriorityId > 0)
                {
                    alltickets = alltickets.Where(x => x.PriorityId == vm.PriorityId);
                }

                if (vm.CategoryId > 0)
                {
                    alltickets = alltickets.Where(x => x.SubCategory.CategoryId == vm.CategoryId);
                }
            }

            var data = await alltickets.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CreatedOn = x.CreatedOn,
                UserName = x.CreatedBy.FullName,
                Status = x.Status.Description,
                Priority = x.Priority.Description,
                Category = x.SubCategory.Category.Name,
                SubCategory = x.SubCategory.Name,
                Comments = x.TicketComments.Count
            }).ToListAsync();

            return _exportService.ExportToExcel(data, "ResolvedTicketsList");

        }



        [Permission("COMMENTS:VIEW")]
        public async Task<IActionResult> ExportTicketComments(CommentViewModel vm)
        {

            var allcomments = _context.Comments
                 .Include(c => c.CreatedBy)
                 .Include(c => c.Ticket)
                 .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Description))
                {
                    allcomments = allcomments.Where(x => x.Description.Contains(vm.Description));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    allcomments = allcomments.Where(x => x.CreatedById == vm.CreatedById);
                }
            }

            var comments = await allcomments.Select(x => new
            {
                Id = x.Id,
                Ticket = x.Ticket.Title,
                Comment = x.Description,
                CreatedBy = x.CreatedBy.FullName,
                CreatedOn = x.CreatedOn
            }).ToListAsync();


            return _exportService.ExportToExcel(comments, "TicketsCommentsList");
            
        }


        [Permission("CATEGORIES:VIEW")]
        public async Task<IActionResult> ExportTicketCategories(TicketCategoryViewModel vm)
        {
            var ticketCategories = _context.TicketCategories
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Code))
                {
                    ticketCategories = ticketCategories.Where(x => x.Code.Contains(vm.Code));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    ticketCategories = ticketCategories.Where(x => x.CreatedById == vm.CreatedById);
                }
                if (!string.IsNullOrEmpty(vm.Name))
                {
                    ticketCategories = ticketCategories.Where(x => x.Name == vm.Name);
                }
            }

            var TicketCategories = await ticketCategories.Select(x => new
            {
                Id= x.Id,
                CategoryCode = x.Code,
                CategoryName = x.Name,
                CreatedBy = x.CreatedBy.FullName,
                CreatedOn = x.CreatedOn,

            }).ToListAsync();


            return _exportService.ExportToExcel(TicketCategories, "TicketCatgoriesList");
        }


        [Permission("SUBCATEGORIES:VIEW")]
        public async Task<IActionResult> ExportTicketSubCategories(int id, TicketSubCategoriesVM vm)
        {


            var ticketSubCategories = _context.TicketSubCategories
                 .Include(t => t.Category)
                 .Include(t => t.CreatedBy)
                 .Include(t => t.ModifiedBy)
                 .Where(x => x.CategoryId == id)
                 .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Code))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Code.Contains(vm.Code));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CreatedById == vm.CreatedById);
                }
                if (!string.IsNullOrEmpty(vm.Name))
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.Name == vm.Name);
                }

                if (vm.CategoryId > 0)
                {
                    ticketSubCategories = ticketSubCategories.Where(x => x.CategoryId == vm.CategoryId);
                }
            }

            var TicketSubCategories = await ticketSubCategories.Select(x => new
            {
                Id = x.Id,
                SubCategoryCode = x.Code,
                SubCategoryName = x.Name,
                CategoryName = x.Category.Name,
                CreatedBy = x.CreatedBy.FullName,
                CreatedOn = x.CreatedOn,

            }).ToListAsync();


            return _exportService.ExportToExcel(TicketSubCategories, "TicketSubCatgoriesList");

        }

        public async Task<IActionResult> ExportAuditTrails(AuditTrailViewModel vm)
        {
           var AuditTrails = await _context.AuditTrails.Include(a => a.User)
               .Select(x => new
                 {
                   Action = x.Action,
                   NewValues = x.NewValues,
                   OldValues = x.OldValues,
                   CreatedBy = x.User.FullName,
                   CreatedOn = x.TimeStamp,
                   AffectedColumns = x.AffectedColumns,
                   AffectedTable = x.AffectedTable,
                   PrimaryKey = x.PrimaryKey,
                   Module = x.Module
               }).ToListAsync();


            return _exportService.ExportToExcel(AuditTrails, "AuditTrailsList");
        }

        public async Task<ActionResult> ExportUsers(ApplicationUserViewModel vm)
        {
          var ApplicationUsers = await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Gender)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    FullName = x.FullName,
                    Gender = x.Gender.Description,
                    RoleName = x.Role.Name,
                    EmailAddress = x.Email,
                    Country = x.Country,
                    City = x.City,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName
                }).ToListAsync();


            return _exportService.ExportToExcel(ApplicationUsers, "UsersList");
        }

        [Permission("SYSTEMCODES:VIEW")]
        public async Task<IActionResult> ExportSystemCodes(SystemCodeViewModel vm)
        {
            var systemcodes = _context
                 .SystemCodes
                 .Include(x => x.CreatedBy)
                 .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Code))
                {
                    systemcodes = systemcodes.Where(x => x.Code.Contains(vm.Code));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    systemcodes = systemcodes.Where(x => x.CreatedById == vm.CreatedById);
                }
                if (!string.IsNullOrEmpty(vm.Description))
                {
                    systemcodes = systemcodes.Where(x => x.Description == vm.Description);
                }
            }


           var allsystemCodesdetails = await systemcodes
                .Include(x=>x.CreatedBy)
                .Select(x => new
                {
                    Code = x.Code,
                    Description = x.Description,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn
                }).ToListAsync();


            return _exportService.ExportToExcel(allsystemCodesdetails, "SystemCodesList");
        }


        [Permission("SYSTEMCODEDETAILS:VIEW")]
        public async Task<IActionResult> ExportSystemCodedetails(SystemCodeDetailViewModel vm)
        {
            var systemCodeDetails = _context
                .SystemCodeDetails
                .Include(s => s.SystemCode)
                .Include(s => s.CreatedBy)
                .AsQueryable();

            if (vm != null)
            {
                if (!string.IsNullOrEmpty(vm.Code))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.Code.Contains(vm.Code));
                }
                if (!string.IsNullOrEmpty(vm.CreatedById))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.CreatedById == vm.CreatedById);
                }
                if (!string.IsNullOrEmpty(vm.Description))
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.Description == vm.Description);
                }

                if (vm.SystemCodeId > 0)
                {
                    systemCodeDetails = systemCodeDetails.Where(x => x.SystemCodeId == vm.SystemCodeId);
                }
            }


            var allSystemCodeDetails = await systemCodeDetails
                .Select(x => new
                {
                    Code = x.Code,
                    Description = x.Description,
                    SystemCode = x.SystemCode.Description,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn
                }).ToListAsync();

            return _exportService.ExportToExcel(allSystemCodeDetails, "SystemCodeDetailsList");
        }


        [Permission("ROLES:VIEW")]
        public async Task<ActionResult> ExportSystemRoles()
        {
            var roles = await _context.Roles.Select(x => new
            {
                RoleId = x.Id,
                RoleName = x.Name
            }).ToListAsync();

            return _exportService.ExportToExcel(roles, "RolesList");
        }

        [Permission("DEPARTMENTS:VIEW")]
        public async Task<IActionResult> ExportDepartments()
        {
            var alldepartments =  await _context.Departments
                .Include(d => d.CreatedBy)
                .Include(d => d.ModifiedBy)
               .Select(x => new
                {
                    DepartmentCode = x.Code,
                    DepartmentName = x.Name,
                    CreatedBy = x.CreatedBy.FullName,
                    CreatedOn = x.CreatedOn,
               }).ToListAsync();

            return _exportService.ExportToExcel(alldepartments, "DepartmentList");

        }
    }
}
