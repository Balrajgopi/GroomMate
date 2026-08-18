using GroomMate.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace GroomMate.Security
{
    public static class EmailService
    {
        public static void SendAppointmentNotification(int appointmentId, string actionType)
        {
            try
            {
                using (var db = new GroomMateContext())
                {
                    // Load appointment details with relationships
                    var appointment = db.Appointments
                        .Include(a => a.User)
                        .Include(a => a.Service)
                        .Include(a => a.Staff)
                        .FirstOrDefault(a => a.AppointmentID == appointmentId);

                    if (appointment == null || appointment.User == null)
                    {
                        System.Diagnostics.Trace.TraceWarning($"EmailService: Appointment ID {appointmentId} or User not found.");
                        return;
                    }

                    string customerEmail = appointment.User.Email;
                    if (string.IsNullOrWhiteSpace(customerEmail))
                    {
                        System.Diagnostics.Trace.TraceWarning($"EmailService: Customer {appointment.User.Username} does not have an email address.");
                        return;
                    }

                    string subject = "";
                    var body = new StringBuilder();

                    body.AppendLine("<html>");
                    body.AppendLine("<body style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>");
                    body.AppendLine("<div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>");
                    body.AppendLine("<div style='background-color: #0F172A; padding: 15px; text-align: center; border-radius: 5px 5px 0 0;'>");
                    body.AppendLine("  <h2 style='color: #D97706; margin: 0; font-family: Georgia, serif;'>GroomMate Men's Salon</h2>");
                    body.AppendLine("</div>");
                    body.AppendLine("<div style='padding: 20px;'>");
                    body.AppendLine($"  <p>Hello <strong>{appointment.User.FullName}</strong>,</p>");

                    switch (actionType)
                    {
                        case "Created":
                            subject = "GroomMate - Appointment Booked Successfully!";
                            body.AppendLine("  <p>Your appointment has been successfully booked and is currently <strong>Pending</strong> staff assignment and confirmation.</p>");
                            break;

                        case "Assigned":
                            subject = "GroomMate - Staff Assigned to Your Appointment";
                            string staffName = appointment.Staff?.FullName ?? "Staff Member";
                            body.AppendLine($"  <p>A stylist has been assigned to your appointment: <strong>{staffName}</strong>. The appointment is awaiting final confirmation.</p>");
                            break;

                        case "Confirmed":
                            subject = "GroomMate - Appointment Confirmed!";
                            body.AppendLine("  <p>Great news! Your appointment has been <strong>Confirmed</strong> by our team. We look forward to seeing you!</p>");
                            break;

                        case "Completed":
                            subject = "GroomMate - Thank You for Your Visit!";
                            body.AppendLine("  <p>Your appointment has been marked as <strong>Completed</strong>. Thank you for choosing GroomMate! We hope you enjoyed your service.</p>");
                            body.AppendLine("  <p>Please feel free to log in and leave us your feedback on your dashboard.</p>");
                            break;

                        case "Cancelled":
                            subject = "GroomMate - Appointment Cancelled";
                            body.AppendLine("  <p>Your appointment has been <strong>Cancelled</strong>. If this was a mistake, please visit our website to book a new appointment.</p>");
                            break;

                        default:
                            subject = "GroomMate - Appointment Update";
                            body.AppendLine($"  <p>Your appointment status has been updated to: <strong>{appointment.Status}</strong>.</p>");
                            break;
                    }

                    body.AppendLine("<hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'>");
                    body.AppendLine("<h3 style='color: #0F172A; margin-top: 0;'>Appointment Details</h3>");
                    body.AppendLine("<table style='width: 100%; border-collapse: collapse;'>");
                    body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold; width: 150px;'>Appointment ID:</td><td style='padding: 5px 0;'>#{appointment.AppointmentID}</td></tr>");
                    body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold;'>Service:</td><td style='padding: 5px 0;'>{appointment.Service?.ServiceName}</td></tr>");
                    body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold;'>Price:</td><td style='padding: 5px 0;'>&#8377; {appointment.Service?.Price.ToString("N0")}</td></tr>");
                    body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold;'>Date & Time:</td><td style='padding: 5px 0;'>{appointment.AppointmentDate.ToString("f")}</td></tr>");
                    if (appointment.Staff != null)
                    {
                        body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold;'>Stylist:</td><td style='padding: 5px 0;'>{appointment.Staff.FullName}</td></tr>");
                    }
                    body.AppendLine($"  <tr><td style='padding: 5px 0; font-weight: bold;'>Status:</td><td style='padding: 5px 0;'><span style='background-color: #D97706; color: white; padding: 2px 8px; border-radius: 3px; font-size: 0.85em;'>{appointment.Status}</span></td></tr>");
                    body.AppendLine("</table>");

                    body.AppendLine("</div>");
                    body.AppendLine("<div style='background-color: #F8FAFC; padding: 15px; text-align: center; font-size: 0.8em; color: #64748B; border-radius: 0 0 5px 5px;'>");
                    body.AppendLine("  <p>This is an automated notification. Please do not reply to this email.</p>");
                    body.AppendLine("  <p>&copy; " + DateTime.Now.Year + " GroomMate. All rights reserved.</p>");
                    body.AppendLine("</div>");
                    body.AppendLine("</div>");
                    body.AppendLine("</body>");
                    body.AppendLine("</html>");

                    string fromEmail = System.Configuration.ConfigurationManager.AppSettings["SmtpFromEmail"];
                    string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"];
                    string smtpUser = System.Configuration.ConfigurationManager.AppSettings["SmtpUsername"];
                    string smtpPass = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];

                    if (!string.IsNullOrWhiteSpace(smtpUser) && (string.IsNullOrWhiteSpace(fromEmail) || fromEmail == "no-reply@groommate.com"))
                    {
                        fromEmail = smtpUser;
                    }
                    if (string.IsNullOrWhiteSpace(fromEmail))
                    {
                        fromEmail = "no-reply@groommate.com";
                    }

                    using (var mail = new MailMessage())
                    {
                        mail.From = new MailAddress(fromEmail, "GroomMate Salon");
                        mail.To.Add(new MailAddress(customerEmail));
                        mail.Subject = subject;
                        mail.Body = body.ToString();
                        mail.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            if (!string.IsNullOrWhiteSpace(smtpHost))
                            {
                                smtp.Host = smtpHost;
                                string portStr = System.Configuration.ConfigurationManager.AppSettings["SmtpPort"];
                                if (int.TryParse(portStr, out int port))
                                {
                                    smtp.Port = port;
                                }

                                string sslStr = System.Configuration.ConfigurationManager.AppSettings["SmtpEnableSsl"];
                                if (bool.TryParse(sslStr, out bool enableSsl))
                                {
                                    smtp.EnableSsl = enableSsl;
                                }

                                if (!string.IsNullOrWhiteSpace(smtpUser))
                                {
                                    smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                                }
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            }

                            smtp.Send(mail);
                        }
                    }

                    System.Diagnostics.Trace.TraceInformation($"EmailService: Email notification sent successfully to {customerEmail} for Appointment ID {appointmentId} (Action: {actionType}).");
                }
            }
            catch (Exception ex)
            {
                // Graceful handling to prevent crash if SMTP is not configured or network email fails
                System.Diagnostics.Trace.TraceError($"EmailService Exception (Action: {actionType}, ID: {appointmentId}): {ex.Message}");
            }
        }
    }
}
