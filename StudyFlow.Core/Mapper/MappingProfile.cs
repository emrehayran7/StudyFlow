using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using StudyFlow.Core.Commands.Course.CreateCourse.Request;
using StudyFlow.Core.Commands.Course.UpdateCourse.Request;
using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Queries.Course.Response;
using StudyFlow.Domain.Entities;

namespace StudyFlow.Core.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCourseDto, Course>();
        CreateMap<UpdateCourseDto, Course>();

        CreateMap<Course, CreateCourseDto>();
        CreateMap<Course, UpdateCourseDto>();
        CreateMap<CreateCourseCommand, UserCourse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.CourseId, opt => opt.Ignore());

        CreateMap<Topic, CreateTopicDto>();
        CreateMap<CreateTopicDto, Topic>();

    }
}