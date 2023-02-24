using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartStartWebAPI.Domain;
using SmartStartWebAPI.DTOs;
using SmartStartWebAPI.Infrastructure;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace SmartStartWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private StudentsDBContext _dbContext;
        public StudentsController(StudentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<StudentNameDto> GetStudentsNames([FromQuery] string lastName)
        {
            var students = _dbContext.Students.ToList();
            if (!students.Exists(x => x.LastName == lastName))
            {
                return students.Select(x => new StudentNameDto
                {
                    Id = x.StudentId,
                    Name = x.Name,
                    LastName = x.LastName
                });
            }
            return students.Select(x => new StudentNameDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName
            }).Where(x => x.LastName == lastName);
        }

        [HttpGet("{id}")]
        public ActionResult<StudentDto> GetStudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Ok(studentDto);
        }

        [HttpGet("average-age")]
        public int GetStudentsAverageAge()
        {
            var students = _dbContext.Students.ToList();
            var studentsCount = students.Count();
            var sumAge = students.Sum(x => x.Age);
            return sumAge / studentsCount;
        }

        [HttpPost]
        public ActionResult<StudentDto> AddStudent([FromBody] StudentToCreateDto studentToCreateDto)
        {
            var student = new Student
            {
                Name = studentToCreateDto.Name,
                LastName = studentToCreateDto.LastName,
                Email = studentToCreateDto.Email,
                Age = studentToCreateDto.Age,
                IsActive = studentToCreateDto.IsActive
            };
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            var studentDto = new StudentDto
            {
                Id = student.StudentId,
                Name = student.Name,
                LastName = student.LastName,
                Email = student.Email,
                Age = student.Age,
                IsActive = student.IsActive
            };
            return Created(string.Empty, studentDto);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateStudent([FromRoute] int id, [FromBody] StudentToUpdateDto studentToUpdateDto)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            student.Name = studentToUpdateDto.Name;
            student.LastName = studentToUpdateDto.LastName;
            student.Email = studentToUpdateDto.Email;
            student.Age = studentToUpdateDto.Age;
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Deletetudent([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            _dbContext.Students.Remove(student);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpGet("Oldest-student")]
        public ActionResult<StudentDto> GetOldestStudent()
        {
            var students = _dbContext.Students.ToList();
            var maxAge = students.Max(x => x.Age);
            var oldestStudent = _dbContext.Students.FirstOrDefault(x => x.Age == maxAge);
            if (oldestStudent == null)
                return NotFound();
            var studentDto = new StudentDto
            {
                Id = oldestStudent.StudentId,
                Name = oldestStudent.Name,
                LastName = oldestStudent.LastName,
                Email = oldestStudent.Email,
                Age = oldestStudent.Age,
                IsActive = oldestStudent.IsActive
            };
            return Ok(studentDto);
        }
        [HttpGet("Youngest-student")]
        public ActionResult<StudentDto> GetYoungestStudent()
        {
            var students = _dbContext.Students.ToList();
            var minAge = students.Min(x => x.Age);
            var youngestStudent = _dbContext.Students.FirstOrDefault(x => x.Age == minAge);
            if (youngestStudent == null)
                return NotFound();
            var studentDto = new StudentDto
            {
                Id = youngestStudent.StudentId,
                Name = youngestStudent.Name,
                LastName = youngestStudent.LastName,
                Email = youngestStudent.Email,
                Age = youngestStudent.Age,
                IsActive = youngestStudent.IsActive
            };
            return Ok(studentDto);
        }

        [HttpGet("older-than-avg")]
        public IEnumerable<StudentDto> GetOlderThanAvg()
        {
            var students = _dbContext.Students.ToList();
            var avgAge = students.Average(x => x.Age);
            return students.Select(x => new StudentDto
            {
                Id = x.StudentId,
                Name = x.Name,
                LastName = x.LastName,
                Email = x.Email,
                Age = x.Age,
                IsActive = x.IsActive
            }).Where(x => x.Age >= avgAge);
        }

        [HttpPut("Change-student-IsActive-{id}")]
        public ActionResult ChangeStudentIsActive([FromRoute] int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null)
                return NotFound();
            student.IsActive = !student.IsActive;
            _dbContext.SaveChanges();
            return Ok(student.IsActive);
        }
    }
}