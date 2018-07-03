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
    public enum RgbArrayCommandType : byte
    {
        Enable,
        Disable,
        LatchNextUpdate,
        
        Update64,
        Update32Bus0,
        Update32Bus1,

        SetOutput0,
        SetOutput1,
        SetOutput2,
        SetOutput3,
        SetOutput4,

        ClearOutput0,
        ClearOutput1,
        ClearOutput2,
        ClearOutput3,
        ClearOutput4,

        ToggleOutput0,
        ToggleOutput1,
        ToggleOutput2,
        ToggleOutput3,
        ToggleOutput4,

        PulsePeriod,
        PulseRepetitions,

        RegisterSetOutputs,
        RegisterClearOutputs,
        RegisterToggleOutputs,
        RegisterOutputs
    }

    [Description(
        "\n" +
        "Enable: Any\n" +
        "Disable: Any\n" +
        "LatchNextUpdate: Any\n" +
        "\n" +
        "Update64: Array[64][3] (U8)\n" +
        "Update32Bus0: Array[32][3] (U8)\n" +
        "Update32Bus1: Array[32][3] (U8)\n" +
        "\n" +
        "SetOutput0: Any\n" +
        "SetOutput1: Any\n" +
        "SetOutput2: Any\n" +
        "SetOutput3: Any\n" +
        "SetOutput4: Any\n" +
        "SetOutput5: Any\n" +
        "ClearOutput0: Any\n" +
        "ClearOutput1: Any\n" +
        "ClearOutput2: Any\n" +
        "ClearOutput3: Any\n" +
        "ClearOutput4: Any\n" +
        "ClearOutput5: Any\n" +
        "ToggleOutput0: Any\n" +
        "ToggleOutput1: Any\n" +
        "ToggleOutput2: Any\n" +
        "ToggleOutput3: Any\n" +
        "ToggleOutput4: Any\n" +
        "ToggleOutput5: Any\n" +
        "\n" +
        "PulsePeriod: Integer\n" +
        "PulseRepetitions: Integer\n" +
        "\n" +
        "RegisterSetOutputs: Bitmask\n" +
        "RegisterClearOutputs: Bitmask\n" +
        "RegisterToggleOutputs: Bitmask\n" +
        "RegisterOutputs: Bitmask\n"
    )]
    public class RgbArrayCommand : SelectBuilder, INamedElement
    {
        public RgbArrayCommand()
        {
            Type = RgbArrayCommandType.Enable;
        }

        string INamedElement.Name
        {
            get { return typeof(RgbArrayCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public RgbArrayCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case RgbArrayCommandType.Enable:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessEnable", new[] { expression.Type }, expression);
                case RgbArrayCommandType.Disable:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessDisable", new[] { expression.Type }, expression);
                case RgbArrayCommandType.LatchNextUpdate:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessLatchNextUpdate", new[] { expression.Type }, expression);

                case RgbArrayCommandType.Update64:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessUpdate64", null, expression);
                case RgbArrayCommandType.Update32Bus0:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessUpdate32Bus0", null, expression);
                case RgbArrayCommandType.Update32Bus1:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessUpdate32Bus1", null, expression);

                case RgbArrayCommandType.SetOutput0:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessSetOutput0", new[] { expression.Type }, expression);
                case RgbArrayCommandType.SetOutput1:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessSetOutput1", new[] { expression.Type }, expression);
                case RgbArrayCommandType.SetOutput2:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessSetOutput2", new[] { expression.Type }, expression);
                case RgbArrayCommandType.SetOutput3:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessSetOutput3", new[] { expression.Type }, expression);
                case RgbArrayCommandType.SetOutput4:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessSetOutput4", new[] { expression.Type }, expression);

                case RgbArrayCommandType.ClearOutput0:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearOutput0", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ClearOutput1:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearOutput1", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ClearOutput2:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearOutput2", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ClearOutput3:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearOutput3", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ClearOutput4:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearOutput4", new[] { expression.Type }, expression);

                case RgbArrayCommandType.ToggleOutput0:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessToggleOutput0", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ToggleOutput1:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessToggleOutput1", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ToggleOutput2:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessToggleOutput2", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ToggleOutput3:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessToggleOutput3", new[] { expression.Type }, expression);
                case RgbArrayCommandType.ToggleOutput4:
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessToggleOutput4", new[] { expression.Type }, expression);


                case RgbArrayCommandType.PulsePeriod:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessPulsePeriod", null, expression);
                case RgbArrayCommandType.PulseRepetitions:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessPulseRepetitions", null, expression);

                case RgbArrayCommandType.RegisterSetOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessRegisterSetOutputs", null, expression);
                case RgbArrayCommandType.RegisterClearOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessClearDigitalOutputs", null, expression);
                case RgbArrayCommandType.RegisterToggleOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessRegisterToggleOutputs", null, expression);
                case RgbArrayCommandType.RegisterOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(RgbArrayCommand), "ProcessRegisterOutputs", null, expression);

                default:
                    break;
            }
            return expression;
        }

        static HarpMessage ProcessEnable<TSource>(TSource input)  { return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, (1<<0), 0); }
        static HarpMessage ProcessDisable<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, (1<<1), 0); }
        static HarpMessage ProcessLatchNextUpdate<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 43, 255, (byte)PayloadType.U8, 1, 0); }

        static HarpMessage ProcessUpdate64(byte[] input)
        {
            var harpMessage = new byte[192+6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192+6-2;               // Size
            harpMessage[2] = 34;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192+6-1] = 0;               // Checksum

            for (int i = 0; i < 192; i++)
            {
                if (i < input.Length)
                    harpMessage[i + 5] = input[i];
                else
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessUpdate64(byte[,] input)
        {
            var harpMessage = new byte[192 + 6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192 + 6 - 2;           // Size
            harpMessage[2] = 34;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192 + 6 - 1] = 0;           // Checksum

            for (int i = 0; i < 192 / 3; i++)
            {
                if (i < input.Length / 3)
                {
                    harpMessage[i * 3 + 5 + 0] = input[i, 0];
                    harpMessage[i * 3 + 5 + 1] = input[i, 1];
                    harpMessage[i * 3 + 5 + 2] = input[i, 2];
                }
                else
                {
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
                }
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessUpdate32Bus0(byte[] input)
        {
            var harpMessage = new byte[192/2 + 6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192 / 2 + 6 - 2;       // Size
            harpMessage[2] = 35;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192 / 2 + 6 - 1] = 0;       // Checksum

            for (int i = 0; i < 192/2; i++)
            {
                if (i < input.Length)
                    harpMessage[i + 5] = input[i];
                else
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessUpdate32Bus0(byte[,] input)
        {
            var harpMessage = new byte[192 / 2 + 6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192 / 2 + 6 - 2;       // Size
            harpMessage[2] = 35;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192 / 2 + 6 - 1] = 0;       // Checksum

            for (int i = 0; i < 192 / 2 / 3; i++)
            {
                if (i < input.Length / 3)
                {
                    harpMessage[i * 3 + 5 + 0] = input[i, 0];
                    harpMessage[i * 3 + 5 + 1] = input[i, 1];
                    harpMessage[i * 3 + 5 + 2] = input[i, 2];
                }
                else
                {
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
                }
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessUpdate32Bus1(byte[] input)
        {
            var harpMessage = new byte[192 / 2 + 6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192 / 2 + 6 - 2;       // Size
            harpMessage[2] = 36;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192 / 2 + 6 - 1] = 0;       // Checksum

            for (int i = 0; i < 192 / 2; i++)
            {
                if (i < input.Length)
                    harpMessage[i + 5] = input[i];
                else
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessUpdate32Bus1(byte[,] input)
        {
            var harpMessage = new byte[192 / 2 + 6];
            harpMessage[0] = 2;                     // Write
            harpMessage[1] = 192 / 2 + 6 - 2;       // Size
            harpMessage[2] = 36;                    // Address
            harpMessage[3] = 255;                   // Port
            harpMessage[4] = (byte)PayloadType.U8;  // Data Type
            harpMessage[192 / 2 + 6 - 1] = 0;       // Checksum

            for (int i = 0; i < 192 / 2 / 3; i++)
            {
                if (i < input.Length / 3)
                {
                    harpMessage[i * 3 + 5 + 0] = input[i, 0];
                    harpMessage[i * 3 + 5 + 1] = input[i, 1];
                    harpMessage[i * 3 + 5 + 2] = input[i, 2];
                }
                else
                {
                    // By default, Visual Studio start all variables with 0
                    return new HarpMessage(true, harpMessage);
                }
            }

            return new HarpMessage(true, harpMessage);
        }

        static HarpMessage ProcessSetOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8,   1, 0); }
        static HarpMessage ProcessSetOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8,   2, 0); }
        static HarpMessage ProcessSetOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8,   4, 0); }
        static HarpMessage ProcessSetOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8,   8, 0); }
        static HarpMessage ProcessSetOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8,  16, 0); }
        
        static HarpMessage ProcessClearOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8,   1, 0); }
        static HarpMessage ProcessClearOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8,   2, 0); }
        static HarpMessage ProcessClearOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8,   4, 0); }
        static HarpMessage ProcessClearOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8,   8, 0); }
        static HarpMessage ProcessClearOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8,  16, 0); }
        
        static HarpMessage ProcessToggleOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8,   1, 0); }
        static HarpMessage ProcessToggleOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8,   2, 0); }
        static HarpMessage ProcessToggleOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8,   4, 0); }
        static HarpMessage ProcessToggleOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8,   8, 0); }
        static HarpMessage ProcessToggleOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8,  16, 0); }

        static HarpMessage ProcessPulsePeriod(int input) {
            return new HarpMessage(true, 2, 6, 49, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessPulseRepetitions(int input) {
            return new HarpMessage(true, 2, 5, 50, 255, (byte)PayloadType.U8, (byte)input, 0);
        }

        static HarpMessage ProcessRegisterSetOutputs(byte input) {
            return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessClearDigitalOutputs(byte input) {
            return new HarpMessage(true, 2, 5, 46, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessRegisterToggleOutputs(byte input) {
            return new HarpMessage(true, 2, 5, 47, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessRegisterOutputs(byte input) {
            return new HarpMessage(true, 2, 5, 48, 255, (byte)PayloadType.U8, input, 0);
        }
    }
}
