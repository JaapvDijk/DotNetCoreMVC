using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.DataAccesPatterns
{
    public class LazyProductRepository : GenericRepository<Product>, ILazyProductRepository
    {
        public LazyProductRepository(DatabaseContext context) : base(context)
        {

        }

        //With a ProxyProduct class
        //public override IEnumerable<Product> All()
        //{
        //    return base.All().Select(product => ToProductProxy(product));
        //}

        //With a ValueHolder property
        public override IEnumerable<Product> All()
        {
            return base.All().Select(p =>
            {
                p.ProductPictureHolder = new ValueHolder<byte[]>((parameter) =>
                {
                    return ProductPictureService.GetFor(parameter.ToString());
                });

                return p;
            });
        }

        public ProductProxy ToProductProxy(Product product)
        {
            //Picture ignored twice (here and DatabaseContext onmodeluild)
            //Is ignore required?
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Product, ProductProxy>()
                    .ForMember(x => x.Picture2, opt => opt.Ignore())
            );

            return new Mapper(config).Map<ProductProxy>(product);
        }
    }
}
