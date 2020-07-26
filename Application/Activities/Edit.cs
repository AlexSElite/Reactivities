using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using System;

namespace Application.Activities
{
    public class Edit
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
            public DateTime? Date { get; set; }
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

                var activity = await _context.Activities.FindAsync(request.Id);

                if (activity == null)
                {
                    throw new Exception("Could not find activity");
                }

                activity.Title = request.Title ?? activity.Title;
                activity.Description = request.Description ?? activity.Description;
                activity.Category = request.Category ?? activity.Category;
                activity.Date = request.Date ?? activity.Date;
                activity.City = request.City ?? activity.City;
                activity.Venue = request.Venue ?? activity.Venue;

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