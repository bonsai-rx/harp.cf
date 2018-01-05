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
    public enum WearEventType : byte
    {
        /* Event: DATA */
        Motion9Axis = 0,
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

    [Description(
        "\n" +
        "Motion9Axis: Integer Mat[9]\n" +
        "Accelerometer: Integer Mat[3]\n" +
        "Gyroscope: Integer Mat[3]\n" +
        "Magnetometer: Integer Mat[3]\n" +
        "\n" +
        "AnalogInput: Decimal (V)\n" +
        "DigitalInput0: Boolean (*)\n" +
        "DigitalInput1: Boolean (*)\n" +
        "DigitalInputs: Integer Mat[2]\n" +
        "\n" +
        "Acquiring: Boolean\n" +
        "DeviceSelected: String\n" +
        "SensorTemperature: Decimal (ºC)\n" +
        "TxRetries: Integer\n" +
        "Battery: Integer\n" +
        "RxGood: Boolean\n" +
        "SensorVersions: String\n" +
        "\n" +
        "RegisterStimulationStart: U8\n" +
        "RegisterMisc: U16\n" +
        "RegisterCamera0: U8\n" +
        "RegisterCamera1: U8\n" +
        "RegisterAcquisitionStatus: U8\n" +
        "RegisterDeviceSelected: Groupmask U8\n" +
        "RegisterSensorTemperature: U8\n" +
        "RegisterTxRetries: U16\n" +
        "RegisterBattery: U8\n" +
        "RegisterRxGood: U8\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class WearEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public WearEvent()
        {
            Type = WearEventType.Motion9Axis;
        }

        string INamedElement.Name
        {
            get { return typeof(WearEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public WearEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: DATA                                                       */
                /************************************************************************/
                case WearEventType.Motion9Axis:
                    return Expression.Call(typeof(WearEvent), "ProcessMotion9Axis", null, expression);
                case WearEventType.Accelerometer:
                    return Expression.Call(typeof(WearEvent), "ProcessAccelerometer", null, expression);
                case WearEventType.Gyroscope:
                    return Expression.Call(typeof(WearEvent), "ProcessGyroscope", null, expression);
                case WearEventType.Magnetometer:
                    return Expression.Call(typeof(WearEvent), "ProcessMagnetometer", null, expression);

                /************************************************************************/
                /* Register: MISC                                                       */
                /************************************************************************/
                case WearEventType.AnalogInput:
                    return Expression.Call(typeof(WearEvent), "ProcessAnalogInput", null, expression);
                case WearEventType.DigitalInput0:
                    return Expression.Call(typeof(WearEvent), "ProcessDigitalInput0", null, expression);
                case WearEventType.DigitalInput1:
                    return Expression.Call(typeof(WearEvent), "ProcessDigitalInput1", null, expression);
                case WearEventType.DigitalInputs:
                    return Expression.Call(typeof(WearEvent), "ProcessDigitalInputs", null, expression);

                case WearEventType.RegisterMisc:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterMisc", null, expression);

                /************************************************************************/
                /* Register: ACQ_STATUS                                                 */
                /************************************************************************/
                case WearEventType.Acquiring:
                    return Expression.Call(typeof(WearEvent), "ProcessAcquiring", null, expression);
                case WearEventType.RegisterAcquisitionStatus:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterAcquisitionStatus", null, expression);

                /************************************************************************/
                /* Register: START_STIM                                                 */
                /************************************************************************/
                case WearEventType.RegisterStimulationStart:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterStimulationStart", null, expression);

                /************************************************************************/
                /* Register: DEV_SELECT                                                 */
                /************************************************************************/
                case WearEventType.DeviceSelected:
                    return Expression.Call(typeof(WearEvent), "ProcessDeviceSelected", null, expression);
                case WearEventType.RegisterDeviceSelected:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterDeviceSelected", null, expression);

                /************************************************************************/
                /* Registers: TEMP, BATTERY, TX_RETRIES, RX_GOOD                        */
                /************************************************************************/
                case WearEventType.SensorTemperature:
                    return Expression.Call(typeof(WearEvent), "ProcessSensorTemperature", null, expression);
                case WearEventType.Battery:
                    return Expression.Call(typeof(WearEvent), "ProcessBatery", null, expression);
                case WearEventType.TxRetries:
                    return Expression.Call(typeof(WearEvent), "ProcessTxRetries", null, expression);
                case WearEventType.RxGood:
                    return Expression.Call(typeof(WearEvent), "ProcessRxGood", null, expression);

                case WearEventType.RegisterSensorTemperature:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterSensorTemperature", null, expression);
                case WearEventType.RegisterBattery:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterBatery", null, expression);
                case WearEventType.RegisterTxRetries:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterTxRetries", null, expression);
                case WearEventType.RegisterRxGood:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterRxGood", null, expression);

                /************************************************************************/
                /* Versions                                                             */
                /************************************************************************/
                case WearEventType.SensorVersions:
                    return Expression.Call(typeof(WearEvent), "ProcessSensorVersions", null, expression);

                /************************************************************************/
                /* Registers: CAM0, CAM1                                                */
                /************************************************************************/
                case WearEventType.RegisterCamera0:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterCamera0", null, expression);
                case WearEventType.RegisterCamera1:
                    return Expression.Call(typeof(WearEvent), "ProcessRegisterCamera1", null, expression);

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
        static bool is_evt35(HarpMessage input) { return ((input.Address == 35) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt36(HarpMessage input) { return ((input.Address == 36) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt37(HarpMessage input) { return ((input.Address == 37) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        static bool is_evt40(HarpMessage input) { return ((input.Address == 40) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt42(HarpMessage input) { return ((input.Address == 42) && (input.Error == false)); }

        static bool is_evt43(HarpMessage input) { return ((input.Address == 43) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt44(HarpMessage input) { return ((input.Address == 44) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt45(HarpMessage input) { return ((input.Address == 45) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt55(HarpMessage input) { return ((input.Address == 55) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        static bool is_evt47(HarpMessage input) { return ((input.Address == 47) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt48(HarpMessage input) { return ((input.Address == 48) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt49(HarpMessage input) { return ((input.Address == 49) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt50(HarpMessage input) { return ((input.Address == 50) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt51(HarpMessage input) { return ((input.Address == 51) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt52(HarpMessage input) { return ((input.Address == 52) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt53(HarpMessage input) { return ((input.Address == 53) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt54(HarpMessage input) { return ((input.Address == 54) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        /************************************************************************/
        /* Register: DATA                                                       */
        /************************************************************************/
        static IObservable<Mat> ProcessMotion9Axis(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[9];
                return source.Where(is_evt34).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = BitConverter.ToInt16(input.MessageBytes, 11 + i * 2);
                    }

                    return Mat.FromArray(buffer, 9, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessAccelerometer(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Where(is_evt34).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = BitConverter.ToInt16(input.MessageBytes, 11 + (i + 0) * 2);
                    }

                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessGyroscope(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Where(is_evt34).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = BitConverter.ToInt16(input.MessageBytes, 11 + (i + 3) * 2);
                    }

                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        static IObservable<Mat> ProcessMagnetometer(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new short[3];
                return source.Where(is_evt34).Select(input =>
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = BitConverter.ToInt16(input.MessageBytes, 11 + (i + 6) * 2);
                    }

                    return Mat.FromArray(buffer, 3, 1, Depth.S16, 1);
                });
            });
        }

        /************************************************************************/
        /* Register: MISC                                                       */
        /************************************************************************/
        static IObservable<float> ProcessAnalogInput(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return (float) ((3.3/1.6)/4096) * ((int)((UInt16)(BitConverter.ToUInt16(input.MessageBytes, 11) & (UInt16)(0x0FFF)))); });
        }
        static IObservable<bool> ProcessDigitalInput0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return ((input.MessageBytes[12] & (1 << 6)) == (1 << 6)); }).DistinctUntilChanged();
        }
        static IObservable<bool> ProcessDigitalInput1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return ((input.MessageBytes[12] & (1 << 7)) == (1 << 7)); }).DistinctUntilChanged();
        }
        static IObservable<Mat> ProcessDigitalInputs(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var buffer = new byte[2];
                return source.Where(is_evt35).Select(input =>
                {
                    buffer[0] = (byte)((input.MessageBytes[12] >> 0) & 1);
                    buffer[1] = (byte)((input.MessageBytes[12] >> 1) & 1);

                    return Mat.FromArray(buffer, 2, 1, Depth.U8, 1);
                });
            });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterMisc(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt35).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Registers: TEMP, BATTERY, TX_RETRIES, RX_GOOD                        */
        /************************************************************************/
        static IObservable<float> ProcessSensorTemperature(IObservable<HarpMessage> source)
        {            
            return source.Where(is_evt43).Select(input => { return ((float)(input.MessageBytes[11]) * 256) / 340 + 35; });
        }
        static IObservable<int> ProcessBatery(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return (int)(input.MessageBytes[11]); });
        }
        static IObservable<int> ProcessTxRetries(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt44).Select(input => { return (int)BitConverter.ToUInt16(input.MessageBytes, 11); });
        }
        static IObservable<bool> ProcessRxGood(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt55).Select(input => { return (input.MessageBytes[11] == 1); });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterSensorTemperature(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt43).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
        static IObservable<Timestamped<byte>> ProcessRegisterBatery(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt45).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
        static IObservable<Timestamped<UInt16>> ProcessRegisterTxRetries(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt44).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }
        static IObservable<Timestamped<byte>> ProcessRegisterRxGood(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt55).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Versions                                                             */
        /************************************************************************/
        static IObservable<string> ProcessSensorVersions(IObservable<HarpMessage> source)
        {
            return Observable.Defer(() =>
            {
                var VersionsReceived = new bool[8] { false, false, false, false, false, false, false, false };
                var Versions = new byte[8];

                return source.Where(input => is_evt47(input) || is_evt48(input) || is_evt49(input) || is_evt50(input) || is_evt51(input) || is_evt52(input) || is_evt53(input) || is_evt54(input)).Select(input =>
                {
                    if (is_evt47(input)) { VersionsReceived[0] = true; Versions[0] = input.MessageBytes[11]; }
                    if (is_evt48(input)) { VersionsReceived[1] = true; Versions[1] = input.MessageBytes[11]; }
                    if (is_evt49(input)) { VersionsReceived[2] = true; Versions[2] = input.MessageBytes[11]; }
                    if (is_evt50(input)) { VersionsReceived[3] = true; Versions[3] = input.MessageBytes[11]; }

                    if (is_evt51(input)) { VersionsReceived[4] = true; Versions[4] = input.MessageBytes[11]; }
                    if (is_evt52(input)) { VersionsReceived[5] = true; Versions[5] = input.MessageBytes[11]; }
                    if (is_evt53(input)) { VersionsReceived[6] = true; Versions[6] = input.MessageBytes[11]; }
                    if (is_evt54(input)) { VersionsReceived[7] = true; Versions[7] = input.MessageBytes[11]; }

                    if (VersionsReceived[0] && VersionsReceived[1] && VersionsReceived[2] && VersionsReceived[3])
                    {
                        VersionsReceived[0] = false;
                        VersionsReceived[1] = false;
                        VersionsReceived[2] = false;
                        VersionsReceived[3] = false;

                        string version;
                        version = "Sensor: Firmware " + System.Convert.ToString(Versions[0]);
                        version += "." + System.Convert.ToString(Versions[1]) + System.Environment.NewLine;
                        version += "Sensor: Hardware " + System.Convert.ToString(Versions[2]);
                        version += "." + System.Convert.ToString(Versions[3]);

                        return version;                                   
                    }
                    else if (VersionsReceived[4] && VersionsReceived[5] && VersionsReceived[6] && VersionsReceived[7])
                    {
                        VersionsReceived[4] = false;
                        VersionsReceived[5] = false;
                        VersionsReceived[6] = false;
                        VersionsReceived[7] = false;

                        string version;
                        version = "Sensor Receiver: Firmware " + System.Convert.ToString(Versions[4]);
                        version += "." + System.Convert.ToString(Versions[5]) + System.Environment.NewLine;
                        version += "Sensor Receiver: Hardware " + System.Convert.ToString(Versions[6]);
                        version += "." + System.Convert.ToString(Versions[7]);

                        return version;
                    }
                    else
                    {
                        return null;
                    }
                }).Where(output => output != null);
            });
        }

        /************************************************************************/
        /* Register: ACQ_STATUS                                                 */
        /************************************************************************/
        static IObservable<bool> ProcessAcquiring(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt40).Select(input => { return ((input.MessageBytes[11] & 1) == 1); });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterAcquisitionStatus(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt40).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }


        /************************************************************************/
        /* Register: START_STIM                                                 */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterStimulationStart(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt33).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Register: DEV_SELECT                                                 */
        /************************************************************************/
        static IObservable<string> ProcessDeviceSelected(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt42).Select(input =>
            {
                switch (input.MessageBytes[11] & 3)
                {
                    case 0:
                        return "(0) Wired";
                    case 1:
                        return "(1) Wireless RF1";
                    case 2:
                        return "(2) Wireless RF2";
                    default:
                        return "";
                }
            });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterDeviceSelected(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt42).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }

        /************************************************************************/
        /* Registers: CAM0, CAM1                                                */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterCamera0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt36).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterCamera1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt37).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }
    }
}
