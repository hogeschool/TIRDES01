using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesPatSampleUnstructured.Scripts
{
  class Wait : Instruction
  {
    float timeToWait, initialTimeWait;
    public Wait(float timeToWait)
    {
      this.timeToWait = timeToWait;
      this.initialTimeWait = timeToWait;
    }

    public override InstructionResult Execute(float dt)
    {
      timeToWait -= dt;
      if (timeToWait <= 0.0f)
        return InstructionResult.Done;
      else
        return InstructionResult.Running;
    }

    public override Instruction Reset()
    {
      return new Wait(initialTimeWait);
    }
  }
}
