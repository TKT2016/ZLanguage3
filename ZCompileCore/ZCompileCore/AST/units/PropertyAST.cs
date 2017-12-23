using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.ASTExps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;

using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

using ZCompileKit.Tools;
using ZLangRT.Attributes;

namespace ZCompileCore.AST
{
    /// <summary>
    /// 属性AST
    /// </summary>
    public class PropertyAST : SectionPartProc
    {
        public LexToken NameToken;
        public Exp PropertyValueExp;

        string PropertyName;
        ZType PropertyZType;
        ContextExp PropertyExpContext;
        ZCPropertyInfo ZPropertyCompiling;

        bool _isExist = false;
        public override void AnalyText()
        {
            ZPropertyCompiling = new ZCPropertyInfo();
            PropertyName = NameToken.GetText();
            if (this.ClassContext.ContainsPropertyName(PropertyName))
            {
                _isExist = true;
                ErrorF(this.Position, "属性'{0}'重复", PropertyName);
            }
            else
            {
                this.ClassContext.AddPropertyName(PropertyName);
                ZPropertyCompiling.ZPropertyZName = PropertyName;
                this.ClassContext.AddMember(ZPropertyCompiling);
            }
        }

        public override void AnalyType()
        {
            //if (this.PropertyValueExp != null && PropertyValueExp.ToString().IndexOf("图片")!=-1)
            //{
            //    Console.WriteLine("图片");
            //}
            if (_isExist) return;
            PropertyZType = AnalyPropertyZType();
            ZPropertyCompiling.ZPropertyType = PropertyZType;//.SetMemberType(PropertyZType);
        }

        private ZType AnalyPropertyZType()
        {
            bool isStatic = this.ClassContext.IsStatic();
            ZPropertyCompiling.IsStatic = isStatic;//.SetIsStatic(isStatic);
            ContextImportUse importUseContext = this.FileContext.ImportUseContext;
            if (HasValue())
            {
                PropertyValueExp = AnalyPropertyValueExp();
                ZType ztype = PropertyValueExp.RetType;
                if (ztype != null)
                {
                    //PropertyZType = ztype;
                    return ztype;
                }
            }
            ZType[] ztypes = importUseContext.SearchImportType(this.PropertyName);
            
            if (ztypes.Length == 1)
            {
                //PropertyZType = ztypes[0];
                return ztypes[0];
            }
             else if (ztypes.Length == 0)
             {
                // return ZLangBasicTypes.ZOBJECT;
             }
             else
             {
                 // return ztypes[1];
                 //return ZLangBasicTypes.ZOBJECT;
            }
            return ZLangBasicTypes.ZOBJECT;
        }

        public override void AnalyBody()
        {
            //if (this.PropertyValueExp != null && PropertyValueExp.ToString().IndexOf("图片") != -1)
            //{
            //    Console.WriteLine("图片");
            //}
            if (PropertyValueExp != null)
            {
              PropertyValueExp=  PropertyValueExp.Analy();
            }
        }

        public override void EmitName()
        {
            bool isStatic = this.ClassContext.IsStatic();
            var classBuilder = this.ClassContext.GetTypeBuilder();
            ZPropertyCompiling.IsStatic = isStatic;//.SetIsStatic(isStatic);
            Type propertyType = null;//
            if( PropertyZType is ZLType)
            {
                propertyType = ((ZLType)PropertyZType).SharpType;
            }
            else// (PropertyZType is ZLType)
            {
                propertyType = ((ZCClassInfo)PropertyZType).ClassBuilder;
            }
            MethodAttributes methodAttr =BuilderUtil.GetMethodAttr(isStatic);
            FieldAttributes fieldAttr = BuilderUtil.GetFieldAttr(isStatic);

            _fieldBuilder = classBuilder.DefineField("_" + PropertyName, propertyType, fieldAttr);
            _propertyBuilder = classBuilder.DefineProperty(PropertyName, PropertyAttributes.None, propertyType, null);

            EmitGet(classBuilder, PropertyName, propertyType, isStatic, _fieldBuilder, _propertyBuilder, methodAttr);
            EmitSet(classBuilder, PropertyName, propertyType, isStatic, _fieldBuilder, _propertyBuilder, methodAttr);
            SetAttrZCode(_propertyBuilder, PropertyName);
            ZPropertyCompiling.PropertyBuilder = _propertyBuilder;//.SetBuilder(_propertyBuilder);
            
        }

        public override void EmitBody()
        {
            if(HasValue())
            {
                var method = this.ClassContext.InitPropertyMethod;
                var IL = method.GetILGenerator();
                bool isStatic = this.ClassContext.IsStatic();
                if (!isStatic)
                {
                    IL.Emit(OpCodes.Ldarg_0);
                }
                PropertyValueExp.Emit();
                EmitHelper.StormField(IL, _fieldBuilder);
            }
            //throw new CCException();
        }

        private bool HasValue()
        {
            return this.PropertyValueExp != null;
        }

        FieldBuilder _fieldBuilder;
        PropertyBuilder _propertyBuilder;

        private void SetAttrZCode(PropertyBuilder builder, string name)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { name });
            builder.SetCustomAttribute(attributeBuilder);
        }

        private void EmitGet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodGet = classBuilder.DefineMethod("get_" + propertyName, methodAttr, propertyType, null);
            var ilget = methodGet.GetILGenerator();
            EmitHelper.EmitThis(ilget, isStatic);
            LocalBuilder retBuilder = ilget.DeclareLocal(propertyType);
            EmitHelper.LoadField(ilget, field);
            EmitHelper.StormVar(ilget, retBuilder);
            EmitHelper.LoadVar(ilget, retBuilder);
            ilget.Emit(OpCodes.Ret);
            property.SetGetMethod(methodGet);
        }

        private void EmitSet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodSet = classBuilder.DefineMethod("set_" + propertyName, methodAttr, typeof(void), new Type[] { propertyType });
            ILGenerator ilSet = methodSet.GetILGenerator();
            EmitHelper.EmitThis(ilSet, isStatic);
            if (isStatic)
            {
                ilSet.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                ilSet.Emit(OpCodes.Ldarg_1);
            }

            EmitHelper.StormField(ilSet, field);
            ilSet.Emit(OpCodes.Ret);
            property.SetSetMethod(methodSet);
        }

        private Exp AnalyPropertyValueExp()
        {
            Exp exp1 = PropertyValueExp.Parse();
            Exp exp2 = exp1.Analy();
            return exp2;
            //if(PropertyValueExp is ExpRaw)
            //{
            //    ExpRaw rawExp = (ExpRaw)PropertyValueExp;
            //    if(rawExp.RawTokens.Count==1)
            //    {
            //        LexToken tok = rawExp.RawTokens[0];
            //        if(tok.IsLiteral)
            //        {
            //            Exp literalExp = new ExpPropertyValueLiteral(tok);//{ LiteralToken = tok };
            //            literalExp.SetContext(this.PropertyExpContext);
            //            return literalExp;
            //        }
            //    }
            //}
            //return null;
        }

        
        public void SetContext(ContextMethod procContext)
        {
            this.ProcContext = procContext;
            this.ClassContext = this.ProcContext.ClassContext;
            this.FileContext = this.ClassContext.FileContext;
            PropertyExpContext = new ContextExp(procContext);
            if(PropertyValueExp!=null)
            {
                PropertyValueExp.SetContext(this.PropertyExpContext);
            }
        }

        public CodePosition Position
        {
            get
            {
                return this.NameToken.Position;
            }
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.NameToken.GetText());
            if (this.PropertyValueExp != null)
            {
                buff.Append("=");
                buff.Append(PropertyValueExp.ToString());
            }
            return buff.ToString();
        }
    }
}
