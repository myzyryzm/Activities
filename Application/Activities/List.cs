using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities {
    public class List {
        public class Query : IRequest<List<ActivityDto>> { };
        public class Handler : IRequestHandler<Query, List<ActivityDto>> {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler (DataContext context, IMapper mapper) 
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<ActivityDto>> Handle (Query request, CancellationToken cancellationToken) {
                //Include theninclude is an example of eager loading
                //when we get the activities we tell it to then get all pertinent useractivities and then get all appusers
                var activities = await _context.Activities
                    .ToListAsync();
                    // .Include (x => x.UserActivities)
                    // .ThenInclude (x => x.AppUser)
                    // .ToListAsync ();

                return _mapper.Map<List<Activity>, List<ActivityDto>>(activities);
            }
        }
    }
}