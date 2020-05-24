namespace ActivityLogger.Dtos
{
    public class ActivityDto
    {
        public long Id { get; set; }
        public CategoryDto Category { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Comment { get; set; }
    }
}