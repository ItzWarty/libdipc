﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Ipc.Components
{
   public interface IComponent
   {
      void Attach(ILocalNode node);
   }
}
