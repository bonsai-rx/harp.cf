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
    public enum BehaviorEventType : byte
    {
        /* Event: POKE_IN */
        Poke0Input = 0,
        Poke1Input,
        Poke2Input,
        PokesInput,

        /* Event: ADC */
        AnalogInput,

        /* Raw Registers */
        RegisterPokesInput,
        RegisterAnalogInput,
    }

    [Description(
        "\n" +
        "Poke0Input: Boolean (*)\n" +
        "Poke1Input: Boolean (*)\n" +
        "Poke2Input: Boolean (*)\n" +
        "PokesInput: Integer Mat[3]\n" +
        "\n" +
        "AnalogInput: Decimal (V)\n" +
        "\n" +
        "RegisterPokesIn: Groupmask U8\n" +
        "RegisterAnalogInput: U16\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class BehaviorEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public BehaviorEvent()
        {
            Type = BehaviorEventType.Poke0Input;
        }

        string INamedElement.Name
        {
            get { return typeof(BehaviorEvent).Name + "." + Type.ToString(); }
        }

        public BehaviorEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: POKE_IN                                                    */
                /************************************************************************/
                case BehaviorEventType.Poke0Input:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessPoke0Input", null, expression);
                case BehaviorEventType.Poke1Input:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessPoke1Input", null, expression);
                case BehaviorEventType.Poke2Input:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessPoke2Input", null, expression);
                case BehaviorEventType.PokesInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessPokesInput", null, expression);

                /************************************************************************/
                /* Register: MISC                                                       */
                /************************************************************************/
                case BehaviorEventType.AnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessAnalogInput", null, expression);

                /************************************************************************/
                /* Raw Registers                                                        */
                /************************************************************************/
                case BehaviorEventType.RegisterPokesInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessRegisterPokesInput", null, expression);
                case BehaviorEventType.RegisterAnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessRegisterAnalogInput", null, expression);

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

        static bool is_evt32(HarpDataFrame input) { return ((input.Address == 32) && (input.Error == false) && (input.Id == MessageId.Event)); }
        static bool is_evt43(HarpDataFrame input) { return ((input.Address == 43) && (input.Error == false) && (input.Id == MessageId.Event)); }
        static bool is_evt44(HarpDataFrame input) { return ((input.Address == 44) && (input.Error == false) && (input.Id == MessageId.Event)); }

        /************************************************************************/
        /* Register: POKE_IN                                                    */
        /************************************************************************/
        static IObservable<bool> ProcessPoke0Input(IObservable<HarpDataFrame> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.Message[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessPoke1Input(IObservable<HarpDataFrame> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.Message[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessPoke2Input(IObservable<HarpDataFrame> source)
        {
            return source.Where(is_evt32).Select(input => { return ((input.Message[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
        }
        static IObservable<Mat> ProcessPokesInput(IObservable<HarpDataFrame> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new byte[3];
                return source.Where(is_evt32).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = ((input.Message[11] & (1 << i)) == (1 << i)) ? (byte)1 : (byte)0;
                    }

                    return Mat.FromArray(buffer, 3, 1, Depth.U8, 1);
                });
            });
        }

        /************************************************************************/
        /* Register: ADC                                                        */
        /************************************************************************/
        static IObservable<float> ProcessAnalogInput(IObservable<HarpDataFrame> source)
        {
            // ADC input = 2.0 V means 5.0 V on boards input
            // 4096 -> 3.3/1.6 = 2.0625 V
            // ~3972 -> 2.0 V @ ADC -> 5.0 V @ Analog input
            return source.Where(is_evt44).Select(input => { return (float)(5.0 / 3972.0) * ((int)((UInt16)(BitConverter.ToUInt16(input.Message, 11) & (UInt16)(0x0FFF)))); });
        }

        /************************************************************************/
        /* Raw Registers                                                        */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterPokesInput(IObservable<HarpDataFrame> source)
        {
            return source.Where(is_evt32).Select(input => { return new Timestamped<byte>(input.Message[11], ParseTimestamp(input.Message, 5)); });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterAnalogInput(IObservable<HarpDataFrame> source)
        {
            return source.Where(is_evt44).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.Message, 11), ParseTimestamp(input.Message, 5)); });
        }
    }
}
