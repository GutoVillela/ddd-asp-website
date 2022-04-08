namespace KadoshWebsite.Models
{
    public class ChartReportModel
    {
        /// <summary>
        /// Labels related to every data to display.
        /// </summary>
        public IEnumerable<string> Labels { get; set; } = new List<string>();

        /// <summary>
        /// Data to display in chart.
        /// </summary>
        public IEnumerable<string> Data { get; set; } = new List<string>();
    }
}
