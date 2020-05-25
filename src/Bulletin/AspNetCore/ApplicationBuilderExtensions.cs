using Microsoft.AspNetCore.Builder;

namespace Bulletin.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseBulletinBoardRoute(
            this IApplicationBuilder applicationBuilder,
            IBulletinBoard board)
        {
            var fileServerOptions = new FileServerOptions
            {
                EnableDefaultFiles = false,
                RequestPath = $"/bulletin-static/{board.GetName()}",
                FileProvider = board.GetFileProvider()
            };

            return applicationBuilder.UseFileServer(fileServerOptions);
        }
    }
}