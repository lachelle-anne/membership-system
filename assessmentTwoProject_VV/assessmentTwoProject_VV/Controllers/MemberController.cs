using assessmentTwoProject_VV.Data;
using assessmentTwoProject_VV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace assessmentTwoProject_VV.Controllers
{
    public class MemberController : Controller
    {
        private readonly ILogger<MemberController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public MemberController(ApplicationDbContext dbContext, ILogger<MemberController> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var members = await _dbContext.Members.ToArrayAsync();
            List<Models.Member.Details> modelMembers = new List<Models.Member.Details>();
            _logger.LogInformation("Retrieving member entries " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
            foreach (var m in members)
            {
                var _member = new Models.Member.Details
                {
                    Id = m.Id,
                    Name = m.Name,
                    Surname = m.Surname,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    DOB = m.DOB
                };
                modelMembers.Add(_member);
            }
            _logger.LogInformation("Displaying members on the Member Deatils page " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
            return View(modelMembers);
        }

        [HttpGet]
        public async Task<IActionResult> Add(int? id)
        {
            if (id != null)
            {
                
                var m = await _dbContext.Members.Where(p => p.Id == id).FirstOrDefaultAsync();
                var member = new Models.Member.Member
                {
                    Name = m.Name,
                    Surname = m.Surname,
                    Email = m.Email,
                    PhoneNumber = m.PhoneNumber,
                    DOB = m.DOB
                };
                return View(member);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([Bind("Id, Name, Surname, DOB, Email, PhoneNumber")] Member Member)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model is valid " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
                var person = await _dbContext.Members.Where(p => p.Id == Member.Id).FirstOrDefaultAsync();
                if (person != null)
                {
                    person.Name = Member.Name;
                    person.Surname = Member.Surname;
                    person.Email = Member.Email;
                    person.PhoneNumber = Member.PhoneNumber;
                    person.DOB = Member.DOB;
                    
                    _dbContext.Members.Update(person);
                    await _dbContext.SaveChangesAsync();
                    
                }
                else
                {
                    Debug.Assert(person == null, "person is not found in the database");
                    _dbContext.Members.Add(Member);
                    _logger.LogInformation("Member added " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("Member added, redirecting to Member Details page " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
                }
                FileInfo file = new FileInfo("./debug-log.txt");
                var writer = file.CreateText();
                using (var sw = writer)
                {
                    sw.WriteLine("Member has been added into the database" + " " + Member.Name + " " + Member.Surname);
                    sw.Close();
                }
                    return RedirectToAction("Details", "Member");
            }
            _logger.LogInformation("Model is invalid, redirecting to Add Member page  " + DateTime.Now.ToString("ddd d MMM - hh:mm tt"));
            return View();
        }

        public async Task<IActionResult> Confirm(int id)
        {
            var person = await _dbContext.Members.Where(p => p.Id == id).FirstOrDefaultAsync();
            var p = new Models.Member.Details
            {
                Id = person.Id,
                Name = person.Name,
                Surname = person.Surname,
                Email = person.Email,
                PhoneNumber = person.PhoneNumber,
                DOB = person.DOB
            };
            return View(p);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var person = await _dbContext.Members.Where(p => p.Id == id).FirstOrDefaultAsync();
            _dbContext.Members.Remove(person);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Details", "Member");
        }



       [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
