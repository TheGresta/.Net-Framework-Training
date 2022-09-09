using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty()
                .MinimumLength(2)
                .Must(StartWithA);

            RuleFor(p => p.UnitPrice)
                .NotEmpty()
                .GreaterThan(0)
                .GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1);
        }

        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
