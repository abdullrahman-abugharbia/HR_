using HR.DTOs.Vacations;
using Microsoft.AspNetCore.Mvc;
using HR.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Azure.Core.HttpHeader;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Authorization;

namespace HR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class VacationsController : Controller
    {
        private HrDbContext _dbContext;

        public VacationsController(HrDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll([FromQuery] FilterVacationDto filterDto)
        {
            var data = from vacation in _dbContext.Vacations
                       from employee in _dbContext.Employees.Where(x => x.Id == vacation.EmployeeId)
                       from lookup in _dbContext.Lookups.Where(x=> x.Id == vacation.TypeId).DefaultIfEmpty()
                       where //(filterDto.VacationTypeId == vacation.TypeId) &&
                       (filterDto.EmployeeId == null || vacation.EmployeeId == filterDto.EmployeeId) &&
                       (filterDto.VacationTypeId == null || vacation.TypeId == filterDto.VacationTypeId)
                       select new VacationDto
                       {
                           Id = vacation.Id,
                           EmployeeId = employee.Id,
                           EmployeeName =employee.Name,
                           CreationDate = vacation.CreationDate,
                           StartDate = vacation.StartDate,
                           EndDate = vacation.EndDate,
                           TypeId = lookup.Id,
                           TypeName = lookup.Name,
                           Notes = vacation.Notes


                       };

            return Ok(data);
        }



        [HttpGet("GetById")]
        public IActionResult GetById([FromQuery] long Id)
        {
            var data = _dbContext.Vacations.Select(x => new VacationDto
            {
                Id = x.Id,
                EmployeeId = x.Id,
                EmployeeName = x.Emp.Name,
                CreationDate = x.CreationDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                TypeId = x.Type.Id,
                TypeName = x.Type.Name,
                Notes = x.Notes

            }).FirstOrDefault(x => x.Id == Id);



            return Ok(data);
        }

        [Authorize(Roles ="Admin,HR")]
        [HttpPost("Add")]
        public IActionResult Add([FromBody] SaveVacationDto vacationDto)
        {
            var vacation = new Vacation
            {
                Id = vacationDto.Id,
                StartDate = vacationDto.StartDate,
                EndDate = vacationDto.EndDate,
                EmployeeId = vacationDto.EmployeeId,
                TypeId = vacationDto.TypeId,
                Notes = vacationDto.Notes

            };

            _dbContext.Vacations.Add(vacation);
            _dbContext.SaveChanges();
            return Ok();
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPut("Update")]
        public IActionResult Update([FromBody] SaveVacationDto vacationDto)
        {
            var vacation = _dbContext.Vacations.FirstOrDefault( x => x.Id == vacationDto.Id );

            if (vacation == null) 
            {
                return BadRequest("Vacation Does Not Exist");
            }

            vacation.StartDate = vacationDto.StartDate;
            vacation.EndDate = vacationDto.EndDate;
            vacation.Notes = vacationDto.Notes;
            vacation.EmployeeId = vacationDto.EmployeeId;
            vacation.TypeId = vacationDto.TypeId;

            _dbContext.SaveChanges();


            return Ok();
        }


        [Authorize(Roles = "Admin,HR")]
        [HttpDelete("Delete")]
        public IActionResult Delete(long Id) 
        {
            var vacation = _dbContext.Vacations.FirstOrDefault(x => x.Id == Id);
            if (vacation == null) 
            {
                return BadRequest("Vacation Does Not Exist");
            }

            _dbContext.Vacations.Remove(vacation);
            _dbContext.SaveChanges();
            return Ok();


        }
        [HttpGet("EmployeeVacationsCount")]
        public IActionResult EmployeeVacationsCount()
        {
            var data = from emp in _dbContext.Employees
                       from vac in _dbContext.Vacations.Where(x => x.EmployeeId == emp.Id).DefaultIfEmpty()
                       group new { Employee = emp, Vacation = vac } by new {Id = emp.Id, Name=emp.Name } into vacationsCount
                       select new VacationCountDto
                       {
                           EmployeeId = vacationsCount.Key.Id,
                           EmployeeName = vacationsCount.Key.Name,

                           VacationsCount = vacationsCount.ToList().Count(x => x.Vacation !=null )

                       };
            return Ok(data);
        }





    }
}
