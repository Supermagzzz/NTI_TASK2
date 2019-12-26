using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution {
    public SceneController controller;

    public Solution(SceneController controller) {
        this.controller = controller;
        controller.Add("cntSections", 0);
    }

    public Vector4 getMove() {
        controller.Set("cntSections", (int)controller.Get("cntSections") + 1);
        if ((int)controller.Get("cntSections") % 2 == 0) {
            return new Vector4(4, 0, 0, 1);
        } else {
            return new Vector4(0, 0, 0, 1);
        }
    }
}
