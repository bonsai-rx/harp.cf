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

    public enum MultiPwmCommandType : byte
    {
        SetTrigger0 = 0,
        SetTrigger1,
        SetTrigger2,
        SetTrigger3,
        SetTriggers,

        ClearTrigger0,
        ClearTrigger1,
        ClearTrigger2,
        ClearTrigger3,
        ClearTriggers,

        EnableOutputs
    }

    [Description(
        "\n" +
        "SetTrigger0: Any\n" +
        "SetTrigger1: Any\n" +
        "SetTrigger2: Any\n" +
        "SetTrigger3: Any\n" +
        "SetTriggers: Bitmask\n" +      // Don't need to indicate it's a byte since the code makes the conversion
        "\n" +
        "ClearTrigger0: Any\n" +
        "ClearTrigger1: Any\n" +
        "ClearTrigger2: Any\n" +
        "ClearTrigger3: Any\n" +
        "ClearTriggers: Bitmask\n" +    // Don't need to indicate it's a byte since the code makes the conversion
        "\n" +
        "EnableOutputs: Bitmask\n"
    )]

    public class MultiPwmCommand : SelectBuilder, INamedElement
    {
        public MultiPwmCommand()
        {
            Type = MultiPwmCommandType.SetTrigger0;
        }

        string INamedElement.Name
        {
            get { return typeof(MultiPwmCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public MultiPwmCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Register: START_PWM                                                  */
                /************************************************************************/
                case MultiPwmCommandType.SetTrigger0:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessSetTrigger0", new[] { expression.Type }, expression);
                case MultiPwmCommandType.SetTrigger1:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessSetTrigger1", new[] { expression.Type }, expression);
                case MultiPwmCommandType.SetTrigger2:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessSetTrigger2", new[] { expression.Type }, expression);
                case MultiPwmCommandType.SetTrigger3:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessSetTrigger3", new[] { expression.Type }, expression);

                case MultiPwmCommandType.SetTriggers:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }     // If possible, convert to byte
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessSetTriggers", null, expression);

                /************************************************************************/
                /* Register: STOP_PWM                                                   */
                /************************************************************************/
                case MultiPwmCommandType.ClearTrigger0:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessClearTrigger0", new[] { expression.Type }, expression);
                case MultiPwmCommandType.ClearTrigger1:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessClearTrigger1", new[] { expression.Type }, expression);
                case MultiPwmCommandType.ClearTrigger2:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessClearTrigger2", new[] { expression.Type }, expression);
                case MultiPwmCommandType.ClearTrigger3:
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessClearTrigger3", new[] { expression.Type }, expression);

                case MultiPwmCommandType.ClearTriggers:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }     // If possible, convert to byte
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessClearTriggers", null, expression);

                /************************************************************************/
                /* Register: CH_ENABLE                                                  */
                /************************************************************************/
                case MultiPwmCommandType.EnableOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }     // If possible, convert to byte
                    return Expression.Call(typeof(MultiPwmCommand), "ProcessEnableOutputs", null, expression);

                default:
                    break;
            }
            return expression;
        }


        /************************************************************************/
        /* Register: START_PWM                                                  */
        /************************************************************************/
        static HarpMessage ProcessSetTrigger0<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 60, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessSetTrigger1<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 60, 255, (byte)PayloadType.U8, 2, 0);
        }
        static HarpMessage ProcessSetTrigger2<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 60, 255, (byte)PayloadType.U8, 4, 0);
        }
        static HarpMessage ProcessSetTrigger3<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 60, 255, (byte)PayloadType.U8, 8, 0);
        }
        static HarpMessage ProcessSetTriggers(byte input)
        {
            return new HarpMessage(true, 2, 5, 60, 255, (byte)PayloadType.U8, (byte)(input & 0x0F), 0);
        }

        /************************************************************************/
        /* Register: STOP_PWM                                                   */
        /************************************************************************/
        static HarpMessage ProcessClearTrigger0<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 61, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessClearTrigger1<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 61, 255, (byte)PayloadType.U8, 2, 0);
        }
        static HarpMessage ProcessClearTrigger2<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 61, 255, (byte)PayloadType.U8, 4, 0);
        }
        static HarpMessage ProcessClearTrigger3<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 61, 255, (byte)PayloadType.U8, 8, 0);
        }
        static HarpMessage ProcessClearTriggers(byte input)
        {
            return new HarpMessage(true, 2, 5, 61, 255, (byte)PayloadType.U8, (byte)(input & 0x0F), 0);
        }

        /************************************************************************/
        /* Register: CH_ENABLE                                                  */
        /************************************************************************/
        static HarpMessage ProcessEnableOutputs(byte input)
        {
            return new HarpMessage(true, 2, 5, 69, 255, (byte)PayloadType.U8, (byte)(input & 0x0F), 0);
        }
    }
}
