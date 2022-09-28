using OpenCV.Net;
using Bonsai;
using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using TResult = System.String;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    public enum RgbArrayEventType : byte
    {
        Updated,
        Disabled,

        Input0
    }

    [Description(
        "\n" +
        "Updated: Boolean\n" +
        "Disabled: Boolean\n" +
        "\n" +
        "Input0: Boolean\n"
    )]

    public class RgbArrayEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public RgbArrayEvent()
        {
            Type = RgbArrayEventType.Updated;
        }

        string INamedElement.Name
        {
            get { return typeof(RgbArrayEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public RgbArrayEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                case RgbArrayEventType.Updated:
                    return Expression.Call(typeof(RgbArrayEvent), "ProcessUpdated", null, expression);
                case RgbArrayEventType.Disabled:
                    return Expression.Call(typeof(RgbArrayEvent), "ProcessDisabled", null, expression);
                    
                case RgbArrayEventType.Input0:
                    return Expression.Call(typeof(RgbArrayEvent), "ProcessInput0", null, expression);

                /************************************************************************/
                /* Default                                                              */
                /************************************************************************/
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static double ParseTimestamp(byte[] message, int index)
        {
            var seconds = BitConverter.ToUInt32(message, index);
            var microseconds = BitConverter.ToUInt16(message, index + 4);
            return seconds + microseconds * 32e-6;
        }

        
        static bool is_evt32_LedsUpdated(HarpMessage input)  { return ((input.Address == 32) && (input.Error == false) && (input.MessageType == MessageType.Event) && input.MessageBytes[11] == 1); }
        static bool is_evt32_LedsDisabled(HarpMessage input) { return ((input.Address == 32) && (input.Error == false) && (input.MessageType == MessageType.Event) && input.MessageBytes[11] == 2); }
        static bool is_evt44(HarpMessage input) { return ((input.Address == 44) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        static IObservable<bool> ProcessUpdated(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32_LedsUpdated).Select(input => { return true; });
        }

        static IObservable<bool> ProcessDisabled(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32_LedsDisabled).Select(input => { return true; });
        }

        static IObservable<bool> ProcessInput0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt44).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); });
        }
    }
}
