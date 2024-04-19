namespace FunFoxTask.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Levels { get; set; }
        public string Timings { get; set; }
        public int MaxStrengh { get; set; }

    }

    public class CourseVM : Course
    {
        public int CurrentStrengh { get; set; }
        public bool EnrollFlag { get; set; }
        public string TextToShow { get; set; }
    }

}
