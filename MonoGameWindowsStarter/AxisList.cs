using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// Defines the EndPoint structure to be used by AxisList and Box
    /// </summary>
    class EndPoint
    {
        public Box Box;
        public bool IsStart;
        public float Value;
    }

    /// <summary>
    /// Defines the Box structure to be used by AxisList and EndPoint
    /// </summary>
    class Box
    {
        public EndPoint Start;
        public EndPoint End;
        public IBoundable GameObject;
    }

    /// <summary>
    /// Defines a structure to store and sort game objects by an axis
    /// </summary>
    public class AxisList
    {
        Dictionary<IBoundable, Box> boxes = new Dictionary<IBoundable, Box>();

        List<EndPoint> endPoints = new List<EndPoint>();

        public void AddGameObject(IBoundable gameObject)
        {
            var box = new Box()
            {
                GameObject = gameObject
            };

            EndPoint start = new EndPoint()
            {
                Box = box,
                IsStart = true,
                Value = gameObject.Bounds.X
            };
            box.Start = start;

            EndPoint end = new EndPoint()
            {
                Box = box,
                IsStart = false,
                Value = gameObject.Bounds.X + gameObject.Bounds.Width
            };
            box.End = end;

            boxes.Add(gameObject, box);
            endPoints.Add(start);
            endPoints.Add(end);
            Sort();
        }

        public void UpdateGameObject(IBoundable gameObject)
        {
            var box = boxes[gameObject];
            box.Start.Value = gameObject.Bounds.X;
            box.End.Value = gameObject.Bounds.X + gameObject.Bounds.Width;
            Sort();
        }

        /// <summary>
        /// Sort uses bubble sort due to only ever having to move at most 2 items for any given call
        /// </summary>
        void Sort()
        {
            bool swaped = false;
            int j = endPoints.Count - 1;
            do
            {
                swaped = false;
                for(int i = 0; i < j; i++)
                {
                    if(endPoints[i].Value > endPoints[i + 1].Value)
                    {
                        swaped = true;
                        var tmp = endPoints[i];
                        endPoints[i] = endPoints[i + 1];
                        endPoints[i + 1] = tmp;
                    }
                }
                j--;
            } while (swaped && j > 0);
        }

        public IEnumerable<IBoundable> QueryRange(float start, float end)
        {
            List<IBoundable> open = new List<IBoundable>();
            foreach(var point in endPoints)
            {
                if(point.Value > end) break;

                if(point.IsStart)
                {
                    open.Add(point.Box.GameObject);
                }
                else if(point.Value < start)
                {
                    open.Remove(point.Box.GameObject);
                }
            }

            return open;
        }

        public IEnumerable<Tuple<IBoundable, IBoundable>> GetCollisionPairs()
        {
            List<IBoundable> open = new List<IBoundable>();
            List<Tuple<IBoundable, IBoundable>> pairs = new List<Tuple<IBoundable, IBoundable>>();

            foreach(EndPoint point in endPoints)
            {
                if(point.IsStart)
                {
                    foreach(var other in open)
                    {
                        pairs.Add(new Tuple<IBoundable, IBoundable>(point.Box.GameObject, other));
                    }
                    open.Add(point.Box.GameObject);
                }
                else
                {
                    open.Remove(point.Box.GameObject);
                }
            }

            return pairs;
        }
    }
}
