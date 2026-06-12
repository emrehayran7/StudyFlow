using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Mapper
{
    public static class CourseMapper
    {
        public static Course ToEntity(this CreateCourseDto dto)
        {
            return new Course
            {
                Title = dto.Title,
                Description = dto.Description
            };
        }
        public static void MapToEntity(this UpdateCourseDto dto, Course course)
        {
            course.Title = dto.Title;
            course.Description = dto.Description;

        }

        public static GetCourseDto ToDto(this Course course)
        {
            return new GetCourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description
            };
        }
    }
}
