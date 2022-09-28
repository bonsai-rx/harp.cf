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
    [Flags]
    public enum LedArrayIndexes : int
    {
        Index0 = 1,
        Index1 = 2
    }

    public enum LedArrayCommandType : byte
    {
        EnableSupply = 0,
        DisableSupply,

        StartBehavior,
        StopBehavior,

        EnableArray,
        DisableArray,

        SetOutput,
        ClearOutput,

        SetAuxiliaryOutput,
        ClearAuxiliaryOutput,

        WriteIntensity,
        WriteAuxiliaryIntensity,

        WritePwmFrequency,
        WritePwmDutyCycle,
        WritePwmNumOfPulses,
    }

    [Description(
        "\n" +
        "EnableSupply: Any\n" +
        "DisableSupply: Any\n" +
        "\n" +
        "StartBehavior: Any\n" +
        "StopBehavior: Any\n" +
        "\n" +
        "EnableArray: Any\n" +
        "DisableArray: Any\n" +
        "\n" +
        "SetOutput: Any\n" +
        "ClearOUtput: Any\n" +
        "\n" +
        "SetAuxiliaryOutput: Any\n" +
        "ClearAuxiliaryOutput: Any\n" +
        "\n" +
        "WriteIntensity: Integer\n" +
        "WriteAuxiliaryIntensity: Integer\n" +
        "\n" +
        "WritePwmFrequency: Float\n" +
        "WritePwmDutyCycle: Float\n" +
        "WritePwmNumOfPulses: Integer\n"
    )]

    public class LedArrayCommand : SelectBuilder, INamedElement
    {
        public LedArrayCommand()
        {
            Type = LedArrayCommandType.EnableArray;
            Mask = LedArrayIndexes.Index0;
        }

        string INamedElement.Name
        {
            get { return typeof(LedArrayCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public LedArrayCommandType Type { get; set; }
        public LedArrayIndexes Mask { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Bits                                                                 */
                /************************************************************************/
                case LedArrayCommandType.EnableSupply:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessEnableSupply", new[] { expression.Type }, expression, GetMask());
                case LedArrayCommandType.DisableSupply:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessDisableSupply", new[] { expression.Type }, expression, GetMask());

                case LedArrayCommandType.StartBehavior:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessStartBehavior", new[] { expression.Type }, expression, GetMask());
                case LedArrayCommandType.StopBehavior:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessStopBehavior", new[] { expression.Type }, expression, GetMask());

                case LedArrayCommandType.EnableArray:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessEnableArray", new[] { expression.Type }, expression, GetMask());
                case LedArrayCommandType.DisableArray:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessDisableArray", new[] { expression.Type }, expression, GetMask());

                case LedArrayCommandType.SetOutput:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessSetOutput", new[] { expression.Type }, expression, GetMask());
                case LedArrayCommandType.ClearOutput:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessClearOutput", new[] { expression.Type }, expression, GetMask());

                case LedArrayCommandType.SetAuxiliaryOutput:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessSetAuxiliaryOutput", new[] { expression.Type }, expression, GetMask());
                case LedArrayCommandType.ClearAuxiliaryOutput:
                    return Expression.Call(typeof(LedArrayCommand), "ProcessClearAuxiliaryOutput", new[] { expression.Type }, expression, GetMask());


                /************************************************************************/
                /* Intensity                                                            */
                /************************************************************************/
                case LedArrayCommandType.WriteIntensity:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LedArrayCommand), "ProcessWriteIntensity", null, expression, GetMask());
                case LedArrayCommandType.WriteAuxiliaryIntensity:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LedArrayCommand), "ProcessWriteAuxiliaryIntensity", null, expression, GetMask());

                /************************************************************************/
                /* PWM                                                                  */
                /************************************************************************/
                case LedArrayCommandType.WritePwmFrequency:
                    if (expression.Type != typeof(float)) { expression = Expression.Convert(expression, typeof(float)); }
                    return Expression.Call(typeof(LedArrayCommand), "ProcessWritePwmFrequency", null, expression, GetMask());
                case LedArrayCommandType.WritePwmDutyCycle:
                    if (expression.Type != typeof(float)) { expression = Expression.Convert(expression, typeof(float)); }
                    return Expression.Call(typeof(LedArrayCommand), "ProcessWritePwmDutyCycle", null, expression, GetMask());
                case LedArrayCommandType.WritePwmNumOfPulses:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LedArrayCommand), "ProcessWritePwmNumOfPulses", null, expression, GetMask());

                default:
                    break;
            }
            return expression;
        }
        
        /************************************************************************/
        /* Local functions                                                      */
        /************************************************************************/
        Expression GetMask()
        {
            return Expression.Convert(Expression.Constant(Mask), typeof(int));
        }

        static byte GetIndexBitMask(int bMask)
        {
            if (bMask == 0)
                throw new InvalidOperationException("Invalid Mask selection. Options are Index0 and/or Index1.");

            return (byte) bMask;
        }

        static HarpMessage createFrameU8(byte registerAddress, int content)
        {
            return new HarpMessage(true, 2, 5, registerAddress, 255, (byte)PayloadType.U8, (byte)content, 0);
        }
        static HarpMessage createFrameU16(byte registerAddress, int content)
        {
            return new HarpMessage(true, 2, 6, registerAddress, 255, (byte)PayloadType.U16, (byte)(content & 255), (byte)((content >> 8) & 255), 0);
        }
        static HarpMessage createFrameF32(byte registerAddress, float content)
        {
            var byteArray = BitConverter.GetBytes(content);
            return new HarpMessage(true, 2, 8, registerAddress, 255, (byte)PayloadType.Float, byteArray[0], byteArray[1], byteArray[2], byteArray[3], 0);
        }

        /************************************************************************/
        /* Bits                                                              */
        /************************************************************************/
        static HarpMessage ProcessEnableSupply<TSource>(TSource input, int bMask) { return createFrameU8(32, GetIndexBitMask(bMask)); }
        static HarpMessage ProcessDisableSupply<TSource>(TSource input, int bMask) { return createFrameU8(32, GetIndexBitMask(bMask) << 2); }

        static HarpMessage ProcessStartBehavior<TSource>(TSource input, int bMask) { return createFrameU8(33, GetIndexBitMask(bMask)); }
        static HarpMessage ProcessStopBehavior<TSource>(TSource input, int bMask) { return createFrameU8(33, GetIndexBitMask(bMask) << 2); }

        static HarpMessage ProcessEnableArray<TSource>(TSource input, int bMask) { return createFrameU8(34, GetIndexBitMask(bMask)); }
        static HarpMessage ProcessDisableArray<TSource>(TSource input, int bMask) { return createFrameU8(34, GetIndexBitMask(bMask) << 2); }

        static HarpMessage ProcessSetOutput<TSource>(TSource input, int bMask) { return createFrameU8(63, GetIndexBitMask(bMask)); }
        static HarpMessage ProcessClearOutput<TSource>(TSource input, int bMask) { return createFrameU8(63, GetIndexBitMask(bMask) << 2); }

        static HarpMessage ProcessSetAuxiliaryOutput<TSource>(TSource input, int bMask) { return createFrameU8(61, GetIndexBitMask(bMask)); }
        static HarpMessage ProcessClearAuxiliaryOutput<TSource>(TSource input, int bMask) { return createFrameU8(61, GetIndexBitMask(bMask) << 2); }

        /************************************************************************/
        /* Intensity                                                            */
        /************************************************************************/
        static HarpMessage ProcessWriteIntensity(int input, int bMask)
        {
            if (input < 1) input = 1;
            if (input > 120) input = 120;

            if (GetIndexBitMask(bMask) == ((byte)(LedArrayIndexes.Index0 | LedArrayIndexes.Index1)))
                throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");

            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index0) return createFrameU8(39, (byte)input);
            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index1) return createFrameU8(40, (byte)input);

            throw new InvalidOperationException("Invalid Mask selection");
        }
        static HarpMessage ProcessWriteAuxiliaryIntensity(int input, int bMask)
        {
            if (input < 1) input = 1;
            if (input > 120) input = 120;

            return createFrameU8(62, (byte)input);
        }

        /************************************************************************/
        /* PWM                                                                  */
        /************************************************************************/
        static HarpMessage ProcessWritePwmFrequency(float input, int bMask)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid frequency. Must be above 0.");

            if (GetIndexBitMask(bMask) == ((byte)(LedArrayIndexes.Index0 | LedArrayIndexes.Index1)))
                throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");

            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index0) return createFrameF32(41, input);
            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index1) return createFrameF32(49, input);

            throw new InvalidOperationException("Invalid Mask selection");
        }
        static HarpMessage ProcessWritePwmDutyCycle(float input, int bMask)
        {
            if (input < 0) throw new InvalidOperationException("Invalid duty cycle. Must be above or equal to 0.");
            if (input > 100) throw new InvalidOperationException("Invalid duty cycle. Must be bellow or equal to 100.");

            if (GetIndexBitMask(bMask) == ((byte)(LedArrayIndexes.Index0 | LedArrayIndexes.Index1)))
                throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");

            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index0) return createFrameF32(42, input);
            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index1) return createFrameF32(50, input);

            throw new InvalidOperationException("Invalid Mask selection");
        }
        static HarpMessage ProcessWritePwmNumOfPulses(int input, int bMask)
        {
            if (input > Math.Pow(2, 16) - 1) input = (int)Math.Pow(2, 16) - 1;
            if (input < 1)
                throw new InvalidOperationException("Invalid NumberOfPulses. Must be above 0.");

            if (GetIndexBitMask(bMask) == ((byte)(LedArrayIndexes.Index0 | LedArrayIndexes.Index1)))
                throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");

            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index0) return createFrameU16(41, input);
            if (GetIndexBitMask(bMask) == (byte)LedArrayIndexes.Index1) return createFrameU16(49, input);

            throw new InvalidOperationException("Invalid Mask selection");
        }
    }
}