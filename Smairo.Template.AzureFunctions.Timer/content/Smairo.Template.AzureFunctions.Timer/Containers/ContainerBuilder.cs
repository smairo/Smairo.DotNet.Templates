using System;
using Microsoft.Extensions.DependencyInjection;
using Smairo.Template.AzureFunctions.Timer.Modules;
namespace Smairo.Template.AzureFunctions.Timer.Containers
{
    /// <inheritdoc />
    /// <summary>
    /// This represents the builder entity for IoC container.
    /// </summary>
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBuilder"/> class.
        /// </summary>
        public ContainerBuilder()
        {
            _services = new ServiceCollection();
        }

        /// <inheritdoc />
        public IContainerBuilder RegisterModule(IModule module = null)
        {
            if (module == null) {
                module = new Module();
            }
            module.FunctionStartup(_services);
            return this;
        }

        /// <inheritdoc />
        public IServiceProvider Build()
        {
            var provider = _services.BuildServiceProvider();
            return provider;
        }
    }

    /// <summary>
    /// This provides interfaces to the <see cref="ContainerBuilder"/> class.
    /// </summary>
    public interface IContainerBuilder
    {
        /// <summary>
        /// Registers a dependency collection module.
        /// </summary>
        /// <param name="module"><see cref="IModule"/> instance.</param>
        /// <returns>Returns <see cref="IContainerBuilder"/> instance.</returns>
        IContainerBuilder RegisterModule(IModule module = null);

        /// <summary>
        /// Builds the dependency container.
        /// </summary>
        /// <returns>Returns <see cref="IServiceProvider"/> instance.</returns>
        IServiceProvider Build();
    }
}