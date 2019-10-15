using System;
using System.Reflection;

namespace UnityUnityDevTools.DependencyInjection
{
    /// <summary>
    /// ServiceProviderUtility.
    /// </summary>
    public static class ServiceProviderUtility
    {
        public static object CreateInstance<T>(IServiceProvider serviceProvider, params object[] args)
        {
            var instType = typeof(T);

            var constructors = instType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

            try
            {
                if (constructors.Length > 0)
                {
                    foreach (var ctor in constructors)
                    {

                        if (TryGetMethodArguments(ctor, serviceProvider, args, out object[] argValues))
                        {
                            return ctor.Invoke(argValues);
                        }
                    }
                }
                else
                {
                    if (args == null)
                    {
                        return Activator.CreateInstance(instType);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("A suitable constructor for type '{0}' could not be located. Ensure the type is concrete and services are registered for all parameters of a public constructor.", instType));
                    }
                }
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            throw new InvalidOperationException("inner error");
        }

        public static bool TryGetMethodArguments(MethodBase method, IServiceProvider serviceProvider, object[] args, out object[] argValues)
        {
            var argInfo = method.GetParameters();
            var argumentsValidated = true;

            argValues = new object[argInfo.Length];

            for (var i = 0; i < argInfo.Length; ++i)
            {
                var argType = argInfo[i].ParameterType;
                var argValue = default(object);

                // Try to match the argument using args first.
                for (var j = 0; j < args.Length; ++j)
                {
                    var arg = args[j];

                    if (arg != null && argType.IsAssignableFrom(arg.GetType()))
                    {
                        argValue = arg;
                        break;
                    }
                }

                // If argument matching failed try to resolve the argument using serviceProvider.
                if (argValue == null&& serviceProvider!=null)
                {
                    argValue = serviceProvider.GetService(argType);
                }

                // If the argument is matched/resolved, store the value, otherwise fail the constructor validation.
                if (argValue != null)
                {
                    argValues[i] = argValue;
                }
                else
                {
                    argumentsValidated = false;
                    break;
                }
            }

            // If all arguments matched/resolved, use this constructor for activation.
            if (argumentsValidated)
            {
                return true;
            }

            return false;
        }

    }
}