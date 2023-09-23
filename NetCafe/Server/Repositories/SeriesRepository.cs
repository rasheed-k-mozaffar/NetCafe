
namespace NetCafe.Server.Repositories;

public class SeriesRepository : ISeriesRepository
{
    private readonly ApplicationDbContext context;

    public SeriesRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> CreateSeriesAsync(Series series)
    {
        var result = await context.Series.AddAsync(series);

        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            throw new DataInsertionFailedException(message: "Something went wrong while attempting to create the new series");
        }
    }

    public async Task<bool> DeleteSeriesAsync(Guid seriesId)
    {
        var seriesToDelete = await context.Series.FindAsync(seriesId);

        if (seriesToDelete is null)
        {
            throw new NotFoundException(message: "No series was found with the given ID");
        }

        // if some posts are added to the series being deleted, their SeriesId property would now
        // point at a series that doesn't exist. So, handle the reset of the SeriesId of those posts.
        var seriesIsReferencedByPosts = context.Posts.Any(p => p.SeriesId == seriesId);

        if (seriesIsReferencedByPosts)
        {
            var seriesPosts = await context.Posts
            .Where(p => p.SeriesId == seriesId)
            .ToListAsync();

            // Reset their Series ID
            foreach (var p in seriesPosts)
            {
                p.SeriesId = null;
            }
        }

        var result = context.Series.Remove(seriesToDelete);

        if (result.State == EntityState.Deleted)
        {
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<ICollection<Series>> GetSeriesAsync()
    {
        // get all series (plural)
        var series = await context.Series
        .AsNoTracking().ToListAsync();

        return series;
    }

    public async Task<Series> GetSeriesByIdAsync(Guid seriesId)
    {
        var series = await context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);

        if (series is null)
        {
            throw new NotFoundException(message: "No series was found with the given ID");
        }

        return series;
    }

    public Task<bool> UpdateSeriesAsync()
    {
        throw new NotImplementedException();
    }
}
