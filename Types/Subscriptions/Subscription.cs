using GraphQLDemo.Types.Mutations;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace GraphQLDemo.Types.Subscriptions
{
    [SubscriptionType]
    public class Subscription
    {
        [Subscribe]
        public CourseResult CourseCreated([EventMessage] CourseResult course)
        {
            return course;
        }
        [Subscribe]
        public CourseResult CourseUpdated([EventMessage] CourseResult course)
        {
            return course;
        }

    }
}
