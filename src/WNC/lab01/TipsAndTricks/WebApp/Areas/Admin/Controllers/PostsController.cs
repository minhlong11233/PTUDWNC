using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
	public class PostsController : Controller
	{
		private readonly IMapper _mapper;
		private readonly IBlogRepository _blogRepository;
		private readonly IMediaManager _mediaManager;

		public PostsController(
			IBlogRepository blogRepository,
			IMediaManager mediaManager,
			IMapper mapper)
		{
			_blogRepository = blogRepository;
			IMediaManager mediaManager1;
			_mapper = mapper;
		}

		private async Task PopulatePostFilterModelAsync( PostFilterModel model, IBlogRepository _blogRepository)
		{
			var authors = await _blogRepository.GetAuthorsAsync();
			var categories = await _blogRepository.GetCategoriesAsync();

			model.AuthorList = authors.Select(a => new SelectListItem()
			{
				Text = a.Fullname,
				Value = a.Id.ToString()
			});

			model.CategoryList = categories.Select(c => new SelectListItem()
			{
				Text = c.Name,
				Value = c.Id.ToString()
			});
		}

		public async Task<IActionResult> Index(PostFilterModel model)
		{
			
			/*var postQuery = new PostQuery()
			{
				Keyword = model.Keyword,
				CategoryId = model.CategoryId,
				AuthorId = model.AuthorId,
				YearPost = model.Year,
				MonthPost = model.Month
			};*/
			var postQuery = _mapper.Map<PostQuery>(model);

			ViewBag.PostsList = await _blogRepository
				.GetPagedPostsAsync(postQuery, 1, 10);

			await PopulatePostFilterModelAsync(model);

			return View(model);
		}

		private Task PopulatePostFilterModelAsync(PostFilterModel model)
		{
			throw new NotImplementedException();
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id = 0)
		{
			var post = id > 0
				? await _blogRepository.GetPostByIdAsync(id, true)
				: null;

			var model = post == null
				? new PostEditModel()
				: _mapper.Map<PostEditModel>(post);

			await PopulatePostFilterModelAsync(model);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(PostEditModel model)
		{
			if (!ModelState.IsValid)
			{
				await PopulatePostFilterModelAsync(model);
				return View(model);
			}

			var post = model.Id > 0
				? await _blogRepository.GetPostByIdAsync(model.Id)
				: null;

			if (post == null)
			{
				post.Id = 0;
				post.PostedDate = DateTime.Now;
			}
			else
			{
				_mapper.Map(model, post);

				post.Category = null;
				post.ModifiedDate = DateTime.Now;
			}

			await _blogRepository.CreateOrUpdatePostAsync(
				post, model.GetSelectedTags());

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> VerifyPostSlug(
			int id, string urlSlug)
		{
			var slugExisted = await _blogRepository
				.IsPostSlugExistedAsync(id, urlSlug);

			return slugExisted
				? Json($"Slug '{urlSlug}' đã được sử dụng")
				: Json(true);
		}
	}
}
