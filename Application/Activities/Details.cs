using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities {
    public class Details {
        public class Query : IRequest<ActivityDto> {
            public Guid Id { get; set; }
        }
        //IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
        //in means that TRequest can only be an input method type (this is called contravariance)
        //out T2 (which is not used here) would be covariance; this specifies that T2 must be an output of a function 
        public class Handler : IRequestHandler<Query, ActivityDto> {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler (DataContext context, IMapper mapper) 
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<ActivityDto> Handle (Query request, CancellationToken cancellationToken) {
                var activity = await _context.Activities
                    .FindAsync(request.Id);
                
                if (activity == null)
                    throw new RestException (HttpStatusCode.NotFound, new { activity = "Not Found" });
                
                var activityToReturn = _mapper.Map<Activity, ActivityDto>(activity);
                return activityToReturn;
            }
        }
    }
}