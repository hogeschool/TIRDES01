using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesPatSampleUnstructured.Scripts
{
  class CreateAsteroid : Instruction
  {
    public override InstructionResult Execute(float dt)
    {
      return InstructionResult.DoneAndCreateAsteroid;
    }

    public override Instruction Reset()
    {
      return new CreateAsteroid();
    }
  }
}
