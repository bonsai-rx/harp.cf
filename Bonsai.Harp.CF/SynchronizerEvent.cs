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
    public enum SynchronizerEventType : byte
    {
        /* Event: INPUTS_STATE */
        Inputs = 0,
        Input0,
        Input1,
        Input2,
        Input3,
        Input4,
        Input5,
        Input6,
        Input7,
        Input8,
        Address,

        RegisterInputs,
    }

    [Description(
        "\n" +
        "Intputs: Integer Mat[9]\n" +
        "Input0: Boolean (*)\n" +
        "Input1: Boolean (*)\n" +
        "Input2: Boolean (*)\n" +
        "Input3: Boolean (*)\n" +
        "Input4: Boolean (*)\n" +
        "Input5: Boolean (*)\n" +
        "Input6: Boolean (*)\n" +
        "Input7: Boolean (*)\n" +
        "Input8: Boolean (*)\n" +
        "Address: Integer\n" +
        "\n" +
        "RegisterInputs: INPUTS register U16\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class SynchronizerEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public SynchronizerEvent()
        {
            Type = SynchronizerEventType.Inputs;
        }

        string INamedElement.Name
        {
            get { return typeof(SynchronizerEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public SynchronizerEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: INPUTS_STATE                                               */
                /************************************************************************/
                case SynchronizerEventType.Inputs:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInputs", null, expression);
                case SynchronizerEventType.RegisterInputs:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessRegisterInputs", null, expression);

                /************************************************************************/
                /* Register: INPUTS_STATE (boolean and address)                         */
                /************************************************************************/
                case SynchronizerEventType.Input0:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput0", null, expression);
                case SynchronizerEventType.Input1:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput1", null, expression);
                case SynchronizerEventType.Input2:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput2", null, expression);
                case SynchronizerEventType.Input3:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput3", null, expression);
                case SynchronizerEventType.Input4:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput4", null, expression);
                case SynchronizerEventType.Input5:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput5", null, expression);
                case SynchronizerEventType.Input6:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput6", null, expression);
                case SynchronizerEventType.Input7:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput7", null, expression);
                case SynchronizerEventType.Input8:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessInput8", null, expression);
                case SynchronizerEventType.Address:
                    return Expression.Call(typeof(SynchronizerEvent), "ProcessAddress", null, expression);

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

        static bool is_evt32(HarpMessage input) { return ((input.Address == 32) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        /************************************************************************/
        /* Register: INPUTS_STATE                                               */
        /************************************************************************/
        static IObservable<Mat> ProcessInputs(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new byte[9];
                return source.Where(is_evt32).Select(input =>
                {
                    var inputs = BitConverter.ToUInt16(input.MessageBytes, 11);

                    for (int i = 0; i < buffer.Length; i++)
                       buffer[i] = (byte)((inputs >> i) & 1);

                    return Mat.FromArray(buffer, 9, 1, Depth.U8, 1);
                });
            });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterInputs(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => {  return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Register: INPUTS_STATE                                               */
        /************************************************************************/
        static IObservable<bool> ProcessInput0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput2(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput3(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 3)) == (1 << 3)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput4(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 4)) == (1 << 4)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput5(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 5)) == (1 << 5)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput6(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 6)) == (1 << 6)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput7(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 7)) == (1 << 7)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput8(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 8)) == (1 << 8)); }).DistinctUntilChanged();
        }

        static IObservable<int> ProcessAddress(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return (input.MessageBytes[12] >> 6) & 3; });
        }
    }
}
