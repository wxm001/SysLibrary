namespace SysLibraryWeb.Infrastructure
{
    using System;
    using System.Net;
    using System.Net.Mail;

    using Microsoft.Extensions.Configuration;

    public class EmailSender
    {
        IConfiguration emailConfig=new ConfigurationBuilder().AddJsonFile("Mail.json").Build().GetSection("Mail");
        public SmtpClient SmtpClient=new SmtpClient();

        public EmailSender()
        {
            this.SmtpClient.EnableSsl=Boolean.Parse(this.emailConfig["UseSsl"]);
            SmtpClient.UseDefaultCredentials = bool.Parse(emailConfig["UseDefaultCredentials"]);
            SmtpClient.Credentials = new NetworkCredential(emailConfig["UserName"], emailConfig["Password"]);//。注意需要在为 SmtpClient 的 Credentials 属性赋值前为 UseDefaultCredentials 赋值，否则 Credentials 将被赋值为空值而出 Bug。
            SmtpClient.Port = Int32.Parse(emailConfig["ServerPort"]);
            SmtpClient.Host = emailConfig["ServerName"];
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
    }
}