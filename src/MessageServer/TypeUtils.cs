using System;
using System.Collections.Generic;

namespace MessageServer
{
    public class TypeUtils
    {
        internal static bool TypesMatch(List<Type> matchingMessageTypes, Type messageType)
        {
            for (var i = 0; i < matchingMessageTypes.Count; i++)
            {
                if (TypesMatch(messageType, matchingMessageTypes[i]))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool TypesMatch(Type messageType, Type targetType)
        {
            return messageType == targetType || messageType.IsSubclassOf(targetType);
        }
    }
}
