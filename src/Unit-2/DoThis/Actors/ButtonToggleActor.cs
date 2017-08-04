using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using System.Windows.Forms;

namespace ChartApp.Actors
{
    class ButtonToggleActor : UntypedActor
    {
        #region Message Types
        
        /// <summary>
        /// Toggles this button ON/OFF and sends an appropriate message
        /// </summary>
        public class Toggle { }

        #endregion

        private readonly CounterType _myCounterType;
        private bool _isToggleOn;
        private readonly Button _myButton;
        private readonly IActorRef _coordinatorActor;

        public ButtonToggleActor(IActorRef coordinatorActor, Button myButton, CounterType myCounterType, bool isToggleOn = false)
        {
            _coordinatorActor = coordinatorActor;
            _myButton = myButton;
            _myCounterType = myCounterType;
            _isToggleOn = isToggleOn;
        }

        protected override void OnReceive(object message)
        {
            if(message is Toggle && _isToggleOn)
            {
                // stop watching the counter
                _coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Unwatch(_myCounterType));
                FlipToggle();
            }
            else if( message is Toggle && !_isToggleOn)
            {
                // start watching the counter
                _coordinatorActor.Tell(new PerformanceCounterCoordinatorActor.Watch(_myCounterType));
                FlipToggle();
            }
            else
            {
                Unhandled(message);
            }
        }

        private void FlipToggle()
        {
            _isToggleOn = !_isToggleOn;
            _myButton.Text = string.Format("{0} ({1})", _myCounterType.ToString().ToUpperInvariant(), _isToggleOn ? "ON" : "OFF");
        }
    }
}
