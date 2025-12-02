using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class MsgMove: MsgBase
{
    public int x = 0; 
    public int y = 0;
    public int z = 0;

    public MsgMove() {
        cmd_id_ = (short)MsgType.Move;
    }
}
