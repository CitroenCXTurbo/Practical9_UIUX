using Microsoft.AspNetCore.Mvc;

using SMS.Web.Models;
using SMS.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SMS.Web.Controllers
{
    public class TicketController : BaseController
    {
        private readonly IStudentService svc;
        public TicketController()
        {
            svc = new StudentServiceDb();
        }

        // GET /ticket/index
        public IActionResult Index()
        {
            var tickets = svc.GetOpenTickets();
            return View(tickets);
        }
       
        //  POST /ticket/close/{id}
        [HttpPost]
        public IActionResult Close(int id)
        {
            // TBC - Q5 close ticket via service then check that ticket was closed
            // if not display a warning/error alert otherwise a success alert
             
            var closed = svc.CloseTicket(id);
            if (closed is null)
            {
                Alert ("This ticket was already closed, it can not be closed again", AlertType.warning);
            }
            else
            {
                Alert ("This ticket was closed successfully", AlertType.success);
            }

            return RedirectToAction(nameof(Index));
        }       
        
        // GET /ticket/create
        public IActionResult Create()
        {
            var students = svc.GetStudents();

            var selectListItems = new SelectList(students, "Id", "Name");

                      
            var tvm = new TicketViewModel {
                Students = selectListItems 
                                         
            };
            
            return View(tvm);
        }
       
        // POST /ticket/create
        [HttpPost]
        public IActionResult Create(TicketViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                var ticket = svc.CreateTicket(tvm.StudentId, tvm.Issue); //these paramaters come from the ticket view model
                if (ticket is not null) 
                {
                    Alert($"Ticket Created", AlertType.info);   
                } 
                else 
                {
                    Alert($"Ticket could not be created", AlertType.warning);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tvm);
        }
    }
}

// TBC - Q5 check if modelstate is valid and create ticket, display success alert and redirect to index
            

            // TBC - Q6 before sending viewmodel back (due to validation issues)
            //       repopulate the select list