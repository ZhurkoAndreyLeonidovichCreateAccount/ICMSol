using DataLayer;

namespace ICM
{
    public class RepeatingService : BackgroundService
    {
        private readonly PeriodicTimer _timer = new( TimeSpan.FromMilliseconds(1000));
        private readonly ApplicationDbContext _context;

        public RepeatingService(ApplicationDbContext context)
        {
            _context = context;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested
           while (true)
            {
                await DOSome(_context);
            }
        }

        public static async Task DOSome(ApplicationDbContext context)
        {
            var user =  context.Users.ToList();
            Console.WriteLine(user.ToString());
            //Console.WriteLine(DateTime.Now.ToString("O"));
            //await Task.Delay(500);
        }
    }
}
