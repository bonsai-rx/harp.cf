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
    public enum WearCommandType : byte
    {
        StartAcquisition,
        StopAcquisition,
        StartStimulation,

        PositionMotor0,
        PositionMotor1,

        DigitalOutput0,
        DigitalOutput1,

        StartCamera0,
        StopCamera0,
        StartCamera1,
        StopCamera1,

        CameraOutput0,
        CameraOutput1
    }

    [Description(
        "\n" +
        "StartAcquisition: Any\n" +
        "StopAcquisition: Any\n" +
        "StartStimulation: Any\n" +
        "\n" +
        "PositionMotor0: Positive integer\n" +    // Don't need to indicate it's a UInt16 since the code makes the conversion
        "PositionMotor1: Positive integer\n" +    // Don't need to indicate it's a UInt16 since the code makes the conversion
        "\n" +
        "DigitalOutput0: Boolean\n" +
        "DigitalOutput1: Boolean\n" +
        "\n" +
        "StartCamera0: Any\n" +
        "StopCamera0: Any\n" +
        "StartCamera1: Any\n" +
        "StopCamera1: Any\n" +
        "\n" +
        "CameraOutput0: Boolean\n" +
        "CaemraOutput1: Boolean\n"
    )]
    public class WearCommand : SelectBuilder, INamedElement
    {
        public WearCommand()
        {
            Type = WearCommandType.StartAcquisition;
        }

        string INamedElement.Name
        {
            get { return typeof(WearCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public WearCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case WearCommandType.StartAcquisition:
                    return Expression.Call(typeof(WearCommand), "ProcessStartAcquisition", new[] { expression.Type }, expression);
                case WearCommandType.StopAcquisition:
                    return Expression.Call(typeof(WearCommand), "ProcessStopAcquisition", new[] { expression.Type }, expression);
                case WearCommandType.StartStimulation:
                    return Expression.Call(typeof(WearCommand), "ProcessStartStimulation", new[] { expression.Type }, expression);

                case WearCommandType.PositionMotor0:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(WearCommand), "ProcessPositionMotor0", null, expression);
                case WearCommandType.PositionMotor1:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(WearCommand), "ProcessPositionMotor1", null, expression);

                case WearCommandType.DigitalOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), "ProcessDigitalOutput0", null, expression);
                case WearCommandType.DigitalOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), "ProcessDigitalOutput1", null, expression);

                case WearCommandType.StartCamera0:
                    return Expression.Call(typeof(WearCommand), "ProcessStartCamera0", new[] { expression.Type }, expression);
                case WearCommandType.StopCamera0:
                    return Expression.Call(typeof(WearCommand), "ProcessStopCamera0", new[] { expression.Type }, expression);
                case WearCommandType.StartCamera1:
                    return Expression.Call(typeof(WearCommand), "ProcessStartCamera1", new[] { expression.Type }, expression);
                case WearCommandType.StopCamera1:
                    return Expression.Call(typeof(WearCommand), "ProcessStopCamera1", new[] { expression.Type }, expression);

                case WearCommandType.CameraOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), "ProcessCameraOutput0", null, expression);
                case WearCommandType.CameraOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), "ProcessCameraOutput1", null, expression);

                default:
                    break;
            }
            return expression;
        }

        static HarpMessage ProcessStartAcquisition<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, 1, 0);
        }

        static HarpMessage ProcessStopAcquisition<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, 0, 0);
        }

        static HarpMessage ProcessStartStimulation<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 33, 255, (byte)PayloadType.U8, 1, 0);
        }


        static HarpMessage ProcessPositionMotor0(UInt16 input)
        {
            return new HarpMessage(true, 2, 6, 80, 255, (byte)PayloadType.U16, (byte)(input & 255), (byte)((input >> 8) & 255), 0);
        }

        static HarpMessage ProcessPositionMotor1(UInt16 input)
        {
            return new HarpMessage(true, 2, 6, 85, 255, (byte)PayloadType.U16, (byte)(input & 255), (byte)((input >> 8) & 255), 0);
        }


        static HarpMessage ProcessDigitalOutput0(bool input)
        {
            return new HarpMessage(true, 2, 5, 38, 255, (byte)PayloadType.U8, (byte)(input ? 1 : 0), 0);
        }

        static HarpMessage ProcessDigitalOutput1(bool input)
        {
            return new HarpMessage(true, 2, 5, 39, 255, (byte)PayloadType.U8, (byte)(input ? 1 : 0), 0);
        }


        static HarpMessage ProcessStartCamera0<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 77, 255, (byte)PayloadType.U8, 1, 0);
        }

        static HarpMessage ProcessStopCamera0<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 77, 255, (byte)PayloadType.U8, 0, 0);
        }

        static HarpMessage ProcessStartCamera1<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 82, 255, (byte)PayloadType.U8, 1, 0);
        }

        static HarpMessage ProcessStopCamera1<TSource>(TSource input)
        {
            return new HarpMessage(true, 2, 5, 82, 255, (byte)PayloadType.U8, 0, 0);
        }

        static HarpMessage ProcessCameraOutput0(bool input)
        {
            return new HarpMessage(true, 2, 5, 36, 255, (byte)PayloadType.U8, (byte)(input ? 1 : 0), 0);
        }

        static HarpMessage ProcessCameraOutput1(bool input)
        {
            return new HarpMessage(true, 2, 5, 37, 255, (byte)PayloadType.U8, (byte)(input ? 1 : 0), 0);
        }
    }
}
