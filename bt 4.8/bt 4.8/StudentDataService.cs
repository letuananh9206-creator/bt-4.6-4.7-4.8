namespace bt_4._8
{
    public sealed class StudentDataService
    {
        public Task<List<Student>> LoadStudentsAsync(IProgress<int> progress)
        {
            return Task.Run(async () =>
            {
                const int totalStudents = 30;
                var students = new List<Student>(totalStudents);

                for (var i = 1; i <= totalStudents; i++)
                {
                    await Task.Delay(50);
                    students.Add(new Student
                    {
                        Id = i,
                        Name = $"Sinh viên {i:000}",
                        ClassName = $"CNTT{(i - 1) / 10 + 1}"
                    });
                    progress.Report(i * 100 / totalStudents);
                }

                return students;
            });
        }
    }
}
