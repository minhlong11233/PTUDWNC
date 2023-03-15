using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Services.Media
{
	public interface IMediaManager
	{
		Task<string> SaveFileAsync(
			Stream buffer,
			string originalFileName,
			string contextType,
			CancellationToken cancellationToken = default);
		
		Task<bool> DeleteFileAsync(
			string filePath,
			CancellationToken cancellationToken= default);	
			
	}
}
