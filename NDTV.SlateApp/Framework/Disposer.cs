using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.SlateApp.Framework
{
    /// <summary>
    /// A class which should be called while the object is to be put into the GC Queue
    /// </summary>
    public class Disposer:IDisposable
    {
        private object disposingObject
        {
            get;
            set;
        }

        public void DisposeObject(object obj)
        {
            disposingObject = obj;
            Dispose();
        }

        public void Dispose()
        {
            if (null == disposingObject)
                return;
            GC.SuppressFinalize(disposingObject);//Don't nullify the object rather put it into the GC Queue..
        }

    }
}
