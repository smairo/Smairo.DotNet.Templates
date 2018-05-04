using Microsoft.Extensions.DependencyInjection;
namespace Smairo.Template.AzureFunctions.Timer.Modules
{
    /// <summary>
    /// This represents the entity containing a list of dependencies.
    /// </summary>
    public class Module : IModule
    {
        /// <inheritdoc />
        public virtual void FunctionStartup(IServiceCollection services)
        {
            return;
        }
    }

    /// <summary>
    /// This provides interfaces to the <see cref="Module"/> class.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Loads dependencies to the collection.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance.</param>
        void FunctionStartup(IServiceCollection services);
    }
}