using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
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
                using (ATKEntities db = new ATKEntities())
                {
                    var maxId = db.QRINFO.Max(a => a.ID);
                    var qrinf = new QRINFO //Make sure you have a table called test in DB
                    {
                        ID = maxId + 1,
                        QRCode = QRCode,
                        Plaka = Plaka,
                        Ad = Name,
                        Soyad = Surname,
                        Telefon = TelNO,
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
        public bool AddToken(string qrCode, string token, string type)
        {
            try
            {
                int? qrid = 0;
                ATKEntities db = new ATKEntities();
                qrid = db.QRINFO.First(a => a.QRCode == qrCode).ID;
                var objtoken = new Token
                {
                    QRId = qrid,
                    Token1 = token,
                    Type = type,
                };
                db.Token.Add(objtoken);
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
                using (ATKEntities db = new ATKEntities())
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
        // Delete QR Code
        [HttpGet]
        public bool DeleteQRCode(string QRCode)
        {
            try
            {
                using (ATKEntities db = new ATKEntities())
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
        public string GetQRInfo(string qrCode)
        {
            try
            {
                ATKEntities db = new ATKEntities();
                var qrinf = db.QRINFO.FirstOrDefault(a => a.QRCode == qrCode);
                //Deserialize to concrete  
                // concreteCar = JsonConvert.DeserializeObject<Token>(serializedString);

                return JsonConvert.SerializeObject(qrinf);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
