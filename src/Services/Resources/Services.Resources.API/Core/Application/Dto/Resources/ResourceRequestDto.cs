﻿using Transversal.Application.Dto.Request;

namespace Services.Resources.API.Core.Application.Dto.Resources
{
    public class ResourceRequestDto : BaseLocalizedRequestDto,
        ILocalizedRequestDto
    {
        public int? WithId { get; set; }
        public string WithKey { get; set; }
    }
}