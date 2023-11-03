using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace DEMO.PL.Helper
{
	public class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("frjr312@gmail.com", "rmvfsrkcsqilkjmm");
			client.Send("frjr312@gmail.com", email.To, email.Title, email.Body);
		}
	}
}
