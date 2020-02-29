using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities {
    public class Details {
        public class Query : IRequest<Activity> 
        {
            public Guid Id { get; set; }
        }
        //IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
        //in means that TRequest can only be an input method type (this is called contravariance)
        //out T2 (which is not used here) would be covariance; this specifies that T2 must be an output of a function 
        public class Handler : IRequestHandler<Query, Activity> {
            private readonly DataContext _context;
            public Handler (DataContext context) 
            {
                _context = context;
            }

            public async Task<Activity> Handle (Query request, CancellationToken cancellationToken) 
            {
                var activity = await _context.Activities.FindAsync(request.Id);
                if(activity==null)
                    throw new RestException(HttpStatusCode.NotFound, new {activity="Not Found"});
                return activity;
            }
        }
    }
}