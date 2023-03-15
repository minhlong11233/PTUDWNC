using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TatBlog.WebApp.Areas.Admin.Models
{
	public class PostEditModel
	{
		public int Id { get; set; }

		[DisplayName("Tiêu đề")]
		[Required(ErrorMassage = "tiêu đề không được để trống")]
		[MaxLength(500, ErrorMessage = "Tiêu đề tối đa 500 kí tự")]
		public string Title { get; set; }

		[DisplayName("Giới thiệu")]
		[Required(ErrorMassage = "Giới thiệu không được để trống")]
		[MaxLength(2000, ErrorMessage = "Giới thiệu tối đa 2000 ký tự")]
		public string shortDescription { get; set; }


		[DisplayName("Nội dung")]
		[Required(ErrorMassage = "Nội dung không được để trống")]
		[MaxLength(5000, ErrorMessage = "Nội dung tối đá 5000 ký tự")]
		public string Description { get; set; }


		[DisplayName("Metadata")]
		[Required(ErrorMassage = "Metadata không được để trống")]
		[MaxLength(1000, ErrorMessage = "Metadata tối đa 1000 ký tự")]
		public string Meta { get; set; }


		[DisplayName("Slug")]
		[Remote("VerifyPostSlug", "Posts", "Admin",
			HttpMethod = "POST", AdditionalFields = "Id")]
		[Required(ErrorMassage = "URL slug không được để trống")]
		[MaxLength(200, ErrorMessage = "Slug tối đa 200 ký tự")]
		public string UrlSlug { get; set; }

		[DisplayName("Chọn hình ảnh")]
		public IFormFile ImageFile { get; set; }

		[DisplayName("Hình hiện tại")]
		public string ImageUrl { get; set; }

		[DisplayName("Xuất bản ngay")]
		public bool Published { get; set; }

		[DisplayName("Chủ đề")]
		[Required(ErrorMassage = "Bạn chưa chọn chủ đề")]
		public int CategoryId { get; set; }

		[DisplayName("Tác giả")]
		[Required(ErrorMassage = "Bạn chưa chọn chủ đề")]
		public int AuthorID { get; set; }

		[DisplayName("Từ khóa (mỗi từ 1 dòng)")]
		[Required(ErrorMassage = "Bạn chưa nhập tên thẻ")]
		public string SelectedTag { get; set; }

		public IEnumerable<SelectListItem> AuthorList { get; set; }
		public IEnumerable<SelectListItem> CategoryList { get; set; }

		public List<string> GetSelectedTags()
		{
			return (SelectedTag ?? "")
				.Split(new[] { ',', ',', '\r', '\n' },
					StringSplitOptions.RemoveEmptyEntries)
				.ToList();
		}
	}
}
