''Install package
''1. Install-Package Google.Apis.Gmail.v1
''2. Install-Package MimeKit -Version 1.22.0

''imports
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Gmail.v1
Imports Google.Apis.Services
Imports System.Threading
Imports System.Net.Mail
Public Class Form1
    ''1 Authen สร้าง scope
    Private credential = GoogleWebAuthorizationBroker.AuthorizeAsync(New ClientSecrets With {.ClientId = "yourclientid", .ClientSecret = "wyourClientSecret"}, {GmailService.Scope.GmailCompose}, "user", CancellationToken.None).Result
    ''2 create service
    Private service = New GmailService(New BaseClientService.Initializer() With {.HttpClientInitializer = credential, .ApplicationName = "yourapplicationname"})

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ''3. สร่้าง email message  
        Dim msg = New MailMessage()
        msg.From = New MailAddress("emailAddress", "Sender Name")
        msg.To.Add("ReceiveAddress")
        'mailMessage.ReplyToList.Add(email.FromAddress)
        msg.Subject = "TEST body html ภาษาไทย"
        msg.BodyEncoding = System.Text.Encoding.Unicode
        msg.Body = "<h1>hello สวัสดี</h1>"
        msg.IsBodyHtml = True
        msg.SubjectEncoding = System.Text.Encoding.Unicode

        ''4. สร่้าง  message  สำหรับส่ให้ GMAIL API ด้วย MimeKit
        Dim MimeMsg = MimeKit.MimeMessage.CreateFromMailMessage(msg)

        ''5. แปลง  message เป็น Base64UrlEncod ก่อน
        Dim encodedText = Base64UrlEncode(MimeMsg.ToString())

        ''6.ส่ง messages ให้ google API
        Dim message = New Data.Message With {.Raw = encodedText}
        ''7. ส่ง mail
        Dim request = service.Users.Messages.Send(message, "me").Execute()
        MessageBox.Show("done")
    End Sub
    Private Shared Function Base64UrlEncode(ByVal input As String) As String
        Dim inputBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(input)
        Return Convert.ToBase64String(inputBytes).Replace("+"c, "-"c).Replace("/"c, "_"c).Replace("=", "")
    End Function
End Class
