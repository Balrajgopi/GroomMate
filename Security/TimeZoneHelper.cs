using System;

namespace GroomMate.Security
{
    public static class TimeZoneHelper
    {
        private static readonly TimeZoneInfo IstZone = GetIndiaTimeZone();

        private static TimeZoneInfo GetIndiaTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch (Exception)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
                }
                catch (Exception)
                {
                    return TimeZoneInfo.CreateCustomTimeZone("IST", TimeSpan.FromHours(5.5), "India Standard Time", "India Standard Time");
                }
            }
        }

        /// <summary>
        /// Gets the current date and time in India Standard Time (IST - UTC+05:30).
        /// </summary>
        public static DateTime GetCurrentIst()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IstZone);
        }

        /// <summary>
        /// Checks whether a confirmed appointment has reached or passed its scheduled start time in IST.
        /// </summary>
        public static bool CanCompleteAppointment(string status, DateTime appointmentDate, DateTime currentIst)
        {
            if (string.Equals(status, "Confirmed", StringComparison.OrdinalIgnoreCase))
            {
                return appointmentDate <= currentIst;
            }
            return false;
        }

        /// <summary>
        /// Overload that automatically fetches current IST.
        /// </summary>
        public static bool CanCompleteAppointment(string status, DateTime appointmentDate)
        {
            return CanCompleteAppointment(status, appointmentDate, GetCurrentIst());
        }
    }
}
