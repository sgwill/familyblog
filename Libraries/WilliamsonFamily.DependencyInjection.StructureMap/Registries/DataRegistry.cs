using StructureMap.Configuration.DSL;
using WilliamsonFamily.Models.Blog;
using WilliamsonFamily.Models.Data;
using WilliamsonFamily.Models.Family;
using WilliamsonFamily.Models.FlickrPhoto;
using WilliamsonFamily.Models.Photo;
using WilliamsonFamily.Models.User;
using WilliamsonFamily.Models.AmazonS3Media;
using WilliamsonFamily.Models.Content;
using WilliamsonFamily.Models.Data.Dapper.Content;

namespace WilliamsonFamily.DependencyInjection.StructureMap.Registries
{
    public class DataRegistry : Registry 
    {
        public DataRegistry()
        {
            For<IBlogRepository>().Use<BlogRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IFamilyRepository>().Use<FamilyRepository>();
            For<IPhotoRepository>().Use<FlickrPhotoRepository>();
            //For<IPhotoRepository>().Use<S3PhotoRepository>();
			For<IContentRepository>().Use<ContentRepository>();

            SetAllProperties(p =>
                {
                    p.OfType<IBlogRepository>();
                    p.OfType<IUserRepository>();
                    p.OfType<IFamilyRepository>();
                    p.OfType<IPhotoRepository>();
					p.OfType<IContentRepository>();
                });
        }
    }
}