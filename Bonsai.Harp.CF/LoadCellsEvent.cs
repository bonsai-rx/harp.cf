using Bonsai;
using Bonsai.Expressions;
using OpenCV.Net;
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
    public enum LoadCellsEventType : byte
    {
        Forces = 0,

        Input,

        OutputPeriodicToggle,
        Output1,
        Output2,
        Output3,
        Output4,
        Output5,
        Output6,
        Output7,
        Output8,

        RegisterOutputs
    }

    [Description(
        "\n" +
        "Forces: Integer Mat[8]\n" +
        "\n" +
        "Input: Timestamped<Boolean>\n" +
        "\n" +
        "OutputPeriodicToggle: Timestamped<Boolean>\n" +
        "\n" +
        "Output1: Boolean (*)\n" +
        "Output2: Boolean (*)\n" +
        "Output3: Boolean (*)\n" +
        "Output4: Boolean (*)\n" +
        "Output5: Boolean (*)\n" +
        "Output6: Boolean (*)\n" +
        "Output7: Boolean (*)\n" +
        "Output8: Boolean (*)\n" +
        "\n" +
        "RegisterOutputs: U16\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class LoadCellsEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public LoadCellsEvent()
        {
            Type = LoadCellsEventType.Forces;
        }

        string INamedElement.Name
        {
            get { return typeof(LoadCellsEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public LoadCellsEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: LOAD_CELLS                                                 */
                /************************************************************************/
                case LoadCellsEventType.Forces:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessForces", null, expression);

                /************************************************************************/
                /* Register: ADD_REG_DI0                                                */
                /************************************************************************/
                case LoadCellsEventType.Input:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessInput", null, expression);

                /************************************************************************/
                /* Register: ADD_REG_DO0                                                */
                /************************************************************************/
                case LoadCellsEventType.OutputPeriodicToggle:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutputPeriodicToggle", null, expression);

                /************************************************************************/
                /* Register: DO_OUT                                                     */
                /************************************************************************/
                case LoadCellsEventType.Output1:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput1", null, expression);
                case LoadCellsEventType.Output2:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput2", null, expression);
                case LoadCellsEventType.Output3:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput3", null, expression);
                case LoadCellsEventType.Output4:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput4", null, expression);
                case LoadCellsEventType.Output5:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput5", null, expression);
                case LoadCellsEventType.Output6:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput6", null, expression);
                case LoadCellsEventType.Output7:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput7", null, expression);
                case LoadCellsEventType.Output8:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessOutput8", null, expression);

                case LoadCellsEventType.RegisterOutputs:
                    return Expression.Call(typeof(LoadCellsEvent), "ProcessRegisterOutputs", null, expression);

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

        static bool is_evt33(HarpMessage input) { return ((input.Address == 33) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt34(HarpMessage input) { return ((input.Address == 34) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt35(HarpMessage input) { return ((input.Address == 35) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt45(HarpMessage input) { return ((input.Address == 45) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        /************************************************************************/
        /* Register: LOAD_CELLS                                                 */
        /************************************************************************/
        static IObservable<Mat> ProcessForces(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[8];
                return source.Where(is_evt33).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = BitConverter.ToInt16(input.MessageBytes, 11 + i * 2);
                    }

                    return Mat.FromArray(buffer, 8, 1, Depth.S16, 1);
                });
            });
        }

        /************************************************************************/
        /* Register: ADD_REG_DI0                                                */
        /************************************************************************/
        static IObservable<Timestamped<bool>> ProcessInput(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt34).Select(input => { return new Timestamped<bool>((input.MessageBytes[11] == 1), ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Register: ADD_REG_DO0                                                */
        /************************************************************************/
        static IObservable<Timestamped<bool>> ProcessOutputPeriodicToggle(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return new Timestamped<bool>((input.MessageBytes[11] == 1), ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Register: DO_OUT                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessOutput1(IObservable<HarpMessage> source) {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput2(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput3(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 3)) == (1 << 3)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput4(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 4)) == (1 << 4)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput5(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 5)) == (1 << 5)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput6(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 6)) == (1 << 6)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput7(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[11] & (1 << 7)) == (1 << 7)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessOutput8(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return ((input.MessageBytes[12] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterOutputs(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt34).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }
    }
}
