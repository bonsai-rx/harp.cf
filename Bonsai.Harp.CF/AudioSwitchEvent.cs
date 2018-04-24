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
    public enum AudioSwitchEventType : byte
    {
        Channel = 0,

        Input0,
        Input1,
        Input2,
        Input3,
        Input4,

        RegisterChannels,
        RegisterInputs
    }

    [Description(
        "\n" +
        "Channel: Integer\n" +
        "\n" +
        "Input0: Boolean (*)\n" +
        "Input1: Boolean (*)\n" +
        "Input2: Boolean (*)\n" +
        "Input3: Boolean (*)\n" +
        "Input4: Boolean (*)\n" +
        "\n" +
        "RegisterChannels: Timestamped<Bitmask U16>\n" +
        "RegisterInputs: Timestamped<Bitmask U8>\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class AudioSwitchEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public AudioSwitchEvent()
        {
            Type = AudioSwitchEventType.Channel;
        }

        string INamedElement.Name
        {
            get { return typeof(AudioSwitchEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public AudioSwitchEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                case AudioSwitchEventType.Channel:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessChannel", null, expression);
                
                case AudioSwitchEventType.Input0:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessInput0", null, expression);
                case AudioSwitchEventType.Input1:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessInput1", null, expression);
                case AudioSwitchEventType.Input2:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessInput2", null, expression);
                case AudioSwitchEventType.Input3:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessInput3", null, expression);
                case AudioSwitchEventType.Input4:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessInput4", null, expression);

                case AudioSwitchEventType.RegisterChannels:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessRegisterChannels", null, expression);
                case AudioSwitchEventType.RegisterInputs:
                    return Expression.Call(typeof(AudioSwitchEvent), "ProcessRegisterInputs", null, expression);

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

        
        /************************************************************************/
        /* Register: CHANNEL_SEL                                                */
        /************************************************************************/
        static IObservable<int> ProcessChannel(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt33).Select(input => {
                var payload = BitConverter.ToUInt16(input.MessageBytes, 11);

                for (int i = 0; i < 16; i++)
                {
                    if (payload == (1 << i))
                    {
                        return i;
                    }
                }

                return -1;
            });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterChannels(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt33).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }


        /************************************************************************/
        /* Register: DI_STATE                                                   */
        /************************************************************************/
        static IObservable<bool> ProcessInput0(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput1(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput2(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return ((input.MessageBytes[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput3(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return ((input.MessageBytes[11] & (1 << 3)) == (1 << 3)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessInput4(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return ((input.MessageBytes[11] & (1 << 4)) == (1 << 4)); }).DistinctUntilChanged();
        }

        static IObservable<Timestamped<byte>> ProcessRegisterInputs(IObservable<HarpMessage> source) {
            return source.Where(is_evt34).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
    }
}
