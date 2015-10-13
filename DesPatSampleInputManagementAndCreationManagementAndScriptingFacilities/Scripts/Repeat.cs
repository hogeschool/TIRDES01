using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesPatSampleUnstructured.Scripts
{
  class Repeat : Instruction
  {
    Instruction body;
    public Repeat(Instruction body)
    {
      this.body = body;
    }

    public override InstructionResult Execute(float dt)
    {
      switch (body.Execute(dt))
      {
        case InstructionResult.Done:
          body = body.Reset();
          return InstructionResult.Running;
        case InstructionResult.DoneAndCreateAsteroid:
          body = body.Reset();
          return InstructionResult.RunningAndCreateAsteroid;
        case InstructionResult.Running:
          return InstructionResult.Running;
        case InstructionResult.RunningAndCreateAsteroid:
          return InstructionResult.RunningAndCreateAsteroid;
      }
      return InstructionResult.Running;
    }

    public override Instruction Reset()
    {
      return new Repeat(body.Reset());
    }
  }
}
