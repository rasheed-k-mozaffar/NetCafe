using Microsoft.AspNetCore.Mvc;

namespace NetCafe.Server.Controllers;

public class SeriesController : BaseController
{
    private readonly ISeriesRepository seriesRepository;
    private readonly ILogger<SeriesController> logger;

    public SeriesController
    (
        ISeriesRepository seriesRepository,
        ILogger<SeriesController> logger,
        ApplicationDbContext context

    ) : base(context)
    {
        this.seriesRepository = seriesRepository;
        this.logger = logger;
    }

    #region GET
    [HttpGet]
    public async Task<IActionResult> GetAllSeries()
    {
        var series = await seriesRepository.GetSeriesAsync();
        var seriesAsDtos = series.Select(s => s.ToSeriesSummary());

        logger.LogInformation("All series have been retrieved successfully");
        return Ok(new ApiResponse<IEnumerable<SeriesSummaryDto>>
        {
            Message = "Series retrieved successfully",
            Value = seriesAsDtos,
            IsSuccess = true
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeriesById(Guid id)
    {
        try
        {
            var series = await seriesRepository.GetSeriesByIdAsync(id);
            var seriesAsDto = series.ToSeriesDto();

            logger.LogInformation("Series with the ID: {id} has been retrieved successfully", id);
            return Ok(new ApiResponse<SeriesDto>
            {
                Message = "Series has been retrived successfully",
                Value = seriesAsDto,
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            logger.LogError("No series was found with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion

    #region POST
    [HttpPost]
    public async Task<IActionResult> CreateSeries([FromBody] SeriesCreateDto model)
    {
        if (ModelState.IsValid)
        {
            var seriesToCreate = model.ToSeriesCreate();

            try
            {
                var creationResult = await seriesRepository.CreateSeriesAsync(seriesToCreate);
                logger.LogInformation("New series ({seriesName}) was successfully created", model.Name);

                return Ok(new ApiResponse
                {
                    Message = "New series was created successfully",
                    IsSuccess = true
                });
            }
            catch (DataInsertionFailedException ex)
            {
                logger.LogError("Failed attempt to create a new series");
                return BadRequest(new ApiErrorResponse
                {
                    Message = ex.Message
                });
            }
        }
        else
        {
            logger.LogError("Invalid data format for creating the series");
            return BadRequest(ModelState);
        }
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeries(Guid id)
    {
        try
        {
            await seriesRepository.DeleteSeriesAsync(id);

            logger.LogInformation("Series with the ID: {id} has been deleted successfully", id);
            return Ok(new ApiResponse
            {
                Message = "Series has been deleted successfully",
                IsSuccess = true
            });
        }
        catch (NotFoundException ex)
        {
            logger.LogError("Couldn't find series to delete with the ID: {id}", id);
            return BadRequest(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    #endregion

    #region PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeries(Guid id, [FromBody] SeriesUpdateDto model)
    {
        if (ModelState.IsValid)
        {
            var seriesToUpdate = await context.Series.FindAsync(id);

            if (seriesToUpdate is null)
            {
                logger.LogError("No series was found with the ID: {id}", id);
                return BadRequest(new ApiErrorResponse
                {
                    Message = "No series was found with the given ID"
                });
            }

            seriesToUpdate.Name = model.Name;
            seriesToUpdate.Description = model.Description;
            seriesToUpdate.CoverImageUrl = model.CoverImageUrl;

            await context.SaveChangesAsync();
            logger.LogInformation("Series with the ID: {id} has been updated successfully", id);
            return Ok(new ApiResponse
            {
                Message = "Series was updated successfully",
                IsSuccess = true
            });
        }
        else
        {
            logger.LogError("Invalid data format to update the series");
            return BadRequest(ModelState);
        }
    }
    #endregion
}
