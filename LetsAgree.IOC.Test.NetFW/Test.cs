﻿using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LetsAgree.IOC.Test.NetFW
{
    [TestFixture]
    public class UseTest
    {
        public abstract class MyLibrary
        {
            static ICustomTypeProvider myIOCConstructedClassHonestly;
            // Repeating constraints is annoying, but it allows users to send 3rd party DI frameworks to 3rd party Libraries without any fuss.
            // (generic paramaters on classes cannot be inferred)
            public static MyLibrary Create<R, CBasic, CGen, T>(Func<R> c)
                where R : IDynamicRegistration<CBasic>, IGenericRegistration<CGen>, IContainerGeneration<T>
                where CBasic : ISingletonConfig
                where CGen : ISingletonConfig, IDecoratorConfig
                where T : IBasicContainer, IGenericContainer
            {
                return new MYDiClass<R, CBasic, CGen, T>(c);
            }
            class MYDiClass<R, CBasic, CGen, T> : MyLibrary
                where R : IDynamicRegistration<CBasic>, IGenericRegistration<CGen>, IContainerGeneration<T>
                where CBasic : ISingletonConfig
                where CGen : ISingletonConfig, IDecoratorConfig
                where T : IBasicContainer, IGenericContainer
            {
                public MYDiClass(Func<R> creator)
                {
                    // And then doing some random stuff
                    var reg = creator();
                    reg.Register<IList, ArrayList>();
                    reg.Register(typeof(string), typeof(Assembly));
                    var ct = reg.GenerateContainer();
                    ct.TryResolve(out int lol);
                    lol += 1;
                    ct.TryResolve(out myIOCConstructedClassHonestly);
                }
            }
        }

        public abstract class MyUnspecificLibrary
        {
            static ICustomTypeProvider myIOCConstructedClassHonestly;
            // Repeating constraints is annoying, but it allows users to send 3rd party DI frameworks to 3rd party Libraries without any fuss.
            // (generic paramaters on classes cannot be inferred)
            public static MyLibrary Create<R, C, T>(Func<R> c)
                where R : IDynamicRegistration<C>, IGenericRegistration<C>, IContainerGeneration<T>
                where C : ISingletonConfig, IDecoratorConfig
                where T : IBasicContainer, IGenericContainer
            {
                return new MYDiClass<R, C, T>(c);
            }
            class MYDiClass<R, C, T> : MyLibrary
                where R : IDynamicRegistration<C>, IGenericRegistration<C>, IContainerGeneration<T>
                where C : ISingletonConfig, IDecoratorConfig
                where T : IBasicContainer, IGenericContainer
            {
                public MYDiClass(Func<R> creator)
                {
                    // And then doing some random stuff
                    var reg = creator();
                    reg.Register<IEnumerable<int>, List<int>>();
                    reg.Register(typeof(string), typeof(Assembly));
                    var ct = reg.GenerateContainer();
                    ct.TryResolve(out int lol);
                    lol += 1;
                    ct.TryResolve(out myIOCConstructedClassHonestly);
                }
            }
        }

        [Test]
        public void Use()
        {
            // Can we avoid specifying the specs? Thats annoying for a user.  (one way was to not allow different C and T for each R)
            Assert.Throws<NotImplementedException>(() => MyLibrary.Create<IRegistrySpec, IConfigSpec, IConfigSpec, IContainerSpec>(() => new MyRegistry()));
            Assert.Throws<NotImplementedException>(() => MyUnspecificLibrary.Create<IRegistrySpec, IConfigSpec, IContainerSpec>(() => new MyRegistry()));
        }

        // Implimentation capabilities
        public interface IConfigSpec : 
            ISingletonConfig, 
            IDecoratorConfig
        {
        }
        public interface IContainerSpec :
            IBasicContainer,
            IGenericContainer
        {
        }
        public interface IRegistrySpec :
            IDynamicRegistration<IConfigSpec>,
            IGenericRegistration<IConfigSpec>,
            IContainerGeneration<IContainerSpec>
        {
        }

        // Implimentation
        class MyRegistry : IRegistrySpec
        {
            public IContainerSpec GenerateContainer()
            {
                throw new NotImplementedException();
            }

            public IConfigSpec Register(Type service, Type impl)
            {
                throw new NotImplementedException();
            }

            public IConfigSpec Register(Type service, Func<object> creator)
            {
                throw new NotImplementedException();
            }

            public IConfigSpec Register<Service, Implimentation>() where Implimentation : Service
            {
                throw new NotImplementedException();
            }

            public IConfigSpec Register<Service>(Func<Service> implimentation)
            {
                throw new NotImplementedException();
            }
        }
        class MyContainer : IContainerSpec
        {
            public object Resolve(Type t)
            {
                throw new NotImplementedException();
            }

            public T Resolve<T>()
            {
                throw new NotImplementedException();
            }

            public bool TryResolve(Type t, out object service)
            {
                throw new NotImplementedException();
            }

            public bool TryResolve<T>(out T service)
            {
                throw new NotImplementedException();
            }
        }
        class MyConfig : IConfigSpec
        {
            public void AsDecorator()
            {
                throw new NotImplementedException();
            }

            public void AsSingleton()
            {
                throw new NotImplementedException();
            }
        }
    }
}
