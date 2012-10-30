using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TestLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();
            float x, y;
            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.width;
            y = size.height;
            //CCMutableArray *array = [UIFont familyNames];
            //for( CCString *s in array )
            //	NSLog( s );
            CCLabelTTF label = CCLabelTTF.Create("cocos2d", "arial", 64);
            label.Position = new CCPoint(x / 2, y / 2);
            AddChild(label);
        }

        public static new TestLayer node()
        {
            TestLayer node = new TestLayer();
            return node;
        }
    }
}