using Bonsai.Expressions;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;
using System.Reactive;

namespace Bonsai.Harp.CF
{
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<WearEvent>))]
    [Description("Filters and selects event messages reported by the WEAR wireless, wired and basestation devices.")]
    public class WearEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which event to select from the WEAR devices.")]
        public WearEventType Type { get; set; } = WearEventType.Motion;

        string INamedElement.Name => $"Wear.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case WearEventType.Motion: return "The 9-axis of the inertial motion sensor unit, as a 1D column vector.";
                    case WearEventType.Accelerometer: return "The 3-axis accelerometer values, as a 1D column vector.";
                    case WearEventType.Gyroscope: return "The 3-axis gyroscope values, as a 1D column vector.";
                    case WearEventType.Magnetometer: return "The 3-axis magnetometer values, as a 1D column vector.";
                    case WearEventType.AnalogInput: return "The value of the basestation auxiliary analog input, in volts.";
                    case WearEventType.DigitalInput0: return "The state of the basestation auxiliary digital input 0.";
                    case WearEventType.DigitalInput1: return "The state of the basestation auxiliary digital input 1.";
                    case WearEventType.DigitalInputs: return "The state of the basestation auxiliary digital inputs, as a 1D column vector.";
                    case WearEventType.Acquiring: return "A value indicating whether the device is currently acquiring data.";
                    case WearEventType.DeviceSelected: return "The name of the currently selected device.";
                    case WearEventType.SensorTemperature: return "The value of the temperature sensors, in degrees celsius.";
                    case WearEventType.TxRetries: return "The number of retransmissions in the last second.";
                    case WearEventType.Battery: return "The percentage value of the current device battery level.";
                    case WearEventType.RxGood: return "A value indicating whether the power on the receiver is above -64 dBm.";
                    case WearEventType.SensorVersions: return "The hardware and firmware versions of the WEAR device and basestation.";
                    case WearEventType.RegisterStimulationStart: return "The timestamped raw value of the register indicating stimulation start.";
                    case WearEventType.RegisterMisc: return "The timestamped raw value containing the state of both digital inputs and the analog input value.";
                    case WearEventType.RegisterCamera0: return "The timestamped raw value of the register indicating the trigger state of camera 0.";
                    case WearEventType.RegisterCamera1: return "The timestamped raw value of the register indicating the trigger state of camera 1.";
                    case WearEventType.RegisterAcquisitionStatus: return "The timestamped raw value of the acquisition status register.";
                    case WearEventType.RegisterDeviceSelected: return "The timestamped raw value of the register indicating the currently selected device.";
                    case WearEventType.RegisterSensorTemperature: return "The timestamped raw value of the temperature sensor.";
                    case WearEventType.RegisterTxRetries: return "The timestamped raw value of the retransmissions register.";
                    case WearEventType.RegisterBattery: return "The timestamped raw value of the battery register.";
                    case WearEventType.RegisterRxGood: return "The timestamped raw value of the receiver status indicator register.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: DATA                                                       */
                /************************************************************************/
                case WearEventType.Motion:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessMotion), null, expression);
                case WearEventType.Accelerometer:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessAccelerometer), null, expression);
                case WearEventType.Gyroscope:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessGyroscope), null, expression);
                case WearEventType.Magnetometer:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessMagnetometer), null, expression);

                /************************************************************************/
                /* Register: MISC                                                       */
                /************************************************************************/
                case WearEventType.AnalogInput:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessAnalogInput), null, expression);
                case WearEventType.DigitalInput0:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessDigitalInput0), null, expression);
                case WearEventType.DigitalInput1:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessDigitalInput1), null, expression);
                case WearEventType.DigitalInputs:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessDigitalInputs), null, expression);

                case WearEventType.RegisterMisc:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterMisc), null, expression);

                /************************************************************************/
                /* Register: ACQ_STATUS                                                 */
                /************************************************************************/
                case WearEventType.Acquiring:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessAcquiring), null, expression);
                case WearEventType.RegisterAcquisitionStatus:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterAcquisitionStatus), null, expression);

                /************************************************************************/
                /* Register: START_STIM                                                 */
                /************************************************************************/
                case WearEventType.RegisterStimulationStart:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterStimulationStart), null, expression);

                /************************************************************************/
                /* Register: DEV_SELECT                                                 */
                /************************************************************************/
                case WearEventType.DeviceSelected:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessDeviceSelected), null, expression);
                case WearEventType.RegisterDeviceSelected:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterDeviceSelected), null, expression);

                /************************************************************************/
                /* Registers: TEMP, BATTERY, TX_RETRIES, RX_GOOD                        */
                /************************************************************************/
                case WearEventType.SensorTemperature:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessSensorTemperature), null, expression);
                case WearEventType.Battery:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessBattery), null, expression);
                case WearEventType.TxRetries:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessTxRetries), null, expression);
                case WearEventType.RxGood:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRxGood), null, expression);

                case WearEventType.RegisterSensorTemperature:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterSensorTemperature), null, expression);
                case WearEventType.RegisterBattery:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterBattery), null, expression);
                case WearEventType.RegisterTxRetries:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterTxRetries), null, expression);
                case WearEventType.RegisterRxGood:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterRxGood), null, expression);

                /************************************************************************/
                /* Versions                                                             */
                /************************************************************************/
                case WearEventType.SensorVersions:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessSensorVersions), null, expression);

                /************************************************************************/
                /* Registers: CAM0, CAM1                                                */
                /************************************************************************/
                case WearEventType.RegisterCamera0:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterCamera0), null, expression);
                case WearEventType.RegisterCamera1:
                    return Expression.Call(typeof(WearEvent), nameof(ProcessRegisterCamera1), null, expression);

                /************************************************************************/
                /* Default                                                              */
                /************************************************************************/
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        /************************************************************************/
        /* Register: DATA                                                       */
        /************************************************************************/
        static IObservable<Mat> ProcessMotion(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[9];
                return source.Event(address: 34).Select(input =>
                {
                    input.CopyTo(buffer);
                    return Mat.FromArray(buffer, 9, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessAccelerometer(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Event(address: 34).Select(input =>
                {
                    var payload = input.GetPayload();
                    Buffer.BlockCopy(payload.Array, payload.Offset, buffer, 0, 6);
                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessGyroscope(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Event(address: 34).Select(input =>
                {
                    var payload = input.GetPayload();
                    Buffer.BlockCopy(payload.Array, payload.Offset + 6, buffer, 0, 6);
                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessMagnetometer(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Event(address: 34).Select(input =>
                {
                    var payload = input.GetPayload();
                    Buffer.BlockCopy(payload.Array, payload.Offset + 12, buffer, 0, 6);
                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        /************************************************************************/
        /* Register: MISC                                                       */
        /************************************************************************/
        static IObservable<float> ProcessAnalogInput(IObservable<HarpMessage> source)
        {
            return source.Event(address: 35).Select(input => (float)((3.3 / 1.6) / 4096) * (input.GetPayloadUInt16() & 0x0FFF));
        }
        static IObservable<bool> ProcessDigitalInput0(IObservable<HarpMessage> source)
        {
            return source.Event(address: 35).Select(input => (input.GetPayloadByte(1) & (1 << 6)) == (1 << 6)).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessDigitalInput1(IObservable<HarpMessage> source)
        {
            return source.Event(address: 35).Select(input => (input.GetPayloadByte(1) & (1 << 7)) == (1 << 7)).DistinctUntilChanged();
        }
        static IObservable<Mat> ProcessDigitalInputs(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new byte[2];
                return source.Event(address: 35).Select(input =>
                {
                    var payload = input.GetPayloadByte(1);
                    buffer[0] = (byte)((payload >> 0) & 1);
                    buffer[1] = (byte)((payload >> 1) & 1);
                    return Mat.FromArray(buffer, 2, 1, Depth.U8, 1);
                });
            });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterMisc(IObservable<HarpMessage> source)
        {
            return source.Event(address: 35).Select(input => input.GetTimestampedPayloadUInt16());
        }

        /************************************************************************/
        /* Registers: TEMP, BATTERY, TX_RETRIES, RX_GOOD                        */
        /************************************************************************/
        static IObservable<float> ProcessSensorTemperature(IObservable<HarpMessage> source)
        {            
            return source.Event(address: 43).Select(input => (float)input.GetPayloadByte() * 256 / 340 + 35);
        }

        static IObservable<int> ProcessBattery(IObservable<HarpMessage> source)
        {
            return source.Event(address: 45).Select(input => (int)input.GetPayloadByte());
        }

        static IObservable<int> ProcessTxRetries(IObservable<HarpMessage> source)
        {
            return source.Event(address: 44).Select(input => (int)input.GetPayloadUInt16());
        }

        static IObservable<bool> ProcessRxGood(IObservable<HarpMessage> source)
        {
            return source.Event(address: 55).Select(input => input.GetPayloadByte() == 1);
        }

        static IObservable<Timestamped<byte>> ProcessRegisterSensorTemperature(IObservable<HarpMessage> source)
        {
            return source.Event(address: 43).Select(input => input.GetTimestampedPayloadByte());
        }
        static IObservable<Timestamped<byte>> ProcessRegisterBattery(IObservable<HarpMessage> source)
        {
            return source.Event(address: 45).Select(input => input.GetTimestampedPayloadByte());
        }
        static IObservable<Timestamped<ushort>> ProcessRegisterTxRetries(IObservable<HarpMessage> source)
        {
            return source.Event(address: 44).Select(input => input.GetTimestampedPayloadUInt16());
        }
        static IObservable<Timestamped<byte>> ProcessRegisterRxGood(IObservable<HarpMessage> source)
        {
            return source.Event(address: 55).Select(input => input.GetTimestampedPayloadByte());
        }

        /************************************************************************/
        /* Versions                                                             */
        /************************************************************************/
        static IObservable<string> ProcessSensorVersions(IObservable<HarpMessage> source)
        {
            return Observable.Create<string>(observer =>
            {
                var versions = new byte[8];
                var versionsReceived = new bool[8];
                var processor = Observer.Create<HarpMessage>(
                    input =>
                    {
                        switch (input.Address)
                        {
                            case 47: versions[0] = input.GetPayloadByte(); versionsReceived[0] = true; break;
                            case 48: versions[1] = input.GetPayloadByte(); versionsReceived[1] = true; break;
                            case 49: versions[2] = input.GetPayloadByte(); versionsReceived[2] = true; break;
                            case 50: versions[3] = input.GetPayloadByte(); versionsReceived[3] = true; break;
                            case 51: versions[4] = input.GetPayloadByte(); versionsReceived[4] = true; break;
                            case 52: versions[5] = input.GetPayloadByte(); versionsReceived[5] = true; break;
                            case 53: versions[6] = input.GetPayloadByte(); versionsReceived[6] = true; break;
                            case 54: versions[7] = input.GetPayloadByte(); versionsReceived[7] = true; break;
                            default: return;
                        }

                        if (versionsReceived[0] && versionsReceived[1] && versionsReceived[2] && versionsReceived[3])
                        {
                            var newLine = Environment.NewLine;
                            versionsReceived[0] = versionsReceived[1] = versionsReceived[2] = versionsReceived[3] = false;
                            observer.OnNext($"Sensor: Firmware {versions[0]}.{versions[1]}{newLine}Sensor: Hardware {versions[2]}.{versions[3]}");
                        }
                        else if (versionsReceived[4] && versionsReceived[5] && versionsReceived[6] && versionsReceived[7])
                        {
                            var newLine = Environment.NewLine;
                            versionsReceived[4] = versionsReceived[5] = versionsReceived[6] = versionsReceived[7] = false;
                            observer.OnNext($"Sensor Receiver: Firmware {versions[4]}.{versions[5]}{newLine}Sensor Receiver: Hardware {versions[6]}.{versions[7]}");
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);
                return source.Where(MessageType.Event).SubscribeSafe(processor);
            });
        }

        /************************************************************************/
        /* Register: ACQ_STATUS                                                 */
        /************************************************************************/
        static IObservable<bool> ProcessAcquiring(IObservable<HarpMessage> source)
        {
            return source.Event(address: 40).Select(input => input.GetPayloadByte() == 1);
        }

        static IObservable<Timestamped<byte>> ProcessRegisterAcquisitionStatus(IObservable<HarpMessage> source)
        {
            return source.Event(address: 40).Select(input => input.GetTimestampedPayloadByte());
        }


        /************************************************************************/
        /* Register: START_STIM                                                 */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterStimulationStart(IObservable<HarpMessage> source)
        {
            return source.Event(address: 33).Select(input => input.GetTimestampedPayloadByte());
        }

        /************************************************************************/
        /* Register: DEV_SELECT                                                 */
        /************************************************************************/
        static IObservable<string> ProcessDeviceSelected(IObservable<HarpMessage> source)
        {
            return source.Event(address: 42).Select(input =>
            {
                switch (input.GetPayloadByte() & 3)
                {
                    case 0: return "(0) Wired";
                    case 1: return "(1) Wireless RF1";
                    case 2: return "(2) Wireless RF2";
                    default: return string.Empty;
                }
            });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterDeviceSelected(IObservable<HarpMessage> source)
        {
            return source.Event(address: 42).Select(input => input.GetTimestampedPayloadByte());
        }

        /************************************************************************/
        /* Registers: CAM0, CAM1                                                */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterCamera0(IObservable<HarpMessage> source)
        {
            return source.Event(address: 36).Select(input => input.GetTimestampedPayloadByte());
        }

        static IObservable<Timestamped<byte>> ProcessRegisterCamera1(IObservable<HarpMessage> source)
        {
            return source.Event(address: 37).Select(input => input.GetTimestampedPayloadByte());
        }
    }

    public enum WearEventType : byte
    {
        /* Event: DATA */
        Motion = 0,
        Accelerometer,
        Gyroscope,
        Magnetometer,

        /* Event: MISC */
        AnalogInput,
        DigitalInput0,
        DigitalInput1,
        DigitalInputs,

        /* Event: ACQ_STATUS */
        Acquiring,

        /* Event: DEV_SELECT */
        DeviceSelected,

        /* Event: BATTERY, TX_RETRIES and RX_GOOD */
        SensorTemperature,
        TxRetries,
        Battery,
        RxGood,

        /* Event: ALL VERSIONS */
        SensorVersions,

        /* Raw Registers */
        RegisterStimulationStart,
        RegisterMisc,
        RegisterCamera0,
        RegisterCamera1,
        RegisterAcquisitionStatus,
        RegisterDeviceSelected,
        RegisterSensorTemperature,
        RegisterTxRetries,
        RegisterBattery,
        RegisterRxGood,
    }
}
