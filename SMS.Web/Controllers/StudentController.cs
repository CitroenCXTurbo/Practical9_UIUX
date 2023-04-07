
using Microsoft.AspNetCore.Mvc;

using SMS.Data.Entities;
using SMS.Data.Services;

namespace SMS.Web.Controllers;
public class StudentController : BaseController
{
    private IStudentService svc;

    public StudentController()
    {
        svc = new StudentServiceDb();            
    }

    // GET /student
    public IActionResult Index()
    {
        // load students using service and pass to view
        var data = svc.GetStudents();
        
        return View(data);
    }

    // GET /student/details/{id}
    public IActionResult Details(int id)
    {
        var student = svc.GetStudent(id);
      
        if (student is null) {
            // TBC - Q2 replace with Alert and Redirect
            return NotFound();
        }
        return View(student);
    }

    // GET: /student/create
    public IActionResult Create()
    {
        // display blank form to create a student
        return View();
    }

    // POST /student/create
    [HttpPost]
    public IActionResult Create(Student s)
    {   
        // TBC - Q1 validate email is unique
        

        // complete POST action to add student
        if (ModelState.IsValid)
        {
            // call service AddStudent method using data in s
            var student = svc.AddStudent(s);
            if (student is null) 
            {
                // TBC - Q2 replace with Alert and Redirect
                return NotFound();
            }
            return RedirectToAction(nameof(Details), new { Id = student.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(s);
    }

    // GET /student/edit/{id}
    public IActionResult Edit(int id)
    {
        // load the student using the service
        var student = svc.GetStudent(id);

        // check if student is null
        if (student is null)
        {
            // TBC - Q2 replace with Alert and Redirect
            return NotFound();
        }  

        // pass student to view for editing
        return View(student);
    }

    // POST /student/edit/{id}
    [HttpPost]
    public IActionResult Edit(int id, Student s)
    {
        // TBC - Q1 add validation error if email exists and is not owned by student being edited 
        

        // complete POST action to save student changes
        if (ModelState.IsValid)
        {            
            var student = svc.UpdateStudent(s);
            // TBC - Q2 Add alert when update failed

            // redirect back to view the student details
            return RedirectToAction(nameof(Details), new { Id = s.Id });
        }

        // redisplay the form for editing as validation errors
        return View(s);
    }

    // GET / student/delete/{id}
    public IActionResult Delete(int id)
    {
        // load the student using the service
        var student = svc.GetStudent(id);
        // check the returned student is not null 
        if (student == null)
        {
            // TBC - Q2 replace with Alert and Redirect
            return NotFound();
        }     
        
        // pass student to view for deletion confirmation
        return View(student);
    }

    // POST /student/delete/{id}
    [HttpPost]
    public IActionResult DeleteConfirm(int id)
    {
        // delete student via service
        var deleted = svc.DeleteStudent(id);
        // TBC - Q2 add success / failure Alert
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }

     // ============== Student ticket management ==============

    // GET /student/ticketcreate/{id}
    public IActionResult TicketCreate(int id)
    {
        var student = svc.GetStudent(id);
        if (student == null)
        {
            // TBC - replace with Alert and Redirect
            return NotFound();
        }

        // create a ticket view model and set foreign key
        var ticket = new Ticket { StudentId = id }; 
        // render blank form
        return View( ticket );
    }

    // POST /student/ticketcreate
    [HttpPost]
    public IActionResult TicketCreate(Ticket t)
    {
        if (ModelState.IsValid)
        {                
            var ticket = svc.CreateTicket(t.StudentId, t.Issue); 
            // TBC - Q2 add alert for success/failure

            // redirect to display student - note how Id is passed
            return RedirectToAction(
                nameof(Details), new { Id = ticket.StudentId }
            );
        }
        // redisplay the form for editing
        return View(t);
    }

     // GET /student/ticketedit/{id}
    public IActionResult TicketEdit(int id)
    {
        var ticket = svc.GetTicket(id);
        if (ticket == null)
        {
            // TBC - replace with Alert and Redirect
            return NotFound();
        }        
        return View( ticket );
    }

    // POST /student/ticketedit
    [HttpPost]
    public IActionResult TicketEdit(int id, Ticket t)
    {
        if (ModelState.IsValid)
        {                
            var ticket = svc.UpdateTicket(id, t.Issue);
            // TBC - Q2 add alert for success/failure

            // redirect to display student - note how Id is passed
            return RedirectToAction(
                nameof(Details), new { Id = ticket.StudentId }
            );
        }
        // redisplay the form for editing
        return View(t);
    }

    // GET /student/ticketdelete/{id}
    public IActionResult TicketDelete(int id)
    {
        // load the ticket using the service
        var ticket = svc.GetTicket(id);
        // check the returned Ticket is not null
        if (ticket == null)
        {
            // TBC - Q2 replace with Alert and Redirect
            return NotFound();
        }     
        
        // pass ticket to view for deletion confirmation
        return View(ticket);
    }

    // POST /student/ticketdeleteconfirm/{id}
    [HttpPost]
    public IActionResult TicketDeleteConfirm(int id, int studentId)
    {
        var deleted = svc.DeleteTicket(id);

        // TBC - Q2 add success/failure alert

        // redirect to the student details view
        return RedirectToAction(nameof(Details), new { Id = studentId });
    }

}