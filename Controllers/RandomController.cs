using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomController : ControllerBase
    {
        private IRandomService _randomServiceSingleton;
        private IRandomService _randomServiceScope;
        private IRandomService _randomServiceTransient;

        private IRandomService _randomService2Singleton;
        private IRandomService _randomService2Scope;
        private IRandomService _randomService2Transient;

        public RandomController(
            [FromKeyedServices("randomSingleton")] IRandomService randomServiceSingleton,
            [FromKeyedServices("randomScoped")] IRandomService randomServiceScoped,
            [FromKeyedServices("randomTransient")] IRandomService randomServiceTransient,

            [FromKeyedServices("randomSingleton")] IRandomService randomService2Singleton,
            [FromKeyedServices("randomScoped")] IRandomService randomService2Scoped,
            [FromKeyedServices("randomTransient")] IRandomService randomService2Transient
        )
        {
            _randomServiceScope = randomServiceScoped;
            _randomService2Scope = randomService2Scoped;
            _randomServiceSingleton = randomServiceSingleton;
            _randomService2Singleton = randomService2Singleton;
            _randomServiceTransient = randomServiceTransient;
            _randomService2Transient = randomService2Transient;
        }
        [HttpGet]
        public ActionResult<Dictionary<string, int>> Get()
        {
            var result = new Dictionary<string, int>();
            result.Add("Singleton 1", _randomServiceSingleton.Value);
            result.Add("Singleton 2", _randomService2Singleton.Value);
            result.Add("Scope 1", _randomServiceScope.Value);
            result.Add("Scope 2", _randomService2Scope.Value);
            result.Add("Transient 1", _randomServiceTransient.Value);
            result.Add("Transient 2", _randomService2Transient.Value);
            return result;
        }
    }
}
