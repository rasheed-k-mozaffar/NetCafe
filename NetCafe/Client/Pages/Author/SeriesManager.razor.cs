using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace NetCafe.Client.Pages.Author;

public partial class SeriesManager : ComponentBase
{
    [Inject]
    public NavigationManager Nav { get; set; } = default!;
    [Inject]
    public ISeriesService SeriesService { get; set; } = default!;
    [Inject]
    public IFilesService FilesService { get; set; } = default!;

    private List<SeriesSummaryDto> series = new();
    private bool isLoadingData = true;
    private string errorMessage = string.Empty;

    // delete series related variables
    private bool wantsToDeleteSeries = false;
    private Guid seriesToDeleteId;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        errorMessage = string.Empty;
        try
        {
            var result = await SeriesService.GetAllSeriesAsync();
            if (result.IsSuccess)
            {
                series = result.Value!.ToList();
                isLoadingData = false;
            }
        }
        catch (DataRetrievalFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }

    private void OpenDeleteDialog(Guid seriesId)
    {
        wantsToDeleteSeries = true;
        seriesToDeleteId = seriesId;
    }

    private void CloseDeleteDialog()
    {
        wantsToDeleteSeries = false;
        seriesToDeleteId = default;
    }

    private async Task DeleteSeriesAsync()
    {
        errorMessage = string.Empty;
        try
        {
            var result = await SeriesService.DeleteSeriesAsync(seriesToDeleteId);
            if (result.IsSuccess)
            {
                // delete the series on the page without re loading
                var seriesToDelete = series.FirstOrDefault(s => s.SeriesId == seriesToDeleteId);
                series.Remove(seriesToDelete!);
                wantsToDeleteSeries = false;
                seriesToDeleteId = default;
                StateHasChanged();
            }
        }
        catch (DeletionFailedException ex)
        {
            errorMessage = ex.Message;
        }
    }


}
