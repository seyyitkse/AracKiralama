﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentCar.Application.Features.Mediator.Queries.TestimonialQueries;
using RentCar.Application.Features.Mediator.Results.FeatureResults;
using RentCar.Application.Features.Mediator.Results.TestimonialResults;
using RentCar.Application.Interfaces;
using RentCar.Domain.Entities;

namespace RentCar.Application.Features.Mediator.Handlers.TestimonialHandlers
{
    public class GetPricingByIdQueryHandler : IRequestHandler<GetTestimonialByIdQuery, GetTestimonialByIdQueryResult>
    {
        private readonly IRepository<Testimonial> _repository;
        public GetPricingByIdQueryHandler(IRepository<Testimonial> repository)
        {
            _repository = repository;
        }

        public async Task<GetTestimonialByIdQueryResult> Handle(GetTestimonialByIdQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.Id);
            return new GetTestimonialByIdQueryResult
            {
                TestimonialID = values.TestimonialID,
                Name = values.Name,
                Comment = values.Comment,
                Title = values.Title,
                ImageUrl = values.ImageUrl
            };
        }
    }
}
