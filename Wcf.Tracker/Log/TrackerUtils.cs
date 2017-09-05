using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

namespace Wcf.Tracker.Log
{
    /// <summary>
    /// Contains methods required for info gathering.
    /// </summary>
    internal static class TrackerUtils
    {
        /// <summary>
        /// Get current stack trace.
        /// </summary>
        /// <returns>Stack trace string.</returns>
        public static string GetStackTrace(int skipDepth)
        {
            var stackTrace = new StackTrace();

            // StackTrace.ToString()
            bool hasOffset = true;
            bool isFirstStackLine = true;
            bool wasExternalMethodBefore = false;
            const string AtPrefix = "at";
            var buffer = new StringBuilder(byte.MaxValue);
            for (int i = 1 + skipDepth; i < stackTrace.FrameCount; ++i)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame.GetMethod();
                if (method == null)
                {
                    continue;
                }

                bool isExternalMethod = false;
                if (method.DeclaringType != null)
                {
                    isExternalMethod = IsExternalMethod(method.DeclaringType.FullName);
                }

                if (isExternalMethod)
                {
                    if (wasExternalMethodBefore && !isFirstStackLine)
                    {
                        buffer.Append(Environment.NewLine);
                        buffer.Append("...");
                        wasExternalMethodBefore = false;
                    }

                    if (isFirstStackLine)
                    {
                        isFirstStackLine = false;
                    }
                    else
                    {
                        buffer.Append(Environment.NewLine);
                    }

                    buffer.AppendFormat("{0}  ", AtPrefix);
                    Type declaringType = method.DeclaringType;
                    if (declaringType != null)
                    {
                        buffer.Append(declaringType.FullName.Replace('+', '.'));
                        buffer.Append(".");
                    }

                    buffer.Append(method.Name);
                    if (method is MethodInfo && method.IsGenericMethod)
                    {
                        var genericArguments = method.GetGenericArguments();
                        buffer.Append("[");
                        int j = 0;
                        bool first = true;
                        for (; j < genericArguments.Length; ++j)
                        {
                            if (!first)
                            {
                                buffer.Append(",");
                            }
                            else
                            {
                                first = false;
                            }

                            buffer.Append(genericArguments[j].Name);
                        }

                        buffer.Append("]");
                    }

                    buffer.Append("(");
                    var parameters = method.GetParameters();
                    bool firstParam = true;
                    for (int j = 0; j < parameters.Length; ++j)
                    {
                        if (!firstParam)
                        {
                            buffer.Append(", ");
                        }
                        else
                        {
                            firstParam = false;
                        }

                        var paramName = parameters[j].ParameterType.Name;
                        buffer.Append(paramName + " " + parameters[j].Name);
                    }

                    buffer.Append(")");
                    if (hasOffset && frame.GetILOffset() != -1)
                    {
                        string fileName = null;
                        try
                        {
                            fileName = frame.GetFileName();
                        }
                        catch (NotSupportedException)
                        {
                            hasOffset = false;
                        }
                        catch (SecurityException)
                        {
                            hasOffset = false;
                        }

                        if (fileName != null)
                        {
                            buffer.Append(' ');
                            buffer.AppendFormat("in {0}:line {1}", fileName, frame.GetFileLineNumber());
                        }
                    }
                }
                else
                {
                    wasExternalMethodBefore = true;
                }
            }

            buffer.Append(Environment.NewLine);
            return buffer.ToString();
        }

        private static bool IsExternalMethod(string name)
        {
            var externalMicrosoftNamespaces = new[]
            {
                "System.",
                "MS.",
                "Microsoft."
            };

            return externalMicrosoftNamespaces.All(@namespace => !name.StartsWith(@namespace));
        }
    }
}