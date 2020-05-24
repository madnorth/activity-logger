namespace ActivityLogger.Dtos
{
    public class ActivityCreateDto
    {
        public int CategoryId { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Comment { get; set; }
    }
}