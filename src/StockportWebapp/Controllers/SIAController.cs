using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StockportWebapp.Models;
using StockportWebapp.Wrappers;

//using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    public class SIAController : Controller
    {
        //http://scnportwebdev2.stockport.gov.uk/siarestipa/api/values/?term=lorry
        string Baseurl = "http://scnportwebdev2.stockport.gov.uk/siarestipa/";
        [Route("/sia")]
        public async Task<ActionResult> Index(string term)
        {

            List<Photo> EmpInfo = new List<Photo>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/values/?term=" + term);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    EmpInfo = JsonConvert.DeserializeObject<List<Photo>>(EmpResponse);

                    foreach (var photo in EmpInfo)
                    {
                        photo.imgSrc = "http://interactive.stockport.gov.uk/stockportimagearchive/SIA/" +
                                       photo.AccessionNo.Trim() + ".jpg";
                    }

                }
                //returning the employee list to view  
                return View(EmpInfo);
            }
        }
    }

    
}

