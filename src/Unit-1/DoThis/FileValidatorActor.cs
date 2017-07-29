using System.IO;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor that validates use input and signals result to others
    /// </summary>
    public class FileValidatorActor : UntypedActor
    {
        private readonly IActorRef _consoleWriterActor;
        private readonly IActorRef _tailCoordinatorActor;

        public FileValidatorActor(IActorRef consoleWriterActor, IActorRef tailCoordinatorActor)
        {
            _consoleWriterActor = consoleWriterActor;
            _tailCoordinatorActor = tailCoordinatorActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal  that the user needs to pass an input
                _consoleWriterActor.Tell(new Messages.NullInputError("Input was blank. Please try again.\n"));

                // tell sender to continue doin its thing
                Sender.Tell(new Messages.ContinueProcessing());
            }
            else
            {
                var valid = IsFileUri(msg);
                if (valid)
                {
                    // send success to the console writer
                    _consoleWriterActor.Tell(new Messages.InputSuccess($"Starting processing for {msg}"));

                    // start the coordinator
                    _tailCoordinatorActor.Tell(new TailCoordinatorActor.StartTail(msg, _consoleWriterActor));
                }
                else
                {
                    // signal the inout was bad
                    _consoleWriterActor.Tell(new Messages.ValidationError($"{msg} is not an existing URI on disk."));

                    // tell sender to continue
                    Sender.Tell(new Messages.ContinueProcessing());
                }
            }
            // tell sender to continue doing its thing
            // (whaterever that may be, this actor doesn't care
            Sender.Tell(new Messages.ContinueProcessing());
        }

        private static bool IsFileUri(string path)
        {
            return File.Exists(path);
        }
    }
}
