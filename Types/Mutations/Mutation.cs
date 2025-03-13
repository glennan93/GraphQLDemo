using GraphQLDemo.Types.Subscriptions;
using HotChocolate.Subscriptions;
using System.Threading.Tasks;

namespace GraphQLDemo.Types.Mutations
{
    [MutationType]
    public class Mutation
    {
        private readonly List<CourseResult> _courses;

        public Mutation()
        {
            _courses = new List<CourseResult>();
        }

        public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = new CourseResult()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };

            _courses.Add(course);
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;
        }

        public async Task<CourseResult> Update(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
        {
            CourseResult course = _courses.FirstOrDefault(c => c.Id == id);

            if(course == null)
            {
                var error = ErrorBuilder
                    .New()
                    .SetMessage("Course Not Found.")
                    .SetCode("COURSE_NOT_FOUND")
                    .Build();
                throw new GraphQLException(error);
            }

            course.Name = courseInput.Name;
            course.Subject = courseInput.Subject;
            course.InstructorId = courseInput.InstructorId;

            await topicEventSender.SendAsync(nameof(Subscription.CourseUpdated), course);

            return course;
        }

        public bool Delete(Guid id)
        {
            return _courses.RemoveAll(c => c.Id == id) >= 1;
        }
    }
}
