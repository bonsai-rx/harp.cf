﻿using OpenCV.Net;
using Bonsai;
using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Text;
using TResult = System.String;

namespace Bonsai.Harp.CF
{
    public enum MultiPwmEventType : byte
    {
        /* Event: INPUTS_STATE */
        Output0 = 0,
        Output1,
        Output2,
        Output3,

        RegisterOutputs,
    }

    [Description(
        "\n" +
        "Output0: Boolean (*)\n" +
        "Output1: Boolean (*)\n" +
        "Output2: Boolean (*)\n" +
        "Output3: Boolean (*)\n" +
        "\n" +
        "RegisterOutputs: Bitmask U8\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class MultiPwmEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public MultiPwmEvent()
        {
            Type = MultiPwmEventType.Output0;
        }

        string INamedElement.Name
        {
            get { return typeof(MultiPwmEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public MultiPwmEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: EXEC_STATE                                                 */
                /************************************************************************/
                case MultiPwmEventType.RegisterOutputs:
                    return Expression.Call(typeof(MultiPwmEvent), "ProcessRegisterPwmOutputs", null, expression);

                /************************************************************************/
                /* Register: EXEC_STATE                                                 */
                /************************************************************************/
                case MultiPwmEventType.Output0:
                    return Expression.Call(typeof(MultiPwmEvent), "ProcessPwmOutput0", null, expression);
                case MultiPwmEventType.Output1:
                    return Expression.Call(typeof(MultiPwmEvent), "ProcessPwmOutput1", null, expression);
                case MultiPwmEventType.Output2:
                    return Expression.Call(typeof(MultiPwmEvent), "ProcessPwmOutput2", null, expression);
                case MultiPwmEventType.Output3:
                    return Expression.Call(typeof(MultiPwmEvent), "ProcessPwmOutput3", null, expression);

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

        static bool is_evt73(HarpMessage input) { return ((input.Address == 73) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        /************************************************************************/
        /* Register: EXEC_STATE                                                 */
        /************************************************************************/
        static IObservable<bool> ProcessPwmOutput0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt73).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessPwmOutput1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt73).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessPwmOutput2(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt73).Select(input => { return ((input.MessageBytes[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessPwmOutput3(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt73).Select(input => { return ((input.MessageBytes[11] & (1 << 3)) == (1 << 3)); }).DistinctUntilChanged();
        }

        /************************************************************************/
        /* Register: EXEC_STATE                                                 */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterPwmOutputs(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt73).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
    }
}
