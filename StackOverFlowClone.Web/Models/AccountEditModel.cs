using Autofac;
using AutoMapper;
using StackOverFlowClone.Infrastructure.BusinessObjects;
using StackOverFlowClone.Infrastructure.Services;
using System.ComponentModel.DataAnnotations;

namespace StackOverFlowClone.Web.Models
{
    public class AccountEditModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Image { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }
        private IMapper _mapper;
        private ILifetimeScope _scope;
        private IHttpContextAccessor? _httpContextAccessor;

        public AccountEditModel()
        {

        }

        public AccountEditModel(
            IMapper mapper,
            IHttpContextAccessor? httpContextAccessor)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public void ResolveDependency(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _httpContextAccessor = _scope.Resolve<IHttpContextAccessor>();
        }

    }
}
