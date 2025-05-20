using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace alps.net.api
{
    static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        private static ISet<Assembly> additionalAssemblies;

        public static IEnumerable<T> getEnumerableOfType<T>(T element) where T : class
        {
            // This is the "parent"-type, when want to find all types that extend this one
            Type typeOfBase = element.GetType();
            if (!(element is T)) return null;
            List<T> objects = new List<T>();

            // Add the assembly containing T to the set if not contained already
            Assembly sameAsContainingT = Assembly.GetAssembly(typeof(T));
            addAssemblyToCheckForTypes(sameAsContainingT);

            // Go through all the types that are in the same assembly as T
            // And through all the types that are in registered assemblies
            foreach (Assembly assembly in additionalAssemblies)
                foreach (Type type in assembly.GetTypes()
                    .Where(myType => myType.IsClass && !myType.IsAbstract && myType.BaseType == typeOfBase))
                {
                    T createdObject = createInstance<T>(type);
                    // created Object is null
                    if (createdObject != null) objects.Add(createdObject);

                }
            return objects;
        }

        public static void addAssemblyToCheckForTypes(Assembly assembly)
        {
            if (additionalAssemblies is null)
                additionalAssemblies = new HashSet<Assembly>();
            if (assembly is null) return;
            additionalAssemblies.Add(assembly);
        }


        public static T createInstance<T>(Type type)
        {
            T finalInstance = default(T);
            object[] args;
            try
            {
                args = new object[type.GetConstructors()[0].GetParameters().Length];
                int count = 0;
                foreach (ParameterInfo info in type.GetConstructors()[0].GetParameters())
                {
                    args[count] = (info.HasDefaultValue) ? info.DefaultValue : null;
                    count++;
                }
                finalInstance = (T)Activator.CreateInstance(type, args);
            }
            catch
            {
                bool worked = false;
                int num = 0;
                do
                {
                    try
                    {
                        finalInstance = (T)Activator.CreateInstance(type);
                        worked = true;
                    }
                    catch (Exception)
                    {
                        num++;
                        args = new object[num];
                        for (int i = 0; i < num; i++)
                        {
                            args[i] = null;
                        }
                    }
                } while (!worked && num < 15);
            }

            return finalInstance;
        }

    }


}
