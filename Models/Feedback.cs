namespace GroomMate.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public int AppointmentID { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comments { get; set; }
    }
}
