﻿using HotChocolate.Data.Sorting;
using GraphQLDemo.Types.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GraphQLDemo.Types.Sorters
{
    public class CourseSortType : SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(c => c.Id);
            descriptor.Ignore(c => c.InstructorId);
            base.Configure(descriptor);
        }
    }
}
