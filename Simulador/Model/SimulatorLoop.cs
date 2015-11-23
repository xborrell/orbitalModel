using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CalculadorOrbital;
using System.Windows.Media.Media3D;
using satelite.interfaces;
using satelite.backend;

namespace Xb.Simulador.Model
{
    public class SimulatorLoop
    {
        public TimeSpan ActualTime { get; protected set; }
        public double AcceleratedTime { get; set; }
        public ISatelite Satelite { get; protected set; }

        Task _task;
        CancellationTokenSource _cancelSource;
        const long intervaloFisicoEnTicks = 1000000;
        const long ticksPorSegundo = 10000000;
        Vector posicionInicial;
        Vector velocidadInicial;
        float deltaEnSegundos = 0;
        Constantes constantes;

        public SimulatorLoop(IToolsFactory factory, Constantes constantes)
        {
            ActualTime = new TimeSpan();
            AcceleratedTime = 0;
            posicionInicial = factory.CreateVector(-822.79774F, -4438.63582F, 5049.31502F);
            velocidadInicial = factory.CreateVector(7.418175658F, .709253354F, 1.828703177F);

            this.constantes = constantes;
            Satelite = factory.CreateSatelite(posicionInicial, velocidadInicial);
        }

        public void Start()
        {
            _cancelSource = new CancellationTokenSource();

            Task<Task> task = Task.Factory.StartNew(
                function: ExecuteAsync,
                cancellationToken: _cancelSource.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default);

            _task = task.Unwrap();
        }

        public void Stop()
        {
            _cancelSource.Cancel(); // request the cancellation

            _task.Wait(); // wait for the task to complete
        }

        async Task ExecuteAsync()
        {
            ActualTime = new TimeSpan();
            long tiempoAcumuladoEnTicks = DateTime.Now.Ticks;

            while (!_cancelSource.IsCancellationRequested)
            {
                long tiempoActualEnTicks = DateTime.Now.Ticks;

                if (tiempoActualEnTicks - tiempoAcumuladoEnTicks > intervaloFisicoEnTicks)
                {
                    PhysicsUpdate((long)(intervaloFisicoEnTicks * AcceleratedTime));
                    tiempoAcumuladoEnTicks += intervaloFisicoEnTicks;
                }
            }
        }

        private void PhysicsUpdate(long deltaEnTicks)
        {
            deltaEnSegundos += deltaEnTicks / (float)ticksPorSegundo;

            while (deltaEnSegundos >= constantes.FixedDeltaTime)
            {
                Satelite.Pulse();
                deltaEnSegundos -= constantes.FixedDeltaTime;
            }

            ActualTime += TimeSpan.FromTicks(deltaEnTicks);
        }
    }
}
