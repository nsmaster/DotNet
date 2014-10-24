namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress { get; set; }
        public string MailFromAddress { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ServerName { get; set; }
        public int ServerPort { get; set; }
        public bool WriteAsFile { get; set; }
        public string FileLocation { get; set; }

        public EmailSettings()
        {
            MailToAddress = "nikolay.shatilo@infopulse.com.ua";
            MailFromAddress = "nikolay.shatilo@gmail.com";
            UseSsl = true;
            UserName = "nikolay.shatilo";
            Password = "ytpfdbcbvjcnm";
            ServerName = "smtp.gmail.com";
            ServerPort = 587;
            WriteAsFile = false;
            FileLocation = @"c:\sports_store_emails";
        }
    }
}
