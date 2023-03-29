using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace WebApp.Controllers

{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public async Task<IActionResult> Index(
			[FromQuery(Name = "k")] string keyword = null,
			[FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
          
            var postQuery = new PostQuery()
            {
                
                PublishedOnly = true,

              
                Keyword = keyword
            };

         
            var postList = await _blogRepository
                .GetPagedPostsAsync(postQuery, pageNumber, pageSize);

          
            ViewBag.PostQuery = postQuery;

           
            return View(postList);
        }

		public async Task<IActionResult> Category(
			string slug,
			[FromQuery(Name = "p")] int pageNumber = 1,
			[FromQuery(Name = "ps")] int pageSize = 10)
		{
            
            var postQuery = new PostQuery()
            {
            
                PublishedOnly = true,
                CategorySlug = slug
			};

			var postList = await _blogRepository
				.GetPagedPostsAsync(postQuery, pageNumber, pageSize);

            var category =await _blogRepository.GetCategoryFromSlugAsync(slug);
			
			ViewBag.NameCategory = category.Name;
			return View(postList);
		}

		public IActionResult About() 
            => View();

        public IActionResult Contact() 
            => View();
        public IActionResult Rss()
            => Content("Nội dung sẽ được cập nhật");

        public async Task<IActionResult> SwitchPublished(int id)
        {
            await _blogRepository.TogglePublishedFlagAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            await _blogRepository.DeletePostAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
