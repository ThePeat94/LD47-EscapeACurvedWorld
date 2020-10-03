namespace EventArgs
{
    public class SprintChangedEventArgs : System.EventArgs
    {
        public bool NewState { get; }

        public SprintChangedEventArgs(bool newState)
        {
            this.NewState = newState;
        }
        
    }
}