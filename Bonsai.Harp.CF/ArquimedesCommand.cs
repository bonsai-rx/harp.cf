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
    public enum ArquimedesCommandType : byte
    {
        LoadPosition,

        ResetLeverAngle,
        ResetLoadPosition,
        HideLever,
        UnhideLever,

        DigitalOutput0,
        DigitalOutput1,
        DigitalOutput2,
        DigitalOutput3,
        DigitalOutput4,
        DigitalOutput5,
        DigitalOutputsSet,
        DigitalOutputsClear,

        LedConfig0,
        LedConfig1,
        LedConfig2,
        LedConfig3,
        LedConfig4,
        LedConfig5,
        LedConfig6,
        LedConfig7,
        LedConfigsSet,
        LedConfigsClear,

        ColorsOfLeds
    }

    [Description(
        "\n" +
        "LoadPosition: Positive integer\n" +
        "\n" +
        "ResetLeverAngle: Any\n" +
        "ResetLeverAngle: Any\n" +
        "HideLever: Boolean\n" +
        "UnhideLever: Boolean\n" +
        "\n" +
        "DigitalOutput0: Boolean\n" +
        "DigitalOutput1: Boolean\n" +
        "DigitalOutput2: Boolean\n" +
        "DigitalOutput3: Boolean\n" +
        "DigitalOutput4: Boolean\n" +
        "DigitalOutput5: Boolean\n" +
        "DigitalOutputsSet: Bitmask\n" +
        "DigitalOutputsClear: Bitmask\n" +
        "\n" +
        "LedConfig0: Boolean\n" +
        "LedConfig1: Boolean\n" +
        "LedConfig2: Boolean\n" +
        "LedConfig3: Boolean\n" +
        "LedConfig4: Boolean\n" +
        "LedConfig5: Boolean\n" +
        "LedConfig6: Boolean\n" +
        "LedConfig7: Boolean\n" +
        "LedConfigsSet: Bitmask\n" +    // Don't need to indicate it's a byte since the code makes the conversion
        "LedConfigsClear: Bitmask\n" +  // Don't need to indicate it's a byte since the code makes the conversion
        "\n" +
        "ColorsOfLeds: Positive integer array[9] (G,R,B,G,R,B,G,R,B)\n"
    )]

    public class ArquimedesCommand : SelectBuilder, INamedElement
    {
        public ArquimedesCommand()
        {
            Type = ArquimedesCommandType.LoadPosition;
        }

        string INamedElement.Name
        {
            get { return typeof(ArquimedesCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public ArquimedesCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Register: POS_TARGET                                                 */
                /************************************************************************/
                case ArquimedesCommandType.LoadPosition:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLoadPosition", null, expression);

                /************************************************************************/
                /* Registers: RESET_ANGLE, RESET_MOTOR, HIDE_LEVER                      */
                /************************************************************************/
                case ArquimedesCommandType.ResetLeverAngle:
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessResetLeverAngle", new[] { expression.Type }, expression);
                case ArquimedesCommandType.ResetLoadPosition:
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessResetLoadPosition", new[] { expression.Type }, expression);
                case ArquimedesCommandType.HideLever:
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessHideLever", new[] { expression.Type }, expression);
                case ArquimedesCommandType.UnhideLever:
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessUnhideLever", new[] { expression.Type }, expression);

                /************************************************************************/
                /* Registers: SET_DOUTS, CLR_DOUTS                                      */
                /************************************************************************/
                case ArquimedesCommandType.DigitalOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput0", null, expression);
                case ArquimedesCommandType.DigitalOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput1", null, expression);
                case ArquimedesCommandType.DigitalOutput2:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput2", null, expression);
                case ArquimedesCommandType.DigitalOutput3:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput3", null, expression);
                case ArquimedesCommandType.DigitalOutput4:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput4", null, expression);
                case ArquimedesCommandType.DigitalOutput5:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutput5", null, expression);

                case ArquimedesCommandType.DigitalOutputsSet:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutputsSet", null, expression);
                case ArquimedesCommandType.DigitalOutputsClear:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessDigitalOutputsClear", null, expression);

                /************************************************************************/
                /* Registers: EN_LED_CONFS, DIS_LED_CONFS                               */
                /************************************************************************/
                case ArquimedesCommandType.LedConfig0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig0", null, expression);
                case ArquimedesCommandType.LedConfig1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig1", null, expression);
                case ArquimedesCommandType.LedConfig2:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig2", null, expression);
                case ArquimedesCommandType.LedConfig3:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig3", null, expression);
                case ArquimedesCommandType.LedConfig4:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig4", null, expression);
                case ArquimedesCommandType.LedConfig5:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig5", null, expression);
                case ArquimedesCommandType.LedConfig6:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig6", null, expression);
                case ArquimedesCommandType.LedConfig7:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfig7", null, expression);

                case ArquimedesCommandType.LedConfigsSet:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfigsSet", null, expression);
                case ArquimedesCommandType.LedConfigsClear:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessLedConfigsClear", null, expression);

                /************************************************************************/
                /* Register: LEDS                                                       */
                /************************************************************************/
                case ArquimedesCommandType.ColorsOfLeds:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(ArquimedesCommand), "ProcessColorsOfLeds", null, expression);

                default:
                    break;
            }
            return expression;
        }

        /************************************************************************/
        /* Register: POS_TARGET                                                  */
        /************************************************************************/
        static HarpMessage ProcessLoadPosition(UInt16 input)
        {
            return new HarpMessage(true, 2, 6, 56, 255, (byte)PayloadType.U16, (byte)(input & 255), (byte)((input >> 8) & 255), 0);
        }

        /************************************************************************/
        /* Registers: RESET_ANGLE, RESET_MOTOR, HIDE_LEVER                        */
        /************************************************************************/
        static HarpMessage ProcessResetLeverAngle<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 39, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessResetLoadPosition<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 40, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessHideLever(bool input)
        {
            return new HarpMessage(true, 2, 5, 41, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessUnhideLever(bool input)
        {
            return new HarpMessage(true, 2, 5, 41, 255, (byte)PayloadType.U8, 0, 0);
        }

        /************************************************************************/
        /* Registers: SET_DOUTS, CLR_DOUTS                                      */
        /************************************************************************/
        static HarpMessage CreateHarpFrameForDigitalOutputs (bool toHigh, int DigitalOutputNumber)
        {
            if (toHigh)
                return new HarpMessage(true, 2, 5, 42, 255, (byte)PayloadType.U8, (byte)(1 << DigitalOutputNumber), 0);
            else
                return new HarpMessage(true, 2, 5, 43, 255, (byte)PayloadType.U8, (byte)(1 << DigitalOutputNumber), 0);
        }
        static HarpMessage ProcessDigitalOutput0(bool input) { return CreateHarpFrameForDigitalOutputs(input, 0); }
        static HarpMessage ProcessDigitalOutput1(bool input) { return CreateHarpFrameForDigitalOutputs(input, 1); }
        static HarpMessage ProcessDigitalOutput2(bool input) { return CreateHarpFrameForDigitalOutputs(input, 2); }
        static HarpMessage ProcessDigitalOutput3(bool input) { return CreateHarpFrameForDigitalOutputs(input, 3); }
        static HarpMessage ProcessDigitalOutput4(bool input) { return CreateHarpFrameForDigitalOutputs(input, 4); }
        static HarpMessage ProcessDigitalOutput5(bool input) { return CreateHarpFrameForDigitalOutputs(input, 5); }

        static HarpMessage ProcessDigitalOutputsSet(byte input)
        {
            return new HarpMessage(true, 2, 5, 42, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessDigitalOutputsClear(byte input)
        {
            return new HarpMessage(true, 2, 5, 43, 255, (byte)PayloadType.U8, input, 0);
        }

        /************************************************************************/
        /* Registers: EN_LED_CONFS, DIS_LED_CONFS                                 */
        /************************************************************************/
        static HarpMessage CreateHarpFrameForLeds(bool toEnable, int ledNumber)
        {
            if (toEnable)
                return new HarpMessage(true, 2, 5, 44, 255, (byte)PayloadType.U8, (byte)(1 << ledNumber), 0);
            else
                return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8, (byte)(1 << ledNumber), 0);
        }
        static HarpMessage ProcessLedConfig0(bool input) { return CreateHarpFrameForLeds(input, 0); }
        static HarpMessage ProcessLedConfig1(bool input) { return CreateHarpFrameForLeds(input, 1); }
        static HarpMessage ProcessLedConfig2(bool input) { return CreateHarpFrameForLeds(input, 2); }
        static HarpMessage ProcessLedConfig3(bool input) { return CreateHarpFrameForLeds(input, 3); }
        static HarpMessage ProcessLedConfig4(bool input) { return CreateHarpFrameForLeds(input, 4); }
        static HarpMessage ProcessLedConfig5(bool input) { return CreateHarpFrameForLeds(input, 5); }
        static HarpMessage ProcessLedConfig6(bool input) { return CreateHarpFrameForLeds(input, 6); }
        static HarpMessage ProcessLedConfig7(bool input) { return CreateHarpFrameForLeds(input, 7); }

        static HarpMessage ProcessLedConfigsSet(byte input)
        {
            return new HarpMessage(true, 2, 5, 44, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessLedConfigsClear(byte input)
        {
            return new HarpMessage(true, 2, 5, 45, 255, (byte)PayloadType.U8, input, 0);
        }


        /************************************************************************/
        /* Register: LEDS                                                       */
        /************************************************************************/
        static HarpMessage ProcessColorsOfLeds(byte [] RGBs)
        {
            return new HarpMessage(true, 2, 13, 46, 255, (byte)PayloadType.U8, RGBs[0], RGBs[1], RGBs[2], RGBs[3], RGBs[4], RGBs[5], RGBs[6], RGBs[7], RGBs[8], 0);
        }
    }
}
