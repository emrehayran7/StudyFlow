using StudyFlow.Core.Commands.Topic.CreateTopic.Request;
using StudyFlow.Core.Commands.Topic.UpdateTopic.Request;
using StudyFlow.Core.Queries.Topic.Response.StudyFlow.Core.Queries.Topic.Response;
using StudyFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudyFlow.Core.Mapper
{
    public static class TopicMapper
    {
        public static Topic ToEntity(this CreateTopicDto dto)
        {
            return new Topic
            {
                CourseId = dto.CourseId,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                PriorityLevel = dto.PriorityLevel
            };
        }
        public static void MapToEntity(this UpdateTopicDto dto, Topic topic)
        {
            topic.Title = dto.Title;
            topic.Description = dto.Description;
            topic.Status = dto.Status;
            topic.PriorityLevel = dto.PriorityLevel;

        }
        public static GetTopicDto ToDto(this Topic topic)
        {
            return new GetTopicDto
            {
                Id = topic.Id,
                CourseId = topic.CourseId,
                Title = topic.Title,
                Description = topic.Description,
                Status = topic.Status,
                PriorityLevel = topic.PriorityLevel,
                CreatedAt = topic.CreatedAt,
                UpdatedAt = topic.UpdatedAt,
                CreatedBy = topic.CreatedBy,
                UpdatedBy = topic.UpdatedBy
            };
        }


      
    }
}
