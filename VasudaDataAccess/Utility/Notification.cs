using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using RestSharp;
using RestSharp.Authenticators;

namespace VasudaDataAccess.Utility
{
    public class MailDTO

    {
        public string Message { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
    }
    public  class Notification
  {
      public Logger _logger;
        public Notification()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

       public bool SendEmail(MailDTO model)
        {
            try
            {
                var mailFrom = "Vasuda Support <info@vasudamall.com>";
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator(
                    "api", "key-0b215b55ec418d93124c7eeab51ad795");
                RestRequest request = new RestRequest();
                request.AddParameter("domain",
                    "Vasudamall.com", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                model.Message = getTemplate(model.Message, $"Dear {model.Name}");
                request.AddParameter("from", mailFrom);
                request.AddParameter("to", model.Email);
                request.AddParameter("subject", model.Subject);
                request.AddParameter("html", model.Message);
                request.Method = Method.POST;
                var status = (RestResponse) client.Execute(request);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
            }

            return false;
        }


        public static string getTemplate(string message, string name)
        {
            return string.Format(@"<!doctype html>
<html lang='en'>
	<head>
		<!-- Required meta tags -->
		<meta charset='utf-8'>
		<meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
		<title>Email Message</title>
		<!-- Bootstrap CSS -->
		<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css'>
		<link rel='stylesheet' href='https://use.fontawesome.com/releases/v5.2.0/css/all.css' integrity='sha384-hWVjflwFxL6sNzntih27bfxkr27PmbbK/iSvJ+a4+0owXq79v+lsFkW54bOGbiDQ' crossorigin='anonymous'>
		<style type='text/css'>
			body{{overflow-x:hidden;background:#f0f0f0;max-width:100%;font-family:calibri;}}
			body a:hover{{text-decoration:none}}
			.email-body{{border:1px solid #ccc;width:40%;margin-left:10%;background:#fff;height:auto;min-height:700px;min-width:450px;padding:15px;}}
			.email-wrapper{{background:#f5f5f5 no-repeat;background-size:contain; border-radius:0px 0px 20px 20px;height:auto;padding-bottom:15px;}}
			.logo{{height:80px;box-shadow:0px 2px 5px #999;background:#000;width:100%;text-align:center;padding-top:50px;padding-bottom:;line-height:;font-family:calibri;font-size:1.8em;color:#e59c20;}}
			.logo img{{height:30px;}}
			.quote{{height:auto;padding-top:20px;padding-bottom:20px;box-shadow:0px 2px 5px #999;background:#777;width:80%;text-align:center;margin-top:5%;margin-left:10%;font-family:calibri;font-size:1.8em;color:#ccc}}
			.adverts{{height:auto;padding-top:20px;padding-bottom:20px;box-shadow:0px 2px 5px #999;background:#777;width:80%;text-align:center;margin-top:5%;margin-left:10%;font-family:calibri;font-size:1.8em;color:#ccc}}
			.email-text{{padding:20px;font-size:0.9em;color:#444;margin-top:;}}
			h1{{margin-bottom:;}}
			h2,h3{{color:#777;}}
			.date{{border-bottom:1px solid #ccc;text-align:center;font-weight:bold;font-size:1.5em;width:90%;margin-left:5%;padding-top:20px;padding-bottom:10px;color:#415166}}
			hr{{border:1px solid #ccc;width:90%;margin-left:5%;}}
			.newstab{{width:90%;margin-left:5%;border-bottom:1px solid #ccc;padding-bottom:15px;}}
			.newstab img{{height:200px;}}
			.newstab a{{color:#777;text-decoration:none;font-weight:bold;}}
			.foot1{{font-weight:bold;font-size:0.8em;color:#000;margin-bottom:6%;margin-top:7%;text-align:center;width:96%;margin-left:2%;text-transform:uppercase;}}
			.foot1 a{{color:#000;text-decoration:none;}}
			.foot2{{color:#999;width:80%;margin-left:10%;text-align:center;margin-bottom:1%;}}
			.foot3{{color:#000;width:80%;margin-left:10%;text-align:center;}}
			.foot3 a{{color:#000;text-decoration:none;}}
			.foot4{{color:#777;margin-top:4%;width:80%;margin-left:10%;text-align:center;font-size:1.5em;margin-bottom:5%;}}
			.foot4 a{{color:#777;text-decoration:none;margin:3%;}}
			.foot1-text{{padding-left:30px;padding-right:30px;padding-top:20px;}}
			button{{background:#000;border-radius:5px;color:#ccc;padding:10px;font-weight:bold;border:none;margin-top:5%;}}
		</style>
	</head>
    <body>
		<div class='email-body'>
			<div class='email-wrapper'>
				<div class='logo'>
					<img src='https://vasudamall.com/vasuda-template/images/logo2.png' />
				</div>
				<div class='email-text'>
					<div class='newstab'>
						<h1>{0}</h1>
						<p>{1}</p>
						<!-- <a href='#'>Click this link</a> -->
						<!-- <br/> -->
						<!-- <button class=''>Verify Account</button> -->
					</div>
					
				</div>
			</div>
			<div class='foot'>
				<div class='foot1'><span class='foot1-text'><a href='#'>Help Center</a></span> . <span class='foot1-text'><a href='#'>Support 24/7</a></span> . <span class='foot1-text'><a href='#'>Account</a></span></div>
				<div class='foot2'>Copyright &copy; 2020 . All Rights Reserved. We appreciate you!</div>
				<div class='foot3'>support@vasudamall.com <span style='color:#999'>|</span> 0800 330 0000</div>
				<div class='foot4'>
					<a href='#'><i class='fab fa-instagram' aria-hidden='true'></i> </a>
					<a href='#'><i class='fab fa-facebook' aria-hidden='true'></i> </a> 
					<a href='#'><i class='fab fa-twitter' aria-hidden='true'></i> </a>
					<a href='#'><i class='fab fa-google-plus-g' aria-hidden='true'></i>  </a>
				</div>
				<div class='foot3'><a href='#'>Preferences</a> <span style='color:#999'>|</span> <a href='#'>Forward</a> 
			</div>
		</div>
    </body>
</html>", name, message);
        }
    }
}
