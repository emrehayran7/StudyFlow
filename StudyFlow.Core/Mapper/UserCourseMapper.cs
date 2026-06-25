using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Mapper
{
    public static class UserCourseMapper
    {
        public static UserCourse ToUserCourse(this CreateCourseCommand command, int userId)
        {
            return new UserCourse
            {
                UserId = userId
            };
        }
    }
}
