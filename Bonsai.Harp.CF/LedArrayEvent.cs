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
    public enum LedArrayEventType : byte
    {
        /* Event: INPUTS_STATE */
        Input0 = 0,
        Input1,

        RegisterInputs,
    }

    [Description(
        "\n" +
        "Input0: Boolean (*)\n" +
        "Input1: Boolean (*)\n" +
        "\n" +
        "RegisterInputs: Bitmask U8\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class LedArrayEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public LedArrayEvent()
        {
            Type = LedArrayEventType.Input0;
        }

        string INamedElement.Name
        {
            get { return typeof(LedArrayEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public LedArrayEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                
                /************************************************************************/
                case LedArrayEventType.Input0:
                    return Expression.Call(typeof(LedArrayEvent), "ProcessInput0", null, expression);
                case LedArrayEventType.Input1:
                    return Expression.Call(typeof(LedArrayEvent), "ProcessInput1", null, expression);

                case LedArrayEventType.RegisterInputs:
                    return Expression.Call(typeof(LedArrayEvent), "ProcessRegisterInputs", null, expression);

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

        static bool is_evt35(HarpMessage input) { return ((input.Address == 35) && (input.Error == false) && (input.MessageType == MessageType.Event)); }        

        /************************************************************************/
        /* Register: IN_STATE                                                   */
        /************************************************************************/
        static IObservable<bool> ProcessInput0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }

        static IObservable<Timestamped<byte>> ProcessRegisterInputs(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
    }
}
