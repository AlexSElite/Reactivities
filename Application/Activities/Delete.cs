using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Persistence;
using System;

namespace Application.Activities
{
    public class Delete
    {
        /*
             Instead of using the Query request were going use a Command request, because were sending requests to
             Create, Update, and Delete commands against the database, instead of getting data from the database.
           */
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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

                _context.Remove(activity);

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