using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelme
{
    class Program
    {
        //class Barness
        //{
        //}

        //class Bar
        //{
        //}

        //class Foo
        //{
        //    public int Fooness { get; }

        //    public Foo(int fooness, Barness barness, Bar bar)
        //    {
        //        Fooness = fooness;
        //        // Doing something with barness and bar...
        //    }

        //    public Foo(int fooness, double bazness, Bar bar)
        //    {
        //        Fooness = fooness;
        //        // Doing something with barness and bar...
        //    }

        //}

        class Bar { }

        class Foo
        {
            public Foo(string magic, Bar bar) { }
        }

        static void Main(string[] args)
        {
            //IContainer container = new Container(_ => { _.ForSingletonOf<Bar>(); });

            IContainer container = new Container();

            //container.GetInstance<Foo>

            //Foo foo = container
            //                .With("fooness").EqualTo(1)
            //                .With("bazness").EqualTo(1d)
            //                .GetInstance<Foo>();

            //Foo foo2 = container
            //    .With("fooness").EqualTo(1)
            //    .With("barness").EqualTo(new Barness())
            //    .GetInstance<Foo>();
        }
    }
}
