using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.GenericRepository;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Repository.Repositories;
using Talabat.Services;
using Talabat.WebAPI.Errors;
using Talabat.WebAPI.Utilities;

namespace Talabat.WebAPI.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton<ICachingService, CachingService>();
            // Comment this after refactoring all the controllers to depend directly on the IUnitOfWork instead IGenericRepository<T>
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            Services.AddAutoMapper(typeof(MappingProfiles));

            // ApiBehaviorOptions -> are the options of the api controllers
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                /*
                 It's Func<ActionContext, IActionResult>:
                 The method that it carries is fired when the model state of the parameters
                 of the api actions are invalid

                ActionContext:
                The instance that it carries represents the context of the execution of the
                api action
                */
                options.InvalidModelStateResponseFactory = (actionContext) =>
               {
                   /*
                       ActionContext.ModelState:
                       - It is a dictionary where each pair represents an action parameter & its value
                       - The value is an instance that has the [Errors] property which is
                         an IEnumerable<SomeType>
                       - This SomeType has the ErrorMessage property that carries the message of that error
                    */
                   var errors = actionContext.ModelState
                   .Where(P => P.Value.Errors.Count() != 0)
                   .SelectMany(P => P.Value.Errors)
                   .Select(E => E.ErrorMessage).ToList();

                   return new BadRequestObjectResult(new ValidationErrorReturn(errors));
               };
            });

            Services.AddScoped<ITokenProvider, TokenProvider>();

            Services.AddScoped(typeof(IOrderService),typeof(OrderService));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            return Services;
        }
    }
}
