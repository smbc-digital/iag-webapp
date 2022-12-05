using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StockportWebapp.Models;
using StockportWebapp.ViewModels;

//using StockportWebapp.Http;

namespace StockportWebapp.Controllers
{
    public class SIAController : Controller
    {
        //http://scnportwebdev2.stockport.gov.uk/siarestipa/api/values/?term=lorry
        //string Baseurl = "http://scnportwebdev2.stockport.gov.uk/siarestipa/";
        string Baseurl = "https://interactive.stockport.gov.uk/siarestapi/";
        //string Baseurl = "http://localhost:59356/";

        [Route("/sia")]
        public async Task<ActionResult> Index(string term, string selectedArea, string SearchDepth)
        {

            SIAViewModel viewModel = new SIAViewModel();
            viewModel.Photos = new List<Photo>();


            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //areas
                HttpResponseMessage areas = await client.GetAsync("v1/GetAreas/");
                var areaResponse = areas.Content.ReadAsStringAsync().Result;
                viewModel.Areas = JsonConvert.DeserializeObject<List<SIAArea>>(areaResponse);
                viewModel.AreaList = new SelectList(viewModel.Areas, "ID", "Area1");


                //albums
                //HttpResponseMessage albums = await client.GetAsync("v1/GetAlbumInfo/");
                //var albumResponse = albums.Content.ReadAsStringAsync().Result;
                //viewModel.Albums = JsonConvert.DeserializeObject<List<AlbumInfo>>(albumResponse);

                //foreach (var item in viewModel.Albums)
                //{
                //    HttpResponseMessage albumsphotos = await client.GetAsync("v1/GetAlbumPhoto/?id=" + item.albumidno);
                //    var albumphotosResponse = albumsphotos.Content.ReadAsStringAsync().Result;
                //    item.AlbumPhotos = JsonConvert.DeserializeObject<List<AlbumPhoto>>(albumphotosResponse);

                //    foreach (var photoItem in item.AlbumPhotos)
                //    {
                //        photoItem.photoPath = "http://interactive.stockport.gov.uk/stockportimagearchive/SIA/" +
                //                              photoItem.photograph.Trim() + ".jpg";
                //    }
                //}

                HttpResponseMessage Res;
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient

                if (selectedArea != "All")
                {
                    if (SearchDepth != "Title only")
                    {
                        Res = await client.GetAsync("v1/GetPhotosByTermArea/?term=" + term + "&area=" + selectedArea);
                    }
                    else
                    {
                        Res = await client.GetAsync("v1/GetPhotosByTitleArea/?term=" + term + "&area=" + selectedArea);
                    }

                }
                else
                {
                    if (SearchDepth != "Title only")
                    {
                        Res = await client.GetAsync("v1/GetPhotosByTerm/?term=" + term);
                    }
                    else
                    {
                        Res = await client.GetAsync("v1/GetPhotosByTitle/?term=" + term);
                    }
                }
                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    viewModel.Photos = JsonConvert.DeserializeObject<List<Photo>>(EmpResponse);

                    if (viewModel.Photos != null)
                    {
                        foreach (var photo in viewModel.Photos)
                        {
                            photo.imgSrc = "http://interactive.stockport.gov.uk/stockportimagearchive/SIA/" +
                                           photo.AccessionNo.Trim() + ".jpg";

                            //comments
                            HttpResponseMessage Coms =
                                await client.GetAsync("v1/GetComments/?id=" + photo.AccessionNo.Trim());
                            var blah = Coms.Content.ReadAsStringAsync().Result;
                            photo.Comments = JsonConvert.DeserializeObject<List<PhotoComment>>(blah);



                        }
                    }



                }
                //returning the employee list to view  
                return View(viewModel);
            }
        }

        [Route("/sia/details/")]
        public async Task<ActionResult> Details(string accessionNo)
        {

            //SIAViewModel viewModel = new SIAViewModel();
            //viewModel.Photos = new List<Photo>();
            Photo photoDetails = new Photo();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



                HttpResponseMessage Res;
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient

                Res = await client.GetAsync("v1/GetPhotosByID/?id=" + accessionNo);



                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    photoDetails = JsonConvert.DeserializeObject<Photo>(EmpResponse);

                    if (photoDetails != null)
                    {
                        photoDetails.imgSrc = "http://interactive.stockport.gov.uk/stockportimagearchive/SIA/" +
                                              photoDetails.AccessionNo.Trim() + ".jpg";

                        //comments
                        HttpResponseMessage Coms =
                            await client.GetAsync("v1/GetComments/?id=" + photoDetails.AccessionNo.Trim());
                        var blah = Coms.Content.ReadAsStringAsync().Result;
                        photoDetails.Comments = JsonConvert.DeserializeObject<List<PhotoComment>>(blah);



                    }
                }



            }
            //returning the employee list to view  
            return View(photoDetails);
        }



        [Route("/sia/albums")]
        public async Task<ActionResult> Albums()
        {

            SIAViewModel viewModel = new SIAViewModel();


            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));





                //albums
                HttpResponseMessage albums = await client.GetAsync("v1/GetAlbumInfo/");
                var albumResponse = albums.Content.ReadAsStringAsync().Result;
                viewModel.Albums = JsonConvert.DeserializeObject<List<AlbumInfo>>(albumResponse);

                foreach (var item in viewModel.Albums)
                {
                    HttpResponseMessage albumsphotos = await client.GetAsync("v1/GetAlbumPhoto/?id=" + item.albumidno);
                    var albumphotosResponse = albumsphotos.Content.ReadAsStringAsync().Result;
                    item.AlbumPhotos = JsonConvert.DeserializeObject<List<AlbumPhoto>>(albumphotosResponse);

                    foreach (var photoItem in item.AlbumPhotos)
                    {
                        photoItem.photoPath = "http://interactive.stockport.gov.uk/stockportimagearchive/SIA/" +
                                              photoItem.photograph.Trim() + ".jpg";
                    }
                }


                //returning the employee list to view  
                return View(viewModel);
            }
        }

    }
}





