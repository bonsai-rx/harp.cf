using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using TResult = System.String;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    public enum SyringePumpEventType: byte
    {
        /* Event: STEP_STATE */
        Step = 0,

        /* Event: DIR_STATE */
        Direction,
    }

    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<WearEvent>))]
    [Description("Filters and selects event messages reported by the Syringe Pump device.")]
    public class SyringePumpEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which event to select from the Syringe Pump device.")]
        public SyringePumpEventType Type { get; set; } = SyringePumpEventType.Step;
        
        string INamedElement.Name => $"SyringePump.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case SyringePumpEventType.Step: return "The state of the STEP motor controller pin.";
                    case SyringePumpEventType.Direction: return "The state of the direction of motor's movement.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();

            switch (Type)
            {
                case SyringePumpEventType.Step:
                    return Expression.Call(typeof(SyringePumpEvent), nameof(ProcessStep), null, expression);
                case SyringePumpEventType.Direction:
                    return Expression.Call(typeof(SyringePumpEvent), nameof(ProcessDirection), null, expression);
                default:
                    break;
            }

            return expression;
        }

        /************************************************************************/
        /* Register: STEP_STATE                                                 */
        /************************************************************************/
        static IObservable<bool> ProcessStep(IObservable<HarpMessage> source)
        {
            return source.Event(address: 34).Select(input => input.GetPayloadByte() != 0);
        }

        /************************************************************************/
        /* Register: DIR_STATE                                                  */
        /************************************************************************/
        static IObservable<bool> ProcessDirection(IObservable<HarpMessage> source)
        {
            return source.Event(address: 35).Select(input => input.GetPayloadByte() != 0);
        }
    }
}
