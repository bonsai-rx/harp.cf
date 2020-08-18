using Bonsai.Expressions;
using System.Linq.Expressions;
using System.ComponentModel;
using System;

namespace Bonsai.Harp.CF
{
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<WearCommand>))]
    [Description("Creates standard command messages available to WEAR wireless, wired, and basestation devices.")]
    public class WearCommand : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which command to send to the WEAR devices.")]
        public WearCommandType Type { get; set; } = WearCommandType.StartAcquisition;

        string INamedElement.Name => $"Wear.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case WearCommandType.StartAcquisition: return "Start data acquisition from WEAR devices.";
                    case WearCommandType.StopAcquisition: return "Stop data acquisition from WEAR devices.";
                    case WearCommandType.StartStimulation: return "Start optogenetics stimulation on the WEAR device.";
                    case WearCommandType.PositionMotor0: return "Set the position of servo motor 0 using the input integer value.";
                    case WearCommandType.PositionMotor1: return "Set the position of servo motor 1 using the input integer value.";
                    case WearCommandType.DigitalOutput0: return "Set the state of digital output 0 using the input boolean value.";
                    case WearCommandType.DigitalOutput1: return "Set the state of digital output 1 using the input boolean value.";
                    case WearCommandType.StartCamera0: return "Start triggering camera 0.";
                    case WearCommandType.StopCamera0: return "Stop triggering camera 0.";
                    case WearCommandType.StartCamera1: return "Start triggering camera 1.";
                    case WearCommandType.StopCamera1: return "Stop triggering camera 1.";
                    case WearCommandType.CameraOutput0: return "Trigger camera 0 using the boolean input value to set the state of the trigger.";
                    case WearCommandType.CameraOutput1: return "Trigger camera 1 using the boolean input value to set the state of the trigger.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case WearCommandType.StartAcquisition:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStartAcquisition), null);
                case WearCommandType.StopAcquisition:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStopAcquisition), null);
                case WearCommandType.StartStimulation:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStartStimulation), null);

                case WearCommandType.PositionMotor0:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessPositionMotor0), null, expression);
                case WearCommandType.PositionMotor1:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessPositionMotor1), null, expression);

                case WearCommandType.DigitalOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessDigitalOutput0), null, expression);
                case WearCommandType.DigitalOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessDigitalOutput1), null, expression);

                case WearCommandType.StartCamera0:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStartCamera0), null);
                case WearCommandType.StopCamera0:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStopCamera0), null);
                case WearCommandType.StartCamera1:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStartCamera1), null);
                case WearCommandType.StopCamera1:
                    return Expression.Call(typeof(WearCommand), nameof(ProcessStopCamera1), null);

                case WearCommandType.CameraOutput0:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessCameraOutput0), null, expression);
                case WearCommandType.CameraOutput1:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(WearCommand), nameof(ProcessCameraOutput1), null, expression);

                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static HarpMessage ProcessStartAcquisition() => HarpCommand.WriteByte(address: 32, 1);
        static HarpMessage ProcessStopAcquisition() => HarpCommand.WriteByte(address: 32, 0);
        static HarpMessage ProcessStartStimulation() => HarpCommand.WriteByte(address: 33, 1);
        static HarpMessage ProcessPositionMotor0(ushort input) => HarpCommand.WriteUInt16(address: 80, input);
        static HarpMessage ProcessPositionMotor1(ushort input) => HarpCommand.WriteUInt16(address: 85, input);
        static HarpMessage ProcessDigitalOutput0(bool input) => HarpCommand.WriteByte(address: 38, (byte)(input ? 1 : 0));
        static HarpMessage ProcessDigitalOutput1(bool input) => HarpCommand.WriteByte(address: 39, (byte)(input ? 1 : 0));
        static HarpMessage ProcessStartCamera0() => HarpCommand.WriteByte(address: 77, 1);
        static HarpMessage ProcessStopCamera0() => HarpCommand.WriteByte(address: 77, 0);
        static HarpMessage ProcessStartCamera1() => HarpCommand.WriteByte(address: 82, 1);
        static HarpMessage ProcessStopCamera1() => HarpCommand.WriteByte(address: 82, 0);
        static HarpMessage ProcessCameraOutput0(bool input) => HarpCommand.WriteByte(address: 36, (byte)(input ? 1 : 0));
        static HarpMessage ProcessCameraOutput1(bool input) => HarpCommand.WriteByte(address: 37, (byte)(input ? 1 : 0));
    }

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
}
