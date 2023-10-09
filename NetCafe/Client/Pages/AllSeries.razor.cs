using Microsoft.AspNetCore.Components;

namespace NetCafe.Client.Pages;

public partial class AllSeries : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;
    private List<SeriesSummaryDto> series = new();
    private bool isLoadingData = true;
    private string errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        errorMessage = string.Empty;
        try
        {
            var results = await SeriesService.GetAllSeriesAsync();
            if (results.IsSuccess)
            {
                series = results.Value!.ToList();
                isLoadingData = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }
}
