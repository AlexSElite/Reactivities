using MediatR;
using Domain;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using System;

namespace Application.Activities
{
    public class Create
    {
        /*
          Instead of using the Query request were going use a Command request, because were sending a post request
          to insert data in the table instead of getting data from the table
        */
        public class Command : IRequest
        {
            //Activity Properties
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public DateTime Date { get; set; }
            public string City { get; set; }
            public string Venue { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activityEntity = new Activity
                {
                    Id = request.Id,
                    Title = request.Title,
                    Description = request.Description,
                    Category = request.Category,
                    Date = request.Date,
                    City = request.City,
                    Venue = request.Venue
                };

                //Since were not using any special value generators the non- async method should be used.
                _context.Activities.Add(activityEntity);

                /*
                 If the SaveChangesAsync method returns greater than 0 then we will consider this successful
                 because our activity has been added to the Activities table in the database.
                 */
                var success = await _context.SaveChangesAsync() > 0;

                if (success)
                {
                    return Unit.Value;
                }
                else
                {
                    throw new Exception("Problem saving changes.");
                }
            }
        }
    }
}