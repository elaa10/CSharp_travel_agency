namespace networking.dto
{
    public class SearchTripDTO
    {
        public string Objective { get; set; }
        public DateTime Date { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }

        public SearchTripDTO() { }

        public SearchTripDTO(string objective, DateTime date, int startHour, int endHour)
        {
            Objective = objective;
            Date = date;
            StartHour = startHour;
            EndHour = endHour;
        }
    }
}