using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MimeKit;
using MimeKit.Utils;
using System.Text;
//using System.Net.Mail;

internal class Program
{

    const string username = "kolya.tymo@gmail.com";
    const string password = "ekpz prkm xnna xmnv";
    const string otheruser = "rijiy23097@macsnipe.com";

    private static void Main(string[] args)
    {
        // send mails (SMTP)
        /*MimeMessage message = new(); // виклик деф конструктора

        message.From.Add(new MailboxAddress("Kolya", username));
        message.To.Add(new MailboxAddress("user", otheruser));

        message.Subject = "Hello MailKit";
        message.Importance = MessageImportance.High;

        message.Body = new TextPart("SubTheme")
        {
            Text = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                     Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James Mosley, the librarian at St Bride Printing Library
                     in London, took a 1914 Cicero translation and scrambled it
                     to make dummy text for Letraset's Body Type sheets"
        };

        var body =  new TextPart("SubTheme")
        {
            Text = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                     Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James Mosley, the librarian at St Bride Printing Library
                     in London, took a 1914 Cicero translation and scrambled it
                     to make dummy text for Letraset's Body Type sheets"
        };

        var path = @"C:\Users\Vlad\Downloads\images (3).jpeg"; 

        var attachment = new MimePart("imag", "jpeg")
        {
            Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(path),
        };

        var multi = new Multipart("mixed");
        multi.Add(body);
        multi.Add(attachment);

        message.Body = multi;*/

        /*using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);
            client.Send(message);
        }*/


        /*var builder = new BodyBuilder();

        // Set the plain-text version of the message text
        builder.TextBody = @"Hey Alice,

        What are you up to this weekend? Monica is throwing one of her parties on
        Saturday and I was hoping you could make it.

        Will you be my +1?

        -- Joey
        ";

        // In order to reference selfie.jpg from the html text, we'll need to add it
        // to builder.LinkedResources and then use its Content-Id value in the img src.
        var image = builder.LinkedResources.Add(@"C:\Users\Vlad\Desktop\New folder\kolya.txt");
        image.ContentId = MimeUtils.GenerateMessageId();

        // Set the html version of the message text
        builder.HtmlBody = string.Format($@"<p>Hey Alice,<br>
        <p>What are you up to this weekend? Monica is throwing one of her parties on
        Saturday and I was hoping you could make it.<br>
        <p>Will you be my +1?<br>
        <p>-- Joey<br>
        <center><img src='{image.ContentId}'></center>");

        // We may also want to attach a calendar event for Monica's party...
        *//*builder.Attachments.Add(@"C:\Users\Joey\Documents\party.ics");*//*

        // Now we just need to set the message body and we're done
        message.Body = builder.ToMessageBody();
        using (var client = new SmtpClient())
        {
            client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);
            client.Send(message);
        }*/




        // Get mails (IMAP)

        Console.OutputEncoding = Encoding.UTF8;

        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, MailKit.Security.SecureSocketOptions.SslOnConnect);
            client.Authenticate(username, password);

            foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
            {
                Console.WriteLine(item.Name);
            }
            client.GetFolder(MailKit.SpecialFolder.Sent).Open(MailKit.FolderAccess.ReadWrite);
            var folder = client.GetFolder(MailKit.SpecialFolder.Sent);
            var id = folder.Search(SearchQuery.All)[folder.Search(SearchQuery.All).Count - 1];
            var message = folder.GetMessage(id);

            Console.WriteLine($"{message.Date} {message.Subject}");

            //folder.MoveTo(id, client.GetFolder(MailKit.SpecialFolder.Trash)); // переміщення у кошик

            /*folder.AddFlags(id, MessageFlags.Deleted, true); // видаляє

            folder.Expunge();*/

            /*foreach (var item in folder.Search(SearchQuery.All))
            {
                var res = folder.GetMessage(item);
                Console.WriteLine($"{res.Date} {res.Subject} \t {res?.Body?.ContentBase}");
            }*/

            client.Inbox.Open(FolderAccess.ReadWrite);
            var inbox = client.Inbox.Search(SearchQuery.All);
            foreach (var item in inbox)
            {
                var res = client.Inbox.GetMessage(item);
                Console.WriteLine($"{res.Date} {res.Subject}");
                foreach (var item1 in res.Attachments)
                {
                    Console.WriteLine(item1?.ContentType.Name);
                    Console.WriteLine(item1?.ContentDisposition.FileName);
                }
            }
        }

    }
}