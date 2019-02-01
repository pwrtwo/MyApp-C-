using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly MyAppContext _context;

        public TeacherRepository(MyAppContext context)
        {
            _context = context;
        }

        // List의 IEnumerable의 차이점 IEnumerable read opeartion 할때 효율성이 뛰어나다
        public IEnumerable<Teacher> GetAllTeachers()
        {
            // teachers table안에 있는 데이타를 다 불러와서 result에 저장한다.
            var result = _context.Teachers.ToList();

            return result;
        }

        public Teacher GetTeacher(int id)
        {
            var result = _context.Teachers.Find(id);

            return result;
        }
    }
}
