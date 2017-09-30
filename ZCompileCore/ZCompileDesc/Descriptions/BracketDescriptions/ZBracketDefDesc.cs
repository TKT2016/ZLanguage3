using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;

namespace ZCompileDesc.Descriptions
{
    public class ZBracketDefDesc //: ZBracketDescBase,IDefDesc
    {
        public List<ZParam> Params { get; protected set; }
    //    public NamingList<ZArgDefDescBase> DefArgs { get; protected set; } 

        public ZBracketDefDesc()
        {
            //DefArgs = new NamingList<ZArgDefDescBase>();
            Params = new List<ZParam>();
        }

        public void Add(ZParam zparam)
        {
            Params.Add(zparam);
        }

        public  int ParamsCount
        {
            get { return Params.Count; }
        }

        public ZParam GetParam(int i)
        {
            return Params[i];
        }

        public ZParam GetParam(string name)
        {
            return Params.Where(P => P.ZParamName == name).FirstOrDefault();
        }

        public bool ZEquals(ZBracketDefDesc zdefBracket)
        {
            if (zdefBracket.ParamsCount != this.ParamsCount) return false;
            if (this.ParamsCount == 0) return true;
            if (this.ParamsCount == 1)
            {
                return this.Params[0].ZEquals(zdefBracket.Params[0]);
            }
            else
            {
                for (int i = 0; i < zdefBracket.ParamsCount; i++)// zparam in zdefBracket.Params)
                {
                    var zparam = zdefBracket.GetParam(i);
                    var zparam2 = this.GetParam(zparam.ZParamName);
                    if (zparam2 == null) return false;
                    if (!zparam.ZEquals(zparam2)) return false;
                }
                return true;
            }
            //return false;
        }

        public bool ZEquals(ZBracketCallDesc zcallBracket)
        {
            if (this.ParamsCount != zcallBracket.ArgsCount) return false;
            if (this.ParamsCount == 0) return true;
            if (this.ParamsCount == 1)
            {
                var zparam = this.GetParam(0);
                var zarg = zcallBracket.GetArg(0);
                return zparam.ZEquals(zarg);
            }
            else
            {
                if (zcallBracket.HasNameCount == 0)
                {
                    for (int i = 0; i < this.ParamsCount; i++)// zparam in zdefBracket.Params)
                    {
                        var zarg = zcallBracket.GetArg(i);
                        var  zparam= this.GetParam(i);
                        if (!zparam.ZEquals(zarg)) return false;
                    }
                    return true;
                }
                else if (zcallBracket.AllHasName())
                {
                    for (int i = 0; i < this.ParamsCount; i++)// zparam in zdefBracket.Params)
                    {
                        var zparam = this.GetParam(i);
                        var zarg = zcallBracket.GetArg(zparam.ZParamName);
                        if (zarg == null) return false;
                        if (!zparam.ZEquals(zarg)) return false;
                    }
                    return true;
                }
            }

            return false;
        }

        public  string ToZCode()
        {
            List<string> zcodes = new List<string>();
            int size = this.ParamsCount;
            for (int i = 0; i < size; i++)
            {
                //var defArg = this.Params.Get(i);
                //string name = DefArgs.GetName(defArg);
                //string zcode = name + ":" + defArg.ToZCode();
                var zparam = this.Params[i];
                string zcode = zparam.ToZCode();
                zcodes.Add(zcode);
            }
            return "(" + string.Join(",", zcodes) + ")";
        }

        public override string ToString()
        {
            return ToZCode();
        }

    //    public void Add( ZArgDefDescBase zarg)
    //    {
    //        DefArgs.Add(zarg.ArgName , zarg);
    //    }

        public Type[] GetParamTypes()
        {
            return GetParamNormals().Select(p => (p.ZParamType.SharpType)).ToArray();
        }

        public ZParam[] GetParamNormals()
        {
            return Params
               .Where(p => (p.IsGeneric == false))
               .ToArray();
        }

    //    public bool Compare(ZBracketCallDesc zcallBracket)
    //    {
    //        return zcallBracket.Compare(this);
    //    }

    //    public ZArgDefDescBase Get(int i)
    //    {
    //        return DefArgs.Get(i);
    //    }

    //    public ZArgDefDescBase Get(string argName)
    //    {
    //        return DefArgs.Get(argName);
    //    }

    //    public bool ContainsName(string name)
    //    {
    //        return DefArgs.ContainsName(name);
    //    }

    }
}
