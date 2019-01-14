using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Services.Description;

namespace CarNotifyWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        // Örnek 2
        [HttpGet]
        public string FundAllocation(string jsonResult)
        {
            string item = JsonConvert.DeserializeObject<string>(jsonResult.ToString());
            return ": : " + item;
        }

        // Add QR Code
        [HttpGet]
        public bool AddQRCode(string QRCode, string Plaka, string Name, string Surname, string TelNO)
        {
            try
            {
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    // var maxId = db.QRINFO.Max(a => a.ID);
                    var qrinf = new QRINFO //Make sure you have a table called test in DB
                    {
                        //ID = maxId + 1,
                        QRCode = QRCode,
                        Plaka = Plaka,
                        Ad = Name,
                        Soyad = Surname,
                        Tel = TelNO,
                    };
                    db.QRINFO.Add(qrinf);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // Add Token
        [HttpGet]
        public bool AddToken(string qrCode, string token, int type)
        {
            try
            {
                int? qrid = 0;
                ATKLocalEntities db = new ATKLocalEntities();
                qrid = db.QRINFO.First(a => a.QRCode == qrCode).ID;
                var objtoken = new TOKEN
                {
                    QRId = qrid,
                    TokenInf = token,
                    Type = type,
                };
                db.TOKEN.Add(objtoken);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        // Add Black List
        [HttpGet]
        public bool AddBlackList(string QRCode)
        {
            return false;
        }
        // Notifications Add Log
        [HttpGet]
        public bool AddNotifyLog(string SenderQR, string ReceiverQR, DateTime Date)
        {
            try
            {
                var qrnotifylog = new NOTIFYLOG //Make sure you have a table called test in DB
                {
                    ReceiverQR = ReceiverQR,
                    SenderQR = SenderQR,
                    Date = Date,
                };
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    db.NOTIFYLOG.Add(qrnotifylog);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // Add Users
        [HttpGet]
        public bool AddUsers(string Ad, string Soyad, string Tel, string eMail,string Password)
        {
            try
            {
                //if (!IsValidEmail2(eMail)) return false;
                var users = new USERS //Make sure you have a table called test in DB
                {
                    Ad = Ad,
                    Soyad = Soyad,
                    Tel = Tel,
                    eMail = eMail,
                };
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    db.USERS.Add(users);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // Delete QR Code
        [HttpGet]
        public bool DeleteQRCode(string QRCode)
        {
            try
            {
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    var qrInfo = db.QRINFO.First(c => c.QRCode == QRCode);

                    db.QRINFO.Remove(qrInfo);

                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        // Delete Black List
        [HttpGet]
        public bool DeleteBlackList(string QRCode)
        {
            return false;
        }
        // Delete Token
        [HttpGet]
        public bool DeleteToken(string Token)
        {
            return false;
        }
        // Get QRInfo
        [HttpGet]
        public IEnumerable<QRINFO> GetQRInfo(string qrCode)
        {
            try
            {
                //JsonSerializerSettings jss = new JsonSerializerSettings();
                //jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //string json = JsonConvert.SerializeObject(qrinf, jss);3
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    var qrinf = db.QRINFO.Where(a => a.QRCode == qrCode).ToList();
                    return qrinf;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }// Get QRInfo
        [HttpGet]
        public bool GetUsers(string eMail,string pass)
        {
            try
            {
                USERS qrinf;
                //JsonSerializerSettings jss = new JsonSerializerSettings();
                //jss.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //string json = JsonConvert.SerializeObject(qrinf, jss);3
                using (ATKLocalEntities db = new ATKLocalEntities())
                {
                    qrinf = db.USERS.First(a => a.eMail.Contains(eMail)&&a.Password.Contains(pass));
                    if (qrinf != null) //Db de kayıt varsa true döndür 
                        return true;
                    else return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        bool IsValidEmail2(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
