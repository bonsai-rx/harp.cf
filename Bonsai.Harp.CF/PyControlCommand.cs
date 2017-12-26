using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    public enum PyControlCommandType : byte
    {
        UseBnc2,
        UseDio6B
    }

    [Description(
        "\n" +
        "UseBnc1: Any\n" +
        "UseDio6B: Any\n"
    )]
    public class PyControlCommand : SelectBuilder, INamedElement
    {
        public PyControlCommand()
        {
            Type = PyControlCommandType.UseBnc2;
        }

        string INamedElement.Name
        {
            get { return typeof(PyControlCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public PyControlCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case PyControlCommandType.UseBnc2:
                    return Expression.Call(typeof(PyControlCommand), "ProcessUseBnc1", new[] { expression.Type }, expression);
                case PyControlCommandType.UseDio6B:
                    return Expression.Call(typeof(PyControlCommand), "ProcessUseDio6B", new[] { expression.Type }, expression);

                default:
                    break;
            }
            return expression;
        }

        static HarpMessage ProcessUseBnc2<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 42, 255, (byte)PayloadType.U8, 1, 0);
        }

        static HarpMessage ProcessUseDio6B<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 42, 255, (byte)PayloadType.U8, 0, 0);
        }
    }
}
