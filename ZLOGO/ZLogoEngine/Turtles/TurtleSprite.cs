using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ZGameEngine;
using ZGameEngine.Graphics;
using ZLangRT.Utils;
using ZLogoEngine.Sprites;
using ZLogoEngine.Turtles.Actions;
using ZOpen2D;

namespace ZLogoEngine.Turtles
{
    public class TurtleSprite : Sprite2DBase
    {
        internal ZLogoActionBase CurrentAction = null;
        internal TurtleStart StartAction;
        public TurtleForm Form { get;private set; }

        public TurtleSprite( )
        {
            
        }

        public void SetForm(TurtleForm form)
        {
            Form = form;
            this.Painter = new Draw2D(form.Width, form.Height);
            StartAction = new TurtleStart(Painter);
            AddAction(StartAction);
        }

        public virtual void RunZLogo()
        {

        }

        //public TurtleSprite(TurtleForm form)
        //{
        //    Form = form;
        //    this.Painter = new Draw2D(form.Width, form.Height);
        //    StartAction = new TurtleStart(Painter);
        //    AddAction(StartAction);
        //}

        public void AddAction(ZLogoActionBase turtleActionBase)
        {
            Animations.Add(turtleActionBase);
            CurrentAction = turtleActionBase;
        }

        //public void StartMove()
        //{
        //    //ResetTurtle();
        //}

        public void Forward(float distance)
        {
            TurtleMoveForward faction = new TurtleMoveForward(CurrentAction,distance);
            AddAction(faction);
            //AnimationMove animationTo = new AnimationMove(this, distance, false, this.Pen.Visible);
            //this.X = animationTo.Path.ToPosition.X;
            //this.Y = animationTo.Path.ToPosition.Y;
            //Animations.Add(animationTo);
        }

        public void Backward(float distance)
        {
            TurtleMoveBack faction = new TurtleMoveBack(CurrentAction, distance);
            AddAction(faction);
        }

        public void MoveTo(float newX, float newY)
        {
            TurtleJump faction = new TurtleJump(CurrentAction, newX,newY);
            AddAction(faction);
        }

        public void RotateLeft(float angleDelta)
        {
            TurtleRotateLeft faction = new TurtleRotateLeft(CurrentAction, angleDelta);
            AddAction(faction);
        }

        public void RotateRight(float angleDelta)
        {
            TurtleRotateRight faction = new TurtleRotateRight(CurrentAction, angleDelta);
            AddAction(faction);
        }

        internal void AddEndAction()
        {
            TurtleEnd faction = new TurtleEnd(CurrentAction);
            AddAction(faction);
        }

        public void PenUp()
        {
            //TurtlePen newPen = CurrentAction.GetEndTurleInfo().Pen.Clone();
            //newPen.IsDraw = false;
            TurtlePenChange faction = new TurtlePenChange(CurrentAction, false);
            AddAction(faction);
        }

        public void PenDown()
        {
            //TurtlePen newPen = CurrentAction.GetEndTurleInfo().Pen.Clone();
            //newPen.IsDraw = true;
            TurtlePenChange faction = new TurtlePenChange(CurrentAction, true);
            AddAction(faction);
        }

        public void SetPenColor(Color color)
        {
            TurtlePenChange faction = new TurtlePenChange(CurrentAction, color);
            AddAction(faction);
        }

        public void Show()
        {
            TurtleVisibleChange faction = new TurtleVisibleChange(CurrentAction, true);
            AddAction(faction);
        }

        public void Hide()
        {
            TurtleVisibleChange faction = new TurtleVisibleChange(CurrentAction,false);
            AddAction(faction);
        }

        public void SetBgColor(Color color)
        {
            FormChangeColor fcc = new FormChangeColor(CurrentAction, this.Form , color);
            AddAction(fcc);
            //this.BackgroundColor = color;
        }

        public void Wait(int millisecond)
        {
            System.Threading.Thread.Sleep(millisecond);
        }

        public void WaitSecond(int second)
        {
            Wait(second*1000);
        }

        public void Home()
        {
            TurtlePen newPen = CurrentAction.GetEndTurleInfo().Pen.Clone();
            newPen.IsDraw = true;
            TurtleHome faction = new TurtleHome(CurrentAction, StartAction.GetStartTurleInfo());
            AddAction(faction);
        }
    }
}
