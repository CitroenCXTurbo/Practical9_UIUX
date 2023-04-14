
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

        if (student is null)
        {

            Alert("Student not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
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
        var exists = svc.GetStudentByEmail(s.Email);
        if (exists is not null)
        {
            ModelState.AddModelError(nameof(s.Email), "The email address is already in use");
        }

        // complete POST action to add student
        if (ModelState.IsValid)
        {
            // call service AddStudent method using data in s
            var student = svc.AddStudent(s);
            if (student is null) // if the student cant be added for whatever reason
            {
                // TBC - Q2 replace with Alert and Redirect

                Alert($"Student could not be created, possible duplicate email", AlertType.warning);
                return RedirectToAction(nameof(Index)); // if the student cant be created redirect back to the index
            }
            else
            {
                Alert($"Student created successfully", AlertType.success); //nice wee alert if it works
            }
            return RedirectToAction(nameof(Details), new { Id = student.Id }); //if the student could be created redirect to view the student 
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

            Alert($"No such student with the id of {id}", AlertType.info);
            return RedirectToAction(nameof(Index));
        }

        // pass student to view for editing
        return View(student);
    }

    // POST /student/edit/{id}
    [HttpPost]
    public IActionResult Edit(int id, Student s)
    {
        // TBC - Q1 add validation error if email exists and is not owned by student being edited 
        var exists = svc.GetStudentByEmail(s.Email);
        if (exists != null && exists.Id != s.Id) //if the id of the student who owned the email is not the same student who we are currently trying to save then that is a problem
        {
            ModelState.AddModelError(nameof(s.Email), "The email address is already in use");
        }

        // complete POST action to save student changes
        if (ModelState.IsValid)
        {
            var student = svc.UpdateStudent(s);
            // TBC - Q2 Add alert when update failed
            Alert($"Invald Parameters", AlertType.info);
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
            Alert($"Student with id of {id} not found", AlertType.info);
            return RedirectToAction(nameof(Index));
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
            // Alert($"No such student with the id of {id}", AlertType.info);
            // return RedirectToAction(nameof(Index));
            return NotFound();
        } 

        // create a ticket view model and set foreign key
        var ticket = new Ticket { StudentId = id };
        // render blank form
        return View(ticket);
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
        return View(ticket);
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