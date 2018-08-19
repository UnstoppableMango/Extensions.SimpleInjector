using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using SimpleInjector;
using UnMango.Extensions.SimpleInjector.Caching.Memory;
using UnMango.Extensions.SimpleInjector.Logging;

namespace UnMango.Extensions.SimpleInjector.EntityFrameworkCore
{
    /// <summary>
    ///     Extension methods for setting up Entity Framework related services in a <see cref="Container" />.
    /// </summary>
    public static partial class ContainerExtensions
    {
        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, contextLifetime, optionsLifetime, optionsAction);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, contextLifetime, Lifestyle.Scoped, optionsAction);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, Lifestyle.Scoped, Lifestyle.Scoped, optionsAction);


        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContextImplementation : DbContext, TContextService
            => AddDbContext<TContextService, TContextImplementation>(
                container,
                optionsAction == null
                    ? (Action<IServiceProvider, DbContextOptionsBuilder>)null
                    : (p, b) => optionsAction.Invoke(b), contextLifetime, optionsLifetime);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContextImplementation : DbContext, TContextService
            => AddDbContext<TContextService, TContextImplementation>(
                container,
                optionsAction == null
                    ? (Action<IServiceProvider, DbContextOptionsBuilder>)null
                    : (p, b) => optionsAction.Invoke(b), contextLifetime, Lifestyle.Scoped);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            [CanBeNull] Action<DbContextOptionsBuilder> optionsAction = null)
            where TContextImplementation : DbContext, TContextService
            => AddDbContext<TContextService, TContextImplementation>(
                container,
                optionsAction == null
                    ? (Action<IServiceProvider, DbContextOptionsBuilder>)null
                    : (p, b) => optionsAction.Invoke(b), Lifestyle.Scoped, Lifestyle.Scoped);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" /> and enables DbContext pooling.
        ///     Instance pooling can increase throughput in high-scale scenarios such as web servers by re-using
        ///     DbContext instances, rather than creating new instances for each request.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         A required action to configure the <see cref="DbContextOptions" /> for the context. When using
        ///         context pooling, options configuration must be performed externally; <see cref="DbContext.OnConfiguring" />
        ///         will not be called.
        ///     </para>
        /// </param>
        /// <param name="poolSize">
        ///     ESets the maximum number of instances retained by the pool.
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContextPool<TContext>(
            [NotNull] this Container container,
            [NotNull] Action<DbContextOptionsBuilder> optionsAction,
            int poolSize = 128)
            where TContext : DbContext
            => AddDbContextPool<TContext, TContext>(container, optionsAction, poolSize);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" /> and enables DbContext pooling.
        ///     Instance pooling can increase throughput in high-scale scenarios such as web servers by re-using
        ///     DbContext instances, rather than creating new instances for each request.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="serviceCollection"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         A required action to configure the <see cref="DbContextOptions" /> for the context. When using
        ///         context pooling, options configuration must be performed externally; <see cref="DbContext.OnConfiguring" />
        ///         will not be called.
        ///     </para>
        /// </param>
        /// <param name="poolSize">
        ///     ESets the maximum number of instances retained by the pool.
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContextPool<TContextService, TContextImplementation>(
            [NotNull] this Container serviceCollection,
            [NotNull] Action<DbContextOptionsBuilder> optionsAction,
            int poolSize = 128)
            where TContextImplementation : DbContext, TContextService
            where TContextService : class {
            Check.NotNull(optionsAction, nameof(optionsAction));

            return AddDbContextPool<TContextService, TContextImplementation>(serviceCollection, (_, ob) => optionsAction(ob), poolSize);
        }

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" /> and enables DbContext pooling.
        ///         Instance pooling can increase throughput in high-scale scenarios such as web servers by re-using
        ///         DbContext instances, rather than creating new instances for each request.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         A required action to configure the <see cref="DbContextOptions" /> for the context. When using
        ///         context pooling, options configuration must be performed externally; <see cref="DbContext.OnConfiguring" />
        ///         will not be called.
        ///     </para>
        /// </param>
        /// <param name="poolSize">
        ///     Sets the maximum number of instances retained by the pool.
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContextPool<TContext>(
            [NotNull] this Container container,
            [NotNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            int poolSize = 128)
            where TContext : DbContext
            => AddDbContextPool<TContext, TContext>(container, optionsAction);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" /> and enables DbContext pooling.
        ///         Instance pooling can increase throughput in high-scale scenarios such as web servers by re-using
        ///         DbContext instances, rather than creating new instances for each request.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         A required action to configure the <see cref="DbContextOptions" /> for the context. When using
        ///         context pooling, options configuration must be performed externally; <see cref="DbContext.OnConfiguring" />
        ///         will not be called.
        ///     </para>
        /// </param>
        /// <param name="poolSize">
        ///     Sets the maximum number of instances retained by the pool.
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContextPool<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            [NotNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            int poolSize = 128)
            where TContextImplementation : DbContext, TContextService
            where TContextService : class {
            Check.NotNull(container, nameof(container));
            Check.NotNull(optionsAction, nameof(optionsAction));

            if (poolSize <= 0) {
                throw new ArgumentOutOfRangeException(nameof(poolSize), CoreStrings.InvalidPoolSize);
            }

            CheckContextConstructors<TContextImplementation>();

            AddCoreServices<TContextImplementation>(
                container,
                (sp, ob) => {
                    optionsAction(sp, ob);

                    var extension = (ob.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension())
                        .WithMaxPoolSize(poolSize);

                    ((IDbContextOptionsBuilderInfrastructure)ob).AddOrUpdateExtension(extension);
                },
                Lifestyle.Singleton);

            container.TryRegisterSingleton(
                () => new DbContextPool<TContextImplementation>(
                    container.GetInstance<DbContextOptions<TContextImplementation>>()));

            container.Register<DbContextPool<TContextImplementation>.Lease>(Lifestyle.Scoped);

            container.Register<TContextService>(
                () => container.GetInstance<DbContextPool<TContextImplementation>.Lease>().Context,
                Lifestyle.Scoped);

            return container;
        }

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(ServiceLifetime.Scoped);
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, contextLifetime, optionsLifetime);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(ServiceLifetime.Scoped);
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            Lifestyle contextLifetime)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, contextLifetime, Lifestyle.Scoped);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(ServiceLifetime.Scoped);
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime)
            where TContextImplementation : DbContext, TContextService
            where TContextService : class
            => AddDbContext<TContextService, TContextImplementation>(
                container,
                null,
                contextLifetime,
                optionsLifetime);

        /// <summary>
        ///     Registers the given context as a service in the <see cref="Container" />.
        ///     You use this method when using dependency injection in your application, such as with ASP.NET.
        ///     For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services.AddDbContext&lt;MyContext&gt;(ServiceLifetime.Scoped);
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            Lifestyle contextLifetime)
            where TContextImplementation : DbContext, TContextService
            where TContextService : class
            => AddDbContext<TContextService, TContextImplementation>(
                container,
                null,
                contextLifetime,
                Lifestyle.Scoped);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, optionsAction, contextLifetime, optionsLifetime);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            Lifestyle contextLifetime)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, optionsAction, contextLifetime, Lifestyle.Scoped);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContext"> The type of context to be registered. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContext>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
            => AddDbContext<TContext, TContext>(container, optionsAction, Lifestyle.Scoped, Lifestyle.Scoped);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <param name="optionsLifetime"> The lifetime with which to register the DbContextOptions service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            Lifestyle contextLifetime,
            Lifestyle optionsLifetime)
            where TContextImplementation : DbContext, TContextService {
            Check.NotNull(container, nameof(container));

            if (contextLifetime == Lifestyle.Singleton) {
                optionsLifetime = Lifestyle.Singleton;
            }

            if (optionsAction != null) {
                CheckContextConstructors<TContextImplementation>();
            }

            AddCoreServices<TContextImplementation>(container, optionsAction, optionsLifetime);

            container.TryRegister(typeof(TContextService), typeof(TContextImplementation), contextLifetime);

            return container;
        }

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <param name="contextLifetime"> The lifetime with which to register the DbContext service in the container. </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            Lifestyle contextLifetime)
            where TContextImplementation : DbContext, TContextService
            => AddDbContext<TContextService, TContextImplementation>(container, optionsAction, contextLifetime, Lifestyle.Scoped);

        /// <summary>
        ///     <para>
        ///         Registers the given context as a service in the <see cref="Container" />.
        ///         You use this method when using dependency injection in your application, such as with ASP.NET.
        ///         For more information on setting up dependency injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         This overload has an <paramref name="optionsAction" /> that provides the applications <see cref="IServiceProvider" />.
        ///         This is useful if you want to setup Entity Framework to resolve its internal services from the primary application service
        ///         provider.
        ///         By default, we recommend using the other overload, which allows Entity Framework to create and maintain its own
        ///         <see cref="IServiceProvider" />
        ///         for internal Entity Framework services.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///          public void ConfigureServices(Container services)
        ///          {
        ///              var connectionString = "connection string to database";
        ///
        ///              services
        ///                  .AddEntityFrameworkSqlServer()
        ///                  .AddDbContext&lt;MyContext&gt;((serviceProvider, options) =>
        ///                      options.UseSqlServer(connectionString)
        ///                             .UseInternalServiceProvider(serviceProvider));
        ///          }
        ///      </code>
        /// </example>
        /// <typeparam name="TContextService"> The class or interface that will be used to resolve the context from the container. </typeparam>
        /// <typeparam name="TContextImplementation"> The concrete implementation type to create. </typeparam>
        /// <param name="container"> The <see cref="Container" /> to add services to. </param>
        /// <param name="optionsAction">
        ///     <para>
        ///         An optional action to configure the <see cref="DbContextOptions" /> for the context. This provides an
        ///         alternative to performing configuration of the context by overriding the
        ///         <see cref="DbContext.OnConfiguring" /> method in your derived context.
        ///     </para>
        ///     <para>
        ///         If an action is supplied here, the <see cref="DbContext.OnConfiguring" /> method will still be run if it has
        ///         been overridden on the derived context. <see cref="DbContext.OnConfiguring" /> configuration will be applied
        ///         in addition to configuration performed here.
        ///     </para>
        ///     <para>
        ///         In order for the options to be passed into your context, you need to expose a constructor on your context that takes
        ///         <see cref="DbContextOptions{TContext}" /> and passes it to the base constructor of <see cref="DbContext" />.
        ///     </para>
        /// </param>
        /// <returns>
        ///     The same service collection so that multiple calls can be chained.
        /// </returns>
        public static Container AddDbContext<TContextService, TContextImplementation>(
            [NotNull] this Container container,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
            where TContextImplementation : DbContext, TContextService
            => AddDbContext<TContextService, TContextImplementation>(container, optionsAction, Lifestyle.Scoped, Lifestyle.Scoped);

        private static void AddCoreServices<TContextImplementation>(
            Container container,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction,
            Lifestyle optionsLifetime)
            where TContextImplementation : DbContext {
            container
                .AddMemoryCache()
                .AddLogging();

            container.TryRegister(
                    typeof(DbContextOptions<TContextImplementation>),
                    () => DbContextOptionsFactory<TContextImplementation>(container, optionsAction),
                    optionsLifetime);

            container.Register(
                    typeof(DbContextOptions),
                    () => container.GetInstance<DbContextOptions<TContextImplementation>>(),
                    optionsLifetime);
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
            [NotNull] IServiceProvider applicationServiceProvider,
            [CanBeNull] Action<IServiceProvider, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));

            builder.UseApplicationServiceProvider(applicationServiceProvider);

            optionsAction?.Invoke(applicationServiceProvider, builder);

            return builder.Options;
        }

        private static void CheckContextConstructors<TContext>()
            where TContext : DbContext {
            var declaredConstructors = typeof(TContext).GetTypeInfo().DeclaredConstructors.ToList();
            if (declaredConstructors.Count == 1
                && declaredConstructors[0].GetParameters().Length == 0) {
                throw new ArgumentException(CoreStrings.DbContextMissingConstructor(typeof(TContext).ShortDisplayName()));
            }
        }
    }
}
