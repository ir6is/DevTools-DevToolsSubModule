using UnityUnityDevTools.DependencyInjection;
using UnityEngine;

namespace UnityDevTools.Ui.Example
{
    /// <summary>
    /// TestUnityDevTools.DependencyInjection.
    /// </summary>
    public class DI : MonoBehaviour
    {
        Container container;
        private void Awake()
        {
            container = new Container();

            container.RegisterSingleton<IWokrkerService , WorkerService>();
            container.RegisterType<IDoubleWorker, DoubleWorkerService >();

            var test = container.GetService<IDoubleWorker>();

            test.DoubleWork();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

                var test = container.GetService<IDoubleWorker>();

                test.DoubleWork();
            }
        }
    }
}