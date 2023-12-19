using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViggoQR.Models;

namespace ViggoQR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Reemplaza con tu clave de API
            string apiKey = "AIzaSyBozuSyd4R_senHX2jfvy16UyAva0TzUyc";

            // Crea la instancia del servicio de YouTube
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "ViggoQR" // Puedes poner el nombre de tu aplicación aquí
            });

            // ID del canal
            string channelId = "UCd-MBgftR7TmAzgfZhDczog";

            // Lista de videos del canal
            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
            playlistItemsListRequest.PlaylistId = "UU" + channelId.Substring(2); // Agrega "UU" al comienzo del canal ID
            playlistItemsListRequest.MaxResults = 10; //Se puede ajustar el número de videos que se desea obtener

            var playlistItemsListResponse = playlistItemsListRequest.Execute();

            // Se crea una lista de VideoModel para almacenar la información de los videos
            var videos = playlistItemsListResponse.Items.Select(playlistItem => new VideoModel
            {
                Title = playlistItem.Snippet.Title,
                FilePath = playlistItem.Snippet.ResourceId.VideoId,
                Description = playlistItem.Snippet.Description
            }).ToList();

            // Pasa la lista de videos a la vista
            return View(videos);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}