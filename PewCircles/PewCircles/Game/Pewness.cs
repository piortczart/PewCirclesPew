using PewCircles.Game;
using System.Drawing;
using System;
using System.Collections.Generic;
using PewCircles.Mechanics;
using HelloGame.Client;
using System.Threading;
using System.Linq;
using HelloGame.Common.Logging;

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
        Overlay _overlay;
        int currentId;

        public Pewness(InputManager inputManager, TimeSource timeSource, ClientNetwork clientNetwork, GameStuffFactory stuffFactory, LoggerFactory loggerFactory)
        {
            _timeSource = timeSource;
            _inputManager = inputManager;
            clientNetwork.OnServerWelcome += OnServerWelcome;
            clientNetwork.OnServerUpdateLazers += UpdateLazers;
            clientNetwork.OnServerUpdateCircle += UpdateCircle;
            clientNetwork.GetMyGameObjects = GetMyGameObjects;
            _overlay = new Overlay(GetCircles);
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

        private void UpdateLazers(List<LazerPewPewSettings> lazersSettings)
        {
            foreach (var lazerSetting in lazersSettings)
            {
                var existing = GetObjectById<LazerPewPew>(lazerSetting.Id);
                if (existing != null)
                {
                    _logger.LogInfo("LAZER: Updating existing, id: " + lazerSetting.Id);
                    existing.Initialize(lazerSetting);
                }
                else
                {
                    _logger.LogInfo("LAZER: Adding a new, id: " + lazerSetting.Id);
                    lock (objectsSynchro)
                    {
                        _objects.Add(_stuffFactory.CreateLazer(lazerSetting));
                    }
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
            currentId = id;
            _pewzor = _stuffFactory.CreatePewCircle(
                new PewCircleSettings
                {
                    Id = currentId,
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
            Point mouse = _inputManager.GetMousePosition();

            int newId = Interlocked.Increment(ref currentId);
            LazerPewPew lazer = _stuffFactory
                .CreateLazer(new LazerPewPewSettings
                {
                    Id = newId,
                    CreatorId = pewCircle.Id,
                    Direction = mouse,
                    SpawnPoint = pewCircle.Physics.Center
                });

            lock (objectsSynchro)
            {
                _objects.Add(lazer);
            }
        }
    }
}
