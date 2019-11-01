﻿/****************************************************************************

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
            UnityEngine.Debug.Log("blocky_move_forward");
        }
    }
	
    [CodeInterpreter(BlockType = "blocky_move_backward")]
    public class Blocky_Move_Backward_Cmdtor : EnumeratorCmdtor
    {
        protected override IEnumerator Execute(Block block)
        {
			yield return null;
            UnityEngine.Debug.Log("blocky_move_backward");
        }
    }
	
}
