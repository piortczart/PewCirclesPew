using PewCircles.Game;
using System.Drawing;
using System;
using System.Collections.Generic;
using PewCircles.Mechanics;
using HelloGame.Client;
using System.Threading;
using System.Linq;
using HelloGame.Common.Logging;
using PewCircles.Extensions;
using System.Collections.Concurrent;

namespace PewCircles
{
    public class MyGameObjects
    {
        public PewCircle PewCircle { get; set; }
        public List<LazerPewPew> MehLazers { get; set; }
    }

    public class Pewness
    {
        private InputManager _inputManager;
        private TimeSource _timeSource;
        private PewCircle _pewzor;
        private GameStuffFactory _stuffFactory;
        private List<GameObject> _objects = new List<GameObject>();
        private object objectsSynchro = new object();
        private ILogger _logger;
        private Overlay _overlay;
        private int _currentId;

        private ConcurrentQueue<PewCircleSettings> _netCircleSettings = new ConcurrentQueue<PewCircleSettings>();
        private ConcurrentQueue<LazerPewPewSettings> _netLazerSettings = new ConcurrentQueue<LazerPewPewSettings>();

        public Pewness(InputManager inputManager, TimeSource timeSource, ClientNetwork clientNetwork, GameStuffFactory stuffFactory, LoggerFactory loggerFactory)
        {
            _timeSource = timeSource;
            _inputManager = inputManager;
            clientNetwork.OnServerWelcome += OnServerWelcome;
            clientNetwork.OnServerUpdateLazers += QueueUpdateLazers;
            clientNetwork.OnServerUpdateCircle += QueueUpdateCircle;
            clientNetwork.GetMyGameObjects = GetMyGameObjects;
            _overlay = new Overlay(GetCircles, timeSource);
            _stuffFactory = stuffFactory;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        private TType GetObjectById<TType>(int id) where TType : GameObject
        {
            lock (objectsSynchro)
            {
                return (TType)_objects.Where(o => o.Id == id).SingleOrDefault();
            }
        }
        private void QueueUpdateCircle(PewCircleSettings circleSettings)
        {
            _netCircleSettings.Enqueue(circleSettings);
        }

        private void QueueUpdateLazers(List<LazerPewPewSettings> lazersSettings)
        {
            foreach (var lazerSetting in lazersSettings)
            {
                _netLazerSettings.Enqueue(lazerSetting);
            }
        }

        private void UpdateLazers(List<LazerPewPewSettings> lazersSettings)
        {
            foreach (var lazerSetting in lazersSettings)
            {
                UpdateLazer(lazerSetting);
            }
        }

        private void UpdateLazer(LazerPewPewSettings lazerSettings)
        {
            var existing = GetObjectById<LazerPewPew>(lazerSettings.Id);
            if (existing != null)
            {
                _logger.LogInfo("LAZER: Updating existing, id: " + lazerSettings.Id);
                existing.Initialize(lazerSettings);
            }
            else
            {
                _logger.LogInfo("LAZER: Adding a new, id: " + lazerSettings.Id);
                lock (objectsSynchro)
                {
                    _objects.Add(_stuffFactory.CreateLazer(lazerSettings));
                }
            }
        }

        private void UpdateCircle(PewCircleSettings circleSettings)
        {
            var existing = GetObjectById<PewCircle>(circleSettings.Id);
            if (existing != null)
            {
                _logger.LogInfo("CIRCLE: Updating existing, id: " + circleSettings.Id);
                existing.Initialize(circleSettings, false);
            }
            else
            {
                _logger.LogInfo("CIRCLE: Adding a new, id: " + circleSettings.Id);
                lock (objectsSynchro)
                {
                    _objects.Add(_stuffFactory.CreatePewCircle(circleSettings, false));
                }
            }
        }

        private List<PewCircle> GetCircles()
        {
            lock (objectsSynchro)
            {
                return _objects.Where(o => o is PewCircle).Cast<PewCircle>().ToList();
            }
        }

        private MyGameObjects GetMyGameObjects()
        {
            lock (objectsSynchro)
            {
                return new MyGameObjects
                {
                    PewCircle = _pewzor,
                    MehLazers = _objects.Where(o => o.CreatorId == _pewzor.Id && o is LazerPewPew).Cast<LazerPewPew>().ToList()
                };
            }
        }

        private void OnServerWelcome(int id)
        {
            _currentId = id;
            _pewzor = _stuffFactory.CreatePewCircle(
                new PewCircleSettings
                {
                    Id = _currentId,
                    Name = "X" + id,
                    SpawnPoint = new PointF(100, 100)
                }, true);
            lock (objectsSynchro)
            {
                _objects.Add(_pewzor);
            }
        }

        internal void Render(Graphics graphics)
        {
            foreach (GameObject gameObject in _objects.ToList())
            {
                gameObject.Render(graphics);
            }

            _overlay.Render(graphics);
        }

        internal void Update(TimeSpan timeDelta)
        {
            foreach (var item in _netCircleSettings.DequeueAll())
            {
                UpdateCircle(item);
            }

            foreach (var item in _netLazerSettings.DequeueAll())
            {
                UpdateLazer(item);
            }

            foreach (GameObject gameObject in _objects.ToList())
            {
                if (gameObject.IsDead)
                {
                    _objects.Remove(gameObject);
                }

                gameObject.Update(timeDelta);
            }
        }

        internal void GoGoLazer(PewCircle pewCircle)
        {
            PointF mouse = _inputManager.GetMousePosition().ToPointF();

            int newId = Interlocked.Increment(ref _currentId);
            LazerPewPew lazer = _stuffFactory
                .CreateLazer(new LazerPewPewSettings
                {
                    Id = newId,
                    CreatorId = pewCircle.Id,
                    Direction = mouse.Subtract(pewCircle.Physics.Center),
                    SpawnPoint = pewCircle.Physics.GetDirectionAsPoint(15).Add(pewCircle.Physics.Center)
                });

            lock (objectsSynchro)
            {
                _objects.Add(lazer);
            }
        }
    }
}
