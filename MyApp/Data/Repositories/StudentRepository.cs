using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MyAppContext _context;

        public StudentRepository(MyAppContext context)
        {
            _context = context;
        }

        // adds students to db
        public void AddStudent(Student student)
        {
            // 추가될 변동사항만 트랙하는 중 add된건 아니다
            _context.Students.Add(student); 
        }

        public void Save()
        {
            // stduent 테이블에 저장함.
            _context.SaveChanges();
        }

        public IEnumerable<Student> GetAllStudents()
        {
            var result = _context.Students.ToList();

            return result;
        }

        public Student GetStudents(int id)
        {
            var result = _context.Students.Find(id);

            return result;
        }

        public void Edit(Student student)
        {
            _context.Update(student);
        }

        public void Delete(Student student)
        {
            _context.Remove(student); 
        }
    }
}
