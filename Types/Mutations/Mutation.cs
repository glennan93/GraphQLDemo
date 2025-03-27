using FirebaseAdminAuthentication.DependencyInjection.Models;
using GraphQLDemo.DTOs;
using GraphQLDemo.Services.Courses;
using GraphQLDemo.Types.Subscriptions;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;
using System.Threading.Tasks;
namespace GraphQLDemo.Types.Mutations
{
    [MutationType]
    public class Mutation
    {
        private readonly CoursesRepository _coursesRepository;

        public Mutation(CoursesRepository coursesRepository)
        {
            _coursesRepository = coursesRepository;
        }

        [Authorize]
        public async Task<CourseResult> CreateCourse(CourseInputType courseInput, [Service] ITopicEventSender topicEventSender, ClaimsPrincipal claimsPrincipal)
        {

            string userId = claimsPrincipal.FindFirstValue(FirebaseUserClaimType.ID);

            CourseDTO courseDTO = new CourseDTO()
            {
                Id = Guid.NewGuid(),
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId,
            };

            courseDTO = await _coursesRepository.Create(courseDTO);

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;
        }

        [Authorize]
        public async Task<CourseResult> Update(Guid id, CourseInputType courseInput, [Service] ITopicEventSender topicEventSender)
        {

            CourseDTO courseDTO = new CourseDTO()
            {
                Id = id,
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };

            courseDTO = await _coursesRepository.Update(courseDTO);

            CourseResult course = new CourseResult()
            {
                Id = courseDTO.Id,
                Name = courseDTO.Name,
                Subject = courseDTO.Subject,
                InstructorId = courseDTO.InstructorId
            };

            await topicEventSender.SendAsync(nameof(Subscription.CourseUpdated), course);

            return course;
        }

        [Authorize]
        public async Task<bool> Delete(Guid id)
        {
            return await _coursesRepository.Delete(id);
        }
    }
}
