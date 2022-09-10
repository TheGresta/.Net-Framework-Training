using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("product.add,admin")]
        [CacheRemoveAspect("IProductService.Get")]
        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRule.Run(
                CheckIfProductNameAlreadyExist(product.ProductName),
                CheckIfProductCauntOfCategoryCorrect(product.CategoryId),
                CheckCategoryCaunt());

            if (result != null)
                return result;

            _productDal.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }
        
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            var categoryList = _productDal.GetAll(x => x.CategoryId == product.CategoryId).ToList();
            if (categoryList.Count >= 10)
                return new ErrorResult(Messages.ProductCountOfCategoryError);

            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(x => x.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        private IResult CheckIfProductCauntOfCategoryCorrect(int categoryId)
        {
            var categoryList = _productDal.GetAll(x => x.CategoryId == categoryId);
            if (categoryList.Count >= 10)
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            return new SuccessResult();
        }

        private IResult CheckIfProductNameAlreadyExist(string productName)
        {
            var result = _productDal.GetAll(x => x.ProductName == productName).Any();
            if (result)
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            return new SuccessResult();
        }

        private IResult CheckCategoryCaunt()
        {
            var result = _categoryService.GetAll().Data.Count;
            if (result > 15)
                return new ErrorResult(Messages.CategoryCountOutOfLimit);
            return new SuccessResult();
        }
    }
}
