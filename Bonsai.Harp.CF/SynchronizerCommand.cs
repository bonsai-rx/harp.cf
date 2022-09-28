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
    public enum SynchronizerCommandType : byte
    {
        Outputs
    }

    [Description(
        "\n" +
        "Outputs: Bitmask\n"
    )]

    public class SynchronizerCommand : SelectBuilder, INamedElement
    {
        public SynchronizerCommand()
        {
            Type = SynchronizerCommandType.Outputs;
        }

        string INamedElement.Name
        {
            get { return typeof(SynchronizerCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public SynchronizerCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case SynchronizerCommandType.Outputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SynchronizerCommand), "ProcessOutputs", null, expression);

                default:
                    break;
            }
            return expression;
        }

        static HarpMessage ProcessOutputs(byte input)
        {
            return new HarpMessage(true, 2, 5, 33, 255, (byte)PayloadType.U8, input, 0);
        }
    }
}
