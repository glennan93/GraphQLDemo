namespace GraphQLDemo.Types
{
    public class StudentType
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [GraphQLName("gpa")]
        public Double GPA { get; set; }

    }
}
