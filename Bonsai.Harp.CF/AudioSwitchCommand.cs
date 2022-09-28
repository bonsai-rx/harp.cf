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
    public enum AudioSwitchCommandType : byte
    {
        Channel,
        DisableChannels,
        SetOutput0,
        ClearOutput0,

        RegisterChannels,
    }

    [Description(
        "\n" +
        "Channel: Integer\n" +
        "DisableChannels: Any\n" +
        "\n" +
        "SetOutput0: Any\n" +
        "ClearOutput0: Any\n" +
        "\n" +
        "RegisterChannels: Bitmask\n"
    )]
    public class AudioSwitchCommand : SelectBuilder, INamedElement
    {
        public AudioSwitchCommand()
        {
            Type = AudioSwitchCommandType.Channel;
        }

        string INamedElement.Name
        {
            get { return typeof(AudioSwitchCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public AudioSwitchCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case AudioSwitchCommandType.Channel:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(AudioSwitchCommand), "ProcessChannel", null, expression);

                case AudioSwitchCommandType.DisableChannels:
                    return Expression.Call(typeof(AudioSwitchCommand), "ProcessDisableChannels", new[] { expression.Type }, expression);

                case AudioSwitchCommandType.SetOutput0:
                    return Expression.Call(typeof(AudioSwitchCommand), "ProcessSetOutput0", new[] { expression.Type }, expression);
                case AudioSwitchCommandType.ClearOutput0:
                    return Expression.Call(typeof(AudioSwitchCommand), "ProcessClearOutput0", new[] { expression.Type }, expression);

                case AudioSwitchCommandType.RegisterChannels:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(AudioSwitchCommand), "ProcessRegisterChannels", null, expression);

                default:
                    break;
            }
            return expression;
        }
        

        static HarpMessage ProcessChannel(int input) {
            var payload = (1 << input);
            return new HarpMessage(true, 2, 6, 33, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(payload) & 255), (byte)((Convert.ToUInt16(payload) >> 8) & 255), 0);
        }

        static HarpMessage ProcessDisableChannels<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 33, 255, (byte)PayloadType.U16, 0, 0, 0); }

        static HarpMessage ProcessSetOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 35, 255, (byte)PayloadType.U8, 1, 0); }
        static HarpMessage ProcessClearOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 35, 255, (byte)PayloadType.U8, 0, 0); }

        static HarpMessage ProcessRegisterChannels(UInt16 input)
        {
            return new HarpMessage(true, 2, 6, 33, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
    }
}
