namespace UnityDevTools.Ui
{
    /// <summary>
    /// Controller.
    /// </summary>
    public class Controller<TView>
    {
        #region data

        #endregion

        #region interface
        protected TView View { get;private set; }
        public Controller(TView view)
        {
            View = view;
        }

        #endregion

        #region implementation

        #endregion
    }
}