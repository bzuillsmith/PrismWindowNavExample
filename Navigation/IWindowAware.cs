namespace PrismWindowNavExample.Navigation
{
    /// <summary>
    /// Interface that view models must implement to receive the current window's WindowManager.
    /// </summary>
    public interface IWindowManagerAware
    {
        IWindowManager WindowManager { get; set; }
    }
}
