using Microsoft.AspNetCore.Builder;

namespace Bulletin.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder UseBulletinBoardRoute(
            this IApplicationBuilder applicationBuilder,
            IBulletinBoard store)
        {
            // var fileServerOptions = new FileServerOptions
            // {
            //     EnableDefaultFiles = false,
            //     RequestPath = store.GetRoutePrefix(),
            //     FileProvider = store.GetFileProvider()
            // };
            //
            // return applicationBuilder.UseFileServer(fileServerOptions);

            return applicationBuilder;
        }
    }
}