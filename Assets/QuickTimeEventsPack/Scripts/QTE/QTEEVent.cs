using System;
using UnityEngine;

namespace QTEPack
{
    internal class QTEEVent
    {
        public virtual string Text()
        {
            throw new NotImplementedException();
        }

        public virtual bool CheckIfDone()
        {
            throw new NotImplementedException();
        }
    }

    internal class QTEEvent_Key : QTEEVent
    {
        public KeyCode Key { get; private set; }
        public QTEEvent_Key(KeyCode key)
        {
            Key = key;
        }

        public override string Text()
        {
            if (Key == KeyCode.Space)
                return "Sp";

            return Key.ToString();
        }

        public override bool CheckIfDone()
        {
            return Input.GetKeyDown(Key);
        }
    }

    internal class QTEEvent_MouseRightClick : QTEEVent
    {
        public override string Text()
        {
            return "MR";
        }

        public override bool CheckIfDone()
        {
            return Input.GetMouseButton(1);
        }
    }

    internal class QTEEvent_MouseLeftClick : QTEEVent
    {
        public override string Text()
        {
            return "ML";
        }

        public override bool CheckIfDone()
        {
            return Input.GetMouseButton(0);
        }
    }
}