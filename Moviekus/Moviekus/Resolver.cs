using System;
using System.Collections.Generic;
using Autofac;

namespace Moviekus
{
    /*
     * The resolver will be responsible for creating our objects for us based on the type that we request.
     * The container property of the IContainer type is defined in Autofac and represents a container that holds the configuration on how to resolve types. 
     * The Initialize method takes an instance of an object that implements the IContainer interface and assigns it to the container property. 
     * The Resolve method uses the container to resolve a type to an instance of an object.
    */
    public static class Resolver
    {
        private static IContainer container;

        public static void Initialize(IContainer container)
        {
            Resolver.container = container;
        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
