﻿using Bonsai.Expressions;
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
    public enum ArchimedesCommandType : byte
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
        SetDigitalOutputs,
        ClearDigitalOutputs,

        PresetLedConfig0,
        PresetLedConfig1,
        PresetLedConfig2,
        PresetLedConfig3,
        PresetLedConfig4,
        PresetLedConfig5,
        PresetLedConfig6,
        PresetLedConfig7,
        SetPresetLedConfigs,
        ClearPresetLedConfigs,

        ColorsOfLeds
    }

    [Description(
        "\n" +
        "LoadPosition: Positive integer\n" +
        "\n" +
        "ResetLeverAngle: Any\n" +
        "ResetLoadPosition: Any\n" +
        "HideLever: Boolean\n" +
        "UnhideLever: Boolean\n" +
        "\n" +
        "DigitalOutput0: Boolean\n" +
        "DigitalOutput1: Boolean\n" +
        "DigitalOutput2: Boolean\n" +
        "DigitalOutput3: Boolean\n" +
        "DigitalOutput4: Boolean\n" +
        "DigitalOutput5: Boolean\n" +
        "SetDigitalOutputs: Bitmask\n" +
        "ClearDigitalOutputs: Bitmask\n" +
        "\n" +
        "PresetLedConfig0: Boolean\n" +
        "PresetLedConfig1: Boolean\n" +
        "PresetLedConfig2: Boolean\n" +
        "PresetLedConfig3: Boolean\n" +
        "PresetLedConfig4: Boolean\n" +
        "PresetLedConfig5: Boolean\n" +
        "PresetLedConfig6: Boolean\n" +
        "PresetLedConfig7: Boolean\n" +
        "SetPresetLedConfigs: Bitmask\n" +    // Don't need to indicate it's a byte since the code makes the conversion
        "ClearPresetLedConfigs: Bitmask\n" +  // Don't need to indicate it's a byte since the code makes the conversion
        "\n" +
        "ColorsOfLeds: Positive integer array[9] (G,R,B,G,R,B,G,R,B)\n"
    )]

    public class ArchimedesCommand : SelectBuilder, INamedElement
    {
        public ArchimedesCommand()
        {
            Type = ArchimedesCommandType.LoadPosition;
        }

        string INamedElement.Name
        {
            get { return typeof(ArchimedesCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public ArchimedesCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Register: POS_TARGET                                                 */
                /************************************************************************/
                case ArchimedesCommandType.LoadPosition:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessLoadPosition", null, expression);

                /************************************************************************/
                /* Registers: RESET_ANGLE, RESET_MOTOR, HIDE_LEVER                      */
                /************************************************************************/
                case ArchimedesCommandType.ResetLeverAngle:
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessResetLeverAngle", new[] { expression.Type }, expression);
                case ArchimedesCommandType.ResetLoadPosition:
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessResetLoadPosition", new[] { expression.Type }, expression);
                case ArchimedesCommandType.HideLever:
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessHideLever", new[] { expression.Type }, expression);
                case ArchimedesCommandType.UnhideLever:
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessUnhideLever", new[] { expression.Type }, expression);

                /************************************************************************/
                /* Registers: SET_DOUTS, CLR_DOUTS                                      */
                /************************************************************************/
                case ArchimedesCommandType.DigitalOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput0", null, expression);
                case ArchimedesCommandType.DigitalOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput1", null, expression);
                case ArchimedesCommandType.DigitalOutput2:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput2", null, expression);
                case ArchimedesCommandType.DigitalOutput3:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput3", null, expression);
                case ArchimedesCommandType.DigitalOutput4:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput4", null, expression);
                case ArchimedesCommandType.DigitalOutput5:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessDigitalOutput5", null, expression);

                case ArchimedesCommandType.SetDigitalOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessSetDigitalOutputs", null, expression);
                case ArchimedesCommandType.ClearDigitalOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessClearDigitalOutputs", null, expression);

                /************************************************************************/
                /* Registers: EN_LED_CONFS, DIS_LED_CONFS                               */
                /************************************************************************/
                case ArchimedesCommandType.PresetLedConfig0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig0", null, expression);
                case ArchimedesCommandType.PresetLedConfig1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig1", null, expression);
                case ArchimedesCommandType.PresetLedConfig2:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig2", null, expression);
                case ArchimedesCommandType.PresetLedConfig3:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig3", null, expression);
                case ArchimedesCommandType.PresetLedConfig4:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig4", null, expression);
                case ArchimedesCommandType.PresetLedConfig5:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig5", null, expression);
                case ArchimedesCommandType.PresetLedConfig6:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig6", null, expression);
                case ArchimedesCommandType.PresetLedConfig7:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessPresetLedConfig7", null, expression);

                case ArchimedesCommandType.SetPresetLedConfigs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessSetPresetLedConfigs", null, expression);
                case ArchimedesCommandType.ClearPresetLedConfigs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessClearPresetLedConfigs", null, expression);

                /************************************************************************/
                /* Register: LEDS                                                       */
                /************************************************************************/
                case ArchimedesCommandType.ColorsOfLeds:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(ArchimedesCommand), "ProcessColorsOfLeds", null, expression);

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

        static HarpMessage ProcessSetDigitalOutputs(byte input)
        {
            return new HarpMessage(true, 2, 5, 42, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessClearDigitalOutputs(byte input)
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
        static HarpMessage ProcessPresetLedConfig0(bool input) { return CreateHarpFrameForLeds(input, 0); }
        static HarpMessage ProcessPresetLedConfig1(bool input) { return CreateHarpFrameForLeds(input, 1); }
        static HarpMessage ProcessPresetLedConfig2(bool input) { return CreateHarpFrameForLeds(input, 2); }
        static HarpMessage ProcessPresetLedConfig3(bool input) { return CreateHarpFrameForLeds(input, 3); }
        static HarpMessage ProcessPresetLedConfig4(bool input) { return CreateHarpFrameForLeds(input, 4); }
        static HarpMessage ProcessPresetLedConfig5(bool input) { return CreateHarpFrameForLeds(input, 5); }
        static HarpMessage ProcessPresetLedConfig6(bool input) { return CreateHarpFrameForLeds(input, 6); }
        static HarpMessage ProcessPresetLedConfig7(bool input) { return CreateHarpFrameForLeds(input, 7); }

        static HarpMessage ProcessSetPresetLedConfigs(byte input)
        {
            return new HarpMessage(true, 2, 5, 44, 255, (byte)PayloadType.U8, input, 0);
        }
        static HarpMessage ProcessClearPresetLedConfigs(byte input)
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
