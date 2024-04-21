using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SuperTorus.Application.DTO;

namespace SuperTorus.Application.Validation
{
    public class RequestDataValidator : AbstractValidator<RequestData>
    {
        public RequestDataValidator() 
        {
            RuleFor(x => x.A).GreaterThan(0);
            RuleFor(x => x.Thickness).GreaterThan(0).LessThan(x => x.MinRadius);
            RuleFor(x => x.MinRadius).GreaterThan(0);
            RuleFor(x => x.MaxRadius).GreaterThan(0);
            RuleFor(x => x.MinRadius).LessThan(x => x.MaxRadius);
            RuleFor(x => x.MaxRadius).LessThan(x => x.A);
            RuleFor(x => x.MinRadius).LessThan(x => x.A);


        }
    }
}
