using System.Collections.Generic;

namespace ActivityLogger.Dtos
{
    public class ReportItemDto
    {
        public string CategoryName { get; set; }
        public double Duration { get; set; }
        public List<string> Comments { get; set; }
    }
}