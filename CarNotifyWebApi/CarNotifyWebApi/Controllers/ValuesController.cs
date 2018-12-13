using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

		// Örnek 1
		[HttpGet]
		public string Test()
		{
			return "Test Başarılı.";
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
			return false;
		}
		// Add Token
		[HttpGet]
		public bool AddToken(string QRCode, string Token, string Type)
		{
			return false;
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
			return false;
		}
		// Delete QR Code
		[HttpGet]
		public bool DeleteQRCode(string QRCode)
		{
			return false;
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
	}
}
