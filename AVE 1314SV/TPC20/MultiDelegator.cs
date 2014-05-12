using System;
using System.Windows.Forms;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Func<String> h1 = () => "Ola ISEL";
        Func<double> h2 = () => Math.PI;
        Func<double> h3 = () => Math.Sqrt(2);
        Func<String> h4 = () => "Ola Cheguei";

        MultiDelegator md = new MultiDelegator();
        md.AddHandler(h1);
        md.AddHandler(h2);
        md.AddHandler(h3);
        md.AddHandler(h4);
        md.DispatchAndPrint(typeof(Func<String>));
        md.DispatchAndPrint(typeof(Func<double>));
        md.RemoveHandler(h3);
        md.DispatchAndPrint(typeof(Func<double>));
    }
}


class MultiDelegator
{
    private Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

    public void AddHandler(Delegate h)
    {
        Type t = h.GetType();
        if (handlers.ContainsKey(t))
        {
            Delegate d;
            handlers.TryGetValue(t, out d);
            handlers.Remove(t);   
            h =  Delegate.Combine(d,h);
        }  
            handlers.Add(t, h);
    }

    public void RemoveHandler(Delegate h)
    {
        Type t = h.GetType();
        Delegate d;
        handlers.TryGetValue(t, out d);
         if(d != null) 
         {
            d = Delegate.Remove(d, h);
            handlers.Remove(t);
            handlers.Add(t, d);
        }
        
    }

    public void DispatchAndPrint(Type t)
    {
        Delegate d;
        handlers.TryGetValue(t, out d);
        if (d != null)
        {
            Delegate[] list = d.GetInvocationList();
            foreach (Delegate aux in list)
                Console.WriteLine(aux.DynamicInvoke());
        }
    }

} 