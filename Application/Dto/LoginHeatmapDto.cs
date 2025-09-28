namespace Application.Dto
{
    public record LoginHeatmapDto(
     List<string> Labels,              
     List<LoginHeatmapDatasetDto> Datasets
 );

    public record LoginHeatmapDatasetDto(
        string Label,                        
        List<int> Data,                   
        string BackgroundColor
    );

}
