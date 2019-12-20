/****************************************************************************

Functions for interpreting c# code for blocks.

Copyright 2016 dtknowlove@qq.com
Copyright 2016 sophieml1989@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

****************************************************************************/


using System;
using System.Collections;
using System.Linq;

namespace UBlockly
{
    [CodeInterpreter(BlockType = "blocky_move_forward")]
    public class Blocky_Move_Forward_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_move_forward");
            //UnityEngine.Debug.Log("blocky_move_forward");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_move_backward")]
    public class Blocky_Move_Backward_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_move_backward");
            //UnityEngine.Debug.Log("blocky_move_backward");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_turn_left")]
    public class Blocky_Turn_Left_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_turn_left");
            //UnityEngine.Debug.Log("blocky_turn_left");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_turn_right")]
    public class Blocky_Turn_Right_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_turn_right");
            //UnityEngine.Debug.Log("blocky_turn_right");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_jump_forward")]
    public class Blocky_Jump_Forward_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_jump_forward");
            //UnityEngine.Debug.Log("blocky_jump_forward");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_jump_backward")]
    public class Blocky_Jump_Backward_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.commands.Add("blocky_jump_backward");
            //UnityEngine.Debug.Log("blocky_jump_backward");
        }
    }

    [CodeInterpreter(BlockType = "blocky_move_left")]
    public class Blocky_Move_Left_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.workspace.AddCommand("blocky_move_left");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_move_right")]
    public class Blocky_Move_Right_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
			Game.workspace.AddCommand("blocky_move_right");
        }
    }
}
