using System;
using cocos2d;
using Random = cocos2d.Random;

namespace tests
{
    public class EaseSpriteDemo : CCLayer
    {
        protected CCSprite m_grossini;
        protected CCSprite m_kathia;

        protected String m_strTitle;
        protected CCSprite m_tamara;

        public virtual String title()
        {
            return "No title";
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_grossini = CCSprite.Create(TestResource.s_pPathGrossini);
            m_tamara = CCSprite.Create(TestResource.s_pPathSister1);
            m_kathia = CCSprite.Create(TestResource.s_pPathSister2);

            AddChild(m_grossini, 3);
            AddChild(m_kathia, 2);
            AddChild(m_tamara, 1);

            var s = CCDirector.SharedDirector.WinSize;

            m_grossini.Position = new CCPoint(60, 50);
            m_kathia.Position = new CCPoint(60, 150);
            m_tamara.Position = new CCPoint(60, 250);

            var label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label);
            label.Position = new CCPoint(s.width / 2, s.height - 50);

            var item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            var item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            var item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            var menu = CCMenu.Create(item1, item2, item3);
            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.width / 2 - 100, 30);
            item2.Position = new CCPoint(s.width / 2, 30);
            item3.Position = new CCPoint(s.width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.restartEaseAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.nextEaseAction());
            CCDirector.SharedDirector.ReplaceScene(s);
            ;
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new EaseActionsTestScene();
            s.AddChild(EaseTest.backEaseAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void positionForTwo()
        {
            m_grossini.Position = new CCPoint(60, 120);
            m_tamara.Position = new CCPoint(60, 220);
            m_kathia.Visible = false;
        }
    }

    public class SpriteEase : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var size = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(size.width - 130, 0));
            var move_back = (CCActionInterval) move.Reverse();

            var move_ease_in = CCEaseIn.Create((CCActionInterval) move.Copy(), 2.5f);
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseOut.Create((CCActionInterval) move.Copy(), 2.5f);
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());

            var a2 = m_grossini.RunAction(CCRepeatForever.Create((CCActionInterval)seq1));
            a2.Tag = 1;

            var a1 = m_tamara.RunAction(CCRepeatForever.Create((CCActionInterval)seq2));
            a1.Tag = 1;

            var a = m_kathia.RunAction(CCRepeatForever.Create((CCActionInterval)seq3));
            a.Tag = 1;

            Schedule(testStopAction, 6.25f);
        }

        public override String title()
        {
            return "EaseIn - EaseOut - Stop";
        }

        public void testStopAction(float dt)
        {
            Unschedule(testStopAction);
            m_kathia.StopActionByTag(1);
            m_tamara.StopActionByTag(1);
            m_grossini.StopActionByTag(1);
        }
    }

    public class SpriteEaseInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var size = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(size.width - 130, 0));

            var move_ease_inout1 = CCEaseInOut.Create((CCActionInterval) move.Copy(), 0.65f);
            var move_ease_inout_back1 = move_ease_inout1.Reverse();

            var move_ease_inout2 = CCEaseInOut.Create((CCActionInterval) move.Copy(), 1.35f);
            var move_ease_inout_back2 = move_ease_inout2.Reverse();

            var move_ease_inout3 = CCEaseInOut.Create((CCActionInterval) move.Copy(), 1.0f);
            var move_ease_inout_back3 = move_ease_inout3.Reverse() as CCActionInterval;

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move_ease_inout1, delay, move_ease_inout_back1, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_inout2, (CCFiniteTimeAction) delay.Copy(), move_ease_inout_back2,
                                                (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_inout3, (CCFiniteTimeAction) delay.Copy(), move_ease_inout_back3,
                                                (CCFiniteTimeAction) delay.Copy());

            m_tamara.RunAction(CCRepeatForever.Create((CCActionInterval)seq1));
            m_kathia.RunAction(CCRepeatForever.Create((CCActionInterval)seq2));
            m_grossini.RunAction(CCRepeatForever.Create((CCActionInterval)seq3));
        }

        public override String title()
        {
            return "EaseInOut and rates";
        }
    }

    public class SpriteEaseExponential : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = CCEaseExponentialIn.Create((CCActionInterval) (move.Copy()));
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseExponentialOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());


            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
            m_kathia.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "ExpIn - ExpOut actions";
        }
    }

    public class SpriteEaseExponentialInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = CCEaseExponentialInOut.Create((CCActionInterval) move.Copy());
            var move_ease_back = move_ease.Reverse(); //-. reverse()

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease, (CCFiniteTimeAction) delay.Copy(), move_ease_back, (CCFiniteTimeAction) delay.Copy());

            positionForTwo();

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
        }

        public override String title()
        {
            return "EaseExponentialInOut action";
        }
    }

    public class SpriteEaseSine : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = CCEaseSineIn.Create((CCActionInterval) move.Copy());
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseSineOut.Create((CCActionInterval) move.Copy());
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
            m_kathia.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "EaseSineIn - EaseSineOut";
        }
    }

    public class SpriteEaseSineInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = CCEaseSineInOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_back = move_ease.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease, (CCFiniteTimeAction) delay.Copy(), move_ease_back, (CCFiniteTimeAction) delay.Copy());

            positionForTwo();

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
        }

        public override String title()
        {
            return "EaseSineInOut action";
        }
    }

    public class SpriteEaseElastic : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = CCEaseElasticIn.Create((CCActionInterval) (move.Copy()));
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseElasticOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
            m_kathia.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "Elastic In - Out actions";
        }
    }

    public class SpriteEaseElasticInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));

            var move_ease_inout1 = CCEaseElasticInOut.Create((CCActionInterval) (move.Copy()), 0.3f);
            var move_ease_inout_back1 = move_ease_inout1.Reverse();

            var move_ease_inout2 = CCEaseElasticInOut.Create((CCActionInterval) (move.Copy()), 0.45f);
            var move_ease_inout_back2 = move_ease_inout2.Reverse();

            var move_ease_inout3 = CCEaseElasticInOut.Create((CCActionInterval) (move.Copy()), 0.6f);
            var move_ease_inout_back3 = move_ease_inout3.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move_ease_inout1, delay, move_ease_inout_back1, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_inout2, (CCFiniteTimeAction) delay.Copy(), move_ease_inout_back2,
                                                (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_inout3, (CCFiniteTimeAction) delay.Copy(), move_ease_inout_back3,
                                                (CCFiniteTimeAction) delay.Copy());

            m_tamara.RunAction(CCRepeatForever.Create(seq1));
            m_kathia.RunAction(CCRepeatForever.Create(seq2));
            m_grossini.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "EaseElasticInOut action";
        }
    }

    public class SpriteEaseBounce : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = CCEaseBounceIn.Create((CCActionInterval) (move.Copy()));
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseBounceOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
            m_kathia.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "Bounce In - Out actions";
        }
    }

    public class SpriteEaseBounceInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = CCEaseBounceInOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_back = move_ease.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease, (CCFiniteTimeAction) delay.Copy(), move_ease_back, (CCFiniteTimeAction) delay.Copy());

            positionForTwo();

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
        }

        public override String title()
        {
            return "EaseBounceInOut action";
        }
    }

    public class SpriteEaseBack : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease_in = CCEaseBackIn.Create((CCActionInterval) (move.Copy()));
            var move_ease_in_back = move_ease_in.Reverse();

            var move_ease_out = CCEaseBackOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_out_back = move_ease_out.Reverse();

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease_in, (CCFiniteTimeAction) delay.Copy(), move_ease_in_back, (CCFiniteTimeAction) delay.Copy());
            var seq3 = CCSequence.Create(move_ease_out, (CCFiniteTimeAction) delay.Copy(), move_ease_out_back,
                                                (CCFiniteTimeAction) delay.Copy());

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
            m_kathia.RunAction(CCRepeatForever.Create(seq3));
        }

        public override String title()
        {
            return "Back In - Out actions";
        }
    }

    public class SpriteEaseBackInOut : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            var move = CCMoveBy.Create(3, new CCPoint(s.width - 130, 0));
            var move_back = move.Reverse();

            var move_ease = CCEaseBackInOut.Create((CCActionInterval) (move.Copy()));
            var move_ease_back = move_ease.Reverse() as CCActionInterval;

            var delay = CCDelayTime.Create(0.25f);

            var seq1 = CCSequence.Create(move, delay, move_back, (CCFiniteTimeAction) delay.Copy());
            var seq2 = CCSequence.Create(move_ease, (CCFiniteTimeAction) delay.Copy(), move_ease_back, (CCFiniteTimeAction) delay.Copy());

            positionForTwo();

            m_grossini.RunAction(CCRepeatForever.Create(seq1));
            m_tamara.RunAction(CCRepeatForever.Create(seq2));
        }

        public override String title()
        {
            return "EaseBackInOut action";
        }
    }

    public class SpeedTest : EaseSpriteDemo
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var s = CCDirector.SharedDirector.WinSize;

            // rotate and jump
            var jump1 = CCJumpBy.Create(4, new CCPoint(-s.width + 80, 0), 100, 4);
            var jump2 = jump1.Reverse();
            var rot1 = CCRotateBy.Create(4, 360 * 2);
            var rot2 = rot1.Reverse();

            var seq3_1 = CCSequence.Create(jump2, jump1);
            var seq3_2 = CCSequence.Create(rot1, rot2);
            var spawn = CCSpawn.Create(seq3_1, seq3_2);
            var action = CCSpeed.Create(CCRepeatForever.Create(spawn), 1.0f);
            action.Tag = EaseTest.kTagAction1;

            var action2 = (CCAction) (action.Copy());
            var action3 = (CCAction) (action.Copy());

            action2.Tag = EaseTest.kTagAction1;
            action3.Tag = EaseTest.kTagAction1;

            m_grossini.RunAction(action2);
            m_tamara.RunAction(action3);
            m_kathia.RunAction(action);

            Schedule(altertime, 1.0f);
        }

        public void altertime(float dt)
        {
            var action1 = (CCSpeed) (m_grossini.GetActionByTag(EaseTest.kTagAction1));
            var action2 = (CCSpeed) (m_tamara.GetActionByTag(EaseTest.kTagAction1));
            var action3 = (CCSpeed) (m_kathia.GetActionByTag(EaseTest.kTagAction1));

            action1.Speed = Random.Float_0_1() * 2;
            action2.Speed = Random.Float_0_1() * 2;
            action3.Speed = Random.Float_0_1() * 2;
        }

        public override String title()
        {
            return "Speed action";
        }
    }

    public class EaseActionsTestScene : TestScene
    {
        public override void runThisTest()
        {
            var pLayer = EaseTest.nextEaseAction();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }

    public static class EaseTest
    {
        public const int MAX_LAYER = 13;
        public const int kTagAction1 = 1;
        public const int kTagAction2 = 2;
        public const int kTagSlider = 1;
        private static int sceneIdx = -1;

        public static CCLayer createEaseLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0:
                    return new SpriteEase();
                case 1:
                    return new SpriteEaseInOut();
                case 2:
                    return new SpriteEaseExponential();
                case 3:
                    return new SpriteEaseExponentialInOut();
                case 4:
                    return new SpriteEaseSine();
                case 5:
                    return new SpriteEaseSineInOut();
                case 6:
                    return new SpriteEaseElastic();
                case 7:
                    return new SpriteEaseElasticInOut();
                case 8:
                    return new SpriteEaseBounce();
                case 9:
                    return new SpriteEaseBounceInOut();
                case 10:
                    return new SpriteEaseBack();
                case 11:
                    return new SpriteEaseBackInOut();
                case 12:
                    return new SpeedTest();
            }
            return null;
        }

        public static CCLayer nextEaseAction()
        {
            sceneIdx++;
            sceneIdx %= MAX_LAYER;

            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backEaseAction()
        {
            sceneIdx--;
            var total = MAX_LAYER;
            if (sceneIdx < 0) sceneIdx += total;
            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartEaseAction()
        {
            var pLayer = createEaseLayer(sceneIdx);
            return pLayer;
        }
    }
}