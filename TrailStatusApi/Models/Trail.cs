namespace TrailStatusApi.Models
{
    public class Trail
    {
        public Trail(string name, string area, string url, Status status)
        {
            Name = name;
            Status = status;
            Area = area;
            URL = url;
        }

        public string Name { get; set; }
        public string Area { get; set; }
        public string URL { get; set; }
        public Status Status { get; set; }
    }
}
