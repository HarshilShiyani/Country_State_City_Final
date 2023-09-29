using System.Net.Mail;
using System.Net;
using Country_State_City_Final.Models;

namespace Country_State_City_Final.DAL
{
    public class Email
    {

        public void SendMail(string email,string password)
        {
            using (MailMessage mm = new MailMessage(email, email))
            {
                mm.Subject = "Successfully Ragister";
                mm.Body = "your password is" + password + "email is " + password;

                //if (emailModel.Attachment.Length > -1)
                //{
                //    string fileName = Path.GetFileName(emailModel.Attachment.FileName);
                //    mm.Attachments.Add(new Attachment(emailModel.Attachment.OpenReadStream(), fileName));
                //}
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("harshilshiyani5@gmail.com", "rxoqekpraeztcncr");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    //TempData["mailmessege"] = "Successfully mail sended to ";
                    //@TempData["To"] = emailModel.To;
                }
            }
        }

    }
}
